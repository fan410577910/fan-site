#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2018 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-TI50KE6KO4 
     * 文件名：  UrlRoutingBus 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan
     * 创建时间： 2018/5/12 14:01:52 
     * 描述    :
     * =====================================================================
     * 修改时间：2018/5/12 14:01:52 
     * 修改人  ： Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using FAN.UrlRouting;
using FAN.UrlRouting.Config;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace FAN.WebSite.Code
{
    public class UrlRoutingBus
    {
        public static void RegisterRoutes(RouteCollection routeCollection)
        {
            foreach (UrlRoutingSetting urlRoutingSetting in UrlRoutingSettingConfig.UrlRoutingSettingCollection)
            {
                if (!string.IsNullOrWhiteSpace(urlRoutingSetting.DomainName))
                {
                    RouteTable.Routes.Add(new DomainRoute1(urlRoutingSetting.DomainName, urlRoutingSetting.RouteUrl, urlRoutingSetting.PhysicalFile, urlRoutingSetting.CheckPhysicalUrlAccess, urlRoutingSetting.Defaults, urlRoutingSetting.Constraints));
                }
                else
                {
                    if (urlRoutingSetting.RouteName.Equals("ajax", StringComparison.CurrentCultureIgnoreCase))
                    {
                        RouteTable.Routes.Add(new Route(urlRoutingSetting.RouteUrl, urlRoutingSetting.Defaults, urlRoutingSetting.Constraints, new HttpHandlerRoute(urlRoutingSetting.PhysicalFile)));
                    }
                    else
                    {
                        RouteTable.Routes.MapPageRoute(urlRoutingSetting.RouteName, urlRoutingSetting.RouteUrl, urlRoutingSetting.PhysicalFile, true, urlRoutingSetting.Defaults, urlRoutingSetting.Constraints);
                    }
                }
            }
        }

        public static string GetParameter(HttpRequest request,string parameterName)
        {
            string parameterValue = null;
            RouteData routeData = request.RequestContext.RouteData;
           var testPath = routeData.Route.GetVirtualPath(request.RequestContext, null); 
            if (routeData.Values[parameterName]!=null)
            {
                parameterValue = routeData.GetRequiredString(parameterName);
            }
            if (string.IsNullOrWhiteSpace(parameterValue)&&!string.IsNullOrWhiteSpace(request.QueryString[parameterName]))
            {
                parameterValue = request.QueryString[parameterName];
            }
            return parameterValue;
        }
        /// <summary>
        /// 构建Url地址栏参数的方法(id=123&name=fan)
        /// </summary>
        /// <param name="parameters">打开页面请求的参数</param>
        /// <returns>id=123&name=fan</returns>
        public static string BuildQueryString(NameValueCollection parameters, bool isLower = true)
        {
            List<string> pairs = new List<string>();
            foreach (string key in parameters.Keys)
            {
                string value = parameters[key];
                if (!string.IsNullOrEmpty(value))
                {
                    pairs.Add(string.Format("{0}={1}", Uri.EscapeDataString(key.Trim().ToLower()), Microsoft.Security.Application.Encoder.UrlEncode(isLower ? value.ToLower().Trim() : value.Trim())));
                }
            }
            return string.Join("&", pairs.ToArray());
        }
        /// <summary>
        /// 获取VirtualPath
        /// </summary>
        /// <param name="request"></param>
        /// <param name="routeName"></param>
        /// <param name="routeParameters"></param>
        /// <returns></returns>
        private static string GetRouteUrl(HttpRequest request, string routeName, RouteValueDictionary routeParameters)
        {
            string path = null;
            VirtualPathData data = RouteTable.Routes.GetVirtualPath(request.RequestContext, routeName, routeParameters);
            if (data != null)
            {
                path = data.VirtualPath;
            }
            if (!string.IsNullOrWhiteSpace(path))
            {
                //处理URL
                if (path.IndexOf("?") == -1 && path.IndexOf(".html") == -1)
                {
                    path = path + "/";
                }
            }
            return path;
        }


        public static string GetHomeUrl(HttpRequest request, string culture)
        {
            string url = null;
            if (string.IsNullOrWhiteSpace(culture) || culture == "en")
            {
                url = GetRouteUrl(request, "index", null);
            }
            else
            {
                RouteValueDictionary dict = new RouteValueDictionary { { culture, culture } };
                url = GetRouteUrl(request, "index_culture", dict);
            }
            url = ConfigSetting.WEB_SITE_HOST + url;
            return url;
        }


    }
}