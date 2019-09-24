#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  CustomScoreDict
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TLZ.LuceneNet
{

    /// <summary>
    /// 自定义评分字典
    /// </summary>
    public class CustomScoreDict
    {
        public const string CUSTOMERSCORE_FILE_NAME = "CustomScore.xml";
        private static Dictionary<string, List<CustomScoreInfo>> _CustomScoreInfoDict = null;
        private static object lockObject = new object();
        static CustomScoreDict()
        {
            if (LuceneNetConfig.ChildrenCultureDirectoryList != null && LuceneNetConfig.ChildrenCultureDirectoryList.Count > 0)
            {
                _CustomScoreInfoDict = new Dictionary<string, List<CustomScoreInfo>>(LuceneNetConfig.ChildrenCultureDirectoryList.Count);
                InitDict();
                if (!System.Web.Hosting.HostingEnvironment.IsHosted)
                {
                    string filePath = null;
                    foreach (string childDirectory in LuceneNetConfig.ChildrenCultureDirectoryList)
                    {
                        filePath = Path.Combine(LuceneNetConfig.LuceneDictDirectory, childDirectory, CUSTOMERSCORE_FILE_NAME);
                        if (File.Exists(filePath))
                        {
                            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.Combine(LuceneNetConfig.LuceneDictDirectory, childDirectory), CUSTOMERSCORE_FILE_NAME);
                            fileSystemWatcher.Changed += fileSystemWatcher_Changed;
                            fileSystemWatcher.EnableRaisingEvents = true;
                        }
                    }
                }
            }
        }

        static void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            InitDict();
            LuceneNetConfig.OnConfigChangedEvent();
        }

        private static void InitDict()
        {
            if (LuceneNetConfig.ChildrenCultureDirectoryList != null && LuceneNetConfig.ChildrenCultureDirectoryList.Count > 0)
            {
                lock (lockObject)
                {
                    string applicationPath = null;
                    List<CustomScoreInfo> customScoreInfoList = null;
                    foreach (string childDirectory in LuceneNetConfig.ChildrenCultureDirectoryList)
                    {
                        applicationPath = Path.Combine(LuceneNetConfig.LuceneDictDirectory, childDirectory, CUSTOMERSCORE_FILE_NAME);
                        if (File.Exists(applicationPath))
                        {
                            Encoding encoding = EncodingType.GetType(applicationPath);
                            try
                            {
                                customScoreInfoList = SerializeHelper.XmlDeserialize<List<CustomScoreInfo>>(applicationPath, encoding);
                            }
                            catch
                            {
                                customScoreInfoList = new List<CustomScoreInfo>();
                            }
                            _CustomScoreInfoDict[childDirectory] = customScoreInfoList;
                        }
                    }
                }
            }
        }
        public static void SaveCustomScoreXML(string language, List<CustomScoreInfo> customScoreInfoList)
        {
            try
            {
                SerializeHelper.XmlSerialize(Path.Combine(LuceneNetConfig.LuceneDictDirectory, language, CUSTOMERSCORE_FILE_NAME), customScoreInfoList);
            }
            catch (Exception ex)
            {
                throw new Exception("保存" + CUSTOMERSCORE_FILE_NAME + "失败了！" + ex.Message);
            }
        }
        /// <summary>
        /// 根据某一个域的值进行自定义评分的设置信息
        /// </summary>
        public static List<CustomScoreInfo> CustomScoreInfos(string language)
        {
            List<CustomScoreInfo> customScoreInfoList = null;
            if (_CustomScoreInfoDict != null && _CustomScoreInfoDict.ContainsKey(language))
            {
                customScoreInfoList = _CustomScoreInfoDict[language];
            }
            if (customScoreInfoList == null)
            {
                customScoreInfoList = new List<CustomScoreInfo>();
            }
            return customScoreInfoList;
        }
    }
}
