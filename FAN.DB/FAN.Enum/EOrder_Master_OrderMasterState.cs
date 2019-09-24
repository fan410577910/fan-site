using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    public enum EOrder_Master_OrderMasterState
    {
        /// <summary>
        /// 等待付款
        /// </summary>
        [Description("等待付款")]
        Awaiting_Payment = 1,
        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        Completed_Payment = 2,
        /// <summary>
        /// 备货期
        /// </summary>
        [Description("备货期")]
        Preparing = 3,
        /// <summary>
        /// 部分发货
        /// </summary>
        [Description("部分发货")]
        Partly_Shipped = 4,
        /// <summary>
        /// 已发货
        /// </summary>
        [Description("已发货")]
        Shipped = 16,
        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消")]
        Cancelled = 20,
        /// <summary>
        /// 待审核付款
        /// </summary>
        [Description("待审核付款")]
        Verifying_Payment = 22,
        /// <summary>
        /// 已付款未确定
        /// </summary>
        [Description("已付款未确定")]
        Unconfirmed_Payment = 23,
        /// <summary>
        /// 已完成订单
        /// </summary>
        [Description("已完成订单")]
        Completed = 19,
        /// <summary>
        /// 等待评价
        /// </summary>
        [Description("等待评价")]
        Awaiting_Evaluation = 18
    }
}
