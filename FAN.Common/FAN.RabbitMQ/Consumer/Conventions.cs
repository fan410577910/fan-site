﻿#region << 版 本 注 释 >>
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

namespace FAN.RabbitMQ
{
    public delegate string ErrorQueueNameConvention();
    public delegate string ErrorExchangeNameConvention(MessageReceivedInfo info);
    public delegate string ConsumerTagConvention();

    public static class Conventions
    {
        static Conventions()
        {
            ErrorQueueNamingConvention = () => "TLZ_RabbitMQ_Default_Error_Queue";
            ErrorExchangeNamingConvention = info => "ErrorExchange_" + info.RoutingKey;

            ConsumerTagConvention = () => Guid.NewGuid().ToString();
        }


        public static ErrorQueueNameConvention ErrorQueueNamingConvention { get; set; }
        public static ErrorExchangeNameConvention ErrorExchangeNamingConvention { get; set; }

        public static ConsumerTagConvention ConsumerTagConvention { get; set; }
    }
}
