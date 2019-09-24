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
using System.Collections.Generic;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 不使用event的方式，也能实现类似winform中event的功能，这是在BS结构里面一种通用实现事件的编程思想，区别于CS结构的code方式
    /// </summary>
    public class EventBus
    {
        private readonly ConcurrentDictionary<Type, IList<object>> _subscriptions = new ConcurrentDictionary<Type, IList<object>>();
        private static EventBus _Instance = null;

        public static EventBus Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new EventBus();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// 发布(执行)，等同于 执行Onclick方法（执行委托所关联的方法）
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        public void Publish<TEvent>(TEvent @event)
        {
            if (!this._subscriptions.ContainsKey(typeof(TEvent)))
                return;

            var handlers = new List<object>(this._subscriptions[typeof(TEvent)]);
            foreach (var eventHandler in handlers)
            {
                ((Action<TEvent>)eventHandler)(@event);
            }
        }
        /// <summary>
        /// 订阅(注册)，等同于 Click += Method
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public CancelSubscription Subscribe<TEvent>(Action<TEvent> eventHandler)
        {
            CancelSubscription cancelSubscription = null;

            this._subscriptions.AddOrUpdate(typeof(TEvent),
                    t =>
                    {
                        var l = new List<object> { eventHandler };
                        cancelSubscription = () => l.Remove(eventHandler);
                        return l;
                    },
                    (t, l) =>
                    {
                        l.Add(eventHandler);
                        cancelSubscription = () => l.Remove(eventHandler);
                        return l;
                    }
                );

            return cancelSubscription;
        }
    }

    public delegate void CancelSubscription();
}
