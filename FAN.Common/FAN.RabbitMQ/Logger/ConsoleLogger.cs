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
    public static class ConsoleLogger
    {
        public static bool Debug { get; set; }
        public static bool Info { get; set; }
        public static bool Error { get; set; }

        static ConsoleLogger()
        {
            Debug = System.Diagnostics.Debugger.IsAttached;
            Info = true;
            Error = true;
        }

        public static void DebugWrite(string format, params object[] args)
        {
            if (!Debug) return;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            SafeConsoleWrite("DEBUG: " + format, args);
            Console.ResetColor();
        }

        public static void InfoWrite(string format, params object[] args)
        {
            if (!Info) return;
            SafeConsoleWrite("INFO: " + format, args);
        }

        public static void ErrorWrite(Exception exception)
        {
            ErrorWrite(exception.ToString());
        }

        public static void ErrorWrite(string format, params object[] args)
        {
            if (!Error) return;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            SafeConsoleWrite("ERROR: " + format, args);
            Console.ResetColor();
        }

        private static void SafeConsoleWrite(string format, params object[] args)
        {
            // even a zero length args paramter causes WriteLine to interpret 'format' as
            // a format string. Rather than escape JSON, better to check the intention of 
            // the caller.
            if (args.Length == 0)
            {
                Console.WriteLine(format);
            }
            else
            {
                Console.WriteLine(format, args);
            }
        }

    }
}
