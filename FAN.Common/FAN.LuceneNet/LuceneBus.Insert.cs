#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Insert 
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
using Lucene.Net.Documents;
using Lucene.Net.Index;
using System.Collections.Generic;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 新增索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="document"></param>
        public static void Insert(IndexWriter indexWriter, Document document, bool isCommit = true)
        {
            indexWriter.AddDocument(document);
            if (isCommit)
            {
                Commit(indexWriter);
            }
        }
        /// <summary>
        /// 新增索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="object"></param>
        /// <param name="columnFields"></param>
        /// <param name="isCommit"></param>
        public static void Insert(IndexWriter indexWriter, object @object, ColumnField[] columnFields, bool isCommit = true)
        {
            Document document = Convert(@object, columnFields);
            Insert(indexWriter, document, isCommit);
        }
        /// <summary>
        /// 新增索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="documentList"></param>
        /// <param name="isCommit"></param>
        public static void Insert(IndexWriter indexWriter, List<Document> documentList, bool isCommit = true)
        {
            foreach (Document document in documentList)
            {
                Insert(indexWriter, document, false);
            }
            if (isCommit)
            {
                Commit(indexWriter);
            }
        }
        /// <summary>
        /// 新增索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="document"></param>
        /// <param name="analyzer"></param>
        /// <param name="isCommit"></param>
        public static void Insert(IndexWriter indexWriter, Document document, Analyzer analyzer, bool isCommit = true)
        {
            indexWriter.AddDocument(document, analyzer);
            if (isCommit)
            {
                Commit(indexWriter);
            }
        }
        /// <summary>
        /// 新增索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="object"></param>
        /// <param name="columnFields"></param>
        /// <param name="analyzer"></param>
        /// <param name="isCommit"></param>
        public static void Insert(IndexWriter indexWriter, object @object, ColumnField[] columnFields, Analyzer analyzer, bool isCommit = true)
        {
            Document document = Convert(@object, columnFields);
            Insert(indexWriter, document, analyzer, isCommit);
        }
        /// <summary>
        /// 新增索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="documentList"></param>
        /// <param name="analyzer"></param>
        /// <param name="isCommit"></param>
        public static void Insert(IndexWriter indexWriter, List<Document> documentList, Analyzer analyzer, bool isCommit = true)
        {
            foreach (Document document in documentList)
            {
                Insert(indexWriter, document, analyzer, false);
            }
            if (isCommit)
            {
                Commit(indexWriter);
            }
        }
        /// <summary>
        /// 新增索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="objectList"></param>
        /// <param name="columnFields"></param>
        /// <param name="isCommit"></param>
        public static void Insert(IndexWriter indexWriter, List<object> objectList, ColumnField[] columnFields, bool isCommit = true)
        {
            foreach (object obj in objectList)
            {
                Document document = Convert(obj, columnFields);
                if (document != null)
                {
                    Insert(indexWriter, document, false);
                }
            }
            if (isCommit)
            {
                Commit(indexWriter);
            }
        }
        /// <summary>
        /// 新增索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="objectList"></param>
        /// <param name="columnFields"></param>
        /// <param name="analyzer"></param>
        /// <param name="isCommit"></param>
        public static void Insert(IndexWriter indexWriter, List<object> objectList, ColumnField[] columnFields, Analyzer analyzer, bool isCommit = true)
        {
            foreach (object obj in objectList)
            {
                Document document = Convert(obj, columnFields);
                if (document != null)
                {
                    Insert(indexWriter, document, analyzer, false);
                }
            }
            if (isCommit)
            {
                Commit(indexWriter);
            }
        }
        /// <summary>
        /// 新增索引文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexWriter"></param>
        /// <param name="objectList"></param>
        /// <param name="columnFields"></param>
        /// <param name="isCommit"></param>
        public static void Insert<T>(IndexWriter indexWriter, List<T> objectList, ColumnField[] columnFields, bool isCommit = true)
        {
            foreach (T obj in objectList)
            {
                Document document = Convert(obj, columnFields);
                if (document != null)
                {
                    Insert(indexWriter, document, false);
                }
            }
            if (isCommit)
            {
                Commit(indexWriter);
            }
        }
    }
}
