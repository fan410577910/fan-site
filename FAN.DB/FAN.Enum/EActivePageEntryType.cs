#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2016 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-JLFFS1KMVG 
     * 文件名：  EActivePageEntryType 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间： 2016/6/7 14:45:24 
     * 描述    :
     * =====================================================================
     * 修改时间：2016/6/7 14:45:24 
     * 修改人  ： wangyunpeng
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion

namespace FAN.Enum
{
    /// <summary>
    /// 进入活动计算的入口页面类型(从哪个页面进入到活动计算的)
    /// </summary>
    public enum EActivePageEntryType
    {
        /// <summary>
        /// 购物车页面
        /// </summary>
        ShopCartPage,
        /// <summary>
        /// 快捷支付页面（PayPal）
        /// </summary>
        IsFastPaypalPage,
        /// <summary>
        /// 地址页面
        /// </summary>
        AddressPage
    }
}
