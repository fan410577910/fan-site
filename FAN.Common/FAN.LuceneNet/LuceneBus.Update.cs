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
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="fieldName">更新条件字段名称，相当于where条件中的primaryKey</param>
        /// <param name="fieldValue">更新条件字段值，相当于where条件中的primaryValue</param>
        /// <param name="document"></param>
        public static void Update(IndexWriter indexWriter, string fieldName, string fieldValue, Document document, bool isCommit = true)
        {
            Term term = new Term(fieldName, fieldValue);
            Update(indexWriter, term, document, isCommit);
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="indexWriter"></param>
        /// <param name="term">更新条件相当于where条件中的primaryKey=primaryValue</param>
        /// <param name="document"></param>
        public static void Update(IndexWriter indexWriter, Term term, Document document, bool isCommit = true)
        {
            indexWriter.UpdateDocument(term, document);//lucene的修改操作，是先删除，后新增，所以原来文档的DocId修改成功之后会变成新的值。
            if (isCommit)
            {
                Commit(indexWriter);
            }
        }
        public static void Update(IndexWriter indexWriter, string fieldName, string fieldValue, Document document, Analyzer analyzer, bool isCommit = true)
        {
            Term term = new Term(fieldName, fieldValue);
            Update(indexWriter, term, document, analyzer, isCommit);
        }
        public static void Update(IndexWriter indexWriter, Term term, Document document, Analyzer analyzer, bool isCommit = true)
        {
            indexWriter.UpdateDocument(term, document, analyzer);//lucene的修改操作，是先删除，后新增，所以原来文档的DocId修改成功之后会变成新的值。
            if (isCommit)
            {
                Commit(indexWriter);
            }
        }
    }
}
