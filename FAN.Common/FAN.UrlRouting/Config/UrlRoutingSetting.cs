#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  UrlRoutingSetting 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan 
     * 创建时间：2014/11/5 11:33:32 
     * 描述    : Url路由信息
     * =====================================================================
     * 修改时间：2014/11/5 11:33:32 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion

using System;
using System.Web.Routing;
using FAN.Helper;
namespace FAN.UrlRouting.Config
{
    /// <summary>
    /// Url路由信息
    /// </summary>
    public sealed class UrlRoutingSetting
    {
        /// <summary>
        /// 泛域名名称
        /// </summary>
        public string DomainName { get; set; }
        /// <summary>
        /// 路由的名称随便起,但是要求唯一.
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 路由的 URL 模式
        /// </summary>
        public string RouteUrl { get; set; }
        /// <summary>
        /// 路由的物理 URL
        /// </summary>
        public string PhysicalFile { get; set; }
        /// <summary>
        /// 一个值，该值指示 ASP.NET 是否应验证用户是否有权访问物理 URL（始终会检查路由 URL）。此参数设置 System.Web.Routing.PageRouteHandler.CheckPhysicalUrlAccess
        /// </summary>
        public bool CheckPhysicalUrlAccess { get; set; }
        /// <summary>
        /// 路由的默认值
        /// </summary>
        public RouteValueDictionary Defaults { get; set; }
        /// <summary>
        /// 一些约束，URL 请求必须满足这些约束才能作为此路由处理。
        /// </summary>
        public RouteValueDictionary Constraints { get; set; }

        public UrlRoutingSetting()
        {

        }
        public UrlRoutingSetting(string domainName, string routeName, string routeUrl, string physicalFile, string checkPhysicalUrlAccess, string defaults, string constraints)
            : this(routeName, routeUrl, physicalFile, checkPhysicalUrlAccess, defaults, constraints)
        {
            this.DomainName = domainName;
        }
        public UrlRoutingSetting(string routeName, string routeUrl, string physicalFile, string checkPhysicalUrlAccess, string defaults, string constraints)
        {
            this.RouteName = routeName;
            this.RouteUrl = routeUrl;
            this.PhysicalFile = physicalFile;
            try
            {
                this.CheckPhysicalUrlAccess = TypeParseHelper.StrToBoolean(checkPhysicalUrlAccess);
                this.Defaults = string.IsNullOrEmpty(defaults) ? new RouteValueDictionary() : JsonHelper.ConvertStrToJson<RouteValueDictionary>(defaults);
                this.Constraints = string.IsNullOrEmpty(constraints) ? new RouteValueDictionary() : JsonHelper.ConvertStrToJson<RouteValueDictionary>(constraints); ;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                throw ex;
            }
        }
    }
}
