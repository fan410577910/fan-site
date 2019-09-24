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
using FAN.RabbitMQ.Topology;

namespace FAN.RabbitMQ
{
    partial class RabbitAdvancedBus
    {
        #region publish 发布消息
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        public void Publish(string exchange, string routingKey, byte[] body)
        {
            this.Publish(exchange, routingKey, null, body);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="messageProperties"></param>
        /// <param name="body"></param>
        public void Publish(string exchange, string routingKey, MessageProperties messageProperties, byte[] body)
        {
            this.Publish(exchange, routingKey, false, false, messageProperties, body);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        public void Publish(IExchange exchange, string routingKey, byte[] body)
        {
            this.Publish(exchange, routingKey, null, body);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="messageProperties"></param>
        /// <param name="body"></param>
        public void Publish(IExchange exchange, string routingKey, MessageProperties messageProperties, byte[] body)
        {
            this.Publish(exchange, routingKey, false, false, messageProperties, body);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="mandatory">当mandatory标志位设置为true时，如果exchange根据自身类型和消息routeKey无法找到一个符合条件的queue，那么会调用basic.return方法将消息返还给生产者；当mandatory设为false时，出现上述情形broker会直接将消息扔掉。</param>
        /// <param name="immediate">当immediate标志位设置为true时，如果exchange在将消息route到queue(s)时发现对应的queue上没有消费者，那么这条消息不会放入队列中。当与消息routeKey关联的所有queue(一个或多个)都没有消费者时，该消息会通过basic.return方法返还给生产者。</param>
        /// <param name="messageProperties"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public void Publish(IExchange exchange, string routingKey, bool mandatory, bool immediate, MessageProperties messageProperties, byte[] body)
        {
            Preconditions.CheckNotNull(exchange, "exchange");
            this.Publish(exchange.Name, routingKey, mandatory, immediate, messageProperties, body);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="mandatory">当mandatory标志位设置为true时，如果exchange根据自身类型和消息routeKey无法找到一个符合条件的queue，那么会调用basic.return方法将消息返还给生产者；当mandatory设为false时，出现上述情形broker会直接将消息扔掉。</param>
        /// <param name="immediate">当immediate标志位设置为true时，如果exchange在将消息route到queue(s)时发现对应的queue上没有消费者，那么这条消息不会放入队列中。当与消息routeKey关联的所有queue(一个或多个)都没有消费者时，该消息会通过basic.return方法返还给生产者。</param>
        /// <param name="messageProperties"></param>
        /// <param name="body"></param>
        public void Publish(string exchange, string routingKey, bool mandatory, bool immediate, MessageProperties messageProperties, byte[] body)
        {
            try
            {
                this.PublishAsync(exchange, routingKey, mandatory, immediate, messageProperties, body);
            }
            catch (AggregateException aggregateException)
            {
                throw aggregateException.InnerException;
            }
        }
        /// <summary>
        /// 发布消息
        /// http://www.sjsjw.com/kf_jiagou/article/023563ABA032194.asp
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="mandatory">当mandatory标志位设置为true时，如果exchange根据自身类型和消息routeKey无法找到一个符合条件的queue，那么会调用basic.return方法将消息返还给生产者；当mandatory设为false时，出现上述情形broker会直接将消息扔掉。</param>
        /// <param name="immediate">当immediate标志位设置为true时，如果exchange在将消息route到queue(s)时发现对应的queue上没有消费者，那么这条消息不会放入队列中。当与消息routeKey关联的所有queue(一个或多个)都没有消费者时，该消息会通过basic.return方法返还给生产者。</param>
        /// <param name="messageProperties"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private void PublishAsync(string exchange, string routingKey, bool mandatory, bool immediate, MessageProperties messageProperties, byte[] body)
        {
            Preconditions.CheckNotNull(exchange, "exchange");
            Preconditions.CheckShortString(routingKey, "routingKey");
            Preconditions.CheckNotNull(body, "body");

            this._clientCommandDispatcher.Invoke(x =>
            {
                IBasicProperties basicProperties = null;
                if (messageProperties != null)
                {
                    basicProperties = x.CreateBasicProperties();
                    messageProperties.CopyTo(basicProperties);
                }
                this._publisher.Publish(x, body, messageProperties, (m, b, p) => m.BasicPublish(exchange, routingKey, mandatory, immediate, basicProperties, body));
            }).Wait();

            ConsoleLogger.DebugWrite("Published to exchange: '{0}', routing key: '{1}', correlationId: '{2}'", exchange, routingKey, messageProperties == null ? "null" : messageProperties.CorrelationId);
        }
        #endregion
    }
}
