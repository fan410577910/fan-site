
using System;

namespace FAN.Remoting
{
    public class RemotingClient<T> where T : class
    {
        private T _Client = null;
        private string _RemoteHost = string.Empty;
        private object _locker = new object();

        /// <summary>
        /// Remoting的完整URI路径
        /// </summary>
        public string RemoteHost
        {
            get { return _RemoteHost; }
            set { _RemoteHost = value; }
        }

        public RemotingClient()
        { }

        public T Instance
        {
            get
            {
                if (_Client == null)
                {
                    lock (_locker)
                    {
                        if (_Client == null)
                        {
                            //RemotingConfiguration.RegisterWellKnownClientType(typeof(T), _RemoteHost + "/" + typeof(T).Name);
                            //_Client = (T)Activator.CreateInstance(typeof(T));
                            _Client = (T)Activator.GetObject(typeof(T), _RemoteHost + "/" + typeof(T).Name);
                        }
                    }
                }
                return _Client;
            }
        }
    }
}
