#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2018 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-TI50KE6KO4 
     * 文件名：  ConfigSetting 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan
     * 创建时间： 2018/5/14 19:46:43 
     * 描述    :
     * =====================================================================
     * 修改时间：2018/5/14 19:46:43 
     * 修改人  ： Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using FAN.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAN.WebSite.Code
{
    public class ConfigSetting
    {
        /// <summary>
        /// 前台网站域名
        /// </summary>
        public static readonly string WEB_SITE_HOST = ConfigHelper.GetAppSettingValue("WEB_SITE_HOST");
        /// <summary>
        /// style项目域名
        /// </summary>
        public static readonly string STATIC_HOST = ConfigHelper.GetAppSettingValue("STATIC_HOST");
        /// <summary>
        /// style项目script路径
        /// </summary>
        public static readonly string STATICFILE_PATH_SCRIPT = ConfigHelper.GetAppSettingValue("STATICFILE_PATH_SCRIPT");
        /// <summary>
        /// style项目css路径
        /// </summary>
        public static readonly string STATICFILE_PATH_CSS = ConfigHelper.GetAppSettingValue("STATICFILE_PATH_CSS");

    }
}