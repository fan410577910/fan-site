#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2017 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-JLFFS1KMVG 
     * 文件名：  ConfirmedMessageTimeOutEvent 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间： 2017/8/24 10:09:03 
     * 描述    : 消息确认超时事件
     * =====================================================================
     * 修改时间：2017/8/24 10:09:03 
     * 修改人  ： wangyunpeng
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 消息确认超时事件
    /// </summary>
    public class ConfirmedMessageTimeOutEvent
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public byte[] Body { get; set; }
        public MessageProperties Properties { get; set; }
        public MessageConfirmedTimeOutInfo Info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="properties"></param>
        /// <param name="info"></param>
        public ConfirmedMessageTimeOutEvent(byte[] body, MessageProperties properties, MessageConfirmedTimeOutInfo info)
        {
            this.Body = body;
            this.Properties = properties;
            this.Info = info;
        }
    }
}
