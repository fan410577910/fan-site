using FAN.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FAN.Nvelocity
{
    public class NVelocityHttpModule
    : IHttpModule
    {

        public void Init(HttpApplication application)
        {
            application.BeginRequest += this.Application_BeginRequest;
        }

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            BeginRequest(context.Request, context.Response);
        }

        public static void BeginRequest(HttpRequest request, HttpResponse response)
        {
            Uri filterUri = GetUriWithoutTrace(request, response);
            int segmentsLength = filterUri.Segments.Length;
            if (string.IsNullOrWhiteSpace(IOHelper.GetFileExtension(segmentsLength > 0 ? filterUri.Segments[segmentsLength - 1] : filterUri.AbsolutePath))
                && string.IsNullOrWhiteSpace(filterUri.Query))
            {//如果页面URL不带"/"结尾,需要添加"/"然后执行301跳转。wangyunpeng.
                if (!string.IsNullOrWhiteSpace(filterUri.AbsolutePath) && !filterUri.AbsolutePath.EndsWith("/"))
                {
                    Uri originalUri = request.Url;
                    UriBuilder uriBuilder = new UriBuilder(originalUri.Scheme, originalUri.Host, originalUri.Port);
                    uriBuilder.Path = string.Concat(originalUri.AbsolutePath, "/");
                    uriBuilder.Query = originalUri.Query;
                    string url301 = uriBuilder.ToString();
                    response.RedirectPermanent(url301, true);
                    return;
                }
            }
            string physicalPath = request.PhysicalPath;//指定的路径或文件名太长,文件名必须少于 260 个字符
            string absoluteFilePath = string.Concat(UrlHasher.Hash(filterUri, physicalPath, "/"), NVelocityBus.DEFAULT_DOCUMENT_SUFFIX);//所有url全部hash存放到redis和本地内存中去。区分URL大小写。wangyunpeng。2016-7-20。

            request.RequestContext.HttpContext.Items[NVelocityBus.NVELOCITY_TARGET_FILE_PATH] = absoluteFilePath;//设置放入上下文对象里面获取,2015-11-17。wangyunpeng
            //读取本地缓存(一级缓存)
            string fileContent = DataCacheBus.Get(absoluteFilePath) as string;
            //读取Redis缓存(二级缓存)
            if (NVelocityBus.CACHE_REDIS && string.IsNullOrWhiteSpace(fileContent))
            {
                try
                {
                    fileContent = "";//StackExchangeRedisBus.StringGet(absoluteFilePath);
                }
                catch (Exception)
                { }
                if (!string.IsNullOrWhiteSpace(fileContent))
                {//放进缓存中（一级缓存）
                    DataCacheBus.Insert(absoluteFilePath, fileContent = ZipHelper.GZipDeCompress(fileContent), DateTime.Now.AddMinutes(NVelocityBus.CACHE_DATETIME));
                }
            }
            //如果仅采用本地缓存策略，需要从磁盘上读取静态文件。(防止清除本地缓存后访问量过大，改从静态文件读取)
            if (!NVelocityBus.CACHE_REDIS && string.IsNullOrWhiteSpace(fileContent))
            {
                string physicalFilePath = null;
                string fileExtension = IOHelper.GetFileExtension(absoluteFilePath);
                if (!string.IsNullOrWhiteSpace(fileExtension)&& fileExtension.Equals(NVelocityBus.DEFAULT_DOCUMENT_SUFFIX, StringComparison.CurrentCultureIgnoreCase))
                {
                        physicalFilePath = NVelocityBus.GenerateFilePath(absoluteFilePath);
                        DateTime fileDateTime = IOHelper.GetFileMaxDateTime(physicalFilePath);
                        if (fileDateTime != DateTime.MinValue)
                        {//如果静态文件存在
                            if (fileDateTime.AddDays(NVelocityBus.CACHE_DAY) > DateTime.Now)
                            {//只读CACHE_DAY天之内的静态文件.
                                fileContent = IOHelper.GetFileContent(physicalFilePath);
                            }
                            else
                            {//超过CACHE_DAY天之外的静态文件会自动删除.
                                IOHelper.DeleteFile(physicalFilePath);
                            }
                        }
                }
                if (!string.IsNullOrWhiteSpace(fileContent) && !string.IsNullOrWhiteSpace(physicalFilePath))
                {//如果读取磁盘上的文件成功
                    DataCacheBus.Insert(absoluteFilePath, fileContent, TimeSpan.FromMinutes(NVelocityBus.CACHE_DATETIME), physicalFilePath);//放入本地缓存(一级缓存)
                }
            }
            if (!string.IsNullOrWhiteSpace(fileContent))
            {
                NVelocityBus.Output(request, response, fileContent, absoluteFilePath, true);
            }
            fileContent = null;
            absoluteFilePath = null;
        }

        /// <summary>
        /// 获取去掉Trace之后的请求Uri信息。
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static Uri GetUriWithoutTrace(HttpRequest request, HttpResponse response)
        {
            string url = null;
            bool isTrack = SearchHelper.GetTrack(request, response, out url);
            Uri uri = null;
            if (isTrack)
            {
                uri = new Uri(url);
            }
            else
            {
                uri = request.Url;
            }
            url = null;
            return uri;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
