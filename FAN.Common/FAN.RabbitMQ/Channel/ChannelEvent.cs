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

namespace FAN.RabbitMQ
{
    public class PublishChannelCreatedEvent
    {
        /// <summary>
        /// 获取通道里面的Model（RabbitMQ里面的类型，就是通道的意思）
        /// </summary>
        public IModel Channel { get; private set; }

        public PublishChannelCreatedEvent(IModel channel)
        {
            this.Channel = channel;
        }
    }
}
