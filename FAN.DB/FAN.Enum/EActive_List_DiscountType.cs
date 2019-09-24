#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  EActive_List_DiscountType 
     * 版本号：  V1.0.0.0 
     * 创建人：  Administrator 
     * 创建时间：2014/11/26 18:31:12 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/11/26 18:31:12 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.ComponentModel;

namespace FAN.Enum
{
    /// <summary>
    /// 优惠种类，Active_List表的DiscountType列
    /// </summary>
    public enum EActive_List_DiscountType
    {
        /// <summary>
        /// 按金额包邮
        /// </summary>
        [Description("运费金额")]
        运费金额 = 1,
        /// <summary>
        /// 按运费打折
        /// </summary>
        [Description("运费折扣")]
        运费折扣 = 2,
        /// <summary>
        /// 按产品打折
        /// </summary>
        [Description("按产品打折")]
        按产品打折 = 3,
        /// <summary>
        /// 按产品减钱
        /// </summary>
        [Description("按产品减钱")]
        按产品减钱 = 4,
        /// <summary>
        /// 按产品数量定价
        /// </summary>
        [Description("按产品数量定总价")]
        按产品数量定总价 = 5,
        /// <summary>
        /// 按SKU赠送产品
        /// </summary>
        [Description("按SKU赠送产品")]
        按SKU赠送产品 = 6,
        /// <summary>
        /// 满M件送N件
        /// </summary>
        [Description("满M件送N件")]
        满M件送N件 = 7,
        /// <summary>
        /// 优惠券
        /// </summary>
        [Description("优惠券")]
        优惠券 = 8,
        /// <summary>
        /// 优惠券
        /// </summary>
        [Description("会员")]
        会员= 9
    }
}
