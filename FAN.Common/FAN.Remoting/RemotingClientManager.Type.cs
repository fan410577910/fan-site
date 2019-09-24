
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Net.Sockets;

namespace FAN.Remoting
{
    partial class RemotingClientManager
    {

        /// <summary>
        /// 调用Remoting远程方法（通过 Type 的方式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object Invoke<T>(string func, params object[] param) where T : class
        {
            if (string.IsNullOrEmpty(func))
                return null;
            Type type = typeof(T);
            string strType = type.ToString();
            object result = null;
            if (_RemotingClientCache.ContainsKey(strType))
            {
                RemotingHandlerItem<T> obj = GetIdleRemotingHandlerItem<T>();
                if (obj == null)
                {
                    obj = GetRandomRemotingHandlerItem<T>();
                    if (obj != null)
                    {
                        obj.AddProccessCount();

                        try
                        {
                            MethodInfo methodInfo = type.GetMethod(func);
                            if (methodInfo != null)
                            {
                                result = methodInfo.Invoke(obj.Instance, param);
                            }
                        }
                        catch (Exception ex)
                        {
                            OnRemotingServerError(obj.Name, obj.Uri, ex.ToString());
                        }
                        finally
                        {
                            obj.SubtractProccessCount();
                        }
                    }
                }
                else
                {
                    bool flag = false;
                    while (obj != null && !flag)
                    {
                        obj.AddProccessCount();

                        try
                        {
                            MethodInfo methodInfo = type.GetMethod(func);
                            if (methodInfo != null)
                            {
                                result = methodInfo.Invoke(obj.Instance, param);
                            }
                            flag = true;
                        }
                        catch (SocketException ex1)
                        {
                            MethodInfo method = typeof(RemotingClientManager).GetMethod("SetFailuredTime", BindingFlags.NonPublic | BindingFlags.Static);
                            method = method.MakeGenericMethod(type);
                            method.Invoke(null, new object[] { obj });
                            OnRemotingServerError(obj.Name, obj.Uri, ex1.ToString());
                        }
                        catch (TargetInvocationException ex2)
                        {
                            MethodInfo method = typeof(RemotingClientManager).GetMethod("SetFailuredTime", BindingFlags.NonPublic | BindingFlags.Static);
                            method = method.MakeGenericMethod(type);
                            method.Invoke(null, new object[] { obj });
                            OnRemotingServerError(obj.Name, obj.Uri, ex2.ToString());
                        }
                        catch (Exception ex)
                        {
                            OnRemotingServerError(obj.Name, obj.Uri, ex.ToString());
                        }
                        finally
                        {
                            obj.SubtractProccessCount();
                        }

                        if (!flag)
                        {
                            obj = GetIdleRemotingHandlerItem<T>();
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获得闲置的Remoting客户端（通过 Type 的方式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RemotingHandlerItem<T> GetIdleRemotingHandlerItem<T>() where T : class
        {
            Type type = typeof(T);
            string strType = type.ToString();
            RemotingHandlerItem<T> result = null;
            if (_RemotingClientCache.ContainsKey(strType))
            {
                List<object> lists = _RemotingClientCache[strType];

                if (lists.Count == 1)
                {
                    result = lists[0] as RemotingHandlerItem<T>;
                    if (result.Status != ERemotingClientStatus.Working)
                    {
                        result = null;
                    }
                }
                else
                {
                    Type t = typeof(RemotingHandlerItem<>);
                    t = t.MakeGenericType(type);

                    PropertyInfo statusPropertyInfo = t.GetProperty("Status");
                    PropertyInfo processCountInfo = t.GetProperty("ProcessCount");
                    int maxValue = int.MaxValue;
                    foreach (object obj in lists)
                    {
                        object statusValue = statusPropertyInfo.GetValue(obj, null);
                        ERemotingClientStatus status = (ERemotingClientStatus)statusValue;
                        if (status == ERemotingClientStatus.Working)
                        {
                            object processCountValue = processCountInfo.GetValue(obj, null);
                            int processCount = int.MaxValue;
                            Int32.TryParse(processCountValue.ToString(), out processCount);
                            if (processCount < maxValue)
                            {
                                result = (RemotingHandlerItem<T>)obj;
                                maxValue = processCount;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获得随机的Remoting客户端（通过 Type 的方式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RemotingHandlerItem<T> GetRandomRemotingHandlerItem<T>() where T : class
        {
            Type type = typeof(T);
            string strType = type.ToString();
            RemotingHandlerItem<T> result = null;
            if (_RemotingClientCache.ContainsKey(strType))
            {
                List<object> lists = _RemotingClientCache[strType];
                if (lists.Count == 1)
                {
                    result = lists[0] as RemotingHandlerItem<T>;
                    if (result.Status != ERemotingClientStatus.Working)
                    {
                        result = null;
                    }
                }
                else
                {
                    List<object> results = lists.FindAll(obj => ((RemotingHandlerItem<T>)obj).Status == ERemotingClientStatus.Working);
                    int count = results.Count;
                    if (count == 1)
                    {
                        result = results[0] as RemotingHandlerItem<T>;
                    }
                    else if (count > 1)
                    {
                        result = results[GetRandom(count)] as RemotingHandlerItem<T>;
                    }
                }
            }
            return result;
        }

    }
}
