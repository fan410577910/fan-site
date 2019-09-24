
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace FAN.Remoting
{
    /// <summary>
    /// Remoting客户端管理
    /// </summary>
    public partial class RemotingClientManager
    {
        /// <summary>
        /// 重新尝试链接服务器的时间（秒）
        /// </summary>
        const int RECALL_SERVER_SECONDS = 5 * 1000;

        /// <summary>
        /// Remoting客户端缓存
        /// </summary>
        private static Dictionary<string, List<object>> _RemotingClientCache = new Dictionary<string, List<object>>();

        private static event Action<string, string, string> _RemotingServerError;

        /// <summary>
        /// Remoting服务器出错
        /// </summary>
        public static event Action<string, string, string> RemotingServerError
        {
            add
            {
                Action<string, string, string> handler2;
                Action<string, string, string> handler3 = _RemotingServerError;
                do
                {
                    handler2 = handler3;
                    Action<string, string, string> handler4 = (Action<string, string, string>)Delegate.Combine(handler2, value);
                    handler3 = Interlocked.CompareExchange<Action<string, string, string>>(ref _RemotingServerError, handler4, handler2);
                }
                while (handler3 != handler2);
            }
            remove
            {
                Action<string, string, string> handler2;
                Action<string, string, string> handler3 = _RemotingServerError;
                do
                {
                    handler2 = handler3;
                    Action<string, string, string> handler4 = (Action<string, string, string>)Delegate.Remove(handler2, value);
                    handler3 = Interlocked.CompareExchange<Action<string, string, string>>(ref _RemotingServerError, handler4, handler2);
                }
                while (handler3 != handler2);
            }
        }

        /// <summary>
        /// 注册Remoting客户端
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">名称</param>
        /// <param name="uri">URI地址</param>
        public static void RegisterRemotingClient<T>(string name, string uri) where T : class
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(uri))
                return;
            Type type = typeof(T);
            string strType = type.ToString();
            if (!Contains(type, uri, name))
            {
                RemotingHandlerItem<T> item = new RemotingHandlerItem<T>();
                item.Name = name;
                item.Uri = uri;
                if (item.Instance != null)
                {
                    if (_RemotingClientCache.ContainsKey(strType))
                    {
                        List<object> lists = _RemotingClientCache[strType];
                        //if (!lists.Contains(item))
                        //{
                        lists.Add(item);
                        //}
                    }
                    else
                    {
                        List<object> lists = new List<object>();
                        _RemotingClientCache.Add(strType, lists);
                        lists.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 判断Remoting客户端是否已经存在于缓存中
        /// </summary>
        /// <param name="type"></param>
        /// <param name="uri"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Contains(Type type, string uri, string name)
        {
            bool flag = false;
            string strType = type.ToString();
            if (_RemotingClientCache.ContainsKey(strType))
            {
                List<object> lists = _RemotingClientCache[strType];
                Type t = typeof(RemotingHandlerItem<>);
                t = t.MakeGenericType(type);
                PropertyInfo propertyInfoName = t.GetProperty("Name");
                PropertyInfo propertyInfoUri = t.GetProperty("Uri");

                foreach (object obj in lists)
                {
                    object objUri = propertyInfoUri.GetValue(obj, null);
                    object objName = propertyInfoName.GetValue(obj, null);
                    if (uri.Equals(objUri.ToString(), StringComparison.CurrentCultureIgnoreCase)
                        && name.Equals(objName.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 反射方式调用的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        private static void SetFailuredTime<T>(RemotingHandlerItem<T> item) where T : class
        {
            if (item != null && item.Status != ERemotingClientStatus.Error)
            {
                item.Status = ERemotingClientStatus.Error;
                item.UpdateFailuredTime();
                //服务链接失败时，指定时间后重新尝试链接
                Action action = new Action(() =>
                {
                    Thread.Sleep(RECALL_SERVER_SECONDS);
                    item.Status = ERemotingClientStatus.Working;
                });
                action.BeginInvoke(null, null);
            }
        }

        private static void OnRemotingServerError(string name, string uri, string exInfo)
        {
            Debug.WriteLine(exInfo);
            if (_RemotingServerError != null)
                _RemotingServerError(name, uri, exInfo);
#if DEBUG
            //throw new Exception(exInfo);//wyp 调试时如果不需要把所有的Remoting服务端程序全部都打开，必须注释掉本行代码，否则会提示Remoting连接失败。
#endif
        }
    }
}
