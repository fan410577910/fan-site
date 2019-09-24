#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Reader 
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
using Lucene.Net.Store;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 获取读取索引reader对象
        /// </summary>
        /// <returns></returns>
        private static IndexReader GetReader()
        {
            return GetReader(LuceneNetConfig.LuceneDirectory);
        }
        /// <summary>
        /// 获取读取索引reader对象
        /// </summary>
        /// <param name="luceneDirectory"></param>
        /// <returns></returns>
        private static IndexReader GetReader(string luceneDirectory)
        {
            Directory dir = GetDirectory(luceneDirectory);
            return GetReader(dir);
            //RAMDirectory ramDir = new RAMDirectory(dir);
            //return GetReader(ramDir);
        }
        /// <summary>
        /// 获取读取索引reader对象
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private static IndexReader GetReader(Directory directory)
        {
            return IndexReader.Open(directory, true);
        }
        /// <summary>
        /// 关闭读取索引reader对象
        /// </summary>
        /// <param name="indexReader"></param>
        private static void Close(IndexReader indexReader)
        {
            if (indexReader != null)
            {
                Directory directory = indexReader.Directory();
                if (directory != null)
                {
                    directory.Dispose();
                }
                indexReader.Dispose();
                directory = null;
                indexReader = null;
            }
        }
    }
}

