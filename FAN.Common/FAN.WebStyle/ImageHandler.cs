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
     * 描述    : 处理样式里面的图片文件
     * =====================================================================
     * 修改时间：2015/3/4 17:38
     * 修改人  ：wangyunpeng
     * 版本号  ：V1.0.0.0 
     * 描述    ：处理样式里面的图片文件
*/
#endregion
using System;
using System.IO;
using System.Web;

namespace FAN.WebStyle
{
    /// <summary>
    /// 处理样式里面的图片文件
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            string physicalPath = request.PhysicalPath;
            if (File.Exists(physicalPath))
            {
                string subfix = Path.GetExtension(physicalPath);
                bool isOutputCache = false;
                HttpResponse response = context.Response;
                if (".png".Equals(subfix, StringComparison.CurrentCultureIgnoreCase))
                {
                    isOutputCache = true;
                    response.ContentType = "image/x-png";
                }
                else if (".jpg".Equals(subfix, StringComparison.CurrentCultureIgnoreCase))
                {
                    isOutputCache = true;
                    response.ContentType = "image/pjpeg";
                }
                if (isOutputCache)
                {
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
                        HttpCachePolicy cache = response.Cache;
                        cache.SetLastModifiedFromFileDependencies();
                        cache.SetETagFromFileDependencies();
                        cache.SetCacheability(HttpCacheability.Public);
                        cache.SetExpires(DateTime.Now.AddDays(DAYS));
                        TimeSpan timeSpan = TimeSpan.FromDays(DAYS);
                        cache.SetMaxAge(timeSpan);
                        cache.SetProxyMaxAge(timeSpan);
                        cache.SetLastModified(context.Timestamp);
                        cache.SetValidUntilExpires(true);
                        cache.SetSlidingExpiration(true);
                    }
                }
                response.WriteFile(physicalPath);
                response.End();
            }
        }
    }
}