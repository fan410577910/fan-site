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
    /// This event is fired when the logical consumer stops consuming.
    /// 
    /// This is _not_ fired when a connection interruption causes RabbitMQ to re-create
    /// a PersistentConsumer.
    /// </summary>
    public class StoppedConsumingEvent
    {
        public IConsumer Consumer { get; private set; }

        public StoppedConsumingEvent(IConsumer consumer)
        {
            Consumer = consumer;
        }
    }
}
