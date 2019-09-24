#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  FilterDict 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/15
     * 描述    : 过滤信息字典数据
     * =====================================================================
     * 修改时间：2014/7/15
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 过滤信息字典数据
    /// </summary>
    public class FilterDict
    {
        private const string FILTER_FILE_NAME = "FilterInfo.xml";
        private static Dictionary<string, FilterInfo> _FilterInfoDict = null;
        private static object lockObject = new object();

        static FilterDict()
        {
            if (LuceneNetConfig.ChildrenCultureDirectoryList != null && LuceneNetConfig.ChildrenCultureDirectoryList.Count > 0)
            {
                if (!System.Web.Hosting.HostingEnvironment.IsHosted)
                {
                    _FilterInfoDict = new Dictionary<string, FilterInfo>(LuceneNetConfig.ChildrenCultureDirectoryList.Count);
                    InitDict();
                    string filePath = null;
                    foreach (string childDirectory in LuceneNetConfig.ChildrenCultureDirectoryList)
                    {
                        filePath = Path.Combine(LuceneNetConfig.LuceneDictDirectory, childDirectory, FILTER_FILE_NAME);
                        if (File.Exists(filePath))
                        {
                            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.Combine(LuceneNetConfig.LuceneDictDirectory, childDirectory), FILTER_FILE_NAME);
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
                    FilterInfo filterInfo = null;
                    foreach (string childDirectory in LuceneNetConfig.ChildrenCultureDirectoryList)
                    {
                        applicationPath = Path.Combine(LuceneNetConfig.LuceneDictDirectory, childDirectory, FILTER_FILE_NAME);
                        if (File.Exists(applicationPath))
                        {
                            Encoding encoding = EncodingType.GetType(applicationPath);
                            try
                            {
                                filterInfo = SerializeHelper.XmlDeserialize<FilterInfo>(applicationPath, encoding);
                            }
                            catch
                            {
                                filterInfo = new FilterInfo();
                            }
                        }
                        else
                        {
                            filterInfo = new FilterInfo();
                        }
                        _FilterInfoDict[childDirectory] = filterInfo;
                    }
                }
            }
        }
        public static FilterInfo FilterInfo(string language)
        {
            FilterInfo filterInfo = null;
            if (_FilterInfoDict.ContainsKey(language))
            {
                filterInfo = _FilterInfoDict[language];
            }
            if (filterInfo == null)
            {
                filterInfo = new FilterInfo();
            }
            return filterInfo;
        }

    }
}
