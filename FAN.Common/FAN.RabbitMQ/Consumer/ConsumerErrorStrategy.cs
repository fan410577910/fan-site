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
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace FAN.RabbitMQ
{
    public interface IConsumerErrorStrategy : IDisposable
    {
        /// <summary>
        /// This method is fired when an exception is thrown. Implement a strategy for
        /// handling the exception here.
        /// </summary>
        /// <param name="context">The consumer execution context.</param>
        /// <param name="exception">The exception</param>
        /// <returns><see cref="AckStrategy"/> for processing the original failed message</returns>
        AckStrategy HandleConsumerError(ConsumerExecutionContext context, Exception exception);

        /// <summary>
        /// This method is fired when the task returned from the UserHandler is cancelled. 
        /// Implement a strategy for handling the cancellation here.
        /// </summary>
        /// <param name="context">The consumer execution context.</param>
        /// <returns><see cref="AckStrategy"/> for processing the original cancelled message</returns>
        AckStrategy HandleConsumerCancelled(ConsumerExecutionContext context);
    }
    /// <summary>
    /// A strategy for dealing with failed messages. When a message consumer thows, HandleConsumerError is invoked.
    /// 
    /// The general priciple is to put all failed messages in a dedicated error queue so that they can be 
    /// examined and retried (or ignored).
    /// 
    /// Each failed message is wrapped in a special system message, 'Error' and routed by a special exchange
    /// named after the orignal message's routing key. This is so that ad-hoc queues can be attached for
    /// errors on specific message types.
    /// 
    /// Each exchange is bound to the central RabbitMQ error queue.
    /// </summary>
    public class ConsumerErrorStrategy : IConsumerErrorStrategy
    {
        private readonly ConnectionFactoryWrapper _connectionFactory;

        private IConnection _connection;
        private bool _errorQueueDeclared;
        private readonly ConcurrentDictionary<string, string> _errorExchanges = new ConcurrentDictionary<string, string>();

        public ConsumerErrorStrategy(
            ConnectionFactoryWrapper connectionFactory)
        {
            Preconditions.CheckNotNull(connectionFactory, "connectionFactory");
            
            this._connectionFactory = connectionFactory;
        }

        private void Connect()
        {
            if (this._connection == null || !this._connection.IsOpen)
            {
                this._connection = this._connectionFactory.CreateConnection();
            }
        }

        private void DeclareDefaultErrorQueue(IModel model)
        {
            if (!this._errorQueueDeclared)
            {
                model.QueueDeclare(
                    queue: Conventions.ErrorQueueNamingConvention(),
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                this._errorQueueDeclared = true;
            }
        }

        private string DeclareErrorExchangeAndBindToDefaultErrorQueue(IModel model, ConsumerExecutionContext context)
        {
            var originalRoutingKey = context.Info.RoutingKey;

            return this._errorExchanges.GetOrAdd(originalRoutingKey, _ =>
            {
                var exchangeName = Conventions.ErrorExchangeNamingConvention(context.Info);
                model.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true);
                model.QueueBind(Conventions.ErrorQueueNamingConvention(), exchangeName, originalRoutingKey);
                return exchangeName;
            });
        }

        private string DeclareErrorExchangeQueueStructure(IModel model, ConsumerExecutionContext context)
        {
            this.DeclareDefaultErrorQueue(model);
            return DeclareErrorExchangeAndBindToDefaultErrorQueue(model, context);
        }

        public virtual AckStrategy HandleConsumerError(ConsumerExecutionContext context, Exception exception)
        {
            Preconditions.CheckNotNull(context, "context");
            Preconditions.CheckNotNull(exception, "exception");

            try
            {
                this.Connect();

                using (var model = this._connection.CreateModel())
                {
                    var errorExchange = DeclareErrorExchangeQueueStructure(model, context);

                    var messageBody = CreateErrorMessage(context, exception);
                    var properties = model.CreateBasicProperties();
                    properties.SetPersistent(true);
                    properties.Type = TypeNameSerializer.Serialize(typeof(FAN.RabbitMQ.Topology.Error));

                    model.BasicPublish(errorExchange, context.Info.RoutingKey, properties, messageBody);
                }
            }
            catch (BrokerUnreachableException)
            {
                // thrown if the broker is unreachable during initial creation.
                ConsoleLogger.ErrorWrite("RabbitMQ Consumer Error Handler cannot connect to Broker\n" +
                    CreateConnectionCheckMessage());
            }
            catch (OperationInterruptedException interruptedException)
            {
                // thrown if the broker connection is broken during declare or publish.
                ConsoleLogger.ErrorWrite("RabbitMQ Consumer Error Handler: Broker connection was closed while attempting to publish Error message.\n" +
                    string.Format("Message was: '{0}'\n", interruptedException.Message) +
                    CreateConnectionCheckMessage());
            }
            catch (Exception unexpectedException)
            {
                // Something else unexpected has gone wrong :(
                ConsoleLogger.ErrorWrite("RabbitMQ Consumer Error Handler: Failed to publish error message\nException is:\n"
                    + unexpectedException);
            }
            return AckStrategies.Ack;
        }

        public AckStrategy HandleConsumerCancelled(ConsumerExecutionContext context)
        {
            return AckStrategies.Ack;
        }

        private byte[] CreateErrorMessage(ConsumerExecutionContext context, Exception exception)
        {
            var messageAsString = Encoding.UTF8.GetString(context.Body);
            var error = new FAN.RabbitMQ.Topology.Error
            {
                RoutingKey = context.Info.RoutingKey,
                Exchange = context.Info.Exchange,
                Exception = exception.ToString(),
                Message = messageAsString,
                DateTime = DateTime.Now,
                BasicProperties = context.Properties
            };

            return MessageToBytes(error);
        }
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        private static byte[] MessageToBytes<T>(T message) where T : class
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, serializerSettings));
        }
        private string CreateConnectionCheckMessage()
        {
            return
                "Please check RabbitMQ connection information and that the RabbitMQ Service is running at the specified endpoint.\n" +
                string.Format("\tHostname: '{0}'\n", _connectionFactory.CurrentHost.Host) +
                string.Format("\tVirtualHost: '{0}'\n", _connectionFactory.ConnectionConfiguration.VirtualHost) +
                string.Format("\tUserName: '{0}'\n", _connectionFactory.ConnectionConfiguration.UserName) +
                "Failed to write error message to error queue";
        }

        private bool _disposed = false;

        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }
            this._disposed = true;
            if (this._connection != null)
            {
                this._connection.Dispose();
            }
        }
    }
}
