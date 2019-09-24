
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FAN.Remoting
{
    /// <summary>
    /// RMI调用者
    /// wangyunpeng
    /// </summary>
    public class RMIInvoker : IDisposable
    {
        #region 变量
        private Dictionary<string, Assembly> _AssemblyDictionary = new Dictionary<string, Assembly>();
        private Dictionary<string, Type> _TypeDictionary = new Dictionary<string, Type>();
        private object lockObj = new object();
        #endregion
        #region 调用对象的非泛型和泛型方法
        /// <summary>
        /// 调用对象的非泛型方法
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoker(object instance, string methodName, params object[] parameters)
        {
            object returnValue = null;
            ParameterInfo[] parameterInfos = parameters as ParameterInfo[];
            object[] parameterValues = null;
            Type[] parameterTypes = null;
            GetParameterType(parameterInfos, out parameterValues, out parameterTypes);
            MethodInfo methodInfo = instance.GetType().GetMethod(methodName, parameterTypes);
            Array.Clear(parameterTypes, 0, parameterTypes.Length);
            parameterTypes = null;
            if (methodInfo != null)
            {
                returnValue = methodInfo.Invoke(instance, parameterValues);
            }
            methodInfo = null;
            RMIInfo rmiInfo = new RMIInfo();
            rmiInfo.ReturnValue = returnValue;
            rmiInfo.ParameterValues = parameterValues;
            return rmiInfo;
        }
        /// <summary>
        /// 调用对象的泛型方法
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="methodName"></param>
        /// <param name="generic"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoker(object instance, string methodName, Type generic, params object[] parameters)
        {
            object returnValue = null;
            ParameterInfo[] parameterInfos = parameters as ParameterInfo[];
            object[] parameterValues = null;
            Type[] parameterTypes = null;
            GetParameterType(parameterInfos, out parameterValues, out parameterTypes);
            MethodInfo methodInfo = instance.GetType().GetMethod(methodName, parameterTypes);
            if (methodInfo != null)
            {
                methodInfo = methodInfo.MakeGenericMethod(generic);
                Array.Clear(parameterTypes, 0, parameterTypes.Length);
                parameterTypes = null;
                if (methodInfo != null)
                {
                    returnValue = methodInfo.Invoke(instance, parameterValues);
                }
            }
            methodInfo = null;
            RMIInfo rmiInfo = new RMIInfo();
            rmiInfo.ReturnValue = returnValue;
            rmiInfo.ParameterValues = parameterValues;
            return rmiInfo;
        }
        #endregion
        #region 调用类的非泛型和泛型方法
        /// <summary>
        /// 调用类的非泛型方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoker(Type type, string methodName, params object[] parameters)
        {
            object returnValue = null;
            ParameterInfo[] parameterInfos = parameters as ParameterInfo[];
            object[] parameterValues = null;
            Type[] parameterTypes = null;
            GetParameterType(parameterInfos, out parameterValues, out parameterTypes);
            MethodInfo methodInfo = type.GetMethod(methodName, parameterTypes);
            Array.Clear(parameterTypes, 0, parameterTypes.Length);
            parameterTypes = null;
            if (methodInfo != null)
            {
                returnValue = methodInfo.Invoke(null, parameterValues);
            }
            methodInfo = null;
            RMIInfo rmiInfo = new RMIInfo();
            rmiInfo.ReturnValue = returnValue;
            rmiInfo.ParameterValues = parameterValues;
            return rmiInfo;
        }
        /// <summary>
        /// 调用类的非泛型方法
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoker(string assemblyName, string className, string methodName, params object[] parameters)
        {
            Type type = GetType(assemblyName, className);
            return Invoker(type, methodName, parameters);
        }
        /// <summary>
        /// 调用类的泛型方法
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="generic"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoker(string assemblyName, string className, string methodName, Type generic, params object[] parameters)
        {
            Type type = GetType(assemblyName, className);
            return Invoker(type, methodName, generic, parameters);
        }
        /// <summary>
        /// 调用类的泛型方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="generic"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoker(Type type, string methodName, Type generic, params object[] parameters)
        {
            object returnValue = null;
            ParameterInfo[] parameterInfos = parameters as ParameterInfo[];
            object[] parameterValues = null;
            Type[] parameterTypes = null;
            GetParameterType(parameterInfos, out parameterValues, out parameterTypes);
            MethodInfo methodInfo = type.GetMethod(methodName, parameterTypes);
            if (methodInfo != null)
            {
                methodInfo = methodInfo.MakeGenericMethod(generic);
                Array.Clear(parameterTypes, 0, parameterTypes.Length);
                parameterTypes = null;
                if (methodInfo != null)
                {
                    returnValue = methodInfo.Invoke(null, parameterValues);
                }
            }
            methodInfo = null;
            RMIInfo rmiInfo = new RMIInfo();
            rmiInfo.ReturnValue = returnValue;
            rmiInfo.ParameterValues = parameterValues;
            return rmiInfo;
        }
        #endregion
        #region 通过类名得到它的类型
        /// <summary>
        /// 得到类名的类型
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        private Type GetType(string assemblyName, string className)
        {
            Type type = null;
            string key = string.Concat(assemblyName, ".", className);
            if (_TypeDictionary.ContainsKey(key))
            {
                type = _TypeDictionary[key];
            }
            if (type == null)
            {
                Assembly assembly = null;
                if (_AssemblyDictionary.ContainsKey(assemblyName))
                {
                    assembly = _AssemblyDictionary[assemblyName];
                }
                if (assembly == null)
                {
                    assembly = Assembly.Load(assemblyName);
                    lock (lockObj)
                    {
                        _AssemblyDictionary[assemblyName] = assembly;
                    }
                }
                type = assembly.GetType(key);
                lock (lockObj)
                {
                    _TypeDictionary[key] = type;
                }
            }
            return type;
        }
        #endregion
        #region 通过参数信息得到参数的类型和参数值
        /// <summary>
        /// 通过参数信息得到参数的类型和参数值
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="objects"></param>
        /// <param name="types"></param>
        private void GetParameterType(ParameterInfo[] parameters, out object[] objects, out Type[] types)
        {
            Dictionary<object, Type> dict = new Dictionary<object, Type>();
            if (parameters != null)
            {
                objects = new object[parameters.Length];
                types = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    objects[i] = parameters[i].ParameterValue;
                    types[i] = Type.GetType(parameters[i].ParameterType);
                }
            }
            else
            {
                objects = new object[0];
                types = new Type[0];
            }
        }
        #endregion
        #region IDisposable
        public void Dispose()
        {
            _TypeDictionary.Clear();
            _AssemblyDictionary.Clear();
            _TypeDictionary = null;
            _AssemblyDictionary = null;
        }
        #endregion
    }
    /// <summary>
    /// RMI方式调用之后返回的结果（包括：方法返回值和参数值[in、ref、out]）
    /// </summary>
    [Serializable]
    public class RMIInfo
    {
        /// <summary>
        /// 调用方法得到的返回值
        /// </summary>
        public object ReturnValue { get; set; }
        /// <summary>
        /// 调用方法得到传递的参数信息[in、ref、out]
        /// </summary>
        public object[] ParameterValues { get; set; }
    }
    /// <summary>
    /// 参数信息（包括：字符串方式的参数类型和参数值）
    /// </summary>
    [Serializable]
    public class ParameterInfo
    {
        public object ParameterValue { get; set; }
        public string ParameterType { get; set; }
    }
}
