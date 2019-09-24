using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{

    public enum EActive_List_IsExistCouponActive
    {
        /// <summary>
        /// 不包含
        /// </summary>
        [Description("不包含")]
        不包含 = 0,
        /// <summary>
        /// 包含
        /// </summary>
        [Description("包含")]
        包含 = 1
    }

    public enum EActive_Order_IsCreateOrder
    {
        /// <summary>
        /// 不执行
        /// </summary>
        [Description("不执行")]
        不执行 = 0,
        /// <summary>
        /// 执行
        /// </summary>
        [Description("执行")]
        执行 = 1
    }
}
