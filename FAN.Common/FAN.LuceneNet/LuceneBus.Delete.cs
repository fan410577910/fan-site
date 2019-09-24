#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Delete 
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

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 删除索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        public static void Delete(IndexWriter indexWriter, string fieldName, string fieldValue, bool isCommit = true)
        {
            Term term = new Term(fieldName, fieldValue);
            Delete(indexWriter, isCommit, term);
        }

        /// <summary>
        /// 删除索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="term">删除的项</param>
        public static void Delete(IndexWriter indexWriter, bool isCommit = true, params Term[] term)
        {
            indexWriter.DeleteDocuments(term);
            if (isCommit)
            {
                Commit(indexWriter);
            }
            indexWriter.Flush(false, true, true);
        }

        /// <summary>
        /// 删除索引文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="queries">删除条件</param>
        public static void Delete(IndexWriter indexWriter, bool isCommit = true, params Query[] queries)
        {
            indexWriter.DeleteDocuments(queries);
            if (isCommit)
            {
                Commit(indexWriter);
            }
            indexWriter.Flush(false, true, true);
        }

        /// <summary>
        /// 删除所有文档
        /// </summary>
        /// <param name="indexWriter"></param>
        public static void DeleteAll(IndexWriter indexWriter, bool isCommit = true)
        {
            indexWriter.DeleteAll();
            if (isCommit)
            {
                Commit(indexWriter);
            }
        }
        /// <summary>
        /// 删除索引文档
        /// </summary>
        /// <param name="indexReader"></param>
        /// <param name="docId"></param>
        public static void Delete(IndexReader indexReader, int docId)
        {
            //注意:http://www.cnblogs.com/zengen/archive/2011/04/18/2019669.html
            indexReader.DeleteDocument(docId);
        }

    }
}
