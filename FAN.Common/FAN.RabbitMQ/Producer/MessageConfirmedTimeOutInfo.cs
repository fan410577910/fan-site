#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2017 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-JLFFS1KMVG 
     * 文件名：  MessageConfirmedTimeOutInfo 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间： 2017/8/24 10:15:07 
     * 描述    :
     * =====================================================================
     * 修改时间：2017/8/24 10:15:07 
     * 修改人  ： wangyunpeng
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion

namespace FAN.RabbitMQ
{
    /// <summary>
    /// 生产者发送消息（带事务）到达服务器之后的指定超过时间后仍未返回的信息。
    /// </summary>
    public class MessageConfirmedTimeOutInfo
    {
        public ulong SequenceNumber { get; set; }
        /// <summary>
        /// 超时消息内容
        /// </summary>
        public string Message { get; set; }
        public MessageConfirmedTimeOutInfo(ulong sequenceNumber, string message)
        {
            this.SequenceNumber = sequenceNumber;
            this.Message = message;
        }
    }
}
