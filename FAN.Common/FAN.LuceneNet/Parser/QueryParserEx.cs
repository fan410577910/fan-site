#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  QueryParserEx
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 扩展QueryParser类型
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using System;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 扩展QueryParser类型
    /// </summary>
    public class QueryParserEx : QueryParser
    {
        private string _fieldType = null;
        private string _fieldName = null;
        public QueryParserEx(Lucene.Net.Util.Version matchVersion, string fieldName)
            : this(matchVersion, fieldName, new Lucene.Net.Analysis.SimpleAnalyzer())
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchVersion"></param>
        /// <param name="fieldName">查找字段名称</param>
        /// <param name="a"></param>
        public QueryParserEx(Lucene.Net.Util.Version matchVersion, string fieldName, Analyzer a)
            : base(matchVersion, fieldName, a)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchVersion"></param>
        /// <param name="fieldName">查找字段名称</param>
        /// <param name="fieldType">查找字段类型</param>
        /// <param name="a"></param>
        public QueryParserEx(Lucene.Net.Util.Version matchVersion, string fieldName, string fieldType, Analyzer a)
            : base(matchVersion, fieldName, a)
        {
            this._fieldName = fieldName;
            this._fieldType = fieldType;
        }

        /// <summary>
        /// 覆盖原来的通配符查询
        /// </summary>
        /// <param name="field"></param>
        /// <param name="termStr"></param>
        /// <returns></returns>
        protected override Lucene.Net.Search.Query GetWildcardQuery(string field, string termStr)
        {
            throw new ParseException("由于性能原因,已经禁止使用通配符查询方法了,请输入更精确的信息查询!");
            //return base.GetWildcardQuery(field, termStr);
        }
        /// <summary>
        /// 覆盖原来的模糊查询
        /// </summary>
        /// <param name="field"></param>
        /// <param name="termStr"></param>
        /// <param name="minSimilarity"></param>
        /// <returns></returns>
        protected override Lucene.Net.Search.Query GetFuzzyQuery(string field, string termStr, float minSimilarity)
        {
            throw new ParseException("由于性能原因,已经禁止使用模糊查询方法了,请输入更精确的信息查询!");
            //return base.GetFuzzyQuery(field, termStr, minSimilarity);
        }
        /// <summary>
        /// 重写范围查询
        /// </summary>
        /// <param name="field"></param>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <param name="inclusive"></param>
        /// <returns></returns>
        protected override Lucene.Net.Search.Query GetRangeQuery(string field, string part1, string part2, bool inclusive)
        {
            Query query = null;
            if (field.Equals(this._fieldName))
            {//如果查找的字段名称是我们需要转换数据类型的字段名称
                switch (this._fieldType)
                {//根据查找字段名称对应的数据类型，创建相对应的数字范围的类型查询
                    case FieldType.INT32:
                        query = NumericRangeQuery.NewIntRange(field, Convert.ToInt32(part1), Convert.ToInt32(part2), inclusive, inclusive);
                        break;
                    case FieldType.INT64:
                        query = NumericRangeQuery.NewLongRange(field, Convert.ToInt64(part1), Convert.ToInt64(part2), inclusive, inclusive);
                        break;
                    case FieldType.SINGLE:
                        query = NumericRangeQuery.NewFloatRange(field, Convert.ToSingle(part1), Convert.ToSingle(part2), inclusive, inclusive);
                        break;
                    case FieldType.DOUBLE:
                        query = NumericRangeQuery.NewDoubleRange(field, Convert.ToDouble(part1), Convert.ToDouble(part2), inclusive, inclusive);
                        break;
                    case FieldType.DATETIME:
                        query = NumericRangeQuery.NewLongRange(field, new DateTime(System.Convert.ToInt64(part1)).Ticks, new DateTime(System.Convert.ToInt64(part2)).Ticks, inclusive, inclusive);
                        break;
                    case FieldType.STRING:
                    default:
                        query = base.GetRangeQuery(field, part1, part2, inclusive);
                        break;
                }
            }
            else
            {
                query = base.GetRangeQuery(field, part1, part2, inclusive);
            }
            return query;
        }

        protected override Query GetFieldQuery(string field, string queryText)
        {
            return base.GetFieldQuery(field, queryText);
        }
    }
}
