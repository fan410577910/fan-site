#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  EActive_List_SettingType 
     * 版本号：  V1.0.0.0 
     * 创建人：  Administrator 
     * 创建时间：2014/12/31 18:14:30 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/12/31 18:14:30 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.ComponentModel;

namespace FAN.Enum
{
    /// <summary>
    /// 活动规则设置-商品设置选择（此枚举只做显示设置时使用，不存储在数据库中）wyp
    /// </summary>
    public enum EActive_List_SettingType
    {
        /// <summary>
        /// 分类
        /// </summary>
        [Description("分类")]
        分类 = 0,
        /// <summary>
        /// 产品
        /// </summary>
        [Description("产品")]
        产品 = 1
    }
}
