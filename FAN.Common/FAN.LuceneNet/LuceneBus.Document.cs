#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  LuceneBus 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2014/10/29 16:11:22 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/10/29 16:11:22 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 通过文档ID获取文档
        /// </summary>
        /// <param name="indexReader"></param>
        /// <param name="docId"></param>
        /// <returns></returns>
        public static Document GetDocument(IndexReader indexReader, int docId)
        {
            Document document = null;
            if (indexReader != null)
            {
                document = indexReader.Document(docId);
            }
            return document;
        }
        /// <summary>
        /// 通过文档ID获取文档
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        public static Document GetDocument(int docId)
        {
            IndexReader indexReader = null;
            Document document = null;
            try
            {
                indexReader = GetReader();
                document = GetDocument(indexReader, docId);
            }
            finally
            {
                Close(indexReader);
            }
            return document;
        }
        /// <summary>
        /// 通过文档ID获取文档
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="docId"></param>
        /// <returns></returns>
        public static Document GetDocument(IndexWriter writer, int docId)
        {
            Document document = null;
            if (writer != null)
            {
                IndexReader indexReader = null;
                try
                {
                    indexReader = writer.GetReader();
                    document = GetDocument(indexReader, docId);
                }
                finally
                {
                    Close(indexReader);
                }
            }
            return document;
        }
    }
}
