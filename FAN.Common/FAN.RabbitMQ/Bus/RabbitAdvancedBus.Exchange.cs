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
using FAN.RabbitMQ.Topology;

namespace FAN.RabbitMQ
{
    partial class RabbitAdvancedBus
    {
        #region Exchange交换
        /// <summary>
        /// 创建一个交换
        /// </summary>
        /// <param name="name">交换名称</param>
        /// <param name="type">交换类型</param>
        /// <param name="passive">被动的,等同于使用同步方式声明Exchange</param>
        /// <param name="durable">将exchange持久化</param>
        /// <param name="autoDelete">自动删除，如果该队列没有任何订阅的消费者的话，该队列会被自动删除。这种队列适用于临时队列。</param>
        /// <param name="alternateExchange"></param>
        /// <returns></returns>
        public IExchange ExchangeDeclare(string name, string type, bool passive = false, bool durable = true, bool autoDelete = false, string alternateExchange = null)
        {
            Preconditions.CheckShortString(name, "name");
            Preconditions.CheckShortString(type, "type");

            if (passive)
            {
                this._clientCommandDispatcher.Invoke(x => x.ExchangeDeclarePassive(name)).Wait();
            }
            else
            {
                IDictionary<string, object> arguments = null;
                if (alternateExchange != null)
                {
                    arguments = new Dictionary<string, object> { { "alternate-exchange", alternateExchange } };
                }

                this._clientCommandDispatcher.Invoke(x => x.ExchangeDeclare(name, type, durable, autoDelete, arguments)).Wait();
                ConsoleLogger.DebugWrite("Declared Exchange: {0} type:{1}, durable:{2}, autoDelete:{3}", name, type, durable, autoDelete);

            }

            return new Exchange(name, type);
        }
        /// <summary>
        /// 删除交换
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="ifUnused"></param>
        public void ExchangeDelete(IExchange exchange, bool ifUnused = false)
        {
            Preconditions.CheckNotNull(exchange, "exchange");

            this._clientCommandDispatcher.Invoke(x => x.ExchangeDelete(exchange.Name, ifUnused)).Wait();
            ConsoleLogger.DebugWrite("Deleted Exchange: {0}", exchange.Name);
        }
        #endregion
    }
}
