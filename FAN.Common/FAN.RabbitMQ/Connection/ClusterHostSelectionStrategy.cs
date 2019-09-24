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
using System.Collections;
using System.Collections.Generic;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 支持集群配置，它里面有每一个集群节点的MQ连接对象
    /// </summary>
    public class ClusterHostSelectionStrategy<T> : IEnumerable<T> where T : class
    {
        private readonly IList<T> _list = new List<T>();
        private int _currentIndex = 0;
        private int _startIndex = 0;

        private static ClusterHostSelectionStrategy<T> _Instance = null;

        public static ClusterHostSelectionStrategy<T> Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ClusterHostSelectionStrategy<T>();
                }
                return _Instance;
            }
        }

        public void Add(T item)
        {
            Preconditions.CheckNotNull(item, "item");
            this._list.Add(item);
            this._startIndex = this._list.Count - 1;
        }

        public T Current()
        {
            if (this._list.Count == 0)
            {
                throw new Exception("No items in collection");
            }

            return this._list[this._currentIndex];
        }

        public bool Next()
        {
            if (this._currentIndex == this._startIndex) return false;
            if (this.Succeeded) return false;

            this.IncrementIndex();

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Success()
        {
            this.Succeeded = true;
            this._startIndex = _currentIndex;
        }

        public bool Succeeded { get; private set; }

        private bool _firstUse = true;

        public ClusterHostSelectionStrategy()
        {
            this.Succeeded = false;
        }

        public void Reset()
        {
            this.Succeeded = false;
            if (this._firstUse)
            {
                this._firstUse = false;
                return;
            }
            this.IncrementIndex();
        }

        private void IncrementIndex()
        {
            this._currentIndex++;
            if (this._currentIndex == this._list.Count)
            {
                this._currentIndex = 0;
            }
        }
    }
}
