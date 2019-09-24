#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Directory 
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
using System;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {

        /// <summary>
        /// 得到Lucene的目录
        /// </summary>
        /// <param name="dataBaseDirectory">数据库的名字</param>
        /// <param name="tableDirecotry">表或者视图的名字</param>
        /// <returns></returns>
        public static Directory GetDirectory(string luceneDirecotry)
        {
            if (!System.IO.Directory.Exists(luceneDirecotry))
            {
                System.IO.Directory.CreateDirectory(luceneDirecotry);
            }
            Directory dir = Lucene.Net.Store.FSDirectory.Open(luceneDirecotry);
            if (IsLocked(dir))
            {
                UnLock(dir);
            }
            return dir;
        }

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static bool Exists(Directory directory)
        {
            return IndexReader.IndexExists(directory);
        }

        public static bool IsLocked(Directory directory)
        {
            return IndexWriter.IsLocked(directory);
        }

        public static void UnLock(Directory directory)
        {
            IndexWriter.Unlock(directory);
        }

        public static string LastModifiedTime(Directory directory)
        {
            return new DateTime(IndexReader.LastModified(directory) * TimeSpan.TicksPerMillisecond).AddYears(1969).ToLocalTime().ToString();
        }
    }
}
