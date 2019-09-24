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
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FAN.RabbitMQ.Topology;

namespace FAN.RabbitMQ
{
    public class InternalConsumer : IBasicConsumer, IDisposable
    {
        #region 变量
        private readonly HandlerRunner _handlerRunner;
        private readonly ConsumerDispatcher _consumerDispatcher;
        private readonly ConnectionConfiguration _connectionConfiguration;
        private Func<byte[], MessageProperties, MessageReceivedInfo, Task> _onMessage;
        private IQueue queue;
        #endregion

        #region 属性
        /// <summary>
        /// 目前就是个Guid
        /// </summary>
        public string ConsumerTag { get; private set; }
        #endregion

        #region 事件
        public event Action<InternalConsumer> Cancelled;
        #endregion

        #region 构造函数
        public InternalConsumer(HandlerRunner handlerRunner, ConsumerDispatcher consumerDispatcher, ConnectionConfiguration connectionConfiguration)
        {
            Preconditions.CheckNotNull(handlerRunner, "handlerRunner");
            Preconditions.CheckNotNull(consumerDispatcher, "consumerDispatcher");
            Preconditions.CheckNotNull(connectionConfiguration, "connectionConfiguration");

            this._handlerRunner = handlerRunner;
            this._consumerDispatcher = consumerDispatcher;
            this._connectionConfiguration = connectionConfiguration;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 收消息
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="queue"></param>
        /// <param name="onMessage"></param>
        /// <param name="configuration"></param>
        public void StartConsuming(PersistentConnection connection, IQueue queue, Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage, ConsumerConfiguration configuration)
        {
            Preconditions.CheckNotNull(connection, "connection");
            Preconditions.CheckNotNull(queue, "queue");
            Preconditions.CheckNotNull(onMessage, "onMessage");
            Preconditions.CheckNotNull(configuration, "configuration");

            this.queue = queue;
            this._onMessage = onMessage;
            var consumerTag = Conventions.ConsumerTagConvention();//目前就是一个Guid的值
            IDictionary<string, object> arguments = new Dictionary<string, object>
                {
                    {"x-priority", configuration.Priority},
                    {"x-cancel-on-ha-failover", configuration.CancelOnHaFailover || this._connectionConfiguration.CancelOnHaFailover}
                };
            try
            {
                this.Model = connection.CreateModel();

                this.Model.BasicQos(0, configuration.PrefetchCount, false);

                this.Model.BasicConsume(
                    queue.Name,         // queue
                    false,              // noAck
                    consumerTag,        // consumerTag
                    arguments,          // arguments
                    this);              // consumer

                ConsoleLogger.InfoWrite("Declared Consumer. queue='{0}', consumer tag='{1}' prefetchcount={2} priority={3} x-cancel-on-ha-failover={4}", queue.Name, consumerTag, _connectionConfiguration.PrefetchCount, configuration.Priority, configuration.CancelOnHaFailover);
            }
            catch (Exception exception)
            {
                ConsoleLogger.ErrorWrite("Consume failed. queue='{0}', consumer tag='{1}', message='{2}'", queue.Name, consumerTag, exception.Message);
            }
        }
        /// <summary>
        /// Cancel means that an external signal has requested that this consumer should
        /// be cancelled. This is _not_ the same as when an internal consumer stops consuming
        /// because it has lost its channel/connection.
        /// </summary>
        private void Cancel()
        {
            // copy to temp variable to be thread safe.
            var cancelled = this.Cancelled;
            if (cancelled != null)
            {
                cancelled(this);
            }

            var consumerCancelled = this.ConsumerCancelled;
            if (consumerCancelled != null)
            {
                consumerCancelled(this, new ConsumerEventArgs(this.ConsumerTag));
            }
        }
        #endregion

        #region IBasicConsumer
        public event ConsumerCancelledEventHandler ConsumerCancelled;

        public void HandleBasicCancel(string consumerTag)
        {
            this.Cancel();
            ConsoleLogger.InfoWrite("BasicCancel(Consumer Cancel Notification from broker) event received. " + "Consumer tag: " + consumerTag);
        }

        public void HandleBasicCancelOk(string consumerTag)
        {
            this.Cancel();
        }

        public void HandleBasicConsumeOk(string consumerTag)
        {
            this.ConsumerTag = consumerTag;
        }
        /// <summary>
        /// 消费者处理交付。IBasicConsumer接口自动完成调用。
        /// </summary>
        /// <param name="consumerTag"></param>
        /// <param name="deliveryTag"></param>
        /// <param name="redelivered"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="properties"></param>
        /// <param name="body"></param>
        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            ConsoleLogger.DebugWrite("HandleBasicDeliver on consumer: {0}, deliveryTag: {1}", consumerTag, deliveryTag);

            if (this._disposed)
            {
                // this message's consumer has stopped, so just return
                ConsoleLogger.InfoWrite("Consumer has stopped running. Consumer '{0}' on queue '{1}'. Ignoring message", this.ConsumerTag, queue.Name);
                return;
            }

            if (this._onMessage == null)
            {
                ConsoleLogger.ErrorWrite("User consumer callback, 'onMessage' has not been set for consumer '{0}'. Please call InternalConsumer.StartConsuming before passing the consumer to basic.consume", this.ConsumerTag);
                return;
            }

            var messageReceivedInfo = new MessageReceivedInfo(consumerTag, deliveryTag, redelivered, exchange, routingKey, queue.Name);
            var messsageProperties = new MessageProperties(properties);
            var context = new ConsumerExecutionContext(this._onMessage, messageReceivedInfo, messsageProperties, body, this);

            this._consumerDispatcher.QueueAction(() => this._handlerRunner.InvokeOnMessageHandler(context));
        }

        public void HandleModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            ConsoleLogger.InfoWrite("Consumer '{0}', consuming from queue '{1}', has shutdown. Reason: '{2}'", this.ConsumerTag, queue.Name, reason.Cause);
        }
        /// <summary>
        /// 获取通道（RabbitMQ里面的类型，就是通道的意思）
        /// </summary>
        public IModel Model { get; private set; }
        #endregion

        #region IDisposable
        private bool _disposed;
        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }
            this._disposed = true;

            var model = this.Model;
            if (model != null)
            {
                // Queued because we may be on the RabbitMQ.Client dispatch thread.
                this._consumerDispatcher.QueueAction(() =>
                {
                    this.Model.Dispose();
                    EventBus.Instance.Publish(new ConsumerModelDisposedEvent(this.ConsumerTag));
                });
            }
        }
        #endregion
    }
}
