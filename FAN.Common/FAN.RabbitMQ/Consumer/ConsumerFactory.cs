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
using System.Collections.Concurrent;
using System.Threading.Tasks;
using FAN.RabbitMQ.Topology;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 消费者工厂，用来创建一个消费者。
    /// </summary>
    public class ConsumerFactory : IDisposable
    {
        private readonly InternalConsumerFactory _internalConsumerFactory;

        private readonly ConcurrentDictionary<IConsumer, object> _consumers = new ConcurrentDictionary<IConsumer, object>();

        public ConsumerFactory(InternalConsumerFactory internalConsumerFactory)
        {
            Preconditions.CheckNotNull(internalConsumerFactory, "internalConsumerFactory");

            this._internalConsumerFactory = internalConsumerFactory;

            EventBus.Instance.Subscribe<StoppedConsumingEvent>(stoppedConsumingEvent =>
            {
                object value;
                this._consumers.TryRemove(stoppedConsumingEvent.Consumer, out value);
            });
        }
        /// <summary>
        /// 创建一个消费者
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="onMessage"></param>
        /// <param name="connection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public IConsumer CreateConsumer(
            IQueue queue,
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage,
            PersistentConnection connection,
            ConsumerConfiguration configuration
            )
        {
            Preconditions.CheckNotNull(queue, "queue");
            Preconditions.CheckNotNull(onMessage, "onMessage");
            Preconditions.CheckNotNull(connection, "connection");

            var consumer = this.CreateConsumerInstance(queue, onMessage, connection, configuration);
            this._consumers.TryAdd(consumer, null);
            return consumer;
        }

        /// <summary>
        /// Create the correct implementation of IConsumer based on queue properties
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="onMessage"></param>
        /// <param name="connection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private IConsumer CreateConsumerInstance(
            IQueue queue,
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage,
            PersistentConnection connection,
            ConsumerConfiguration configuration)
        {
            if (queue.IsExclusive)
            {
                return new TransientConsumer(queue, onMessage, connection, configuration, this._internalConsumerFactory);
            }

            return new PersistentConsumer(queue, onMessage, connection, configuration, this._internalConsumerFactory);
        }

        public void Dispose()
        {
            foreach (var consumer in this._consumers.Keys)
            {
                consumer.Dispose();
            }
            this._internalConsumerFactory.Dispose();
        }
    }
}
