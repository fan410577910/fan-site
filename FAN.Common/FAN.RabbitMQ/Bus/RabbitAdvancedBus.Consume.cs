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
using System;
using System.Threading.Tasks;
using FAN.RabbitMQ.Topology;

namespace FAN.RabbitMQ
{
    partial class RabbitAdvancedBus
    {
        #region consume 接收消息
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="exchange">交换名称</param>
        /// <param name="onMessage">传递消费者接收消息之后要执行的方法</param>
        /// <returns></returns>
        public IDisposable Consume(string queue, string exchange, Action<byte[]> onMessage)
        {
            return this.Consume(queue, exchange, onMessage, x => { });
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="exchange">交换名称</param>
        /// <param name="onMessage">传递消费者接收消息之后要执行的方法</param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public IDisposable Consume(string queue, string exchange, Action<byte[]> onMessage, Action<SubscriptionConfiguration> configure)
        {
            Preconditions.CheckNotNull(queue, "queueName");
            Preconditions.CheckNotNull(exchange, "exchangeName");
            Preconditions.CheckNotNull(onMessage, "onMessage");
            Preconditions.CheckNotNull(configure, "configure");

            return this.Consume(queue, exchange, msg =>
            {
                var tcs = new TaskCompletionSource<object>();
                try
                {
                    onMessage(msg);
                    tcs.SetResult(null);
                }
                catch (Exception exception)
                {
                    tcs.SetException(exception);
                }
                return tcs.Task;
            },
            configure);
        }
        private IDisposable Consume(string queue, string exchange, Func<byte[], Task> onMessage, Action<SubscriptionConfiguration> configure)
        {
            Preconditions.CheckNotNull(queue, "queueName");
            Preconditions.CheckNotNull(exchange, "exchangeName");
            Preconditions.CheckNotNull(onMessage, "onMessage");
            Preconditions.CheckNotNull(configure, "configure");

            SubscriptionConfiguration subscriptionConfiguration = new SubscriptionConfiguration(this._connectionConfiguration.PrefetchCount);
            configure(subscriptionConfiguration);

            IQueue queueObj = this.QueueDeclare(queue, autoDelete: subscriptionConfiguration.AutoDelete);

            return this.Consume(
                queueObj,
                (message, messageReceivedInfo) => onMessage(message),
                x => x.WithPriority(subscriptionConfiguration.Priority)
                      .WithCancelOnHaFailover(subscriptionConfiguration.CancelOnHaFailover)
                      .WithPrefetchCount(subscriptionConfiguration.PrefetchCount));
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="onMessage">传递消费者接收消息之后要执行的方法</param>
        /// <returns></returns>
        public IDisposable Consume(IQueue queue, Action<byte[]> onMessage)
        {
            return this.Consume(queue, msg =>
            {
                var tcs = new TaskCompletionSource<object>();
                try
                {
                    onMessage(msg);
                    tcs.SetResult(null);
                }
                catch (Exception exception)
                {
                    tcs.SetException(exception);
                }
                return tcs.Task;
            });
        }

        private IDisposable Consume(IQueue queue, Func<byte[], Task> onMessage)
        {
            return this.Consume(
               queue,
               (message, messageReceivedInfo) => onMessage(message),
               x => { });
        }

        private IDisposable Consume(IQueue queue, Func<byte[], MessageReceivedInfo, Task> onMessage, Action<ConsumerConfiguration> configure)
        {
            Preconditions.CheckNotNull(queue, "queue");
            Preconditions.CheckNotNull(onMessage, "onMessage");
            Preconditions.CheckNotNull(configure, "configure");

            return this.Consume(queue, (message, properties, messageReceivedInfo) =>
            {//定义三个参数
                properties.ToString();
                return onMessage(message, messageReceivedInfo);
            }, configure);

        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="onMessage"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        private IDisposable Consume(IQueue queue, Func<Byte[], MessageProperties, MessageReceivedInfo, Task> onMessage, Action<ConsumerConfiguration> configure)
        {
            Preconditions.CheckNotNull(queue, "queue");
            Preconditions.CheckNotNull(onMessage, "onMessage");
            Preconditions.CheckNotNull(configure, "configure");

            if (this._disposed)
            {
                throw new Exception("This bus has been disposed");
            }
            ConsumerConfiguration consumerConfiguration = new ConsumerConfiguration(this._connectionConfiguration.PrefetchCount);
            configure(consumerConfiguration);
            IConsumer consumer = this._consumerFactory.CreateConsumer(queue, onMessage, this._connection, consumerConfiguration);
            return consumer.StartConsuming();
        }
        #endregion

    }
}
