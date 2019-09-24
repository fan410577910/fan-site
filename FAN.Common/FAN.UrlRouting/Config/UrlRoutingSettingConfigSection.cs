#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  UrlRoutingSettingConfigSection 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan 
     * 创建时间：2014/11/5 11:27:32 
     * 描述    : 自定义板块配置节设置
     * =====================================================================
     * 修改时间：2014/11/5 11:27:32 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.Configuration;

namespace FAN.UrlRouting.Config
{
    /// <summary>
    /// 自定义板块配置节设置
    /// </summary>
    public sealed class UrlRoutingSettingConfigSection : ConfigurationSection
    {
        public UrlRoutingSettingConfigSection()
        {
        }

        [ConfigurationProperty("settings")]
        public UrlRoutingSettingConfigElementCollection Settings
        {
            get
            {
                return base["settings"] as UrlRoutingSettingConfigElementCollection;
            }
        }

        protected override void DeserializeSection(System.Xml.XmlReader reader)
        {
            base.DeserializeSection(reader);
        }

        protected override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
        {
            return base.SerializeSection(parentElement, name, saveMode);
        }
    }
}
