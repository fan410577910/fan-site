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

namespace FAN.RabbitMQ
{
    public enum AckResult
    {
        Ack,
        Nack,
        Exception,
        Nothing
    }

    public delegate AckResult AckStrategy(IModel model, ulong deliveryTag);

    public static class AckStrategies
    {
        public static AckStrategy Ack = (model, tag) => { model.BasicAck(tag, false); return AckResult.Ack; };
        public static AckStrategy NackWithoutRequeue = (model, tag) => { model.BasicNack(tag, false, false); return AckResult.Nack; };
        public static AckStrategy NackWithRequeue = (model, tag) => { model.BasicNack(tag, false, true); return AckResult.Nack; };
        public static AckStrategy Nothing = (model, tag) => AckResult.Nothing;
    }

    public class AckEvent
    {
        public ConsumerExecutionContext ConsumerExecutionContext { get; private set; }
        public AckResult AckResult { get; private set; }

        public AckEvent(ConsumerExecutionContext consumerExecutionContext, AckResult ackResult)
        {
            ConsumerExecutionContext = consumerExecutionContext;
            AckResult = ackResult;
        }
    }
}
