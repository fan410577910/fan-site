#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  CustomSimilarity
     * 版本号：  V1.0.0.0 
     * 创建人：  王威 
     * 创建时间：2014/12/28
     * 描述    : 修改默认Lucene评分机制算法
     * =====================================================================
     * 修改时间：2014/12/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Search;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 修改默认Lucene评分机制算法
    /// 参考URL:http://blog.chenlb.com/2009/08/lucene-scoring-architecture.html
    /// </summary>
    public class CustomSimilarity : DefaultSimilarity
    {
        /// <summary>
        /// idf(t) 代表（stand for）反转文档频率（Inverse Document Frequency）。
        /// 这个分数与反转（inverse of）的docFreq（出现过term t的文档数目）有关系。
        /// 这个分数的意义是越不常出现（rarer）的term将为最后的总分贡献（contribution）更多的分数。
        /// 缺省idff(t in d)算法实现在DefaultSimilarity类中
        /// </summary>
        /// <param name="docFreq"></param>
        /// <param name="numDocs"></param>
        /// <returns></returns>
        public override float Idf(int docFreq, int numDocs)
        {
            return 1.0f;
        }
        /// <summary>
        /// 由字段内的 Token 的个数来计算此值，字段越短，评分越高，在做索引的时候由 Similarity.lengthNorm 计算
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="numTerms"></param>
        /// <returns></returns>
        public override float LengthNorm(string fieldName, int numTerms)
        {
            return base.LengthNorm(fieldName, numTerms);
        }
    }
}
