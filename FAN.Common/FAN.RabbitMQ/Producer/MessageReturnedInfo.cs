#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  RabbitHutch 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/2 
     * 描述    : RabbitMQ框架
     * =====================================================================
     * 修改时间：2014/7/2
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：调用RabbitMQ里面的功能都从这里面出
*/
#endregion

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 生产者发送消息（不带事务）到达服务器之后的返回信息。
    /// </summary>
    public class MessageReturnedInfo
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string ReturnReason { get; set; }

        public MessageReturnedInfo(
            string exchange,
            string routingKey,
            string returnReason)
        {
            Preconditions.CheckNotNull(exchange, "exchange");
            Preconditions.CheckNotNull(routingKey, "routingKey");
            Preconditions.CheckNotNull(returnReason, "returnReason");

            this.Exchange = exchange;
            this.RoutingKey = routingKey;
            this.ReturnReason = returnReason;
        }
    }
}
