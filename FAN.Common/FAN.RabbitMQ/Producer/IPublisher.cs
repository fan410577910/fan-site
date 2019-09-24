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
using System.Threading.Tasks;

namespace FAN.RabbitMQ
{
    public interface IPublisher
    {
        /// <summary>
        /// 发布(执行)，等同于 执行Onclick方法（执行委托所关联的方法）
        /// 正确理解的理解方式是，通道（model）要执行的方法，相当于model.pubishAction()。此处分开写了，model一个参数，publishAction一个参数。
        /// </summary>
        /// <param name="model">通道对象</param>
        /// <param name="publishAction">要执行的方法，参数就是model通道。</param>
        /// <returns></returns>
        void Publish(IModel model, byte[] body, MessageProperties messageProperties, Action<IModel, byte[], MessageProperties> publishAction);
    }
}
