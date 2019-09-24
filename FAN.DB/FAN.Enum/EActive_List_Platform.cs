#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：YANGHUANWEN 
     * 文件名：  EActive_List_Platform 
     * 版本号：  V1.0.0.0 
     * 创建人：  杨焕文 
     * 创建时间：2014/12/2 11:44:21 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/12/2 11:44:21 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.ComponentModel;

namespace FAN.Enum
{
    /// <summary>
    /// 活动使用平台枚举
    /// </summary>
    [System.Obsolete("过期了,改用EPlatform枚举")]
    public enum EActive_List_Platform
    {
        /// <summary>
        /// 按金额包邮
        /// </summary>
        [Description("PC端")]
        PC端 = 1,
        /// <summary>
        /// 按运费打折
        /// </summary>
        [Description("M端")]
        M端 = 2,
        /// <summary>
        /// 按产品打折
        /// </summary>
        [Description("APP")]
        APP = 3
    }
}
