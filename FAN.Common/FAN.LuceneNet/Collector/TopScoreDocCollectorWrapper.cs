#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  GroupFieldCollector 
     * 版本号：  V1.0.0.0 
     * 创建人：  wyp 
     * 创建时间：2014/9/13 15:29:06 
     * 描述    : 实现按所有字段的所有字段值分组统计
     * =====================================================================
     * 修改时间：2014/9/13 15:29:06 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：实现按所有字段的所有字段值分组统计
*/
#endregion
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 实现按所有字段的所有字段值分组统计,核心实现是系统定义的TopScoreDocCollector类型完成的。
    /// 本类相当于是TopScoreDocCollector的Wrapper。
    /// </summary>
    [Obsolete("wangyunpeng，测试专用")]
    public class TopScoreDocCollectorWrapper : TopDocsCollector<ScoreDoc>
    {
        /// <summary>
        /// 核心实现是系统定义的TopScoreDocCollector类型完成的。
        /// </summary>
        private TopScoreDocCollector _collector = null;
        /// <summary>
        /// 核心实现是系统定义的TopScoreDocCollector类型完成的。
        /// </summary>
        public TopScoreDocCollector TopScoreDocCollector
        {
            get { return this._collector; }
        }

        private int _docBase = 0;

        /// <summary>
        /// 保存分组统计结果
        /// </summary>
        private GroupField[] _groupFields = null;
        /// <summary>
        /// 保存分组统计结果
        /// </summary>
        public GroupField[] GroupFields
        {
            get { return this._groupFields; }
        }
        // prevents instantiation
        private TopScoreDocCollectorWrapper(int numHits)
            : base(new HitQueue(numHits, true))
        {
        }

        public TopScoreDocCollectorWrapper(int numHits, TopScoreDocCollector collector, GroupField[] groupFields)
            :this(numHits)
        {
            this._groupFields = groupFields;
            this._collector = collector;
        }

        public override bool AcceptsDocsOutOfOrder
        {
            get
            {
                return this._collector.AcceptsDocsOutOfOrder;
            }
        }

        public override void Collect(int doc)
        {
            this._collector.Collect(doc);//继续执行原来TopScoreDocCollector的Collect方法。
            //因为doc是每个segment的文档编号，需要加上docBase才是总的文档编号
            int docId = doc + this._docBase;
            //添加的GroupField中，由GroupField负责统计每个不同值的数目
            foreach (GroupField groupField in this._groupFields)
            {
                groupField.AddValue(groupField.FieldValueCaches[docId]);
            }
        }

        public override void SetNextReader(Lucene.Net.Index.IndexReader reader, int docBase)
        {
            this._collector.SetNextReader(reader, docBase);
            this._docBase = docBase;
        }

        public override void SetScorer(Scorer scorer)
        {
            this._collector.SetScorer(scorer);
        }
    }
}
