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
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 调用RabbitMQ里面IModel方法，使用本类可以完成间接调用RabbitMQ里面IModel的方法。
    /// ClientCommandDispatcherSingleton被ClientCommandDispatcherFactory这个类型的对象里面的方法所创建。
    /// </summary>
    public class ClientCommandDispatcherSingleton : IDisposable
    {
        private const int _queueSize = 1;
        private readonly BlockingCollection<Action> _queue = new BlockingCollection<Action>(_queueSize);
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        private readonly PersistentChannel _persistentChannel;

        public ClientCommandDispatcherSingleton(
            PersistentConnection connection,
            PersistentChannelFactory persistentChannelFactory)
        {
            Preconditions.CheckNotNull(connection, "connection");
            Preconditions.CheckNotNull(persistentChannelFactory, "persistentChannelFactory");

            this._persistentChannel = persistentChannelFactory.CreatePersistentChannel(connection);

            this.StartDispatcherThread();
        }

        private void StartDispatcherThread()
        {
            new Thread(() =>
            {
                while (!this._cancellation.IsCancellationRequested)
                {
                    try
                    {
                        var channelAction = this._queue.Take(this._cancellation.Token);
                        channelAction();
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }) { Name = "Client Command Dispatcher Thread" }.Start();
        }
        /// <summary>
        /// 执行传进来的委托所关联的方法。
        /// 由于下面定义Invoke(Action IModel ...)方法没有定义泛型，并且也是Invoke名称，那么这个泛型Invoke T 方法在调用时可以不用加 T，程序自己也能识别出来重载。这个重载很神奇。
        /// </summary>
        /// <typeparam name="T">IModel通道里面的方法</typeparam>
        /// <param name="channelAction">委托所关联的方法</param>
        /// <returns></returns>
        public Task<T> Invoke<T>(Func<IModel, T> channelAction)
        {
            Preconditions.CheckNotNull(channelAction, "channelAction");

            var tcs = new TaskCompletionSource<T>();

            try
            {
                this._queue.Add(() =>
                {
                    if (this._cancellation.IsCancellationRequested)
                    {
                        tcs.TrySetCanceled();
                        return;
                    }
                    try
                    {
                        this._persistentChannel.InvokeChannelAction(channel =>
                        {
                            tcs.TrySetResultSafe(channelAction(channel));
                        });
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, this._cancellation.Token);
            }
            catch (OperationCanceledException)
            {
                tcs.TrySetCanceled();
            }
            return tcs.Task;
        }
        /// <summary>
        /// 执行传进来的委托所关联的方法。
        /// 一定是IModel通道里面的方法
        /// </summary>
        /// <param name="channelAction">委托所关联的方法</param>
        /// <returns></returns>
        public Task Invoke(Action<IModel> channelAction)
        {
            Preconditions.CheckNotNull(channelAction, "channelAction");

            return this.Invoke(x =>
            {
                channelAction(x);
                return new NoContentStruct();
            });
        }

        public void Dispose()
        {
            this._cancellation.Cancel();
            this._persistentChannel.Dispose();
        }

        private struct NoContentStruct { }
    }
}
