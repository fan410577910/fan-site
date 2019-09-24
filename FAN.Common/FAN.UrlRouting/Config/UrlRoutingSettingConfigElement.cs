#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  UrlRoutingSettingConfigElement 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan 
     * 创建时间：2014/11/5 11:15:00 
     * 描述    : Url路由配置元素
     * =====================================================================
     * 修改时间：2014/11/5 11:15:00 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.Configuration;
using System.Web.Routing;
using FAN.Helper;

namespace FAN.UrlRouting.Config
{
    /// <summary>
    /// Url路由配置元素
    /// </summary>
    public class UrlRoutingSettingConfigElement : ConfigurationElement
    {
        /// <summary>
        /// 泛域名名称
        /// </summary>
        [ConfigurationProperty("domainName")]
        public string DomainName
        {
            get { return (string)this["domainName"]; }
            set { this["domainName"] = value; }
        }
        /// <summary>
        /// 路由的名称随便起,但是要求唯一.
        /// </summary>
        [ConfigurationProperty("routeName")]
        public string RouteName
        {
            get { return (string)this["routeName"]; }
            set { this["routeName"] = value; }
        }
        /// <summary>
        /// 路由的 URL 模式
        /// </summary>
        [ConfigurationProperty("routeUrl")]
        public string RouteUrl
        {
            get { return (string)this["routeUrl"]; }
            set { this["routeUrl"] = value; }
        }
        /// <summary>
        /// 路由的物理 URL
        /// </summary>
        [ConfigurationProperty("physicalFile")]
        public string PhysicalFile
        {
            get { return (string)this["physicalFile"]; }
            set { this["physicalFile"] = value; }
        }
        /// <summary>
        /// 一个值，该值指示 ASP.NET 是否应验证用户是否有权访问物理 URL（始终会检查路由 URL）。此参数设置 System.Web.Routing.PageRouteHandler.CheckPhysicalUrlAccess
        /// </summary>
        [ConfigurationProperty("checkPhysicalUrlAccess")]
        public string CheckPhysicalUrlAccess
        {
            get
            {
                return (string)this["checkPhysicalUrlAccess"];
            }
            set { this["checkPhysicalUrlAccess"] = value; }
        }
        /// <summary>
        /// 路由的默认值
        /// </summary>
        [ConfigurationProperty("defaults")]
        public string Defaults
        {
            get
            {
                return (string)this["defaults"];
            }
            set { this["defaults"] = value; }
        }
        /// <summary>
        /// 一些约束，URL 请求必须满足这些约束才能作为此路由处理。
        /// </summary>
        [ConfigurationProperty("constraints")]
        public string Constraints
        {
            get
            {
                return (string)this["constraints"];
            }
            set { this["constraints"] = value; }
        }
    }
}
