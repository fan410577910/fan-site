#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  UrlRoutingSettingConfig 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan 
     * 创建时间：2014/11/5 11:29:16 
     * 描述    : 获取自定义板块配置节信息
     * =====================================================================
     * 修改时间：2014/11/5 11:29:16 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Routing;
using FAN.Helper;

namespace FAN.UrlRouting.Config
{
    /// <summary>
    /// 获取自定义板块配置节信息
    /// </summary>
    public static class UrlRoutingSettingConfig
    {
        private static UrlRoutingSettingCollection _UrlRoutingSettingCollection;
        /// <summary>
        /// 自定义配置节信息集合
        /// </summary>
        public static UrlRoutingSettingCollection UrlRoutingSettingCollection
        {
            get { return UrlRoutingSettingConfig._UrlRoutingSettingCollection; }
            set { UrlRoutingSettingConfig._UrlRoutingSettingCollection = value; }
        }

        static UrlRoutingSettingConfig()
        {
            const string SETTINGS = "urlRoutingSettings";
            object section = null;
            if (System.Web.Hosting.HostingEnvironment.IsHosted)
            {
                //Web.config
                section = ConfigurationManager.GetSection(SETTINGS);
            }
            else
            {
                //App.config
                section = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).Sections[SETTINGS];
            }
            UrlRoutingSettingConfigSection settingConfigSection = null;
            if (section is UrlRoutingSettingConfigSection)
            {
                settingConfigSection = section as UrlRoutingSettingConfigSection;
            }
            if (settingConfigSection == null)
            {
                throw new Exception("请在Config文件中配置<configSections><section name=\"urlRoutingSettings\" type=\"TLZ.UrlRouting.Config.UrlRoutingSettingConfigSection, TLZ.UrlRouting\"/></configSections>");
            }
            UrlRoutingSettingConfig._UrlRoutingSettingCollection = new UrlRoutingSettingCollection();
            foreach (UrlRoutingSettingConfigElement settingConfigElement in settingConfigSection.Settings)
            {
                UrlRoutingSetting urlRoutingSetting = new UrlRoutingSetting(
                    settingConfigElement.DomainName
                    , settingConfigElement.RouteName
                    , settingConfigElement.RouteUrl
                    , settingConfigElement.PhysicalFile
                    , settingConfigElement.CheckPhysicalUrlAccess
                    , settingConfigElement.Defaults
                    , settingConfigElement.Constraints
                );
                UrlRoutingSettingConfig._UrlRoutingSettingCollection.Add(urlRoutingSetting);
            }
        }
    }
}
