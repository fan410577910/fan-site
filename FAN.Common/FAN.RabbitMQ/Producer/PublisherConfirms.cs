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
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 生产者发布消息（带事务）
    /// http://www.rabbitmq.com/blog/2011/02/10/introducing-publisher-confirms/
    /// http://www.rabbitmq.com/confirms.html
    /// 注意，这个类是从一个单线程角度的出发设计的。它不是线程安全。
    /// </summary>
    public class PublisherConfirms : PublisherBase
    {
        private readonly IDictionary<ulong, ConfirmActions> _dictionary = new ConcurrentDictionary<ulong, ConfirmActions>();

        private readonly int _timeoutSeconds;

        public PublisherConfirms(ConnectionConfiguration configuration)
        {
            Preconditions.CheckNotNull(configuration, "configuration");

            this._timeoutSeconds = configuration.Timeout;

            EventBus.Instance.Subscribe<PublishChannelCreatedEvent>(this.OnPublishChannelCreated);
        }
        /// <summary>
        /// 当通道被创建之后，激发事件所对应的处理方法。
        /// </summary>
        /// <param name="publishChannelCreatedEvent"></param>
        private void OnPublishChannelCreated(PublishChannelCreatedEvent publishChannelCreatedEvent)
        {
            Preconditions.CheckNotNull(publishChannelCreatedEvent.Channel, "oldModel");

            List<ConfirmActions> outstandingConfirmList = new List<ConfirmActions>(this._dictionary.Values);

            foreach (ConfirmActions outstandingConfirm in outstandingConfirmList)
            {
                outstandingConfirm.Cancel();
                this.Publish(publishChannelCreatedEvent.Channel, outstandingConfirm.Body, outstandingConfirm.MessageProperties, outstandingConfirm.PublishAction);
            }

        }
        /// <summary>
        /// 表示当一个基础命令到达服务器之后关联一个打开通道的事件
        /// </summary>
        /// <param name="newModel"></param>
        protected override void OnChannelOpened(IModel newModel)
        {
            newModel.ConfirmSelect();

            newModel.BasicAcks += this.ModelOnBasicAcks;//BasicAcks表示从服务器返回一个Basic.Ack信号。
            newModel.BasicNacks += this.ModelOnBasicNacks;//BasicNacks表示从服务器返回一个Basic.Nack信号。
            base.OnChannelOpened(newModel);
        }

        protected override void OnChannelClosed(IModel oldModel)
        {
            this._dictionary.Clear();

            oldModel.BasicAcks -= this.ModelOnBasicAcks;
            oldModel.BasicNacks -= this.ModelOnBasicNacks;
            base.OnChannelClosed(oldModel);
        }

        private void ModelOnBasicNacks(IModel model, BasicNackEventArgs args)
        {
            this.HandleConfirm(args.DeliveryTag, args.Multiple, x => x.OnNack());
        }

        private void ModelOnBasicAcks(IModel model, BasicAckEventArgs args)
        {
            this.HandleConfirm(args.DeliveryTag, args.Multiple, x => x.OnAck());
        }

        private void HandleConfirm(ulong sequenceNumber, bool multiple, Action<ConfirmActions> confirmAction)
        {
            if (multiple)
            {
                ConfirmActions confirmActions = null;
                foreach (ulong match in this._dictionary.Keys.Where(key => key <= sequenceNumber))
                {
                    confirmActions = this._dictionary[match];
                    this._dictionary.Remove(match);
                    confirmAction(confirmActions);//执行ConfirmActions里面的OnAck，OnNack。
                }
            }
            else
            {
                if (this._dictionary.ContainsKey(sequenceNumber))
                {
                    ConfirmActions confirmActions = this._dictionary[sequenceNumber];
                    this._dictionary.Remove(sequenceNumber);
                    confirmAction(confirmActions);//执行ConfirmActions里面的OnAck，OnNack。
                }
            }
        }
        /// <summary>
        /// 发布(执行)，等同于 执行Onclick方法（执行委托所关联的方法）
        /// 生产者发布带有事务确认的操作
        /// 正确理解的理解方式是，通道（model）要执行的方法，相当于model.pubishAction()。此处分开写了，model一个参数，publishAction一个参数。
        /// </summary>
        /// <param name="model">通道对象</param>
        /// <param name="publishAction">要执行的方法，参数就是model通道。</param>
        /// <returns></returns>
        public override void Publish(IModel model, byte[] body, MessageProperties messageProperties, Action<IModel, byte[], MessageProperties> publishAction)
        {
            base.SetModel(model);

            ulong sequenceNumber = model.NextPublishSeqNo;

            Timer timer = null;
            timer = new Timer(state =>
            {
                //改为记录日志的方式处理超时结果。wangyunpeng。2017-8-24
                (state as Timer).Dispose();
                this._dictionary.Remove(sequenceNumber);
                ConsoleLogger.ErrorWrite("生产者发布消息超时。 序列号: {0}", sequenceNumber);
                EventBus.Instance.Publish(new ConfirmedMessageTimeOutEvent(body, messageProperties, new MessageConfirmedTimeOutInfo(sequenceNumber, string.Format("生产者确认超时后，{0}秒后等待来自序列号为{1}的ACK或NACK ", this._timeoutSeconds, sequenceNumber))));
            }, timer, this._timeoutSeconds * 1000, Timeout.Infinite);

            //在执行生产者发布消息之前，增加对这个消息的事务操作，包括事务成功和事务失败的处理。
            this._dictionary.Add(sequenceNumber, new ConfirmActions
            {
                OnAck = () =>
                {
                    timer.Dispose();
                    ConsoleLogger.InfoWrite("生产者发布的消息通过服务器的确认。序列号: {0}", sequenceNumber);
                    EventBus.Instance.Publish(new ConfirmedMessageEvent(body, messageProperties, new MessageConfirmedInfo(sequenceNumber, true)));
                },
                OnNack = () =>
                {
                    timer.Dispose();
                    ConsoleLogger.ErrorWrite("生产者发布的消息未通过服务器的确认。序列号: {0}", sequenceNumber);
                    EventBus.Instance.Publish(new ConfirmedMessageEvent(body, messageProperties, new MessageConfirmedInfo(sequenceNumber, false)));
                },
                Cancel = () => timer.Dispose(),
                Body = body,
                MessageProperties = messageProperties,
                PublishAction = publishAction
            });

            publishAction(model, body, messageProperties);//真正执行生产者发布消息的操作。RabbitAdvancedBus.Publish.cs文件this._publisher.Publish(......)
        }

        private class ConfirmActions
        {
            /// <summary>
            /// 收到发送消息成功之后，执行的操作。
            /// </summary>
            public Action OnAck { get; set; }
            /// <summary>
            /// 收到发送消息失败之后，执行的操作。
            /// </summary>
            public Action OnNack { get; set; }
            /// <summary>
            /// 取消发送带有事务消息的操作。
            /// </summary>
            public Action Cancel { get; set; }
            /// <summary>
            /// 发送消息内容
            /// </summary>
            public byte[] Body { get; set; }
            /// <summary>
            /// 发送消息属性
            /// </summary>
            public MessageProperties MessageProperties { get; set; }
            /// <summary>
            /// 执行发布消息的RabbitMQ操作
            /// </summary>
            public Action<IModel, byte[], MessageProperties> PublishAction { get; set; }
        }
    }
}
