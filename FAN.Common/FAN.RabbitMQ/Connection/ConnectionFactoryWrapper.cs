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
using RabbitMQ.Client;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FAN.RabbitMQ
{
    public class ConnectionFactoryWrapper
    {
        /// <summary>
        /// 连接字符串的对象格式
        /// </summary>
        public ConnectionConfiguration ConnectionConfiguration { get; private set; }

        public ConnectionFactoryWrapper(ConnectionConfiguration connectionConfiguration)
        {
            Preconditions.CheckNotNull(connectionConfiguration, "connectionConfiguration");
            Preconditions.CheckAny(connectionConfiguration.Hosts, "connectionConfiguration", "At least one host must be defined in connectionConfiguration");

            this.ConnectionConfiguration = connectionConfiguration;

            foreach (var hostConfiguration in connectionConfiguration.Hosts)
            {
                var connectionFactory = new ConnectionFactory();
                if (connectionConfiguration.AMQPConnectionString != null)
                {
                    connectionFactory.uri = connectionConfiguration.AMQPConnectionString;
                }

                connectionFactory.HostName = hostConfiguration.Host;

                if (connectionFactory.VirtualHost == "/")
                    connectionFactory.VirtualHost = connectionConfiguration.VirtualHost;

                if (connectionFactory.UserName == "guest")
                    connectionFactory.UserName = connectionConfiguration.UserName;

                if (connectionFactory.Password == "guest")
                    connectionFactory.Password = connectionConfiguration.Password;

                if (connectionFactory.Port == -1)
                    connectionFactory.Port = hostConfiguration.Port;

                if (connectionConfiguration.Ssl.Enabled)
                    connectionFactory.Ssl = connectionConfiguration.Ssl;

                connectionFactory.RequestedHeartbeat = connectionConfiguration.RequestedHeartbeat;
                connectionFactory.ClientProperties = connectionConfiguration.ClientProperties;
                ClusterHostSelectionStrategy<ConnectionFactoryInfo>.Instance.Add(new ConnectionFactoryInfo(connectionFactory, hostConfiguration));
            }
        }

        private static IDictionary ConvertToHashtable(IDictionary<string, string> clientProperties)
        {
            Hashtable dictionary = new Hashtable();
            foreach (var clientProperty in clientProperties)
            {
                dictionary.Add(clientProperty.Key, clientProperty.Value);
            }
            return dictionary;
        }
        /// <summary>
        /// 获取RabbitMQ的连接对象
        /// </summary>
        /// <returns></returns>
        public IConnection CreateConnection()
        {
            return ClusterHostSelectionStrategy<ConnectionFactoryInfo>.Instance.Current().ConnectionFactory.CreateConnection();
        }

        public HostConfiguration CurrentHost
        {
            get { return ClusterHostSelectionStrategy<ConnectionFactoryInfo>.Instance.Current().HostConfiguration; }
        }

        public bool Next()
        {
            return ClusterHostSelectionStrategy<ConnectionFactoryInfo>.Instance.Next();
        }

        public void Reset()
        {
            ClusterHostSelectionStrategy<ConnectionFactoryInfo>.Instance.Reset();
        }

        public void Success()
        {
            ClusterHostSelectionStrategy<ConnectionFactoryInfo>.Instance.Success();
        }

        public bool Succeeded
        {
            get { return ClusterHostSelectionStrategy<ConnectionFactoryInfo>.Instance.Succeeded; }
        }
    }

}
