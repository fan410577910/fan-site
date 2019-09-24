using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    /// <summary>
    /// 这里是把网站上的拷过来的，如果需要可以新增也可以修改名称(张山磊)
    /// 注意:如果这里新增也要在UserCenterBIZ.OrderTrackBIZ.GetOrderTrackDescription这个方法中新增描述信息
    /// </summary>
    public enum EOrderTrackStep
    {
        /// <summary>
        /// 生成订单
        /// </summary>
        [Description("生成订单")]
        生成订单,
        /// <summary>
        /// 待审核付款
        /// </summary>
        [Description("待审核付款")]
        待审核付款,
        /// <summary>
        /// 审核付款成功
        /// </summary>
        [Description("审核付款成功")]
        审核付款成功,
        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        已付款,
        /// <summary>
        /// 子订单确认收货
        /// </summary>
        [Description("子订单确认收货")]
        子订单确认收货,
        /// <summary>
        /// 订单确认收货
        /// </summary>
        [Description("订单确认收货")]
        订单确认收货,
        /// <summary>
        /// 客户选择PayPal支付方式
        /// </summary>
        [Description("客户选择PayPal支付方式")]
        客户选择PayPal支付方式,
        /// <summary>
        /// 客户选择Webmoney支付方式
        /// </summary>
        [Description("客户选择Webmoney支付方式")]
        客户选择Webmoney支付方式,
        /// <summary>
        /// 客户选择Moneybookers支付方式
        /// </summary>
        [Description("客户选择Moneybookers支付方式")]
        客户选择Moneybookers支付方式,
        /// <summary>
        /// 客户选择Ideal支付方式
        /// </summary>
        [Description("客户选择Ideal支付方式")]
        客户选择Ideal支付方式,
        /// <summary>
        /// 客户选择Qiwi支付方式
        /// </summary>
        [Description("客户选择Qiwi支付方式")]
        客户选择Qiwi支付方式,
        /// <summary>
        /// 客户选择Konbini支付方式
        /// </summary>
        [Description("客户选择Konbini支付方式")]
        客户选择Konbini支付方式,
        /// <summary>
        /// 客户选择Banktransfer支付方式
        /// </summary>
        [Description("客户选择Banktransfer支付方式")]
        客户选择Banktransfer支付方式,
        /// <summary>
        /// 客户选择Credit_Card支付方式
        /// </summary>
        [Description("客户选择Credit_Card支付方式")]
        客户选择Credit_Card支付方式,
        /// <summary>
        /// 客户选择Sofort支付方式
        /// </summary>
        [Description("客户选择Sofort支付方式")]
        客户选择Sofort支付方式,
        /// <summary>
        /// 客户选择Transferências_Bancária支付方式
        /// </summary>
        [Description("客户选择Transferências_Bancária支付方式")]
        客户选择Transferências_Bancária支付方式,
        /// <summary>
        /// 客户选择Bank_Transfer_Local支付方式
        /// </summary>
        [Description("客户选择Bank_Transfer_Local支付方式")]
        客户选择Bank_Transfer_Local支付方式,
        /// <summary>
        /// 客户选择Bank_Transfer支付方式
        /// </summary>
        [Description("客户选择Bank_Transfer支付方式")]
        客户选择Bank_Transfer支付方式,
        /// <summary>
        /// 客户选择Western_Union支付方式
        /// </summary>
        [Description("客户选择Western_Union支付方式")]
        客户选择Western_Union支付方式,
        /// <summary>
        /// 客户选择了Boleto_Bancário支付方式
        /// </summary>
        [Description("客户选择了Boleto_Bancário支付方式")]
        客户选择了Boleto_Bancário支付方式,
        /// <summary>
        /// 取消订单
        /// </summary>
        [Description("取消订单")]
        取消订单,
        /// <summary>
        /// 客户选择了FastPaypal支付方式
        /// </summary>
        [Description("客户选择了FastPaypal支付方式")]
        客户选择了FastPaypal支付方式,
        /// <summary>
        /// 客户选择了FastPaypal支付方式
        /// </summary>
        [Description("子订单发货")]
        子订单发货,
        /// <summary>
        /// 客户选择了FastPaypal支付方式
        /// </summary>
        [Description("部分发货")]
        部分发货,
        /// <summary>
        /// 备货期
        /// </summary>
        [Description("备货期")]
        备货期,
        /// <summary>
        /// 已发货
        /// </summary>
        [Description("已发货")]
        已发货,
        /// <summary>
        /// 质检合格
        /// </summary>
        [Description("质检合格")]
        质检合格,
        /// <summary>
        /// 制单
        /// </summary>
        [Description("制单")]
        制单
    }
}
