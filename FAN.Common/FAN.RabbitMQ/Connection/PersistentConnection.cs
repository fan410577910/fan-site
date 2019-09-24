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

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 连接对象，等同于HTTP长连接，TCP连接之后未关闭。
    /// </summary>
    public class PersistentConnection : IDisposable
    {
        private const int CONNECT_ATTEMPT_INTERVAL_MILLISECONDS = 5000;

        private readonly ConnectionFactoryWrapper _connectionFactory;
        private IConnection _connection;

        public PersistentConnection(ConnectionFactoryWrapper connectionFactory)
        {
            Preconditions.CheckNotNull(connectionFactory, "connectionFactory");

            this._connectionFactory = connectionFactory;
            TryToConnect(null);
        }

        public IModel CreateModel()
        {
            if (!this.IsConnected)
            {
                throw new Exception("RabbitMQ还没有创建连接呢！");
            }

            return this._connection.CreateModel();
        }

        public bool IsConnected
        {
            get { return this._connection != null && this._connection.IsOpen && !this._disposed; }
        }

        void StartTryToConnect()
        {
            Timer timer = new Timer(this.TryToConnect);
            timer.Change(CONNECT_ATTEMPT_INTERVAL_MILLISECONDS, Timeout.Infinite);
        }

        void TryToConnect(object timer)
        {
            if (timer != null)
            {
                ((Timer)timer).Dispose();
            }

            ConsoleLogger.DebugWrite("尝试连接...");
            if (this._disposed)
            {
                return;
            }
            this._connectionFactory.Reset();
            do
            {
                try
                {
                    this._connection = this._connectionFactory.CreateConnection();
                    this._connectionFactory.Success();
                }
                catch (System.Net.Sockets.SocketException socketException)
                {
                    this.LogException(socketException);
                }
                catch (BrokerUnreachableException brokerUnreachableException)
                {
                    this.LogException(brokerUnreachableException);
                }
            } while (this._connectionFactory.Next());

            if (this._connectionFactory.Succeeded)
            {
                this._connection.ConnectionShutdown += this.OnConnectionShutdown;

                this.OnConnected();
                ConsoleLogger.InfoWrite("连接到RabbitMQ.服务器成功！: '{0}', Port: {1}, VHost: '{2}'", this._connectionFactory.CurrentHost.Host, this._connectionFactory.CurrentHost.Port, this._connectionFactory.ConnectionConfiguration.VirtualHost);
            }
            else
            {
                ConsoleLogger.ErrorWrite("连接RabbitMQ服务器失败！. 将会在 {0} 毫秒之后重新连接\n", CONNECT_ATTEMPT_INTERVAL_MILLISECONDS);
                this.StartTryToConnect();
            }
        }

        void LogException(Exception exception)
        {
            ConsoleLogger.ErrorWrite("连接到RabbitMQ服务器失败: '{0}', Port: {1} VHost: '{2}'。ExceptionMessage: '{3}'",
                this._connectionFactory.CurrentHost.Host,
                this._connectionFactory.CurrentHost.Port,
                this._connectionFactory.ConnectionConfiguration.VirtualHost,
                exception.Message);
        }

        void OnConnectionShutdown(IConnection _, ShutdownEventArgs reason)
        {
            if (this._disposed)
            {
                return;
            }
            this.OnDisconnected();

            ConsoleLogger.InfoWrite("断开连接RabbitMQ服务器");

            this.TryToConnect(null);
        }

        public void OnConnected()
        {
            ConsoleLogger.DebugWrite("连接服务器成功之后的事件处理方法被调用。");
            EventBus.Instance.Publish(new ConnectionCreatedEvent());
        }

        public void OnDisconnected()
        {
            EventBus.Instance.Publish(new ConnectionDisconnectedEvent());
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
                try
                {
                    this._connection.Dispose();
                }
                catch (System.IO.IOException exception)
                {
                    ConsoleLogger.DebugWrite("连接对象被释放时发生IOException异常。 Message: '{0}'。这是一个不正常的现象，需要被关注！", exception.Message);
                }
            }
        }
    }
}
