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
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 消费者接收消息之后的处理方式。
    /// </summary>
    public class HandlerRunner : IDisposable
    {
        private readonly ConsumerErrorStrategy _consumerErrorStrategy;

        public HandlerRunner(ConsumerErrorStrategy consumerErrorStrategy)
        {
            Preconditions.CheckNotNull(consumerErrorStrategy, "consumerErrorStrategy");

            this._consumerErrorStrategy = consumerErrorStrategy;
        }
        /// <summary>
        /// 消费者收到消息之后，处理方法。
        /// </summary>
        /// <param name="context"></param>
        public void InvokeOnMessageHandler(ConsumerExecutionContext context)
        {
            Preconditions.CheckNotNull(context, "context");

            ConsoleLogger.DebugWrite("Received \n\tRoutingKey: '{0}'\n\tCorrelationId: '{1}'\n\tConsumerTag: '{2}'\n\tDeliveryTag: {3}\n\tRedelivered: {4}",
                context.Info.RoutingKey,
                context.Properties.CorrelationId,
                context.Info.ConsumerTag,
                context.Info.DeliverTag,
                context.Info.Redelivered);

            Task completionTask;

            try
            {
                completionTask = context.OnMessageHandler(context.Body, context.Properties, context.Info);
            }
            catch (Exception exception)
            {
                completionTask = TaskHelpers.FromException(exception);
            }

            if (completionTask.Status == TaskStatus.Created)
            {
                ConsoleLogger.ErrorWrite("Task returned from consumer callback is not started. ConsumerTag: '{0}'", context.Info.ConsumerTag);
                return;
            }
            
            completionTask.ContinueWith(task => this.DoAck(context, this.GetAckStrategy(context, task)));
        }
        /// <summary>
        /// 得到ack的策略
        /// </summary>
        /// <param name="context"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private AckStrategy GetAckStrategy(ConsumerExecutionContext context, Task task)
        {
            var ackStrategy = AckStrategies.Ack;//默认返回ack
            try
            {
                if (task.IsFaulted)
                {
                    ConsoleLogger.ErrorWrite(BuildErrorMessage(context, task.Exception));
                    ackStrategy = _consumerErrorStrategy.HandleConsumerError(context, task.Exception);
                }
                else if (task.IsCanceled)
                {
                    ackStrategy = this._consumerErrorStrategy.HandleConsumerCancelled(context);
                }
            }
            catch (Exception consumerErrorStrategyError)
            {
                ConsoleLogger.ErrorWrite("Exception in ConsumerErrorStrategy:\n{0}", consumerErrorStrategyError);
                return AckStrategies.Nothing;
            }
            return ackStrategy;
        }
        /// <summary>
        /// 发送ack回执
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ackStrategy"></param>
        private void DoAck(ConsumerExecutionContext context, AckStrategy ackStrategy)
        {
            const string failedToAckMessage =
                "Basic ack failed because channel was closed with message '{0}'." +
                " Message remains on RabbitMQ and will be retried." +
                " ConsumerTag: {1}, DeliveryTag: {2}";

            AckResult ackResult = AckResult.Exception;

            try
            {
                Preconditions.CheckNotNull(context.Consumer.Model, "context.Consumer.Model");

                ackResult = ackStrategy(context.Consumer.Model, context.Info.DeliverTag);
            }
            catch (AlreadyClosedException alreadyClosedException)
            {
                ConsoleLogger.InfoWrite(failedToAckMessage, alreadyClosedException.Message, context.Info.ConsumerTag, context.Info.DeliverTag);
            }
            catch (IOException ioException)
            {
                ConsoleLogger.InfoWrite(failedToAckMessage, ioException.Message, context.Info.ConsumerTag, context.Info.DeliverTag);
            }
            catch (Exception exception)
            {
                ConsoleLogger.ErrorWrite("Unexpected exception when attempting to ACK or NACK\n{0}", exception);
            }
            finally
            {
                EventBus.Instance.Publish(new AckEvent(context, ackResult));
            }
        }

        private string BuildErrorMessage(ConsumerExecutionContext context, Exception exception)
        {
            var message = Encoding.UTF8.GetString(context.Body);

            var properties = context.Properties;
            var propertiesMessage = new StringBuilder();
            if (properties != null)
            {
                properties.AppendPropertyDebugStringTo(propertiesMessage);
            }

            return "Exception thrown by subscription callback.\n" +
                   string.Format("\tExchange:    '{0}'\n", context.Info.Exchange) +
                   string.Format("\tRouting Key: '{0}'\n", context.Info.RoutingKey) +
                   string.Format("\tRedelivered: '{0}'\n", context.Info.Redelivered) +
                   string.Format("Message:\n{0}\n", message) +
                   string.Format("BasicProperties:\n{0}\n", propertiesMessage) +
                   string.Format("Exception:\n{0}\n", exception);
        }

        public void Dispose()
        {
            this._consumerErrorStrategy.Dispose();
        }
    }
}
