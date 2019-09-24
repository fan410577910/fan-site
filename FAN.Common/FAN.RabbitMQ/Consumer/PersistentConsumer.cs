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
using System.Collections.Generic;
using System.Threading.Tasks;
using FAN.RabbitMQ.Topology;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 持久的消费者
    /// </summary>
    public class PersistentConsumer : IConsumer, IDisposable
    {
        private readonly IQueue _queue;
        private readonly Func<Byte[], MessageProperties, MessageReceivedInfo, Task> _onMessage;
        private readonly PersistentConnection _connection;
        private readonly ConsumerConfiguration _configuration;
        private readonly InternalConsumerFactory _internalConsumerFactory;

        private readonly ConcurrentDictionary<InternalConsumer, object> _internalConsumers = new ConcurrentDictionary<InternalConsumer, object>();

        private readonly List<CancelSubscription> _eventCancellations = new List<CancelSubscription>();

        public PersistentConsumer(
            IQueue queue,
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage,
            PersistentConnection connection,
            ConsumerConfiguration configuration,
            InternalConsumerFactory internalConsumerFactory)
        {
            Preconditions.CheckNotNull(queue, "queue");
            Preconditions.CheckNotNull(onMessage, "onMessage");
            Preconditions.CheckNotNull(connection, "connection");
            Preconditions.CheckNotNull(configuration, "configuration");
            Preconditions.CheckNotNull(internalConsumerFactory, "internalConsumerFactory");

            this._queue = queue;
            this._onMessage = onMessage;
            this._connection = connection;
            this._configuration = configuration;
            this._internalConsumerFactory = internalConsumerFactory;
        }

        /// <summary>
        /// 开始接受消息
        /// </summary>
        /// <returns></returns>
        public IDisposable StartConsuming()
        {
            this._eventCancellations.Add(EventBus.Instance.Subscribe<ConnectionCreatedEvent>(e => this.ConnectionOnConnected()));
            this._eventCancellations.Add(EventBus.Instance.Subscribe<ConnectionDisconnectedEvent>(e => this.ConnectionOnDisconnected()));

            this.StartConsumingInternal();

            return new ConsumerCancellation(this.Dispose);
        }

        private void StartConsumingInternal()
        {
            if (this._disposed)
            {
                return;
            }
            if (!this._connection.IsConnected)
            {
                return;
            }

            InternalConsumer internalConsumer = this._internalConsumerFactory.CreateConsumer();
            this._internalConsumers.TryAdd(internalConsumer, null);

            internalConsumer.Cancelled += consumer => this.Dispose();

            internalConsumer.StartConsuming(this._connection, this._queue, this._onMessage, this._configuration);
        }

        private void ConnectionOnDisconnected()
        {
            this._internalConsumerFactory.OnDisconnected();
            this._internalConsumers.Clear();
        }

        private void ConnectionOnConnected()
        {
            this.StartConsumingInternal();
        }

        private bool _disposed = false;

        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }

            this._disposed = true;

            EventBus.Instance.Publish(new StoppedConsumingEvent(this));

            foreach (var cancelSubscription in this._eventCancellations)
            {
                cancelSubscription();
            }

            foreach (var internalConsumer in this._internalConsumers.Keys)
            {
                internalConsumer.Dispose();
            }
        }
    }
}
