#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  SynonymsDict 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 同义词，近义词，相关词字典
     * =====================================================================
     * 修改时间：2014/7/28
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
    /// 同义词，近义词，相关词字典
    /// </summary>
    public static class SynonymDict
    {
        private const string SYNONYM_FILE_NAME = "Synonym.txt";
        private static Dictionary<string, Dictionary<string, string[]>> _SynonymsDict = null;
        private static object lockObject = new object();

        static SynonymDict()
        {
            if (LuceneNetConfig.ChildrenCultureDirectoryList != null && LuceneNetConfig.ChildrenCultureDirectoryList.Count > 0)
            {
                _SynonymsDict = new Dictionary<string, Dictionary<string, string[]>>(LuceneNetConfig.ChildrenCultureDirectoryList.Count);
                InitDict();
                if (!System.Web.Hosting.HostingEnvironment.IsHosted)
                {
                    string filePath = null;
                    foreach (string childDirectory in LuceneNetConfig.ChildrenCultureDirectoryList)
                    {
                        filePath = Path.Combine(LuceneNetConfig.LuceneDictDirectory, childDirectory, SYNONYM_FILE_NAME);
                        if (File.Exists(filePath))
                        {
                            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.Combine(LuceneNetConfig.LuceneDictDirectory, childDirectory), SYNONYM_FILE_NAME);
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
                    foreach (string childDirectory in LuceneNetConfig.ChildrenCultureDirectoryList)
                    {
                        applicationPath = Path.Combine(LuceneNetConfig.LuceneDictDirectory, childDirectory, SYNONYM_FILE_NAME);
                        if (File.Exists(applicationPath))
                        {
                            Dictionary<string, string[]> synonymsDict = new Dictionary<string, string[]>();
                            Encoding encoding = EncodingType.GetType(applicationPath);
                            using (StreamReader sr = new StreamReader(applicationPath, encoding))
                            {
                                while (!sr.EndOfStream)
                                {
                                    string line = sr.ReadLine();
                                    if (!string.IsNullOrEmpty(line))
                                    {
                                        string[] synonymsWords = SplitWordTool.SplitWord(line);
                                        if (synonymsWords.Length > 0)
                                        {
                                            synonymsDict[line] = synonymsWords;
                                        }
                                    }
                                }
                            }
                            _SynonymsDict[childDirectory] = synonymsDict;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取同义词
        /// </summary>
        /// <param name="language"></param>
        /// <param name="word">一个字</param>
        /// <returns></returns>
        internal static List<string> GetSynonymsWord(string language, string word)
        {
            List<string> synonymsWordList = new List<string>();
            Dictionary<string, string[]> synonymsDict = null;
            if (_SynonymsDict.ContainsKey(language))
            {
                synonymsDict = _SynonymsDict[language];
            }
            if (synonymsDict != null)
            {
                foreach (KeyValuePair<string, string[]> pair in synonymsDict)
                {
                    if (pair.Key.ToLower().Contains(word))
                    {
                        synonymsWordList.AddRange(pair.Value);
                    }
                }
                if (!synonymsWordList.Contains(word))
                {
                    synonymsWordList.Add(word);
                }
            }
            return synonymsWordList;
        }
    }
}
