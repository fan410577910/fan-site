using FAN.Helper;
using Microsoft.Security.Application;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace FAN.Nvelocity
{
    /// <summary>
    /// NVelocity操作总线
    /// </summary>
    public class NVelocityBus
    {
        internal const string NVELOCITY_TARGET_FILE_PATH = "NVELOCITY_TARGET_FILE_PATH";
        private const string DEFAULT_DOCUMENT_NAME = "index.html";
        internal const string DEFAULT_DOCUMENT_SUFFIX = ".html";
        private const string GZIP = "gzip";
        private const string DEFLATE = "deflate";
        private const string ENCODING = "utf-8";
        private static object @FileLockObject = new object();
        /// <summary>
        /// 缓存时间（单位：分钟）
        /// </summary>
        internal static readonly int CACHE_DATETIME = 0;
        /// <summary>
        /// 缓存时间（单位：天）
        /// </summary>
        internal static readonly int CACHE_DAY = 0;
        /// <summary>
        /// 是否使用Redis缓存
        /// </summary>
        internal static readonly bool CACHE_REDIS = false;
        /// <summary>
        /// 指定生成的文件类型(html,aspx)
        /// </summary>
        private readonly static Regex _FileSubfixRegex = new Regex(@".+\.(html|aspx)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly static VelocityEngine _VelocityEngine = new VelocityEngine();
        private readonly static string _TemplatePhysicalPath = System.Web.Hosting.HostingEnvironment.MapPath("~/templates");
        private readonly static string _StaticFileDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/staticfiles");
        private readonly static GlobalizationSection _GlobalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;

        static NVelocityBus()
        {
            CACHE_DATETIME = TypeParseHelper.StrToInt32(ConfigHelper.GetAppSettingValue("CACHE_DATETIME"));
            CACHE_DATETIME = Math.Max(CACHE_DATETIME, 20);
            CACHE_DAY = TypeParseHelper.StrToInt32(ConfigHelper.GetAppSettingValue("CACHE_DAY"));
            CACHE_DAY = Math.Max(CACHE_DAY, 2);
            //CACHE_REDIS = StackExchangeRedisManager.CACHE_REDIS;
            _VelocityEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            _VelocityEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, NVelocityBus._TemplatePhysicalPath);//模板文件所在的文件夹  
            _VelocityEngine.SetProperty(RuntimeConstants.INPUT_ENCODING, ENCODING);
            _VelocityEngine.SetProperty(RuntimeConstants.OUTPUT_ENCODING, ENCODING);
            _VelocityEngine.Init();
        }
        /// <summary>
        /// 合并模板和数据字典
        /// </summary>
        /// <param name="templateName">Velocity模板文件名称</param>
        /// <param name="dict">Velocity模板文件所用到的数据字典</param>
        /// <returns>返回Velocity模板页面内容（去掉VM语法之后的结果）</returns>
        private static string MergeVM(string templateName, Dictionary<string, object> dict)
        {
            string html = null;
            Template template = NVelocityBus._VelocityEngine.GetTemplate(templateName);//模板页名字
            VelocityContext velcityContext = new VelocityContext();
            foreach (KeyValuePair<string, object> pair in dict)
            {
                velcityContext.Put(pair.Key, pair.Value);//填充数据，在模板中可以通过$data来引用  
            }
            using (System.IO.StringWriter stringWriter = new System.IO.StringWriter())
            {
                template.Merge(velcityContext, stringWriter);//将数据和模板内容进行合并
                html = stringWriter.GetStringBuilder().ToString();
            }
            velcityContext = null;
            template = null;
            html = MinifyHelper.MinifyHtml(html);
            return html;
        }

        /// <summary>
        /// 生成静态文件
        /// </summary>
        /// <param name="absoluteFilePath">生成文件绝对路径（不包含物理路径）</param>
        /// <param name="fileContent">要保存的文件内容</param>
        /// <param name="fullFilePath">生成文件文件成功之后，返回文件的物理路径，返回null表示生成文件失败</param>
        private static void GenerateFile(string absoluteFilePath, string fileContent, out string fullFilePath)
        {
            fullFilePath = null;
            //文件名转码，防止URL参数当文件名过长.wangwei-2014-06-2
            absoluteFilePath = System.Web.HttpUtility.UrlDecode(absoluteFilePath);
            if (!string.IsNullOrEmpty(fileContent))
            {
                fullFilePath = NVelocityBus.GenerateFilePath(absoluteFilePath);
                bool isGenerateFile = true;
                char[] invalidFileNameChars = IOHelper.InvalidFileNameChars;
                string fileName = IOHelper.GetFileNameWithoutExtension(fullFilePath);
                foreach (char invalidChar in invalidFileNameChars)
                {
                    if (fileName.Contains(new string(invalidChar, 1)))
                    {
                        isGenerateFile = false;
                        break;
                    }
                }
                fileName = null;

                if (isGenerateFile)
                {
                    char[] invalidPathChars = IOHelper.InvalidPathChars;
                    string filePath = IOHelper.GetDirectoryName(fullFilePath);
                    foreach (char invalidChar in invalidPathChars)
                    {
                        if (filePath.Contains(new string(invalidChar, 1)))
                        {
                            isGenerateFile = false;
                            break;
                        }
                    }
                    filePath = null;
                }

                if (isGenerateFile)
                {
                    lock (@FileLockObject)
                    {
                        try
                        {
                            IOHelper.GenerateFile(fullFilePath, fileContent);
                        }
                        catch
                        {
                            fullFilePath = null;
                        }
                    }
                }
                else
                {
                    fullFilePath = null;
                }
            }
        }
        /// <summary>
        /// 得到生成后的文件路径
        /// </summary>
        /// <param name="absoluteFilePath">绝对路径</param>
        /// <returns>返回要生成静态文件的物理路径</returns>
        internal static string GenerateFilePath(string absoluteFilePath)
        {
            if (absoluteFilePath.IndexOf("/") == 0 && absoluteFilePath.Length > 1)
            {
                absoluteFilePath = absoluteFilePath.Substring(1);
            }
            return IOHelper.CombinePath(NVelocityBus._StaticFileDirectory, absoluteFilePath.Replace('/', '\\'));
        }

        /// <summary>
        /// 合并模板和数据字典,生成静态文件，同时放入缓存中。
        /// </summary>
        /// <param name="templateName">模板文件名称</param>
        /// <param name="dict">数据字典</param>
        /// <param name="absoluteFilePath">生成文件绝对路径（不包含物理路径）</param>
        /// <param name="isUseLocalCache">是否只使用本地缓存的方式存放静态文件</param>
        /// <returns>返回模板页面内容（去掉VM语法之后的结果），文件的内容</returns>
        private static string MergeAndGenerateFile(string templateName, Dictionary<string, object> dict, string absoluteFilePath, bool isUseLocalCache)
        {
            string fileContent = NVelocityBus.MergeVM(templateName, dict);
            if (!string.IsNullOrWhiteSpace(fileContent))
            {
                if (isUseLocalCache)
                {//如果页面设置生成静态页面，并且该页面单独设置只使用本地缓存存放静态页面（例如：google爬虫收录的页面，因为量很大并且很集中，不适合放在Redis缓存中。）(一级缓存)
                    UseLocalCache(absoluteFilePath, fileContent, true);
                }
                else
                {
                    //使用本地缓存存放数据(一级缓存)。
                    UseLocalCache(absoluteFilePath, fileContent, !NVelocityBus.CACHE_REDIS);
                    if (NVelocityBus.CACHE_REDIS)
                    {//如果页面设置生成静态页面，并且网站设置使用Redis缓存存放静态页面
                        try
                        {
                            //写入Redis缓存(二级缓存)。
                            //StackExchangeRedisBus.StringSet(absoluteFilePath, ZipHelper.GZipCompress(fileContent), TimeSpan.FromDays(NVelocityBus.CACHE_DAY));
                        }
                        catch (Exception)
                        {//写入Redis失败
                        }
                    }
                }
            }
            return fileContent;
        }
        /// <summary>
        /// 使用本地缓存
        /// </summary>
        /// <param name="absoluteFilePath">生成文件的绝对路径</param>
        /// <param name="fileContent">文件内容</param>
        /// <param name="isGenerateFile">是否生成本地文件</param>
        private static void UseLocalCache(string absoluteFilePath, string fileContent, bool isGenerateFile)
        {
            string fullFilePath = null;
            if (isGenerateFile)
            {
                //如果需要生成本地静态文件
                NVelocityBus.GenerateFile(absoluteFilePath, fileContent, out fullFilePath);//生成静态文件
            }
            if (string.IsNullOrEmpty(fullFilePath))
            {
                //如果生成文件失败，也放进缓存中（一级缓存），但是放入缓存的时间设置为绝对过期时间。防止频繁请求服务器端占用更多的资源。
                DataCacheBus.Insert(absoluteFilePath, fileContent, DateTime.Now.AddMinutes(CACHE_DATETIME));
            }
            else
            {
                //如果生成文件成功，放入缓存并设置相对过期时间同时还加上缓存依赖。（一级缓存）
                DataCacheBus.Insert(absoluteFilePath, fileContent, TimeSpan.FromMinutes(CACHE_DATETIME), fullFilePath);
            }
        }

        /// <summary>
        /// 输出到客户端浏览器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="templateName">Velocity模板文件名称</param>
        /// <param name="dict">Velocity模板文件所用到的数据字典</param>
        /// <param name="isGenerateFile">是否生成静态文件</param>
        /// <param name="isUseLocalCache">是否只使用本地缓存的方式存放静态文件（例如：Google爬虫收录的页面，只需要在本地生成静态文件。）</param>
        public static void Print(HttpRequest request, HttpResponse response, string templateName, Dictionary<string, object> dict, bool isGenerateFile, bool isUseLocalCache)
        {
            string fileContent = null;
            string absoluteFilePath = null;
            if (isGenerateFile)
            {
                absoluteFilePath = request.RequestContext.HttpContext.Items[NVelocityBus.NVELOCITY_TARGET_FILE_PATH] as string; //改从上下文对象里面获取,2015-11-17。
                //当设置生成静态页面时才会生成静态的文件。
                fileContent = NVelocityBus.MergeAndGenerateFile(templateName, dict, absoluteFilePath, isUseLocalCache);
            }
            else
            {
                //没有设置生成静态文件，并且不满足生成条件的，一律按动态页面处理。
                fileContent = NVelocityBus.MergeVM(templateName, dict);
            }
            if (!string.IsNullOrWhiteSpace(fileContent))
            {
                NVelocityBus.Output(request, response, fileContent, absoluteFilePath, isGenerateFile);
            }
        }

        /// <summary>
        /// 将文件内容输出到客户端客户端浏览器，支持http头操作。
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="fileContent">要往客户端浏览器输出的文本内容</param>
        /// <param name="isHttpHead">True表示使用http头操作，False表示不使用http头操作</param>
        internal static void Output(HttpRequest request, HttpResponse response, string fileContent, string absoluteFilePath, bool isHttpHead)
        {
            if (string.IsNullOrWhiteSpace(fileContent))
            {
                return;
            }
            //wangyunpeng 增加http头，把生成的路径输出出来，方便修改页面使用。
            if (absoluteFilePath != null)
            {
                response.AddHeader("X-Page-Hash", absoluteFilePath);
            }
            HttpCachePolicy cache = response.Cache;
            cache.SetOmitVaryStar(true);//http://www.cnblogs.com/dudu/archive/2011/11/03/outputcache_Bug_vary.html
//#if DEBUG
            //本地调试不使用浏览器缓存
            //cache.SetCacheability(HttpCacheability.NoCache);
            //cache.SetExpires(DateTime.UtcNow.AddYears(-1));
            //cache.SetMaxAge(TimeSpan.Zero);
            //cache.SetProxyMaxAge(TimeSpan.Zero);
            //cache.SetNoServerCaching();
            //cache.SetNoStore();
            //cache.SetNoTransforms();
//#else
            string ifModifiedSince = request.Headers["If-Modified-Since"];
            if (isHttpHead)
            {
                if (
                !string.IsNullOrWhiteSpace(ifModifiedSince)
                && TimeSpan.FromTicks(DateTime.Now.Ticks - TypeParseHelper.StrToDateTime(ifModifiedSince).Ticks).Minutes < CACHE_DATETIME)
                {
                    response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                    response.StatusDescription = "Not Modified";
                    response.End();
                    return;
                }
                else
                {
                    cache.SetLastModifiedFromFileDependencies();
                    cache.SetETagFromFileDependencies();
                    cache.SetCacheability(HttpCacheability.Public);
                    cache.SetExpires(DateTime.UtcNow.AddMinutes(CACHE_DATETIME));
                    TimeSpan timeSpan = TimeSpan.FromMinutes(CACHE_DATETIME);
                    cache.SetMaxAge(timeSpan);
                    cache.SetProxyMaxAge(timeSpan);
                    cache.SetLastModified(DateTime.Now);
                    cache.SetValidUntilExpires(true);
                    cache.SetSlidingExpiration(true);
                }
            }
            
//#endif
            System.Text.Encoding encoding = IOHelper.GetHtmEncoding(fileContent) ?? NVelocityBus._GlobalizationSection.ResponseEncoding;
            response.ContentEncoding = encoding;
            response.ContentType = response.ContentType;
            response.Write(fileContent);

            //压缩页面
            if (request.ServerVariables["HTTP_X_MICROSOFTAJAX"] == null)
            {
                if (NVelocityBus.IsAcceptEncoding(request, GZIP))
                {
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                    NVelocityBus.SetResponseEncoding(response, GZIP);
                }
                else if (NVelocityBus.IsAcceptEncoding(request, DEFLATE))
                {
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                    NVelocityBus.SetResponseEncoding(response, DEFLATE);
                }
            }
            //强制结束输出
            response.End();
        }

        private static bool IsAcceptEncoding(HttpRequest request, string encoding)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            return !string.IsNullOrEmpty(acceptEncoding) && acceptEncoding.Contains(encoding);
        }
        private static void SetResponseEncoding(HttpResponse response, string encoding)
        {
            response.AddHeader("Content-Encoding", encoding);
        }
    }
}
