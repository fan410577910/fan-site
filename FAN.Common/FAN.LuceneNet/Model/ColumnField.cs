#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  ColumnField 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/15
     * 描述    : Lucene创建索引时需要设置字段的类型
     * =====================================================================
     * 修改时间：2014/7/15
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Documents;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// Lucene里创建索引时需要设置字段的类型
    /// </summary>
    public class ColumnField
    {
        /// <summary>
        /// 列名称
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// 列类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 是否存储
        /// </summary>
        public Field.Store Store { get; set; }
        /// <summary>
        /// 是否索引，以及是否分词，是否可以权重
        /// </summary>
        public Field.Index Index { get; set; }
        /// <summary>
        /// 是否为评分列
        /// </summary>
        public bool IsCustomScore { get; set; }
        /// <summary>
        /// 权重值（字段的权重值）
        /// </summary>
        public float Boost { get; set; }
    }
}
