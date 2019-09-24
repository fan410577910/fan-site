
using System.ComponentModel;

namespace FAN.Enum
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum EOrder_Type
    {
        /// <summary>
        ///主单
        /// </summary>
        [Description("主单")]
        主单 = 0,
        /// <summary>
        /// 子单
        /// </summary>
        [Description("子单")]
        子单 = 1
    }
}
