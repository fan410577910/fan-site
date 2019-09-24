#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2017 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-JLFFS1KMVG 
     * 文件名：  ProcessHelper 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间： 2017/3/10 16:20:54 
     * 描述    :
     * =====================================================================
     * 修改时间：2017/3/10 16:20:54 
     * 修改人  ： wangyunpeng
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Diagnostics;

namespace FAN.Helper
{
    public class ProcessHelper
    {
        /// <summary>
        /// 启动一个新的进程执行exe程序
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        public static void Start(string fileName, string args)
        {
            Process process = null;
            string message = null;
            try
            {
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("{0} {1}", fileName, args);
                }
                process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = fileName,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        CreateNoWindow = true,
                        Arguments = args,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    message = process.StandardError.ReadToEnd();
                }
            }
            finally
            {
                if (process != null)
                {
                    process.Dispose();
                }
            }
            if (message != null)
            {
                throw new Exception(message);
            }
        }
    }
}
