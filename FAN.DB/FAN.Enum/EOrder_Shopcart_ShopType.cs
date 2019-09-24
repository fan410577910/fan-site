using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    public enum EOrder_Shopcart_ShopType
    {
        //购买方式枚举：（1：添加购物车， 2：立即购买）
        /// <summary>
        /// 添加购物车
        /// </summary>
        [Description("添加购物车")]
        添加购物车 = 0,
        /// <summary>
        /// 立即购买
        /// </summary>
        [Description("立即购买")]
        立即购买 = 1,
        /// <summary>
        /// 活动添加赠品
        /// </summary>
        [Description("活动添加赠品")]
        活动添加赠品 = 2

    }
}
