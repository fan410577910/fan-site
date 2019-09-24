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

namespace FAN.RabbitMQ
{
    public class ConsumerConfiguration
    {
        public ConsumerConfiguration(ushort defaultPrefetchCount)
        {
            this.Priority = 0;
            this.CancelOnHaFailover = false;
            this.PrefetchCount = defaultPrefetchCount;
        }

        public int Priority { get; private set; }
        public bool CancelOnHaFailover { get; private set; }
        /// <summary>
        /// 预取数量
        /// 这样RabbitMQ就会使得每个Consumer在同一个时间点最多处理一个Message。换句话说，在接收到该Consumer的ack前，他它不会将新的Message分发给它。 
        /// </summary>
        public ushort PrefetchCount { get; private set; }

        public ConsumerConfiguration WithPriority(int priority)
        {
            this.Priority = priority;
            return this;
        }

        public ConsumerConfiguration WithCancelOnHaFailover(bool cancelOnHaFailover = true)
        {
            this.CancelOnHaFailover = cancelOnHaFailover;
            return this;
        }

        public ConsumerConfiguration WithPrefetchCount(ushort prefetchCount)
        {
            this.PrefetchCount = prefetchCount;
            return this;
        }
    }
}
