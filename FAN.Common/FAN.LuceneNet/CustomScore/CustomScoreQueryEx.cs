#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  CustomScoreInfo
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 自定义评分查询扩展
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Search;
using Lucene.Net.Search.Function;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 自定义评分查询扩展
    /// http://www.cnblogs.com/Laupaul/archive/2012/04/22/2465046.html
    /// 支持增加评分列进行自定义评分和根据索引中某一个列的值进行自定义评分两种方式
    /// </summary>
    public class CustomScoreQueryEx : CustomScoreQuery
    {
        private string _language = null;
        #region 第一种评分方式：根据现有文档索引中的某一个域的值进行自定义评分方式，配合外部CustomScore.xml文件完成自定义评分
        /// <summary>
        /// 根据某一个列的值进行自定义评分
        /// </summary>
        /// <param name="language"></param>
        /// <param name="subQuery"></param>
        public CustomScoreQueryEx(string language, Query subQuery)
            : base(subQuery)
        {
            this._language = language;
        }
        #endregion

        #region 第二种评分方式：使用文档单独定义的自定义评分列后进行自定义评分
        /// <summary>
        /// 使用文档单独增加的评分列进行自定义评分
        /// FieldScoreQuery fieldScoreQuery1 = new FieldScoreQuery("sore", FieldScoreQuery.Type.INT);//FieldScoreQuery.Type.BYTE;表示字符串类型排序
        /// FieldScoreQuery fieldScoreQuery2 = new FieldScoreQuery("name", FieldScoreQuery.Type.BYTE);//FieldScoreQuery.Type.BYTE;表示字符串类型排序
        /// </summary>
        /// <param name="language"></param>
        /// <param name="subQuery"></param>
        /// <param name="valSrcQueries">创建多个评分列</param>
        public CustomScoreQueryEx(string language, Query subQuery, params ValueSourceQuery[] valSrcQueries)
            : base(subQuery, valSrcQueries)
        {
            this._language = language;
        }
        /// <summary>
        /// 使用文档单独增加的评分列进行自定义评分
        /// FieldScoreQuery fieldScoreQuery = new FieldScoreQuery("sore", FieldScoreQuery.Type.INT);//FieldScoreQuery.Type.BYTE;表示字符串类型排序
        /// </summary>
        /// <param name="language"></param>
        /// <param name="subQuery"></param>
        /// <param name="valSrcQuery">创建一个评分列</param>
        public CustomScoreQueryEx(string language, Query subQuery, ValueSourceQuery valSrcQuery)
            : base(subQuery, valSrcQuery)
        {
            this._language = language;
        }
        #endregion
        /// <summary>
        /// 使用自定义评分规则
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override CustomScoreProvider GetCustomScoreProvider(Lucene.Net.Index.IndexReader reader)
        {
            //默认情况实现的评分是通过原有的评分 * 传入进来的评分列所获取的评分来确定最终打分。
            //return base.GetCustomScoreProvider(reader);
            //为了根据不同的需求进行评分，需要自己进行评分的设定。
            /*
             * 自定义评分的步骤
             * 创建一个类继承于CustomScoreProvider
             * 覆盖CustomScore方法
             */
            return new CustomScoreProviderEx(this._language, reader);
        }
    }
}
