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
    /// <summary>
    /// 短暂的消费者
    /// </summary>
    public class TransientConsumer : IConsumer, IDisposable
    {
        private readonly IQueue _queue;
        private readonly Func<Byte[], MessageProperties, MessageReceivedInfo, Task> _onMessage;
        private readonly PersistentConnection _connection;
        private readonly ConsumerConfiguration _configuration;
        private readonly InternalConsumerFactory _internalConsumerFactory;

        private InternalConsumer _internalConsumer;

        public TransientConsumer(
            IQueue queue,
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage,
            PersistentConnection connection,
            ConsumerConfiguration configuration,
            InternalConsumerFactory internalConsumerFactory)
        {
            Preconditions.CheckNotNull(queue, "queue");
            Preconditions.CheckNotNull(onMessage, "onMessage");
            Preconditions.CheckNotNull(connection, "connection");
            Preconditions.CheckNotNull(internalConsumerFactory, "internalConsumerFactory");
            Preconditions.CheckNotNull(configuration, "configuration");

            this._queue = queue;
            this._onMessage = onMessage;
            this._connection = connection;
            this._configuration = configuration;
            this._internalConsumerFactory = internalConsumerFactory;
        }

        public IDisposable StartConsuming()
        {
            this._internalConsumer = this._internalConsumerFactory.CreateConsumer();

            this._internalConsumer.Cancelled += consumer => Dispose();

            this._internalConsumer.StartConsuming(
                this._connection,
                this._queue,
                this._onMessage,
                this._configuration);

            return new ConsumerCancellation(Dispose);
        }

        private bool _disposed;

        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }
            this._disposed = true;

            EventBus.Instance.Publish(new StoppedConsumingEvent(this));
            if (this._internalConsumer != null)
            {
                this._internalConsumer.Dispose();
            }
        }
    }
}
