#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2015 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  LuceneBus 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2015/5/8 13:25:01 
     * 描述    :
     * =====================================================================
     * 修改时间：2015/5/8 13:25:01 
     * 修改人  ：Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Documents;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 分组查询
        /// </summary>
        /// <param name="indexSearcher"></param>
        /// <param name="groupCollector"></param>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        [Obsolete("wangyunpeng，测试专用方法")]
        public static void SelectGroup(IndexSearcher indexSearcher, GroupCollector groupCollector, Query query, Filter filter = null)
        {
            indexSearcher.Search(query, filter, groupCollector);
        }
        /// <summary>
        /// 字段分组统计
        /// </summary>
        /// <param name="indexSearcher"></param>
        /// <param name="collectorWrapper"></param>
        /// <param name="query"></param>
        /// <param name="recordCount"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Obsolete("wangyunpeng，测试专用方法")]
        public static Dictionary<Document, ScoreDoc> SelectGroup(IndexSearcher indexSearcher, TopScoreDocCollectorWrapper collectorWrapper, Query query, out int recordCount, Filter filter = null)
        {
            Dictionary<Document, ScoreDoc> dict = new Dictionary<Document, ScoreDoc>();
            recordCount = 0;
            indexSearcher.Search(query, filter, collectorWrapper);
            TopDocs docs = collectorWrapper.TopScoreDocCollector.TopDocs();
            recordCount = docs.TotalHits;

            ScoreDoc[] scoreDocs = docs.ScoreDocs;
            if (scoreDocs != null)
            {
                foreach (ScoreDoc scoreDoc in scoreDocs)
                {
                    if (scoreDoc.Doc != int.MaxValue && scoreDoc.Score != System.Single.NegativeInfinity)
                    {
                        dict.Add(indexSearcher.Doc(scoreDoc.Doc), scoreDoc);
                    }
                }
                //没加缓存还需要删除数组变量wyp
                Array.Clear(scoreDocs, 0, scoreDocs.Length);
                scoreDocs = null;
            }
            return dict;
        }

        /// <summary>
        /// 字段分组统计(支持分页)
        /// </summary>
        /// <param name="indexSearcher"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="query"></param>
        /// <param name="recordCount"></param>
        /// <param name="groupKeyValueList">分组结果</param>
        /// <param name="filter"></param>
        /// <param name="sortFields"></param>
        /// <returns></returns>
        public static Dictionary<Document, ScoreDoc> SelectGroup(IndexSearcher indexSearcher, int pageSize, int pageIndex, Query query, out int recordCount, out GroupKeyValueList groupKeyValueList, Filter filter = null, params SortField[] sortFields)
        {
            recordCount = 0;
            groupKeyValueList = null;
            Dictionary<Document, ScoreDoc> dictPager = new Dictionary<Document, ScoreDoc>();
            int maxDoc = indexSearcher.IndexReader.MaxDoc;
            if (maxDoc == 0)
            {//返回索引可用的最大的索引ID
                return dictPager;
            }
            TopDocs docs = null;
            string key = string.Format(CACHE_KEY, query.ToString(), string.Join("_", sortFields.Select(item => item.ToString())), filter == null ? string.Empty : filter.ToString());
            string listKey = key + string.Format(",PAGE_INDEX:{0},PAGE_SIZE:{1}", pageIndex, pageSize);
            string groupKey = "GROUP:::" + key;
            docs = MemCache.MemoryCacheBus.Get(listKey) as TopDocs;
            groupKeyValueList = MemCache.MemoryCacheBus.Get(groupKey) as GroupKeyValueList;
            if (docs == null || groupKeyValueList == null)
            {
                //https://searchcode.com/codesearch/view/7233825/
                int start = pageIndex * pageSize;
                start = Math.Min(start, maxDoc);

                using (GroupCollectorField groupCollectorField = new GroupCollectorField("NameValue"))
                {
                    if (sortFields.Length > 0)
                    {//先排序，后分页
                        Sort sort = new Sort();
                        sort.SetSort(sortFields);
                        TopFieldCollector topFieldCollector = TopFieldCollector.Create(sort, start, true, false, false, !query.CreateWeight(indexSearcher).GetScoresDocsOutOfOrder());
                        GroupCollectorWrapper groupCollectorWrapper = new GroupCollectorWrapper(start, topFieldCollector, groupCollectorField);
                        indexSearcher.Search(query, filter, groupCollectorWrapper);
                        start = start - pageSize;
                        if (start < 0)
                        {
                            start = 0;
                        }
                        docs = topFieldCollector.TopDocs(start, pageSize);//只返回前start条记录
                    }
                    else
                    {//不支持排序，只有分页
                        TopScoreDocCollector topScoreDocCollector = TopScoreDocCollector.Create(start + 1, !query.CreateWeight(indexSearcher).GetScoresDocsOutOfOrder());
                        GroupCollectorWrapper groupCollectorWrapper = new GroupCollectorWrapper(start, topScoreDocCollector, groupCollectorField);
                        indexSearcher.Search(query, filter, groupCollectorWrapper);
                        start = start - pageSize;
                        if (start < 0)
                        {
                            start = 0;
                        }
                        docs = topScoreDocCollector.TopDocs(start, pageSize);//只返回前start条记录
                    }
                    groupCollectorField.GroupKeyValueDocCountList.Sort();//排序
                    groupKeyValueList = ObjectExtensions.Clone(groupCollectorField.GroupKeyValueDocCountList);
                    if (docs != null && groupKeyValueList != null)
                    {
                        TimeSpan timeSpan = TimeSpan.FromMinutes(CACHE_TIME);
                        MemCache.MemoryCacheBus.Insert(groupKey, groupKeyValueList, timeSpan);
                        MemCache.MemoryCacheBus.Insert(listKey, docs, timeSpan);
                    }
                }
            }
            #region 返回搜索的结果集合
            if (docs != null)
            {
                recordCount = docs.TotalHits;//搜索结果总数量
                ScoreDoc[] scoreDocs = docs.ScoreDocs;//搜索的结果集合
                if (scoreDocs != null)
                {
                    foreach (ScoreDoc scoreDoc in scoreDocs)
                    {
                        if (scoreDoc.Doc != int.MaxValue && scoreDoc.Score != System.Single.NegativeInfinity)
                        {
                            dictPager.Add(indexSearcher.Doc(scoreDoc.Doc), scoreDoc);
                        }
                    }
                }
            }
            if (dictPager.Count == 0)
            {//如果没有取出符合条件的结果删除缓存。wyp
                MemCache.MemoryCacheBus.Delete(listKey);
            }
            if (groupKeyValueList.Count == 0)
            {//如果没有取出符合条件的结果删除缓存。wyp
                MemCache.MemoryCacheBus.Delete(groupKey);
            }
            #endregion
            groupKeyValueList = groupKeyValueList ?? new GroupKeyValueList(0);
            return dictPager;
        }
    }
}
