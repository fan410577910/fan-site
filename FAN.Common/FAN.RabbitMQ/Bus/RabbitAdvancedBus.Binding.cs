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
using FAN.RabbitMQ.Topology;

namespace FAN.RabbitMQ
{
    partial class RabbitAdvancedBus
    {
        #region Binding绑定
        /// <summary>
        /// 绑定Queue
        /// 消费者：读取交换（Exchange）里面某一个队列（Queue.Name）的数据时需要使用Bind方法
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey">routingKey=""表示exchage的type是fanout(广播模式)</param>
        /// <returns></returns>
        public IBinding Bind(IExchange exchange, IQueue queue, string routingKey)
        {
            Preconditions.CheckNotNull(exchange, "exchange");
            Preconditions.CheckNotNull(queue, "queue");
            Preconditions.CheckShortString(routingKey, "routingKey");

            this._clientCommandDispatcher.Invoke(x => x.QueueBind(queue.Name, exchange.Name, routingKey)).Wait();
            ConsoleLogger.DebugWrite("Bound queue {0} to exchange {1} with routing key {2}", queue.Name, exchange.Name, routingKey);
            return new Binding(queue, exchange, routingKey);
        }
        /// <summary>
        /// 绑定Exchange
        /// 绑定一个交换（Exchange）到另一个交换（Exchange）上去，官网入门例子里面没有该实例。暂时不用关注。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        [Obsolete("官网没有解除一个交换绑定另一个交换的入门例子，暂时不用关注。")]
        public IBinding Bind(IExchange source, IExchange destination, string routingKey)
        {
            Preconditions.CheckNotNull(source, "source");
            Preconditions.CheckNotNull(destination, "destination");
            Preconditions.CheckShortString(routingKey, "routingKey");

            this._clientCommandDispatcher.Invoke(x => x.ExchangeBind(destination.Name, source.Name, routingKey)).Wait();

            ConsoleLogger.DebugWrite("Bound destination exchange {0} to source exchange {1} with routing key {2}", destination.Name, source.Name, routingKey);
            return new Binding(destination, source, routingKey);
        }
        /// <summary>
        /// 如果队列存在，删除该队列（Queue）与交换（Exchange）的绑定；否则，将一个交换从另一个交换中解除绑定。
        /// 官网没有解除一个交换绑定另一个交换的入门例子，暂时不用关注。
        /// </summary>
        /// <param name="binding"></param>
        [Obsolete("官网没有解除一个交换绑定另一个交换的入门例子，暂时不用关注。")]
        public void BindingDelete(IBinding binding)
        {
            Preconditions.CheckNotNull(binding, "binding");

            var queue = binding.Bindable as IQueue;
            if (queue != null)
            {
                //将队列（Queue）从交换（Exchange）中取消绑定。
                this._clientCommandDispatcher.Invoke(x => x.QueueUnbind(queue.Name, binding.Exchange.Name, binding.RoutingKey, null)).Wait();

                ConsoleLogger.DebugWrite("Unbound queue {0} from exchange {1} with routing key {2}", queue.Name, binding.Exchange.Name, binding.RoutingKey);
            }
            else
            {
                var destination = binding.Bindable as IExchange;
                if (destination != null)
                {
                    this._clientCommandDispatcher.Invoke(x => x.ExchangeUnbind(destination.Name, binding.Exchange.Name, binding.RoutingKey)).Wait();
                    ConsoleLogger.DebugWrite("Unbound destination exchange {0} from source exchange {1} with routing key {2}", destination.Name, binding.Exchange.Name, binding.RoutingKey);
                }
            }
        }
        #endregion
    }
}
