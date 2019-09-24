using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    public enum EOrder_Detail_OrderDetailState
    {
        /// <summary>
        /// 等待付款
        /// </summary>
        [Description("Awaiting Payment")]
        Awaiting_Payment = 1,
        /// <summary>
        /// 已付款
        /// </summary>
        [Description("Paid")]
        Paid = 2,
        /// <summary>
        /// 取消
        /// </summary>
        [Description("Cancelled")]
        Cancelled = 41,
        /// <summary>
        /// 待审核付款
        /// </summary>
        [Description("Payment Awaiting Verification")]
        Payment_Awaiting_Verification = 22,
        /// <summary>
        /// 已付款未确定
        /// </summary>
        [Description("Unconfirmed Payment")]
        Unconfirmed_Payment = 23,
        /// <summary>
        /// 备货期
        /// </summary>
        [Description("Preparing")]
        Preparing = 3,
        /// <summary>
        /// 备货中（质检）
        /// </summary>
        [Description("(Supervising Quality)")]
        Preparing_Supervising_Quality = 4,
        /// <summary>
        /// 备货中（制包）
        /// </summary>
        [Description("Preparing(Being Packaged)")]
        Preparing_Being_Packaged = 5,
        /// <summary>
        /// 等待发货
        /// </summary>
        [Description("Ready for Delivery")]
        Ready_for_Delivery = 15,
        /// <summary>
        /// 已发货
        /// </summary>
        [Description("Shipped")]
        Shipped = 16,
        /// <summary>
        /// 等待确认收货
        /// </summary>
        [Description("Awaiting Receipt Confirmation")]
        Awaiting_Receipt_Confirmation = 17,
        /// <summary>
        /// 等待评价
        /// </summary>
        [Description("Awaiting Comment")]
        Awaiting_Comment = 18,
        /// <summary>
        /// 已完成订单
        /// </summary>
        [Description("Completed")]
        Completed = 19,
        /// <summary>
        /// 加急备货中订单
        /// </summary>
        [Description("Expedited Preparing Order")]
        Expedited_Preparing_Order = 24,
        /// <summary>
        /// 加急已付款订单
        /// </summary>
        [Description("Expedited Paid Order")]
        Expedited_Paid_Order = 25,        
        /// <summary>
        /// 全额退款
        /// </summary>
        [Description("Full Refund")]
        Full_Refund = 26,
        /// <summary>
        /// 无法采购
        /// </summary>
        [Description("Unable to Purchase")]
        Unable_to_Purchase = 27,
        /// <summary>
        /// 等件区
        /// </summary>
        [Description("Waiting Zone")]
        Waiting_Zone = 28,
        /// <summary>
        /// 已入库
        /// </summary>
        [Description("In Storage")]
        In_Storage = 29,
        /// <summary>
        /// 部分退款
        /// </summary>
        [Description("Partial Refund")]
        Partial_Refund = 31
    }
}
