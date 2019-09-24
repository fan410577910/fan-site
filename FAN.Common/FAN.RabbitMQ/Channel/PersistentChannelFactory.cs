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

namespace FAN.RabbitMQ
{
    public class PersistentChannelFactory
    {
        private readonly ConnectionConfiguration _configuration;

        public PersistentChannelFactory(ConnectionConfiguration configuration)
        {
            Preconditions.CheckNotNull(configuration, "configuration");

            this._configuration = configuration;
        }

        public PersistentChannel CreatePersistentChannel(PersistentConnection connection)
        {
            Preconditions.CheckNotNull(connection, "connection");

            return new PersistentChannel(connection, this._configuration);
        }
    }
}
