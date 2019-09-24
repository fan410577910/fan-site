#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneNetConfig 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 配置文件信息
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;

namespace FAN.LuceneNet
{
    /// <summary>
    /// 配置文件信息
    /// </summary>
    public static class LuceneNetConfig
    {
        public const string LUCENE_DIRECTORY = "LuceneDirectory";
        public const string LUCENE_DICT_DIRECTORY = "LuceneDictDirectory";
        public const string LUCENE_WEBPAGE_DIRECTORY = "LuceneWebPageDirectory";
        /// <summary>
        /// 应用程序根路径，或者是Web应用程序根路径
        /// </summary>
        public static string LuceneDirectory = null;
        /// <summary>
        /// 字典文件路径
        /// </summary>
        public static string LuceneDictDirectory = null;
        /// <summary>
        /// 网站静态页面文件存放位置
        /// </summary>
        public static string LuceneWebPageDirectory = null;
        /// <summary>
        /// 获取当前Lucene的Dict的所有多语言子目录
        /// </summary>
        internal static List<string> ChildrenCultureDirectoryList = null;

        /// <summary>
        /// 程序运行时的根目录
        /// </summary>
        private static readonly string RootDirectory = System.Web.Hosting.HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath : AppDomain.CurrentDomain.BaseDirectory;

        private static event Action _ConfigChangedEvent;

        public static event Action ConfigChangedEvent
        {
            add
            {
                Action handler2;
                Action handler3 = _ConfigChangedEvent;
                do
                {
                    handler2 = handler3;
                    Action handler4 = (Action)Delegate.Combine(handler2, value);
                    handler3 = Interlocked.CompareExchange<Action>(ref _ConfigChangedEvent, handler4, handler2);
                }
                while (handler3 != handler2);
            }
            remove
            {
                Action handler2;
                Action handler3 = _ConfigChangedEvent;
                do
                {
                    handler2 = handler3;
                    Action handler4 = (Action)Delegate.Remove(handler2, value);
                    handler3 = Interlocked.CompareExchange<Action>(ref _ConfigChangedEvent, handler4, handler2);
                }
                while (handler3 != handler2);
            }
        }
        static LuceneNetConfig()
        {
            InitConfig();
            if (!System.Web.Hosting.HostingEnvironment.IsHosted)
            {
                string currentConfig = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                string currentPath = Path.GetDirectoryName(currentConfig);
                string currentFile = Path.GetFileName(currentConfig);
                FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(currentPath, currentFile);
                fileSystemWatcher.Changed += fileSystemWatcher_Changed;
                fileSystemWatcher.EnableRaisingEvents = true;
            }
        }
        static void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            do
            {
                try
                {
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                    break;
                }
                catch (System.Configuration.ConfigurationErrorsException)
                {
                    Thread.Sleep(10);
                }
            }
            while (true);
            LuceneBus.Close();
            InitConfig();
            OnConfigChangedEvent();
        }
        internal static void OnConfigChangedEvent()
        {
            if (_ConfigChangedEvent != null)
            {
                _ConfigChangedEvent();
            }
        }
        private static void InitConfig()
        {
            string luceneDirectory = GetAppSettingValue(LUCENE_DIRECTORY);
            if (string.IsNullOrEmpty(luceneDirectory))
            {
                LuceneDirectory = RootDirectory;
            }
            else
            {
                LuceneDirectory = luceneDirectory;
            }
            string luceneDictDirectory = GetAppSettingValue(LUCENE_DICT_DIRECTORY);
            if (string.IsNullOrEmpty(luceneDictDirectory))
            {
                LuceneDictDirectory = RootDirectory;
            }
            else
            {
                LuceneDictDirectory = luceneDictDirectory;
            }
            LuceneWebPageDirectory = GetAppSettingValue(LUCENE_WEBPAGE_DIRECTORY);
            ChildrenCultureDirectoryList = GetChildDirectory(LuceneDictDirectory);
        }
        /// <summary>
        /// 获取当前目录的子目录名，不包含子目录里面的目录名称
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static List<string> GetChildDirectory(string dir)
        {
            List<string> childDirectoryList = null;
            if (Directory.Exists(dir))
            {
                DirectoryInfo[] directoryInfos = new DirectoryInfo(dir).GetDirectories();
                childDirectoryList = new List<string>(directoryInfos.Length);
                foreach (DirectoryInfo directoryInfo in directoryInfos)
                {
                    childDirectoryList.Add(directoryInfo.Name);
                }
                Array.Clear(directoryInfos, 0, directoryInfos.Length);
                directoryInfos = null;
            }
            return childDirectoryList;
        }

        /// <summary>
        /// 读取appSettings中的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettingValue(string key)
        {
            string value = null;
            foreach (string item in ConfigurationManager.AppSettings)
            {
                if (item.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                {
                    value = ConfigurationManager.AppSettings[key];
                    break;
                }
            }
            return value;
        }

    }
}
