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
using System.Collections.Generic;
using System.Text;
using FAN.RabbitMQ.Topology;

namespace FAN.RabbitMQ
{
    partial class RabbitAdvancedBus
    {
        #region Queue 队列
        /// <summary>
        /// 创建一个队列
        /// C#实现rabbitmq 延迟队列功能 http://www.cnblogs.com/ListenCode/p/6709145.html
        /// </summary>
        /// <param name="name">队列名称</param>
        /// <param name="passive">被动的,等同于使用同步方式声明Queue</param>
        /// <param name="durable">将queue持久化</param>
        /// <param name="exclusive">排他队列，如果一个队列被声明为排他队列，该队列仅对首次声明它的连接可见，并在连接断开时自动删除。</param>
        /// <param name="autoDelete">自动删除，如果该队列没有任何订阅的消费者的话，该队列会被自动删除。这种队列适用于临时队列。</param>
        /// <param name="perQueueTtl"></param>
        /// <param name="expires"></param>
        /// <param name="deadLetterExchange"></param>
        /// <returns></returns>
        public IQueue QueueDeclare(string name, bool passive = false, bool durable = true, bool exclusive = false, bool autoDelete = false, int perQueueTtl = int.MaxValue, int expires = int.MaxValue, string deadLetterExchange = null)
        {
            Preconditions.CheckNotNull(name, "name");

            IDictionary<string, object> arguments = new Dictionary<string, object>();
            if (passive)
            {
                this._clientCommandDispatcher.Invoke(x => x.QueueDeclarePassive(name)).Wait();
            }
            else
            {
                if (perQueueTtl != int.MaxValue)
                {
                    arguments.Add("x-message-ttl", perQueueTtl);
                }

                if (expires != int.MaxValue)
                {
                    arguments.Add("x-expires", expires);
                }
                if (!string.IsNullOrEmpty(deadLetterExchange))
                {
                    arguments.Add("x-dead-letter-exchange", deadLetterExchange);
                }
                this._clientCommandDispatcher.Invoke(x => x.QueueDeclare(name, durable, exclusive, autoDelete, arguments)).Wait();

                ConsoleLogger.DebugWrite("Declared Queue: '{0}' durable:{1}, exclusive:{2}, autoDelete:{3}, args:{4}", name, durable, exclusive, autoDelete, this.WriteArguments(arguments));
            }

            return new Queue(name, exclusive);
        }
        /// <summary>
        /// 输出到日志里面的内容，可以忽略不看。
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private string WriteArguments(IEnumerable<KeyValuePair<string, object>> arguments)
        {
            var builder = new StringBuilder();
            var first = true;
            foreach (var argument in arguments)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(", ");
                }
                builder.AppendFormat("{0}={1}", argument.Key, argument.Value);
            }
            return builder.ToString();
        }
        /// <summary>
        /// 主要用在exchangetype是fanout模式下面调用本方法，Queue的Name是RabbitMQ会随机为我们分配这个名字。
        /// </summary>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public IQueue QueueDeclare(IExchange exchange)
        {
            Preconditions.CheckNotNull(exchange, "exchange");

            var task = this._clientCommandDispatcher.Invoke(x => x.QueueDeclare());
            task.Wait();
            QueueDeclareOk queueDeclareOk = task.Result;
            string queueName = queueDeclareOk.QueueName;//Queue的Name是RabbitMQ会随机为我们分配这个名字。
            ConsoleLogger.DebugWrite("Declared Server Generted Queue '{0}'", queueName);

            ConsoleLogger.DebugWrite("Declared Queue: '{0}' ,Exchange Name : '{1}' ,Exchange Type : '{2}'", queueName, exchange.Name, exchange.Type);

            return new Queue(queueName, true);

        }
        /// <summary>
        /// 通过队列名称删除一个队列。
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="ifUnused"></param>
        /// <param name="ifEmpty"></param>
        public void QueueDelete(IQueue queue, bool ifUnused = false, bool ifEmpty = false)
        {
            Preconditions.CheckNotNull(queue, "queue");

            this._clientCommandDispatcher.Invoke(x => x.QueueDelete(queue.Name, ifUnused, ifEmpty)).Wait();

            ConsoleLogger.DebugWrite("Deleted Queue: {0}", queue.Name);
        }
        /// <summary>
        /// 通过队列名称清除队列里面的消息
        /// </summary>
        /// <param name="queue"></param>
        public void QueuePurge(IQueue queue)
        {
            Preconditions.CheckNotNull(queue, "queue");

            this._clientCommandDispatcher.Invoke(x => x.QueuePurge(queue.Name)).Wait();

            ConsoleLogger.DebugWrite("Purged Queue: {0}", queue.Name);
        }
        #endregion
    }
}
