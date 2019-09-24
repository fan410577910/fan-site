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
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 封装了RabbitMQ里面的IBasicProperties接口类型数据。
    /// </summary>
    public class MessageProperties
    {
        public MessageProperties()
        {
            this.Headers = new Dictionary<string, object>();
        }

        public MessageProperties(IBasicProperties basicProperties)
            : this()
        {
            this.CopyFrom(basicProperties);
        }

        public void CopyFrom(IBasicProperties basicProperties)
        {
            Preconditions.CheckNotNull(basicProperties, "basicProperties");

            if (basicProperties.IsContentTypePresent()) this.ContentType = basicProperties.ContentType;
            if (basicProperties.IsContentEncodingPresent()) this.ContentEncoding = basicProperties.ContentEncoding;
            if (basicProperties.IsDeliveryModePresent()) this.DeliveryMode = basicProperties.DeliveryMode;
            if (basicProperties.IsPriorityPresent()) this.Priority = basicProperties.Priority;
            if (basicProperties.IsCorrelationIdPresent()) this.CorrelationId = basicProperties.CorrelationId;
            if (basicProperties.IsReplyToPresent()) this.ReplyTo = basicProperties.ReplyTo;
            if (basicProperties.IsExpirationPresent()) this.Expiration = basicProperties.Expiration;
            if (basicProperties.IsMessageIdPresent()) this.MessageId = basicProperties.MessageId;
            if (basicProperties.IsTimestampPresent()) this.Timestamp = basicProperties.Timestamp.UnixTime;
            if (basicProperties.IsTypePresent()) this.Type = basicProperties.Type;
            if (basicProperties.IsUserIdPresent()) this.UserId = basicProperties.UserId;
            if (basicProperties.IsAppIdPresent()) this.AppId = basicProperties.AppId;
            if (basicProperties.IsClusterIdPresent()) this.ClusterId = basicProperties.ClusterId;

            if (basicProperties.IsHeadersPresent())
            {
                foreach (var header in basicProperties.Headers)
                {
                    this.Headers.Add(header.Key, header.Value);
                }
            }
        }

        public void CopyTo(IBasicProperties basicProperties)
        {
            Preconditions.CheckNotNull(basicProperties, "basicProperties");

            if (this._contentTypePresent) basicProperties.ContentType = this.ContentType;
            if (this._contentEncodingPresent) basicProperties.ContentEncoding = this.ContentEncoding;
            if (this._deliveryModePresent) basicProperties.DeliveryMode = this.DeliveryMode;
            if (this._priorityPresent) basicProperties.Priority = this.Priority;
            if (this._correlationIdPresent) basicProperties.CorrelationId = this.CorrelationId;
            if (this._replyToPresent) basicProperties.ReplyTo = this.ReplyTo;
            if (this._expirationPresent) basicProperties.Expiration = this.Expiration;
            if (this._messageIdPresent) basicProperties.MessageId = this.MessageId;
            if (this._timestampPresent) basicProperties.Timestamp = new AmqpTimestamp(this.Timestamp);
            if (this._typePresent) basicProperties.Type = this.Type;
            if (this._userIdPresent) basicProperties.UserId = this.UserId;
            if (this._appIdPresent) basicProperties.AppId = this.AppId;
            if (this._clusterIdPresent) basicProperties.ClusterId = this.ClusterId;

            if (this._headersPresent)
            {
                basicProperties.Headers = new Dictionary<string, object>(this.Headers);
            }
        }

        private bool _contentTypePresent = false;
        private bool _contentEncodingPresent = false;
        private bool _headersPresent = false;
        private bool _deliveryModePresent = false;
        private bool _priorityPresent = false;
        private bool _correlationIdPresent = false;
        private bool _replyToPresent = false;
        private bool _expirationPresent = false;
        private bool _messageIdPresent = false;
        private bool _timestampPresent = false;
        private bool _typePresent = false;
        private bool _userIdPresent = false;
        private bool _appIdPresent = false;
        private bool _clusterIdPresent = false;

        private string _contentType;

        /// <summary>
        /// MIME Content type 
        /// </summary>
        public string ContentType
        {
            get { return this._contentType; }
            set { this._contentType = this.CheckShortString(value, "ContentType"); this._contentTypePresent = true; }
        }

        private string _contentEncoding;

        /// <summary>
        /// MIME content encoding 
        /// </summary>
        public string ContentEncoding
        {
            get { return this._contentEncoding; }
            set { this._contentEncoding = this.CheckShortString(value, "ContentEncoding"); this._contentEncodingPresent = true; }
        }

        private IDictionary<string, object> _headers;

        /// <summary>
        /// message header field table 
        /// </summary>
        public IDictionary<string, object> Headers
        {
            get { return this._headers; }
            set { this._headers = value; this._headersPresent = true; }
        }

        private byte _deliveryMode;

        /// <summary>
        /// non-persistent (1) or persistent (2) 
        /// 消息持久化
        /// </summary>
        public byte DeliveryMode
        {
            get { return this._deliveryMode; }
            set { this._deliveryMode = value; this._deliveryModePresent = true; }
        }

        private byte _priority;

        /// <summary>
        /// message priority, 0 to 9 
        /// </summary>
        public byte Priority
        {
            get { return this._priority; }
            set { this._priority = value; this._priorityPresent = true; }
        }

        private string _correlationId;

        /// <summary>
        /// application correlation identifier 
        /// </summary>
        public string CorrelationId
        {
            get { return this._correlationId; }
            set { this._correlationId = this.CheckShortString(value, "CorrelationId"); this._correlationIdPresent = true; }
        }

        private string _replyTo;

        /// <summary>
        /// destination to reply to 
        /// </summary>
        public string ReplyTo
        {
            get { return this._replyTo; }
            set { this._replyTo = this.CheckShortString(value, "ReplyTo"); this._replyToPresent = true; }
        }

        private string _expiration;

        /// <summary>
        /// message expiration specification 
        /// </summary>
        public string Expiration
        {
            get { return this._expiration; }
            set { this._expiration = this.CheckShortString(value, "Expiration"); this._expirationPresent = true; }
        }

        private string _messageId;

        /// <summary>
        /// application message identifier 
        /// </summary>
        public string MessageId
        {
            get { return this._messageId; }
            set { this._messageId = this.CheckShortString(value, "MessageId"); this._messageIdPresent = true; }
        }

        private long _timestamp;

        /// <summary>
        /// message timestamp 
        /// </summary>
        public long Timestamp
        {
            get { return this._timestamp; }
            set { this._timestamp = value; this._timestampPresent = true; }
        }

        private string _type;

        /// <summary>
        /// message type name 
        /// </summary>
        public string Type
        {
            get { return this._type; }
            set { this._type = this.CheckShortString(value, "Type"); this._typePresent = true; }
        }

        private string _userId;

        /// <summary>
        /// creating user id 
        /// </summary>
        public string UserId
        {
            get { return this._userId; }
            set { this._userId = this.CheckShortString(value, "UserId"); this._userIdPresent = true; }
        }

        private string _appId;

        /// <summary>
        /// creating application id 
        /// </summary>
        public string AppId
        {
            get { return this._appId; }
            set { this._appId = this.CheckShortString(value, "AppId"); this._appIdPresent = true; }
        }

        private string _clusterId;

        /// <summary>
        /// intra-cluster routing identifier 
        /// </summary>
        public string ClusterId
        {
            get { return this._clusterId; }
            set { this._clusterId = this.CheckShortString(value, "ClusterId"); this._clusterIdPresent = true; }
        }

        public bool ContentTypePresent
        {
            get { return this._contentTypePresent; }
            set { this._contentTypePresent = value; }
        }

        public bool ContentEncodingPresent
        {
            get { return this._contentEncodingPresent; }
            set { this._contentEncodingPresent = value; }
        }

        public bool HeadersPresent
        {
            get { return this._headersPresent; }
            set { this._headersPresent = value; }
        }

        public bool DeliveryModePresent
        {
            get { return this._deliveryModePresent; }
            set { this._deliveryModePresent = value; }
        }

        public bool PriorityPresent
        {
            get { return this._priorityPresent; }
            set { this._priorityPresent = value; }
        }

        public bool CorrelationIdPresent
        {
            get { return this._correlationIdPresent; }
            set { this._correlationIdPresent = value; }
        }

        public bool ReplyToPresent
        {
            get { return this._replyToPresent; }
            set { this._replyToPresent = value; }
        }

        public bool ExpirationPresent
        {
            get { return this._expirationPresent; }
            set { this._expirationPresent = value; }
        }

        public bool MessageIdPresent
        {
            get { return this._messageIdPresent; }
            set { this._messageIdPresent = value; }
        }

        public bool TimestampPresent
        {
            get { return this._timestampPresent; }
            set { this._timestampPresent = value; }
        }

        public bool TypePresent
        {
            get { return this._typePresent; }
            set { this._typePresent = value; }
        }

        public bool UserIdPresent
        {
            get { return this._userIdPresent; }
            set { this._userIdPresent = value; }
        }

        public bool AppIdPresent
        {
            get { return this._appIdPresent; }
            set { this._appIdPresent = value; }
        }

        public bool ClusterIdPresent
        {
            get { return this._clusterIdPresent; }
            set { this._clusterIdPresent = value; }
        }

        public void AppendPropertyDebugStringTo(StringBuilder stringBuilder)
        {
            base.GetType()
                .GetProperties()
                .Where(x => !x.Name.EndsWith("Present"))
                .Select(x => string.Format("{0}={1}", x.Name, GetValueString(x.GetValue(this, null))))
                .Intersperse(", ")
                .Aggregate(stringBuilder, (sb, x) =>
                {
                    sb.Append(x);
                    return sb;
                });
        }

        private string GetValueString(object value)
        {
            if (value == null) return "NULL";

            var dictionary = value as IDictionary<string, object>;
            if (dictionary == null) return value.ToString();

            var stringBuilder = new StringBuilder();

            dictionary
                .EnumerateDictionary()
                .Select(x => string.Format("{0}={1}", x.Key, x.Value))
                .Intersperse(", ")
                .SurroundWith("[", "]")
                .Aggregate(stringBuilder, (sb, x) =>
                {
                    sb.Append(x);
                    return sb;
                });

            return stringBuilder.ToString();
        }

        private string CheckShortString(string input, string name)
        {
            if (input == null) return null;

            if (input.Length > 255)
            {
                throw new Exception(string.Format("Exceeded maximum length of basic properties field '{0}'. Value: '{1}'", name, input));
            }

            return input;
        }
    }
}
