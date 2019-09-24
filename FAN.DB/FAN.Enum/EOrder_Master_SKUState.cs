using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    public enum EOrder_Master_SKUState
    {
        /// <summary>
        /// 全部失效
        /// </summary>
        [Description("全部失效")]
        全部失效 = -1,
        /// <summary>
        /// 全部失效
        /// </summary>
        [Description("部分失效")]
        部分失效 = 0,
        /// <summary>
        /// 未失效
        /// </summary>
        [Description("未失效")]
        未失效 = 1
    }
}
