#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  RabbitMQAttribute 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyp 
     * 创建时间：2015/1/7 13:03:09 
     * 描述    : 获取RabbitMQ元数据
     * =====================================================================
     * 修改时间：2015/1/7 13:03:09 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Linq;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 获取RabbitMQ信息的元描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RabbitMQMetaAttribute : Attribute
    {
        private string _queueName = string.Empty;
        /// <summary>
        /// Queue名称
        /// </summary>
        public string QueueName
        {
            get { return this._queueName; }
            set { this._queueName = value; }
        }

        private string _exchangeName = string.Empty;
        /// <summary>
        /// Exchange名称
        /// </summary>
        public string ExchangeName
        {
            get { return this._exchangeName; }
            set { this._exchangeName = value; }
        }

        private string _routingKey = string.Empty;
        /// <summary>
        /// RoutingKey名称
        /// </summary>
        public string RoutingKey
        {
            get { return this._routingKey; }
            set { this._routingKey = value; }
        }

        private string _exchangeType = FAN.RabbitMQ.Topology.ExchangeType.Topic;
        /// <summary>
        /// ExchangeType名称
        /// </summary>
        public string ExchangeType
        {
            get { return this._exchangeType; }
            set { this._exchangeType = value; }
        }

        private string _type = string.Empty;

        /// <summary>
        /// MessageProperties的Type，（.NET类型完全限定名）
        /// </summary>
        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }

        /// <summary>
        /// 得到RabbitMQ的Meta信息
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        private static RabbitMQMetaAttribute GetMetaAttribute(Type type)
        {
            RabbitMQMetaAttribute attribute = type.GetCustomAttributes(typeof(RabbitMQMetaAttribute), true).FirstOrDefault() as RabbitMQMetaAttribute;
            return attribute ?? new RabbitMQMetaAttribute();
        }
        /// <summary>
        /// 得到RabbitMQ的Meta信息
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <param name="exchangeName">Exchange名称</param>
        /// <param name="exchangeType">ExchangeType名称</param>
        /// <param name="queueName">Queue名称</param>
        /// <param name="routingKey">RoutingKey名称</param>
        /// <param name="type">type名称(EasyNetQ中.NET类型完全限定名)</param>
        public static void GetMetaInfo(Type t, out string exchangeName, out string exchangeType, out string queueName, out string routingKey,out string type)
        {
            RabbitMQMetaAttribute attribute = GetMetaAttribute(t);
            exchangeName = attribute.ExchangeName;
            exchangeType = attribute.ExchangeType;
            queueName = attribute.QueueName;
            routingKey = attribute.RoutingKey;
            type = attribute.Type;
        }
    }
}
