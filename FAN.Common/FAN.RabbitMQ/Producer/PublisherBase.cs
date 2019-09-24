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
using System.Threading.Tasks;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 生产者发布信息的基类
    /// </summary>
    public abstract class PublisherBase : IPublisher
    {
        private IModel _cachedModel;

        protected PublisherBase()
        {
        }
        /// <summary>
        /// 设置生产者的通道，同时会激发打开这个通道或关闭这个通道所关联的事件方法。
        /// </summary>
        /// <param name="model"></param>
        protected void SetModel(IModel model)
        {
            if (this._cachedModel == model) return;

            if (this._cachedModel != null)
            {
                this.OnChannelClosed(this._cachedModel);
            }

            this._cachedModel = model;

            this.OnChannelOpened(model);
        }
        /// <summary>
        /// 表示当一个基础命令到达服务器之后关联一个打开通道的事件
        /// </summary>
        /// <param name="newModel"></param>
        protected virtual void OnChannelOpened(IModel newModel)
        {
            newModel.BasicReturn += this.ModelOnBasicReturn;//BasicReturn表示从服务器返回一个Basic.Return信号。
        }
        /// <summary>
        /// 表示当一个基础命令到达服务器之后关联一个关闭通道的事件
        /// </summary>
        /// <param name="oldModel"></param>
        protected virtual void OnChannelClosed(IModel oldModel)
        {
            oldModel.BasicReturn -= this.ModelOnBasicReturn;//BasicReturn表示从服务器返回一个Basic.Return信号。
        }
        /// <summary>
        /// 发布(执行)，等同于 执行Onclick方法（执行委托所关联的方法）
        /// 正确理解的理解方式是，通道（model）要执行的方法，相当于model.pubishAction()。此处分开写了，model一个参数，publishAction一个参数。
        /// </summary>
        /// <param name="model">通道对象</param>
        /// <param name="publishAction">要执行的方法，参数就是model通道。</param>
        /// <returns></returns>
        public abstract void Publish(IModel model, byte[] body, MessageProperties messageProperties, Action<IModel, byte[], MessageProperties> publishAction);
        /// <summary>
        /// 表示当一个基础命令到达服务器之后执行一个打开或关闭通道事件所对应的方法。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="args"></param>
        protected void ModelOnBasicReturn(IModel model, BasicReturnEventArgs args)
        {
            EventBus.Instance.Publish(new ReturnedMessageEvent(args.Body, new MessageProperties(args.BasicProperties), new MessageReturnedInfo(args.Exchange, args.RoutingKey, args.ReplyText)));
        }

    }
}
