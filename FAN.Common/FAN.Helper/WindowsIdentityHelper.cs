#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  WindowsIdentityHelper 
     * 版本号：  V1.0.0.0 
     * 创建人：  王威 
     * 创建时间：2016/3/11 12:34:24 
     * 描述    : windows登录凭据帮助类型(前提是远程机器的目录必须设置网络共享目录)
     * =====================================================================
     * 修改时间：2016/3/11 12:34:24 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace FAN.Helper
{
    /// <summary>
    /// windows远程登录凭据帮助类型(前提是远程机器的目录必须设置网络共享目录)
    /// </summary>
    public class WindowsIdentityHelper : IDisposable
    {
        private IntPtr _userToken = IntPtr.Zero;
        private WindowsImpersonationContext _windowsImpersionationContext = null;
        /// <summary>
        /// windows远程登录凭据帮助类型
        /// </summary>
        /// <param name="loginName">远程访问机器的登录名称</param>
        /// <param name="password">远程访问机器的登录密码</param>
        /// <param name="domain">远程访问机器的IP或者WINS名称</param>
        public WindowsIdentityHelper(string loginName, string password, string domain)
        {
            if (LogonUser(loginName, domain, password, 9, 0, ref this._userToken) != 0)
            {
                this._windowsImpersionationContext = new WindowsIdentity(this._userToken).Impersonate();
            }
        }

        [DllImport("advapi32.DLL", SetLastError = true)]
        private static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        /// <summary>
        /// 释放远程登录信息
        /// </summary>
        public void Dispose()
        {
            if (this._windowsImpersionationContext != null)
            {
                this._windowsImpersionationContext.Undo();
                this._windowsImpersionationContext = null;
            }
            this._userToken = IntPtr.Zero;
        }
    }
}
