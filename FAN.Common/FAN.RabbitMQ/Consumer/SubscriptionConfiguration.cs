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
using System.Collections.Generic;

namespace FAN.RabbitMQ
{

    public class SubscriptionConfiguration
    {
        public IList<string> Topics { get; private set; }
        public bool AutoDelete { get; private set; }
        public int Priority { get; private set; }
        public bool CancelOnHaFailover { get; private set; }
        public ushort PrefetchCount { get; private set; }

        public SubscriptionConfiguration(ushort defaultPrefetchCount)
        {
            this.Topics = new List<string>();
            this.AutoDelete = false;
            this.Priority = 0;
            this.CancelOnHaFailover = false;
            this.PrefetchCount = defaultPrefetchCount;
        }

        public SubscriptionConfiguration WithTopic(string topic)
        {
            this.Topics.Add(topic);
            return this;
        }

        public SubscriptionConfiguration WithAutoDelete(bool autoDelete = true)
        {
            this.AutoDelete = autoDelete;
            return this;
        }

        public SubscriptionConfiguration WithPriority(int priority)
        {
            this.Priority = priority;
            return this;
        }

        public SubscriptionConfiguration WithCancelOnHaFailover(bool cancelOnHaFailover = true)
        {
            this.CancelOnHaFailover = cancelOnHaFailover;
            return this;
        }

        public SubscriptionConfiguration WithPrefetchCount(ushort prefetchCount)
        {
            this.PrefetchCount = prefetchCount;
            return this;
        }
    }
}
