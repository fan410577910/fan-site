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

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 定义Ioc容器的规则
    /// </summary>
    public interface IContainer : IServiceProvider, IServiceRegister
    { }

    /// <summary>
    /// 服务接口
    /// </summary>
    public interface IServiceProvider
    {
        T Resolve<T>() where T : class;
    }

    /// <summary>
    /// 注册服务
    /// </summary>
    public interface IServiceRegister
    {
        IServiceRegister Register<T>(Func<IServiceProvider, T> serviceCreator) where T : class;

        IServiceRegister Register<T>() where T : class;
    }
}
