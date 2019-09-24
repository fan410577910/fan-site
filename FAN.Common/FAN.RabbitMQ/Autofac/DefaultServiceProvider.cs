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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 迷你Ioc容器。自制autofac框架。嘿嘿。
    /// </summary>
    public class DefaultServiceProvider : IContainer
    {
        private readonly IDictionary<string, object> _factories = new Dictionary<string, object>();
        private readonly IDictionary<string, Type> _registrations = new Dictionary<string, Type>();

        private readonly IDictionary<string, object> _instances = new Dictionary<string, object>();

        private static DefaultServiceProvider _Instance = null;
        public static DefaultServiceProvider Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DefaultServiceProvider();
                }
                return _Instance;
            }
        }

        private bool ServiceIsRegistered(string typeName)
        {
            return this._factories.ContainsKey(typeName) || this._registrations.ContainsKey(typeName);
        }

        public IServiceRegister Register<T>(Func<IServiceProvider, T> ctor) where T : class
        {
            Preconditions.CheckNotNull(ctor, "serviceCreator");
            Type type = typeof(T);
            string typeName = type.FullName;
            if (this.ServiceIsRegistered(typeName))
            {
                return this;
            }
            this._factories.Add(typeName, ctor);
            return this;
        }

        public T Resolve<T>() where T : class
        {
            Type serivceType = typeof(T);

            string typeName = serivceType.FullName;

            if (!this.ServiceIsRegistered(typeName))
            {
                throw new Exception(string.Format("类型名称 {0} 已经被注册过！", typeName));
            }

            if (!this._instances.ContainsKey(typeName))
            {
                if (this._registrations.ContainsKey(typeName))
                {
                    Type type = this._registrations[typeName];
                    object @object = this.CreateServiceInstance(type);
                    this._instances.Add(typeName, @object);
                }

                if (this._factories.ContainsKey(typeName))
                {
                    object @object = ((Func<IServiceProvider, T>)this._factories[typeName])(this);
                    this._instances.Add(typeName, @object);
                }
            }

            return (T)this._instances[typeName];
        }

        private object Resolve(Type serviceType)
        {
            return typeof(DefaultServiceProvider).GetMethod("Resolve", new Type[0]).MakeGenericMethod(serviceType).Invoke(this, new object[0]);
        }

        private object CreateServiceInstance(Type type)
        {
            var constructors = type.GetConstructors();

            ParameterInfo[] parameterInfos = constructors[0].GetParameters();
            var parameters = parameterInfos.Select(parameterInfo => this.Resolve(parameterInfo.ParameterType)).ToArray();

            return constructors[0].Invoke(parameters);
        }

        public IServiceRegister Register<T>() where T : class
        {
            Type type = typeof(T);
            string typeName = type.FullName;
            
            if (this.ServiceIsRegistered(typeName)) return this;

            var constructors = type.GetConstructors();
            if (constructors.Length != 1)
            {
                throw new Exception(string.Format("注册类型必须至少要有一个构造函数. 目前这个 {0} 类型里面有 {1} 个构造函数", type.Name, constructors.Length.ToString()));
            }

            this._registrations.Add(typeName, type);
            return this;
        }
    }
}
