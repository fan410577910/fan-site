#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  GroupCollector 
     * 版本号：  V1.0.0.0 
     * 创建人：  wyp 
     * 创建时间：2014/9/13 7:34:34 
     * 描述    : 分组统计（只统计某一个字段的所有值出现的次数）
     * =====================================================================
     * 修改时间：2014/9/13 7:34:34 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：分组统计（只统计某一个字段的所有值出现的次数）
*/
#endregion
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 分组统计（只统计某一个字段的所有值出现的次数）
    /// http://www.czh123.com/blogitem435.html
    /// </summary>
     [Obsolete("wangyunpeng，测试专用")]
    public class GroupCollector : Collector
    {
        private string _fieldName = null;
        /// <summary>
        /// 收集要统计的某一个字段名称
        /// </summary>
        public string FieldName
        {
            get { return this._fieldName; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName">收集要统计的某一个字段名称</param>
        public GroupCollector(string fieldName)
        {
            this._fieldName = fieldName;
        }
        /// <summary>
        /// 用于读取文档
        /// </summary>
        private IndexReader _indexReader = null;
        /// <summary>
        /// 收集信息
        /// </summary>
        private Dictionary<string, int> _dict = new Dictionary<string, int>();
        /// <summary>
        /// 收集信息（按字典里的Value值进行排序）
        /// </summary>
        public Dictionary<string, int> Dict
        {
            get
            {
                List<KeyValuePair<string, int>> tempList = new List<KeyValuePair<string, int>>(this._dict);
                tempList.Sort(delegate(KeyValuePair<string, int> s1, KeyValuePair<string, int> s2)
                {
                    return s2.Value.CompareTo(s1.Value);
                });
                this._dict.Clear();
                foreach (KeyValuePair<string, int> pair in tempList)
                {
                    this._dict.Add(pair.Key, pair.Value);
                }
                tempList.Clear();
                tempList = null;
                return this._dict;
            }
        }

        public override bool AcceptsDocsOutOfOrder
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 收集方法
        /// </summary>
        /// <param name="doc"></param>
        public override void Collect(int doc)
        {
            //object @value = FieldCache_Fields.DEFAULT.GetStrings(this._indexReader, this._fieldName).GetValue(doc);//从索引里取某一列的数据，分词之后的值
            Document document = this._indexReader.Document(doc);
            string fieldValue = document.Get(this._fieldName);//取文档里面某一列的值，原值
            if (this._dict.ContainsKey(fieldValue))
            {
                this._dict[fieldValue] += 1;
            }
            else
            {
                this._dict[fieldValue] = 1;
            }
        }

        public override void SetNextReader(Lucene.Net.Index.IndexReader reader, int docBase)
        {
            this._indexReader = reader;
        }

        public override void SetScorer(Scorer scorer)
        {
            
        }
    }
}
