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
using System.Collections.Concurrent;
using System.Threading;

namespace FAN.RabbitMQ
{
    public class ConsumerDispatcher : IDisposable
    {
        private readonly Thread _dispatchThread;
        private readonly BlockingCollection<Action> _queue;
        private bool _disposed;

        public ConsumerDispatcher()
        {
            this._queue = new BlockingCollection<Action>();

            this._dispatchThread = new Thread(_ =>
            {
                try
                {
                    while (true)
                    {
                        if (this._disposed)
                        {
                            break;
                        }
                        this._queue.Take()();//执行方法
                    }
                }
                catch (InvalidOperationException ioex)
                {
                    ConsoleLogger.ErrorWrite(ioex);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.ErrorWrite(ex);
                }
            }) { Name = "RabbitMQ consumer dispatch thread" };
            this._dispatchThread.Start();
        }

        public void QueueAction(Action action)
        {
            Preconditions.CheckNotNull(action, "action");
            this._queue.Add(action);
        }

        public void OnDisconnected()
        {
            Action result;
            while (this._queue.TryTake(out result)) { }
        }

        public void Dispose()
        {
            this._queue.CompleteAdding();
            this._disposed = true;
        }

        public bool IsDisposed
        {
            get { return this._disposed; }
        }
    }
}
