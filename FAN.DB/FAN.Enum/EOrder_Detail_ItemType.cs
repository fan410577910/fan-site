using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    [Obsolete("已过期，请使用枚举：EOrder_Shopcart_ItemType", true)]
    public enum EOrder_Detail_ItemType
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        正常 = 1,
        /// <summary>
        /// 赠品
        /// </summary>
        [Description("赠品")]
        赠品 = 2,
        /// <summary>
        /// 加价购
        /// </summary>
        [Description("加价购")]
        加价购 = 3,
        /// <summary>
        /// 加价购
        /// </summary>
        [Description("每日特价")]
        每日特价 = 4,
        /// <summary>
        /// 闪购
        /// </summary>
        [Description("闪购")]
        闪购 = 5
    }
}
