#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  RemoveServerModule 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2015/3/4 17:38 
     * 描述    : 处理HTTP头信息去掉Server键
     * =====================================================================
     * 修改时间：2015/3/4 17:38
     * 修改人  ：wangyunpeng
     * 版本号  ：V1.0.0.0 
     * 描述    ：处理HTTP头信息去掉Server键
*/
#endregion
using System;
using System.Collections.Specialized;
using System.Web;

namespace FAN.WebStyle
{
    /// <summary>
    /// 处理HTTP头信息去掉Server键,这里只能单独定义IHttpModule,不能放到Global.asax.cs中
    /// </summary>
    public class RemoveServerModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders -= new EventHandler(this.context_PreSendRequestHeaders);
            context.PreSendRequestHeaders += new EventHandler(this.context_PreSendRequestHeaders);
        }

        private void context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            try
            {
                HttpApplication application = sender as HttpApplication;
                if (null != application
                    && null != application.Request
                    && !application.Request.IsLocal
                    && null != application.Context
                    && null != application.Context.Response)
                {
                    NameValueCollection headers = application.Context.Response.Headers;
                    if (null != headers)
                    {
                        headers.Remove("Server");
                    }
                }
            }
            catch (Exception)
            {   
            }
        }
    }
}