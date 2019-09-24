using System.ComponentModel;

namespace FAN.Enum
{
    /// <summary>
    /// 购物车枚举状态
    /// </summary>
    public enum EOrder_ShopCart_State
    {

        /// <summary>
        /// 无效（已删除-伪删除）
        /// </summary>
        [Description("无效")]
        无效 = 0,

        /// <summary>
        /// 有效（正常产品）
        /// </summary>
        [Description("有效")]
        有效 = 1
    }
}
