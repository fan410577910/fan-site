#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  ImageHandler 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2015/3/4 17:38 
     * 描述    : 处理样式里面的js和css文件
     * =====================================================================
     * 修改时间：2015/3/4 17:38
     * 修改人  ：wangyunpeng
     * 版本号  ：V1.0.0.0 
     * 描述    ：处理样式里面的js和css文件
*/
#endregion
using System;
using System.Linq;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;
using System.Web.Configuration;
using FAN.WebStyle.Pack;
using System.Collections.Generic;

namespace FAN.WebStyle
{
    /// <summary>
    /// 处理样式里面的js和css文件
    /// </summary>
    public class StyleHandler : IHttpHandler
    {
        private const string GZIP = "gzip";
        private const string DEFLATE = "deflate";
        private readonly static GlobalizationSection _GlobalizationSection = null;
        private readonly static string[] _MiniJsFiles = null;
        enum ECacheType
        {
            None = 0,
            Css = 1,
            Js = 2
        }

        static StyleHandler()
        {
            string miniJsFiles = GetAppSettingValue("miniJsFiles");
            if (miniJsFiles == null)
            {
                _MiniJsFiles = new string[0];
            }
            else if (miniJsFiles.Contains(","))
            {
                _MiniJsFiles = miniJsFiles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                _MiniJsFiles = new string[] { miniJsFiles };
            }
            _GlobalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            string phyFilePath = request.PhysicalPath;
            if (!File.Exists(phyFilePath))
            {
                string phyDirPath = Path.GetDirectoryName(phyFilePath);
                if (!string.IsNullOrWhiteSpace(phyDirPath))
                {
                    string fileName = Path.GetFileNameWithoutExtension(phyFilePath);
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        string fileExtension = Path.GetExtension(phyFilePath);
                        if (!string.IsNullOrWhiteSpace(fileExtension))
                        {
                            string[] fileNames = fileName.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                            if (fileNames != null)
                            {
                                int length = fileNames.Length;
                                if (length > 1)
                                {
                                    fileName = string.Join("_", fileNames, 0, length - 1);
                                    phyFilePath = Path.Combine(phyDirPath, string.Concat(fileName, fileExtension));
                                }
                                Array.Clear(fileNames, 0, length);
                                fileNames = null;
                            }
                            fileExtension = null;
                        }
                        fileName = null;
                    }
                    phyDirPath = null;
                }
            }
            if (File.Exists(phyFilePath))
            {
                HttpResponse response = context.Response;
                ECacheType cacheType = ECacheType.None;
                string subfix = Path.GetExtension(phyFilePath);
                if (".css".Equals(subfix, StringComparison.CurrentCultureIgnoreCase))
                {
                    cacheType = ECacheType.Css;
                    response.ContentType = "text/css";
                }
                else if (".js".Equals(subfix, StringComparison.CurrentCultureIgnoreCase))
                {
                    cacheType = ECacheType.Js;
                    response.ContentType = "application/x-javascript";
                }
#if !DEBUG
                if (cacheType != ECacheType.None)
                {
                    //http://www.cnblogs.com/shanyou/archive/2012/05/01/2477500.html
                    const int DAYS = 30;
                    string ifModifiedSince = request.Headers["If-Modified-Since"];
                    if (!string.IsNullOrEmpty(ifModifiedSince)
                        && TimeSpan.FromTicks(DateTime.Now.Ticks - DateTime.Parse(ifModifiedSince).Ticks).Days < DAYS)
                    {
                        response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                        response.StatusDescription = "Not Modified";
                        response.End();
                        return;
                    }
                    else
                    {
                        string fileContent = null;
                        string ifNoneMatch = request.Headers["If-None-Match"];
                        string eTag = string.Format("\"{0}\"", this.GetFileMd5(string.Concat(phyFilePath, fileContent = this.GetFileContent(phyFilePath))));
                        if (!string.IsNullOrEmpty(ifNoneMatch) && ifNoneMatch.Equals(eTag))
                        {
                            response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                            response.StatusDescription = "Not Modified";
                            response.End();
                            return;
                        }
                        else
                        {
                            HttpCachePolicy cache = response.Cache;
                            //cache.SetLastModifiedFromFileDependencies();//程序自动读取文件的LastModified
                            cache.SetLastModified(context.Timestamp);//因为静态文件的URL是特殊处理过，所以程序自动读取文件是在磁盘上找不到的，故改成手工代码设置文件的LastModified的方式。wangyunpeng，2016-4-12
                            //cache.SetETagFromFileDependencies();//程序自动读取文件的ETag
                            cache.SetETag(eTag);//因为静态文件的URL是特殊处理过，所以程序自动读取文件是在磁盘上找不到的，故改成手工代码设置文件的ETag的方式。wangyunpeng，2016-4-12
                            cache.SetCacheability(HttpCacheability.Public);
                            cache.SetExpires(DateTime.Now.AddDays(DAYS));
                            TimeSpan timeSpan = TimeSpan.FromDays(DAYS);
                            cache.SetMaxAge(timeSpan);
                            cache.SetProxyMaxAge(timeSpan);
                            cache.SetValidUntilExpires(true);
                            cache.SetSlidingExpiration(true);
                            #region 压缩js和css文件
                            if (!string.IsNullOrWhiteSpace(fileContent))
                            {
                                if (cacheType == ECacheType.Js)
                                {
                                    string fileName = Path.GetFileName(phyFilePath);
                                    if (_MiniJsFiles.Contains(fileName, new StringComparer()))
                                    {
                                        fileContent = this.PackJavascript(fileContent);//ECMAScript压缩
                                    }
                                }
                                //输出内容
                                response.ContentEncoding = _GlobalizationSection == null ? Encoding.UTF8 : _GlobalizationSection.ResponseEncoding;
                                response.Write(fileContent);
                            }
                            #endregion
                        }
                    }
                }
                else
                {//直接输出文件
                    response.WriteFile(phyFilePath);
                }
#else
                //直接输出文件
                response.WriteFile(phyFilePath);
#endif
                //取消GZIP压缩，IE7,IE8,Safari支持GZIP压缩显示css和js有问题。wangyunpeng
                if (this.IsAcceptEncoding(request, GZIP))
                {
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                    this.SetResponseEncoding(response, GZIP);
                }
                else if (this.IsAcceptEncoding(request, DEFLATE))
                {
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                    this.SetResponseEncoding(response, DEFLATE);
                }
                response.End();
            }
        }
        /// <summary>
        /// 获取文件内存
        /// </summary>
        /// <param name="phyFilePath"></param>
        /// <returns></returns>
        private string GetFileContent(string phyFilePath)
        {
            string fileContent = null;
            using (StreamReader streamReader = new StreamReader(phyFilePath, Encoding.UTF8))
            {
                fileContent = streamReader.ReadToEnd();
            }
            return fileContent ?? string.Empty;
        }

