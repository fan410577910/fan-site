#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneNetCacheInfo 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 分析器总线
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Search;
using System.Collections.Generic;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// LuceneNet缓存对象信息
    /// </summary>
    public class LuceneNetCacheInfo<T>
    {
        /// <summary>
        /// 结果集合
        /// </summary>
        public Dictionary<T, ScoreDoc> Dict { get; set; }
        public int RecordCount { get; set; }
    }
}
