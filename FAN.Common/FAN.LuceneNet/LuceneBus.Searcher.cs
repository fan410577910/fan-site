#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Searcher 
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
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.Collections.Generic;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        private static Dictionary<string, IndexSearcher> _IndexSearcherDict = new Dictionary<string, IndexSearcher>();
        private static object _LockSearcher = new object();
        /// <summary>
        /// 获取搜索索引的对象
        /// </summary>
        /// <param name="directory">语言名称</param>
        /// <returns></returns>
        public static IndexSearcher GetSearcher(string language)
        {
            string luceneDirectory = System.IO.Path.Combine(LuceneNetConfig.LuceneDirectory, language);
            IndexSearcher indexSearcher = null;
            if (_IndexSearcherDict.ContainsKey(language))
            {
                indexSearcher = _IndexSearcherDict[language];
            }
            if (indexSearcher != null)
            {
                return indexSearcher = GetSearcher(indexSearcher, GetDirectory(luceneDirectory));
            }
            return GetSearcher(language, GetDirectory(luceneDirectory));
        }
        /// <summary>
        /// 获取搜索索引的对象
        /// </summary>
        /// <param name="language"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static IndexSearcher GetSearcher(string language, Directory directory)
        {
            IndexSearcher indexSearcher = null;
            if (_IndexSearcherDict.ContainsKey(language))
            {
                indexSearcher = _IndexSearcherDict[language];
            }
            if (indexSearcher == null)
            {
                lock (_LockSearcher)
                {
                    if (_IndexSearcherDict.ContainsKey(language))
                    {
                        indexSearcher = _IndexSearcherDict[language];
                    }
                    if (indexSearcher == null)
                    {
                        _IndexSearcherDict[language] = indexSearcher = new IndexSearcher(GetReader(directory));
                    }
                }
            }
            return GetSearcher(indexSearcher, directory);
        }
        /// <summary>
        /// 获取搜索索引的对象
        /// </summary>
        /// <param name="indexSearcher"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        private static IndexSearcher GetSearcher(IndexSearcher indexSearcher, Directory directory)
        {
            IndexReader indexReader = indexSearcher.IndexReader;
            if (!indexReader.Directory().isOpen_ForNUnit)
            {//this Directory is closed
                indexSearcher.Dispose();
                indexSearcher = new IndexSearcher(GetReader(directory));
                indexReader = indexSearcher.IndexReader;
            }
            if (!indexReader.IsCurrent())
            {//Check whether any new changes have occurred to the index since this reader was opened.
                indexSearcher.Dispose();
                indexSearcher = new IndexSearcher(indexReader.Reopen());
            }
            return indexSearcher;
        }
        /// <summary>
        /// 关闭IndexSearcher
        /// </summary>
        public static void Close()
        {
            foreach (IndexSearcher indexSearcher in _IndexSearcherDict.Values)
            {
                IndexReader indexReader = indexSearcher.IndexReader;
                indexSearcher.Dispose();
                Close(indexReader);
            }
            _IndexSearcherDict.Clear();
        }

    }
}