        /// <summary>
        /// 文件MD5加密
        /// </summary>
        /// <param name="fileTag"></param>
        /// <returns></returns>
        private string GetFileMd5(string fileTag)
        {
            StringBuilder sb = new StringBuilder("");
            if (!string.IsNullOrEmpty(fileTag))
            {
                byte[] md5Hashs = null;
                try
                {
                    md5Hashs = Encoding.UTF8.GetBytes(fileTag);
                    if (md5Hashs != null && md5Hashs.Length > 0)
                    {
                        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                        {
                            md5Hashs = md5.ComputeHash(md5Hashs);
                        }
                        if (md5Hashs != null)
                        {
                            for (int i = 0; i < md5Hashs.Length; i++)
                            {
                                sb.Append(md5Hashs[i].ToString("X2"));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    sb = new StringBuilder("");
                }
                finally
                {
                    if (md5Hashs != null)
                    {
                        if (md5Hashs.Length > 0)
                        {
                            Array.Clear(md5Hashs, 0, md5Hashs.Length);
                        }
                        md5Hashs = null;
                    }
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// wangyunpeng
        /// </summary>
        /// <param name="request"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// [Obsolete("IE7,IE8,Safari支持GZIP压缩显示css和js有问题")]
        private bool IsAcceptEncoding(HttpRequest request, string encoding)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            return !string.IsNullOrEmpty(acceptEncoding) && acceptEncoding.Contains(encoding);
        }
        /// <summary>
        /// wangyunpeng
        /// </summary>
        /// <param name="response"></param>
        /// <param name="encoding"></param>
        /// [Obsolete("IE7,IE8,Safari支持GZIP压缩显示css和js有问题")]
        private void SetResponseEncoding(HttpResponse response, string encoding)
        {
            response.AddHeader("Content-Encoding", encoding);
        }
        /// <summary>
        /// 压缩javascript内容
        /// </summary>
        /// <param name="jsContent"></param>
        /// <returns></returns>
        private string PackJavascript(string jsContent)
        {
            try
            {
                jsContent = new ECMAScriptPacker().Pack(jsContent).Replace("\n", "\r\n");
            }
            catch { }
            return jsContent;
        }
        /// <summary>
        /// 读取appSettings中的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetAppSettingValue(string key)
        {
            string value = null;
            foreach (string item in ConfigurationManager.AppSettings)
            {
                if (item.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                {
                    value = ConfigurationManager.AppSettings[key];
                    break;
                }
            }
            return value;
        }
        class StringComparer : IEqualityComparer<string>
        {

            public bool Equals(string x, string y)
            {
                bool result = false;
                if (x == null && y == null)
                {
                    result = true;
                }
                else
                {
                    if (x == null && y != null)
                    {
                        result = false;
                    }
                    else if (x != null && y == null)
                    {
                        result = false;
                    }
                    else
                    {
                        result = x.ToLower().Equals(y.ToLower());
                    }
                }
                return result;
            }

            public int GetHashCode(string obj)
            {
                return base.GetHashCode();
            }
        }
    }
}