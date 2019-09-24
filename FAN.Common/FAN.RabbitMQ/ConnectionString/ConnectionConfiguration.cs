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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FAN.RabbitMQ
{
    public class HostConfiguration
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
    }
    public class ConnectionConfiguration
    {
        private const int DefaultPort = 5672;
        public ushort Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ushort RequestedHeartbeat { get; set; }
        public ushort PrefetchCount { get; set; }
        public Uri AMQPConnectionString { get; set; }
        public IDictionary<string, object> ClientProperties { get; private set; }
        public IEnumerable<HostConfiguration> Hosts { get; set; }
        public SslOption Ssl { get; private set; }
        public ushort Timeout { get; set; }
        public bool PublisherConfirms { get; set; }
        public bool PersistentMessages { get; set; }
        public bool CancelOnHaFailover { get; set; }
        public string Product { get; set; }
        public string Platform { get; set; }

        public ConnectionConfiguration()
        {
            // set default values
            this.Port = DefaultPort;
            this.VirtualHost = "/";
            this.UserName = "guest";
            this.Password = "guest";
            this.RequestedHeartbeat = 10;
            this.Timeout = 10; // seconds
            this.PublisherConfirms = false;
            this.PersistentMessages = true;
            this.CancelOnHaFailover = false;

            // prefetchCount determines how many messages will be allowed in the local in-memory queue
            // setting to zero makes this infinite, but risks an out-of-memory exception.
            // set to 50 based on this blog post:
            // http://www.rabbitmq.com/blog/2012/04/25/rabbitmq-performance-measurements-part-2/
            this.PrefetchCount = 50;
            this.Hosts = new List<HostConfiguration>();
            this.Ssl = new SslOption();
        }

        public void Validate()
        {
            if (this.AMQPConnectionString != null)
            {
                if (this.Port == DefaultPort && this.AMQPConnectionString.Port > 0)
                    this.Port = (ushort)this.AMQPConnectionString.Port;
                this.Hosts = this.Hosts.Concat(new[] { new HostConfiguration { Host = this.AMQPConnectionString.Host } });
            }
            if (!this.Hosts.Any())
            {
                throw new Exception("Invalid connection string. 'host' value must be supplied. e.g: \"host=myserver\"");
            }
            foreach (var hostConfiguration in this.Hosts)
            {
                if (hostConfiguration.Port == 0)
                {
                    ((HostConfiguration)hostConfiguration).Port = this.Port;
                }
            }

            this.ClientProperties = new Dictionary<string, object>();
            this.SetDefaultClientProperties(this.ClientProperties);
        }

        private void SetDefaultClientProperties(IDictionary<string, object> clientProperties)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var applicationNameAndPath = Environment.GetCommandLineArgs()[0];
            var applicationName = Path.GetFileName(applicationNameAndPath);
            var applicationPath = Path.GetDirectoryName(applicationNameAndPath);
            var hostname = Environment.MachineName;
            var product = this.Product ?? applicationName;
            var platform = this.Platform ?? hostname;

            clientProperties.Add("client_api", "RabbitMQ");
            clientProperties.Add("product", product);
            clientProperties.Add("platform", platform);
            clientProperties.Add("version", version);
            clientProperties.Add("easynetq_version", version);
            clientProperties.Add("application", applicationName);
            clientProperties.Add("application_location", applicationPath);
            clientProperties.Add("machine_name", hostname);
            clientProperties.Add("user", this.UserName);
            clientProperties.Add("connected", DateTime.Now.ToString("MM/dd/yy HH:mm:ss"));
            clientProperties.Add("requested_heartbeat", this.RequestedHeartbeat.ToString());
            clientProperties.Add("timeout", this.Timeout.ToString());
            clientProperties.Add("publisher_confirms", this.PublisherConfirms.ToString());
            clientProperties.Add("persistent_messages", this.PersistentMessages.ToString());
        }

    }

}
