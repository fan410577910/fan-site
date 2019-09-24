#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  FilterEx 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/15
     * 描述    : 例如:在活动时,仅需要搜索出来参加活动的商品,而其他商品不需要搜索出来时使用.
     * =====================================================================
     * 修改时间：2014/7/15
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using System;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 例如:在活动时,仅需要搜索出来参加活动的商品,而其他商品不需要搜索出来时使用.
    /// </summary>
    public class FilterEx : Filter
    {
        private string _language = null;
        private int[] _doc = new int[1];
        private int[] _freqs = new int[1];
        public FilterEx(string language)
        {
            this._language = language;
        }
        public override DocIdSet GetDocIdSet(Lucene.Net.Index.IndexReader reader)
        {
            //创建一个Bit，默认里面所有元素都是0
            OpenBitSet openBitSet = new OpenBitSet(reader.MaxDoc);//获取最大文档最大编号
            FilterInfo filterInfo = FilterDict.FilterInfo(this._language);
            if (filterInfo.Enabled)
            {
                this.SetOrClear(reader, openBitSet, filterInfo.SetOrClear);
            }
            else
            {//先把元素填满，都是1。
                openBitSet.Set(0L, reader.MaxDoc);
            }
            return openBitSet;
        }
        /// <summary>
        /// 执行自定义过滤条件
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="openBitSet"></param>
        /// <param name="setOrClear">Set Or Clear</param>
        private void SetOrClear(IndexReader reader, OpenBitSet openBitSet, bool setOrClear)
        {
            if (!setOrClear)
            {//如果是清除操作，先把所有元素填满，都是1。
                openBitSet.Set(0L, reader.MaxDoc);
            }
            FilterInfo filterInfo = FilterDict.FilterInfo(this._language);
            string[] values = filterInfo.Values;
            if (values != null)
            {
                TermDocs termDocs = null;
                foreach (string value in values)
                {
                    if (filterInfo.FieldType == FieldType.DATETIME)
                    {
                        long ticks = DateTime.Parse(value).Ticks;
                        termDocs = reader.TermDocs(new Term(filterInfo.FieldName, ticks.ToString()));
                    }
                    else
                    {
                        termDocs = reader.TermDocs(new Term(filterInfo.FieldName, value));
                    }

                    int count = termDocs.Read(_doc, _freqs);
                    if (count == 1)
                    {
                        if (setOrClear)
                        {//显示
                            openBitSet.Set(_doc[0]);//将Document的Doc文档表示设置为1。
                        }
                        else
                        {//不显示
                            openBitSet.Clear(_doc[0]);//将Document的Doc文档表示设置为0。
                        }
                    }
                }
            }
        }
    }
}
