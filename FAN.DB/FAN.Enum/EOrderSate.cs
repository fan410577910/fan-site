using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    /// <summary>
    /// 订单状态 
    /// </summary>
    public enum EOrderSate
    {
        全部 = -1,
        等待付款 = 1,
        已付款 = 2,
        备货期 = 3,
        正在补货 = 14,
        等待发货 = 15,
        已发货 = 16,
        等待确认收货 = 17,
        等待评价 = 18,
        已完成订单 = 19,
        取消订单 = 20,
        争议订单 = 21,
        待审核付款 = 22,
        已付款未确定 = 23,
        加急备货中订单 = 24,
        加急已付款订单 = 25,
        已退款 = 26
    }
}
