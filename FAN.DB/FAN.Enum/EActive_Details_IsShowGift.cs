#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2017 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：TIDEBUY-YANG 
     * 文件名：  EActive_Details_IsShowGift 
     * 版本号：  V1.0.0.0 
     * 创建人：  Administrator 
     * 创建时间： 2017/7/5 16:36:45 
     * 描述    :
     * =====================================================================
     * 修改时间：2017/7/5 16:36:45 
     * 修改人  ： Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion

using System.ComponentModel;

namespace FAN.Enum
{
    /// <summary>
    /// 赠品活动是否前台是否显示赠品子单
    /// </summary>
    public enum EActive_Details_IsShowGift
    {
        /// <summary>
        ///隐藏
        /// </summary>
        [Description("隐藏")]
        隐藏 = 0,
        /// <summary>
        /// 显示
        /// </summary>
        [Description("显示")]
        显示 = 1
    }
}
