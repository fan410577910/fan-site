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

namespace FAN.RabbitMQ
{
    public class ConsumerCancellation : IDisposable
    {
        private readonly Action _onCancellation;

        public ConsumerCancellation(Action onCancellation)
        {
            Preconditions.CheckNotNull(onCancellation, "onCancellation");

            this._onCancellation = onCancellation;
        }

        public void Dispose()
        {
            this._onCancellation();
        }
    }
}
