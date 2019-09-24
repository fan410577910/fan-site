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

namespace FAN.RabbitMQ
{
    /// <summary>
    /// http://www.cnblogs.com/daizhj/category/266311.html(安装)
    /// http://blog.chinaunix.net/uid-22312037-id-3468329.html(监控)
    /// http://blog.csdn.net/column/details/rabbitmq.html(好)
    /// http://www.ostest.cn/archives/497
    /// http://backend.blog.163.com/blog/static/202294126201322563245975/
    /// http://hg.rabbitmq.com/rabbitmq-java-client/file/default/test/src/com/rabbitmq/examples/ConfirmDontLoseMessages.java(事务\确认)
    /// 本类在程序运行过程中只能创建一次，不支持使用using方式创建RabbitAdvancedBus对象的同时释放对象资源。
    /// </summary>
    public partial class RabbitAdvancedBus : IDisposable
    {
        #region 变量
        /// <summary>
        /// 创建消费者的工厂
        /// </summary>
        private readonly ConsumerFactory _consumerFactory;
        /// <summary>
        /// MQ的持久连接
        /// </summary>
        private readonly PersistentConnection _connection;
        /// <summary>
        /// 间接方式调用IModel中的API。
        /// </summary>
        private readonly ClientCommandDispatcherSingleton _clientCommandDispatcher;
        /// <summary>
        /// 生产者
        /// </summary>
        private readonly IPublisher _publisher;
        /// <summary>
        /// MQ的连接配置信息
        /// </summary>
        private readonly ConnectionConfiguration _connectionConfiguration;
        #endregion

        #region 构造函数
        public RabbitAdvancedBus(
            ConnectionFactoryWrapper connectionFactory,
            ConsumerFactory consumerFactory,
            IPublisher publisher,
            ConnectionConfiguration connectionConfiguration)
        {
            Preconditions.CheckNotNull(connectionFactory, "connectionFactory");
            Preconditions.CheckNotNull(consumerFactory, "consumerFactory");
            Preconditions.CheckNotNull(publisher, "publisher");
            Preconditions.CheckNotNull(connectionConfiguration, "connectionConfiguration");

            this._consumerFactory = consumerFactory;
            this._publisher = publisher;
            this._connectionConfiguration = connectionConfiguration;

            this._connection = new PersistentConnection(connectionFactory);

            EventBus.Instance.Subscribe<ConnectionCreatedEvent>(e => this.OnConnected());
            EventBus.Instance.Subscribe<ConnectionDisconnectedEvent>(e => this.OnDisconnected());
            EventBus.Instance.Subscribe<ReturnedMessageEvent>(this.OnMessageReturned);
            EventBus.Instance.Subscribe<ConfirmedMessageEvent>(this.OnMessageConfirmed);
            EventBus.Instance.Subscribe<ConfirmedMessageTimeOutEvent>(this.OnMessageConfirmedTimeout);

            PersistentChannelFactory persistentChannelFactory = new PersistentChannelFactory(connectionConfiguration);
            this._clientCommandDispatcher = new ClientCommandDispatcherSingleton(this._connection, persistentChannelFactory);
        }

        #endregion

        #region 事件
        public event Action Connected;

        private void OnConnected()
        {
            if (this.Connected != null)
            {
                this.Connected();
            }
        }

        public event Action Disconnected;

        private void OnDisconnected()
        {
            if (this.Disconnected != null)
            {
                this.Disconnected();
            }
        }
        /// <summary>
        /// 关联IModel里面的BasicReturn事件处理方法
        /// </summary>
        public event Action<byte[], MessageProperties, MessageReturnedInfo> MessageReturned;

        private void OnMessageReturned(ReturnedMessageEvent args)
        {
            if (this.MessageReturned != null)
            {
                this.MessageReturned(args.Body, args.Properties, args.Info);
            }
        }

        /// <summary>
        /// 关联IModel里面的BasicAcks和BasicNacks事件处理方法，不能清除byte[]中的内容，否则会抱错。wangyunpeng
        /// </summary>
        public event Action<byte[], MessageProperties, MessageConfirmedInfo> MessageConfirmed;

        private void OnMessageConfirmed(ConfirmedMessageEvent args)
        {
            if (this.MessageConfirmed != null)
            {
                this.MessageConfirmed(args.Body, args.Properties, args.Info);
            }
        }

        /// <summary>
        /// 关联IModel里面的BasicAcks和BasicNacks事件处理方法，不能清除byte[]中的内容，否则会抱错。wangyunpeng
        /// </summary>
        public event Action<byte[], MessageProperties, MessageConfirmedTimeOutInfo> MessageConfirmedTimeOut;

        private void OnMessageConfirmedTimeout(ConfirmedMessageTimeOutEvent args)
        {
            if (this.MessageConfirmedTimeOut != null)
            {
                this.MessageConfirmedTimeOut(args.Body, args.Properties, args.Info);
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 判断是否已连接MQ服务器
        /// </summary>
        public bool IsConnected
        {
            get { return this._connection.IsConnected; }
        }
        #endregion

        #region 释放资源
        private bool _disposed = false;
        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }
            this._disposed = true;
            this._consumerFactory.Dispose();
            this._clientCommandDispatcher.Dispose();
            this._connection.Dispose();
            ConsoleLogger.DebugWrite("RabbitMQ连接已经关闭");
        }
        #endregion
    }
}
