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
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading;
using FAN.RabbitMQ.AmqpExceptions;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 持久化通道
    /// </summary>
    public class PersistentChannel : IDisposable
    {
        private readonly PersistentConnection _connection;
        private readonly ConnectionConfiguration _configuration;

        private IModel _channel;
        private bool _disconnected = true;

        public PersistentChannel(
            PersistentConnection connection,
            ConnectionConfiguration configuration)
        {
            Preconditions.CheckNotNull(connection, "connection");
            Preconditions.CheckNotNull(configuration, "configuration");

            this._connection = connection;
            this._configuration = configuration;

            this.WireUpEvents();
        }
        /// <summary>
        /// 关联事件,相当于 Click += Method
        /// </summary>
        private void WireUpEvents()
        {
            EventBus.Instance.Subscribe<ConnectionDisconnectedEvent>(OnConnectionDisconnected);
            EventBus.Instance.Subscribe<ConnectionCreatedEvent>(ConnectionOnConnected);
        }

        private void OnConnectionDisconnected(ConnectionDisconnectedEvent @event)
        {
            if (!this._disconnected)
            {
                this._disconnected = true;
                this._channel = null;
                ConsoleLogger.DebugWrite("Persistent channel disconnected.");
            }
        }

        private void ConnectionOnConnected(ConnectionCreatedEvent @event)
        {
            this.OpenChannel();
        }
        /// <summary>
        /// 获取通道（RabbitMQ里面的类型，就是通道的意思）
        /// </summary>
        public IModel Channel
        {
            get
            {
                if (this._channel == null)
                {
                    this.OpenChannel();
                }
                return this._channel;
            }
        }

        private void OpenChannel()
        {
            this._channel = this._connection.CreateModel();
            this._disconnected = false;
            EventBus.Instance.Publish(new PublishChannelCreatedEvent(this._channel));
            ConsoleLogger.DebugWrite("Persistent channel connected.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelAction">Model通道要执行的方法</param>
        public void InvokeChannelAction(Action<IModel> channelAction)
        {
            Preconditions.CheckNotNull(channelAction, "channelAction");
            this.InvokeChannelActionInternal(channelAction, DateTime.Now);
        }

        private void InvokeChannelActionInternal(Action<IModel> channelAction, DateTime startTime)
        {
            if (this.IsTimedOut(startTime))
            {
                ConsoleLogger.ErrorWrite("Channel action timed out. Throwing exception to client.");
                throw new TimeoutException("The operation requested on PersistentChannel timed out.");
            }
            try
            {
                channelAction(this.Channel);//执行这个方法，ClientCommandDispatcherSingleton类的Invoke<T>方法中定义的cs.SetResult(channelAction(channel));话会被执行。
            }
            catch (OperationInterruptedException exception)
            {
                try
                {
                    AmqpException amqpException = AmqpExceptionGrammar.ParseExceptionString(exception.Message);//将AMQP错误信息，转成C#定义的异常类型
                    if (amqpException.Code == AmqpException.ConnectionClosed)
                    {
                        this.OnConnectionDisconnected(null);
                        this.WaitForReconnectionOrTimeout(startTime);
                        this.InvokeChannelActionInternal(channelAction, startTime);
                    }
                    else
                    {
                        this.OpenChannel();
                        throw;
                    }
                }
                catch (Sprache.ParseException)
                {
                    throw exception;
                }
            }
            catch (Exception)
            {
                this.OnConnectionDisconnected(null);
                this.WaitForReconnectionOrTimeout(startTime);
                this.InvokeChannelActionInternal(channelAction, startTime);
            }

        }

        private void WaitForReconnectionOrTimeout(DateTime startTime)
        {
            ConsoleLogger.DebugWrite("Persistent channel operation failed. Waiting for reconnection.");
            int delayMilliseconds = 10;

            while (this._disconnected && !this.IsTimedOut(startTime))
            {
                Thread.Sleep(delayMilliseconds);
                delayMilliseconds *= 2;
                try
                {
                    this.OpenChannel();
                }
                catch (OperationInterruptedException)
                { }
                catch (Exception)
                { }
            }
        }

        private bool IsTimedOut(DateTime startTime)
        {
            return startTime.AddSeconds(this._configuration.Timeout) < DateTime.Now;
        }

        public void Dispose()
        {
            if (this._channel != null)
            {
                this._channel.Dispose();
                ConsoleLogger.DebugWrite("Persistent channel disposed.");
            }
        }
    }
}
