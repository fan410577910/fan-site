#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2015 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  TopScoreDocGroupCollectorWrapper 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2015/5/7 15:38:46 
     * 描述    : 实现按分组字段的所有字段值进行分组统计,核心实现是系统定义的TopScoreDocCollector类型完成的。本类相当于是TopScoreDocCollector或TopFieldCollector的Wrapper。
     * =====================================================================
     * 修改时间：2015/5/7 15:38:46 
     * 修改人  ：Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 实现按分组字段的所有字段值进行分组统计,核心实现是系统定义的TopScoreDocCollector类型完成的。
    /// 本类相当于是TopScoreDocCollector或TopFieldCollector的Wrapper。
    /// http://blog.csdn.net/whuqin/article/details/42524825
    /// </summary>
    internal class GroupCollectorWrapper : TopDocsCollector<ScoreDoc>
    {
        /// <summary>
        /// 核心实现是系统定义的TopScoreDocCollector或TopFieldCollector类型完成的。
        /// </summary>
        private Collector _collector = null;
        /// <summary>
        /// 核心实现是系统定义的TopScoreDocCollector或TopFieldCollector类型完成的。
        /// </summary>
        internal Collector Collector
        {
            get { return this._collector; }
        }

        private int _docBase = 0;
        /// <summary>
        /// 用于读取文档
        /// </summary>
        private IndexReader _indexReader = null;
        /// <summary>
        /// 保存分组统计结果
        /// </summary>
        private GroupCollectorField _groupCollectorField = null;
        // prevents instantiation
        private GroupCollectorWrapper(int numHits)
            : base(new HitQueue(numHits, true))
        {
        }

        internal GroupCollectorWrapper(int numHits, Collector collector, GroupCollectorField groupCollectorField)
            : this(numHits)
        {
            this._groupCollectorField = groupCollectorField;
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
            string fieldValue = FieldCache_Fields.DEFAULT.GetStrings(this._indexReader, this._groupCollectorField.FieldName).GetValue(docId) as string;//从索引里取某一列的数据，分词之后的值
            this._groupCollectorField.AddValue(fieldValue);

            //Document document = this._indexReader.Document(doc);
            //string fieldValue = document.Get(this._groupCollectorField.FieldName);//取文档里面某一列的值，原值
            //this._groupCollectorField.AddValue(fieldValue);
        }

        public override void SetNextReader(Lucene.Net.Index.IndexReader reader, int docBase)
        {
            this._indexReader = reader;
            this._docBase = docBase;
            this._collector.SetNextReader(reader, docBase);
        }

        public override void SetScorer(Scorer scorer)
        {
            this._collector.SetScorer(scorer);
        }
    }
}
