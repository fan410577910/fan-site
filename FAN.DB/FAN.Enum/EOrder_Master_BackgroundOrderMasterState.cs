using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    public enum EOrder_Master_BackgroundOrderMasterState
    {
        /// <summary>
        /// 等待付款
        /// </summary>
        [Description("Awaiting Payment")]
        Awaiting_Payment = 1,
        /// <summary>
        /// 已付款
        /// </summary>
        [Description("Completing Payment")]
        Completing_Payment = 2,
        /// <summary>
        /// 取消
        /// </summary>
        [Description("Cancelled")]
        Cancelled = 20,
        /// <summary>
        /// 待审核付款
        /// </summary>
        [Description("Verifying Payment")]
        Verifying_Payment = 22,
        /// <summary>
        /// 已付款未确定
        /// </summary>
        [Description("Unconfirmed Payment")]
        Unconfirmed_Payment = 23,
        /// <summary>
        /// 已完成订单
        /// </summary>
        [Description("Completed")]
        Completed = 19
    }
}
