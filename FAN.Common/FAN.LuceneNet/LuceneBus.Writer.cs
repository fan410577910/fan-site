#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Update 
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
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System.Reflection;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 得到写入索引的对象
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static IndexWriter GetWriter(string language)
        {
            Directory directory = GetDirectory(System.IO.Path.Combine(LuceneNetConfig.LuceneDirectory, language));
            return GetWriter(directory, new AnalyzerBus(language));
            //return GetWriter(directory, AnalyzerDict.GetAnalyzer(language));
        }
        /// <summary>
        /// 得到写入索引的对象
        /// </summary>
        /// <param name="language"></param>
        /// <param name="luceneDirectory"></param>
        /// <returns></returns>
        public static IndexWriter GetWriter(string language, string luceneDirectory)
        {
            Directory directory = GetDirectory(luceneDirectory);
            return GetWriter(directory, new AnalyzerBus(language));
            //return GetWriter(directory, AnalyzerDict.GetAnalyzer(language));
        }
        /// <summary>
        /// 得到写入索引的对象
        /// </summary>
        /// <param name="language"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static IndexWriter GetWriter(string language, Directory directory)
        {
            return GetWriter(directory, new AnalyzerBus(language));
            //return GetWriter(directory, AnalyzerDict.GetAnalyzer(language));
        }
        /// <summary>
        /// 得到写入索引的对象
        /// </summary>
        /// <param name="language"></param>
        /// <param name="analyzer"></param>
        /// <returns></returns>
        public static IndexWriter GetWriter(string language, Analyzer analyzer)
        {
            Directory directory = GetDirectory(System.IO.Path.Combine(LuceneNetConfig.LuceneDirectory, language));
            return new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
        }
        /// <summary>
        /// 得到写入索引的对象
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="analyzer"></param>
        /// <returns></returns>
        public static IndexWriter GetWriter(Directory directory, Analyzer analyzer)
        {
            return new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
        }
        /// <summary>
        /// 优化Index
        /// </summary>
        /// <param name="language"></param>
        public static void Optimize(string language)
        {
            Directory dir = GetDirectory(language);
            using (IndexWriter indexWriter = new IndexWriter(dir, new AnalyzerBus(language), IndexWriter.MaxFieldLength.UNLIMITED))
            //using (IndexWriter indexWriter = new IndexWriter(dir, AnalyzerDict.GetAnalyzer(language), IndexWriter.MaxFieldLength.UNLIMITED))
            {
                Optimize(indexWriter);
            }
        }
        /// <summary>
        /// 优化Index
        /// </summary>
        /// <param name="language"></param>
        /// <param name="lucenenDirectory"></param>
        public static void Optimize(string language, string lucenenDirectory)
        {
            Directory dir = GetDirectory(lucenenDirectory);
            using (IndexWriter indexWriter = new IndexWriter(dir, new AnalyzerBus(language), IndexWriter.MaxFieldLength.UNLIMITED))
            //using (IndexWriter indexWriter = new IndexWriter(dir, AnalyzerDict.GetAnalyzer(language), IndexWriter.MaxFieldLength.UNLIMITED))
            {
                Optimize(indexWriter);
            }
        }
        /// <summary>
        /// 优化IndexWriter
        /// </summary>
        /// <param name="indexWriter"></param>
        public static void Optimize(IndexWriter indexWriter)
        {
            if (!IsClosed(indexWriter))
            {
                indexWriter.Optimize();
            }
        }
        /// <summary>
        /// 合并IndexWriter
        /// </summary>
        /// <param name="indexWriter"></param>
        public static void MaybeMerge(IndexWriter indexWriter)
        {
            if (!IsClosed(indexWriter))
            {
                indexWriter.MaybeMerge();
            }
        }
        /// <summary>
        /// 关闭IndexWriter
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="isOptimize"></param>
        public static void Close(IndexWriter indexWriter, bool isOptimize = true)
        {
            if (isOptimize)
            {
                Optimize(indexWriter);
            }
            indexWriter.Dispose();
        }
        /// <summary>
        /// 提交IndexWriter
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="isOptimize"></param>
        public static void Commit(IndexWriter indexWriter, bool isOptimize = false)
        {
            if (!IsClosed(indexWriter))
            {
                indexWriter.Commit();
            }
            if (isOptimize)
            {
                Optimize(indexWriter);
            }
        }
        /// <summary>
        /// 判断IndexWriter是否已经关闭
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <returns></returns>
        public static bool IsClosed(IndexWriter indexWriter)
        {
            bool isClosed = false;
            MethodInfo isClosedMethod = typeof(IndexWriter).GetMethod("IsClosed", BindingFlags.Instance | BindingFlags.NonPublic);
            if (isClosedMethod != null)
            {
                isClosed = (bool)isClosedMethod.Invoke(indexWriter, null);
            }
            return isClosed;
        }
    }
}
