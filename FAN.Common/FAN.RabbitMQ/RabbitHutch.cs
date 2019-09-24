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
    /// 调用RabbitMQ里面的功能都从这里面出
    /// </summary>
    public static class RabbitHutch
    {
        static IContainer _container = DefaultServiceProvider.Instance;

        public static RabbitAdvancedBus CreateBus(string connectionString)
        {
            Preconditions.CheckNotNull(connectionString, "connectionString");

            ConnectionConfiguration connectionConfiguration = ConnectionStringParser.Parse(connectionString);

            return CreateBus(connectionConfiguration);
        }
        
        public static RabbitAdvancedBus CreateBus(ConnectionConfiguration connectionConfiguration)
        {
            Preconditions.CheckNotNull(connectionConfiguration, "connectionConfiguration");
            _container.Register(_ => connectionConfiguration);
            _container
                .Register(_ => _container)
                .Register(sp => PublisherFactory.CreatePublisher(sp.Resolve<ConnectionConfiguration>()))
                .Register<ConnectionFactoryWrapper>()
                .Register<ConsumerErrorStrategy>()
                .Register<HandlerRunner>()
                .Register<InternalConsumerFactory>()
                .Register<ConsumerFactory>()
                .Register<RabbitAdvancedBus>();
            return _container.Resolve<RabbitAdvancedBus>();
        }
    }
}
