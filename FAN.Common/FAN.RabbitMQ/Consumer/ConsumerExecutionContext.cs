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
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace FAN.RabbitMQ
{
    public class ConsumerExecutionContext
    {
        public Func<byte[], MessageProperties, MessageReceivedInfo, Task> OnMessageHandler { get; private set; }
        public MessageReceivedInfo Info { get; private set; }
        public MessageProperties Properties { get; private set; }
        public byte[] Body { get; private set; }
        public IBasicConsumer Consumer { get; private set; }

        public ConsumerExecutionContext(
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessageHandler,
            MessageReceivedInfo info,
            MessageProperties properties,
            byte[] body,
            IBasicConsumer consumer)
        {
            Preconditions.CheckNotNull(onMessageHandler, "userHandler");
            Preconditions.CheckNotNull(info, "info");
            Preconditions.CheckNotNull(properties, "properties");
            Preconditions.CheckNotNull(body, "body");
            Preconditions.CheckNotNull(consumer, "consumer");

            OnMessageHandler = onMessageHandler;
            Info = info;
            Properties = properties;
            Body = body;
            Consumer = consumer;
        }
    }
}
