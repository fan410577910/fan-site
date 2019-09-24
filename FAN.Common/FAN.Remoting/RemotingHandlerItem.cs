
using System;
using System.Threading;

namespace FAN.Remoting
{
    /// <summary>
    /// Remoting处理项
    /// </summary>
    /// <typeparam name="T">注册的类型</typeparam>
    public class RemotingHandlerItem<T> : IComparable<RemotingHandlerItem<T>> where T : class
    {
        #region 变量
        private string _Name = string.Empty;
        private string _Uri = string.Empty;
        private RemotingClient<T> _RemotingClient = null;
        private int _ProcessCount = 0;
        private ERemotingClientStatus _Status = ERemotingClientStatus.Stopped;
        private long _FailuredTime = DateTime.Now.Ticks;
        private object _root = new object();
        #endregion

        #region 构造函数
        public RemotingHandlerItem()
        { }
        #endregion

        #region 属性
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string Uri
        {
            get { return _Uri; }
            set { _Uri = value; }
        }
        public RemotingClient<T> RemotingClient
        {
            get { return _RemotingClient; }
            set { _RemotingClient = value; }
        }
        public T Instance
        {
            get
            {
                if (RemotingClient == null)
                {
                    lock (_root)
                    {
                        if (RemotingClient == null)
                        {
                            RemotingClient = new RemotingClient<T>();
                            RemotingClient.RemoteHost = Uri;
                        }
                    }
                }
                T t = null;
                try
                {
                    t = RemotingClient.Instance;
                    _Status = ERemotingClientStatus.Working;
                }
                catch (Exception)
                {
                    _Status = ERemotingClientStatus.Error;
                }
                return t;
            }
        }
        public ERemotingClientStatus Status
        {
            get { return _Status; }
            internal set { _Status = value; }
        }
        public long ProcessCount
        {
            get { return _ProcessCount; }
        }
        public long FailuredTime
        {
            get { return _FailuredTime; }
            set { _FailuredTime = value; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 增加1个服务线程计数
        /// </summary>
        internal void AddProccessCount()
        {
            Interlocked.Increment(ref _ProcessCount);
        }
        /// <summary>
        /// 减少1个服务进程计数
        /// </summary>
        internal void SubtractProccessCount()
        {
            if (_ProcessCount == 0)
                return;
            Interlocked.Decrement(ref _ProcessCount);
        }

        internal void UpdateFailuredTime()
        {
            lock (_root)
            {
                _FailuredTime = DateTime.Now.Ticks;
            }
        }

        public int CompareTo(RemotingHandlerItem<T> other)
        {
            return ProcessCount.CompareTo(other.ProcessCount);
        }

        public override bool Equals(object obj)
        {
            if (obj is RemotingHandlerItem<T>)
            {
                RemotingHandlerItem<T> other = obj as RemotingHandlerItem<T>;
                if (other.Uri == Uri)
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
