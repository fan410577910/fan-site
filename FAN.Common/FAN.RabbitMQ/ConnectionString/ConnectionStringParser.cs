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
using System.Linq;
using FAN.RabbitMQ.Sprache;

namespace FAN.RabbitMQ
{
    public static class ConnectionStringParser
    {
        /// <summary>
        /// 将连接字符串转换为对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static ConnectionConfiguration Parse(string connectionString)
        {
            ConnectionConfiguration connectionConfiguration = null;
            try
            {
                var updater = ConnectionStringGrammar.ConnectionStringBuilder.Parse(connectionString);
                connectionConfiguration = updater.Aggregate(new ConnectionConfiguration(), (current, updateFunction) => updateFunction(current));
                connectionConfiguration.Validate();
            }
            catch (Exception parseException)
            {
                throw new Exception(string.Format("Connection String {0}", parseException.Message));
            }
            return connectionConfiguration;
        }
    }
}
