#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2015 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  DomainRoute 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan 
     * 创建时间：2015/7/23
     * 描述    : 使用ASP.NET的UrlRouting配置URL格式，支持ashx
     * =====================================================================
     * 修改时间：2015/7/23
     * 修改人  ：  
     * 版本号  ：V1.0.0.0 
     * 描述    ：使用ASP.NET的UrlRouting配置URL格式，支持ashx
*/
#endregion
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using FAN.Helper;

namespace FAN.UrlRouting
{
    /// <summary>
    /// 实现HttpHandler的UrlRouting
    /// 
    /// http://codego.net/180476/
    /// </summary>
    public class HttpHandlerRoute : IRouteHandler, IDisposable
    {
        private Dictionary<string, IHttpHandler> _dictionary = new Dictionary<string, IHttpHandler>();
        private string _virtualPath = null;
        public HttpHandlerRoute(string virtualPath)
        {
            this._virtualPath = virtualPath;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string filePath = requestContext.HttpContext.Request.FilePath;
            IHttpHandler result = null;
            if (this._dictionary.ContainsKey(filePath))
            {
                result = this._dictionary[filePath];
            }
            if (result == null)
            {
                string fileName = IOHelper.GetFileNameWithoutExtension(filePath);
                string virtualPath = string.Format(this._virtualPath, fileName);
                try
                {
                    result = BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(IHttpHandler)) as IHttpHandler;
                }
                catch (Exception)
                {
                    result = null;
                }
                if (result != null && result.IsReusable)
                {
                    this._dictionary[filePath] = result;
                }
            }
            return result;
        }

        public void Dispose()
        {
            this._dictionary.Clear();
            this._dictionary = null;
        }
    }
}
