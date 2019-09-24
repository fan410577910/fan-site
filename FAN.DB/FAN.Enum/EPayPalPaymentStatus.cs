
namespace FAN.Enum
{
    /// <summary>
    /// PayPay支付返回的状态类型
    /// </summary>
    public enum EPayPalPaymentStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 失败
        /// </summary>
        Failure = 2,
        /// <summary>
        /// 10486重定向
        /// </summary>
        Redirect = 3
    }
}
