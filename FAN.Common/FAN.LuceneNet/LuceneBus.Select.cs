#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Select 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
#define LAST_SCORE_DOC
//#undef LAST_SCORE_DOC
using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Search;

namespace TLZ.LuceneNet
{
    #region  - LUCENE.NET API -
    /// <summary>
    /// 全部查询
    ///     MatchAllDocsQuery query = new MatchAllDocsQuery();
    /// 精确查询
    ///     Query query = new TermQuery(new Term("name", "mike"));//make或mik就没有了
    /// -----------------------------------------------------------------
    /// 范围查询
    ///     TermRangeQuery query = new TermRangeQuery("id", "1", "3", true, true);//支持字符串，不支持数字
    ///     NumericRangeQuery query = NumericRangeQuery.NewIntRange("attach", 2, 10, true, true);//支持Int类型的数字
    /// -----------------------------------------------------------------
    /// 前缀查询
    ///     PrefixQuery query = new PrefixQuery(new Lucene.Net.Index.Term("name", "j"));//name以j开头
    ///     PrefixQuery query = new PrefixQuery(new Lucene.Net.Index.Term("content", "s"));//content里面的每一个单词以s开头都取出来
    /// -----------------------------------------------------------------
    /// 通配符查询
    ///     WildcardQuery query = new WildcardQuery(new Lucene.Net.Index.Term("email", "*@qq.com"));//匹配@qq.com为结尾的email
    ///     WildcardQuery query = new WildcardQuery(new Lucene.Net.Index.Term("name", "j???"));//匹配j开头的4个字符的名字
    /// -----------------------------------------------------------------
    /// 逻辑查询（可以连接多个子查询）
    ///     第一种方式
    ///     BooleanQuery query = new BooleanQuery();
    ///     query.Add(new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("name", "zhangsan")), Lucene.Net.Search.BooleanClause.Occur.MUST);
    ///     query.Add(new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("content", "like")), Lucene.Net.Search.BooleanClause.Occur.MUST);
    ///     ----------------------------------------
    ///     BooleanQuery query = new BooleanQuery();
    ///     query.Add(new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("name", "zhangsan")), Lucene.Net.Search.BooleanClause.Occur.MUST);
    ///     query.Add(new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("content", "game")), Lucene.Net.Search.BooleanClause.Occur.SHOULD);//可有可无
    ///     ----------------------------------------
    ///     BooleanQuery query = new BooleanQuery();
    ///     query.Add(new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("name", "zhangsan")), Lucene.Net.Search.BooleanClause.Occur.MUSE_NOT);//没有zhangsan
    ///     query.Add(new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("content", "game")), Lucene.Net.Search.BooleanClause.Occur.SHOULD);
    ///     第二种方式
    ///     BooleanQuery query = new BooleanQuery();
    ///     foreach (string text in keyWords)
    ///     {
    ///         Term term = new Lucene.Net.Index.Term(field, text);
    ///         TermQuery termQuery = new Lucene.Net.Search.TermQuery(term);
    ///         query.Add(termQuery, Lucene.Net.Search.BooleanClause.Occur.SHOULD);
    ///     }
    ///     -----------------------------------------------------------------
    ///     Occur.MUST表示必须出现，Occur.SHOULD表示可以出现，Occur.MUSE_NOT表示不能出现
    /// -----------------------------------------------------------------
    /// 短语查询（适用于英语。搜两个边上的词，中间的间隔词记不清楚了，中间的间隔表示跳数）
    ///     PhraseQuery query = new PhraseQuery();
    ///     query.Slop = 1;//跳数
    ///     //第一个term
    ///     query.Add(new Lucene.Net.Index.Term("content", "i"));
    ///     //产生距离之后的第二个term
    ///     query.Add(new Lucene.Net.Index.Term("content", "football"));
    ///     结果：i like football
    /// -----------------------------------------------------------------
    /// 模糊查询（不同于通配符查询，默认会匹配出一个错误的字符结果）
    ///     FuzzyQuery query = new FuzzyQuery(new Lucene.Net.Index.Term("name", "make"));//jak有，jac没有
    ///     有可能会会出现的结果是：mike，jake。
    /// ===================================================================================    
    /// 单列查询表达式
    ///     软件工程师可以使用QueryParser实现各种查询方式，但用户操作必须使用QueryParserEx实现各种查询方式(不支持通配符和模糊查询)。
    ///     QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "content", new Lucene.Net.Analysis.SimpleAnalyzer());//content表示默认搜索字段
    ///     queryParser.DefaultOperator = QueryParser.Operator.AND;//改变默认空格操作符为AND
    ///     Query query = queryParser.Parse("basketball football");//空格默认Or
    ///     Query query = queryParser.Parse("name:mike");//改变搜索字段为name
    ///     Query query = queryParser.Parse("name:j*");//通配符*和？进行匹配
    ///     queryParser.AllowLeadingWildcard = true;//开启第一个通配符的匹配，通配符默认不能放在首位
    ///     Query query = queryParser.Parse("email:*@qq.com");//通配符
    ///     Query query = queryParser.Parse("- name:mike + football);//name没有mike,content有football
    ///     Query query = queryParser.Parse("- name:mike + emaill:qq.com);//name没有mike,email有football
    ///     Query query = queryParser.Parse("id:[1 TO 3]");//匹配一个区间，注意TO必须大写。开区间：1，2，3
    ///     Query query = queryParser.Parse("id:{1 TO 3}");//匹配一个区间，注意TO必须大写。闭区间：2
    ///     Query query = queryParser.Parse("\"I like football\"");//匹配I like football是连起来的，属于短语匹配
    ///     Query query = queryParser.Parse("\"I football\"~1");//匹配I和football之间有一个单词，属于短语匹配
    ///     Query query = queryParser.Parse("name:make~");//模糊查询
    ///     Query query = queryParser.Parse("attach:[2 TO 10]");//QueryParser不能匹配数字类型，（需要使用自己扩展的QueryParserEx匹配数字类型）
    /// -----------------------------------------------------------------
    /// 多列查询表达式
    ///     MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new string[] { "Field1", "Field2" }, new Lucene.Net.Analysis.SimpleAnalyzer());
    ///     Query query = parser.Parse(keyWord);
    /// -------------------------------------------------------------------
    /// 跨度查询
    /// SpanNearQuery query = new Lucene.Net.Search.Spans.SpanNearQuery(
    ///     new Lucene.Net.Search.Spans.SpanQuery[]{
    ///         SpanTermQuery(new Lucene.Net.Index.Term("Summary", keyword1)),  //第一个关键字
    ///         SpanTermQuery(new Lucene.Net.Index.Term("Summary", keyword2))   //第二个关键字
    ///     },
    ///     1,      //1个跨度以内
    ///     true);  //有序
    /// ===================================================================
    /// 多列过滤
    /// FieldCacheTermsFilter cityFilter = new FieldCacheTermsFilter("CITY", new[] { "MUMBAI", "DELHI" });
    /// FieldCacheTermsFilter clientTypeFilter = new FieldCacheTermsFilter("CLIENTTYPE", new[] { "GOLD", "SILVER" });
    /// TermsFilter areaFilter = new TermsFilter();
    /// areaFilter.AddTerm(new Term("Area", "area1"));
    /// areaFilter.AddTerm(new Term("Area", "area2"));
    /// BooleanFilter filter = new BooleanFilter();
    /// filter.Add(new FilterClause(cityFilter, Occur.MUST));
    /// filter.Add(new FilterClause(clientTypeFilter, Occur.MUST));
    /// filter.Add(new FilterClause(areaFilter, Occur.MUST));
    /// --------------------------------------------------------------------
    /// 过滤
    ///     字符串过滤：TermRangeFilter filter = new TermRangeFilter("filename", "java.hhh", "java.she", true, true);
    ///     数字过滤：NumericRangeFilter filter = NumericRangeFilter.NewIntRange("size", 500, 1000, true, true);
    ///     通过查询实现过滤：QueryWrapperFilter filter = new QueryWrapperFilter(new WildcardQuery(new Term("filename", "*.txt")));
    /// ====================================================================
    /// 排序
    ///     Sort.INDEXORDER;//以Doc的id序号排序，可能读不出来评分。
    ///     Sort.RELEVANCE;//以评分排序，可能读不出来评分。
    /// 排序字段
    ///     SortField.SCORE;//以评分排序
    ///     SortField.DOC;//以文档ID排序
    ///     SortField sortField1 = new SortField("size", SortField.INT, true);//true表示降序，false表示升序（默认为false）
    ///     SortField sortField2 = new SortField("date", SortField.LONG, true);
    ///     SortField sortField3 = new SortField("name", SortField.STRING, true);
    /// ====================================================================
    /// 自定义评分排序
    /// a、使用单独定义的评分列
    ///     1、添加自定义排序字段
    ///     document.Add(new NumericField("score", Field.Store.NO, true).SetIntValue(随机数));
    ///     2、原本需要的查询
    ///     TermQuery termQuery = new TermQuery(new Term("content","java"));
    ///     3、创建一个评分列查询
    ///     FieldScoreQuery fieldScoreQuery = new FieldScoreQuery("sore", FieldScoreQuery.Type.INT);//FieldScoreQuery.Type.BYTE;表示字符串类型排序
    ///     4、根据单独定义的评分列和原有的termQuery的评分结果，创建自定义评分的Query对象
    ///     CustomScoreQueryEx query = new CustomScoreQueryEx(termQuery, fieldScoreQuery);
    /// b、根据现有索引中的某一个列的值进行自定义评分
    ///     1、原来的查询
    ///     TermQuery termQuery = new TermQuery(new Term("content","java"));
    ///     2、根据某一个列的值和原有的termQuery创建自定义评分排序的Query对象
    ///     CustomScoreQueryEx query = new CustomScoreQueryEx(termQuery);
    /// </summary>
    /// 
    #endregion

    partial class LuceneBus
    {
        /// <summary>
        /// 缓存关键字
        /// </summary>
        private const string CACHE_KEY = "QUERY:{0},SORT:{1},FILTER:{2},CULTURE:{3}";
        /// <summary>
        /// DataCacheBus缓存的时间,单位:分钟
        /// </summary>
        public static readonly int CACHE_TIME = 20;

        #region TOP查询
        /// <summary>
        /// TOP查询
        /// </summary>
        /// <param name="indexSearcher"></param>
        /// <param name="top"></param>
        /// <param name="query"></param>
        /// <param name="recordCount"></param>
        /// <param name="filter"></param>
        /// <param name="sortFields"></param>
        /// <returns></returns>
        public static Dictionary<Document, ScoreDoc> Select(IndexSearcher indexSearcher, int top, Query query, out int recordCount,string culture="", Filter filter = null, params SortField[] sortFields)
        {
            recordCount = 0;
            Dictionary<Document, ScoreDoc> dictTop = new Dictionary<Document, ScoreDoc>(top);
            string key = string.Format(CACHE_KEY, query.ToString(), string.Join("_", sortFields.Select(item => item.ToString())), filter == null ? string.Empty : filter.ToString()) + string.Format(",TOP:{0}", top, culture.ToUpper());

            TopDocs topDocs = MemCache.MemoryCacheBus.Get(key) as TopDocs;
            if (topDocs == null)
            {
                if (top > 0)
                {
                    if (sortFields.Length > 0)
                    {//支持排序
                        Sort sort = new Sort();
                        sort.SetSort(sortFields);
                        topDocs = indexSearcher.Search(query, filter, top, sort);
                    }
                    else
                    {//不支持排序
                        topDocs = indexSearcher.Search(query, filter, top);//默认按评分排序
                    }
                    if (topDocs != null)
                    {
                        MemCache.MemoryCacheBus.Insert(key, topDocs, TimeSpan.FromMinutes(CACHE_TIME));
                    }
                }
            }
            if (topDocs != null)
            {
                recordCount = topDocs.TotalHits;
                ScoreDoc[] scoreDocs = topDocs.ScoreDocs;
                if (scoreDocs != null)
                {
                    foreach (ScoreDoc scoreDoc in scoreDocs)
                    {
                        if (scoreDoc.Doc != int.MaxValue && scoreDoc.Score != System.Single.NegativeInfinity)
                        {
                            dictTop.Add(indexSearcher.Doc(scoreDoc.Doc), scoreDoc);
                        }
                    }
                }
            }
            if (dictTop.Count == 0)
            {//如果没有取出符合条件的结果删除缓存。wyp
                MemCache.MemoryCacheBus.Delete(key);
            }
            return dictTop;
        }
        #endregion

        #region 查询满足条件的所有结果(不支持分页)
        /// <summary>
        /// 查询满足条件的所有结果(不支持分页)。wyp
        /// </summary>
        /// <param name="indexSearcher"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="query"></param>
        /// <param name="recordCount"></param>
        /// <param name="filter"></param>
        /// <param name="sortFields"></param>
        /// <returns></returns>
        public static Dictionary<Document, ScoreDoc> Select(IndexSearcher indexSearcher, Query query, out int recordCount,string culture="", Filter filter = null, params SortField[] sortFields)
        {
            recordCount = 0;
            Dictionary<Document, ScoreDoc> dictAll = null;
            string key = string.Format(CACHE_KEY, query.ToString(), string.Join("_", sortFields.Select(item => item.ToString())), filter == null ? string.Empty : filter.ToString(), culture.ToUpper());

            TopDocs docs = MemCache.MemoryCacheBus.Get(key) as TopDocs; ;
            if (docs == null)
            {
                if (sortFields.Length > 0)
                {//支持排序
                    Sort sort = new Sort();
                    sort.SetSort(sortFields);
                    docs = indexSearcher.Search(query, filter, int.MaxValue, sort);
                }
                else
                {//不支持排序
                    docs = indexSearcher.Search(query, filter, int.MaxValue);
                }
                if (docs != null)
                {
                    MemCache.MemoryCacheBus.Insert(key, docs, TimeSpan.FromMinutes(CACHE_TIME));
                }
            }
            if (docs != null)
            {
                recordCount = docs.TotalHits;
                dictAll = new Dictionary<Document, ScoreDoc>(recordCount);
                ScoreDoc[] scoreDocs = docs.ScoreDocs;
                if (scoreDocs != null)
                {
                    foreach (ScoreDoc scoreDoc in scoreDocs)
                    {
                        if (scoreDoc.Doc != int.MaxValue && scoreDoc.Score != System.Single.NegativeInfinity)
                        {
                            dictAll.Add(indexSearcher.Doc(scoreDoc.Doc), scoreDoc);
                        }
                    }
                }
            }
            dictAll = dictAll ?? new Dictionary<Document, ScoreDoc>(0);
            if (dictAll.Count == 0)
            {//如果没有取出符合条件的结果删除缓存。wyp
                MemCache.MemoryCacheBus.Delete(key);
            }
            return dictAll;
        }
        #endregion

        #region 分页查询（低效率）
        /// <summary>
        /// 分页查询(低效率)。wyp
        /// </summary>
        /// <param name="indexSearcher"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="query"></param>
        /// <param name="recordCount"></param>
        /// <param name="filter"></param>
        /// <param name="sortFields"></param>
        /// <returns></returns>
        public static Dictionary<Document, ScoreDoc> Select(IndexSearcher indexSearcher, int pageSize, int pageIndex, Query query, out int recordCount, string culture = "", Filter filter = null, params SortField[] sortFields)
        {
            recordCount = 0;
            Dictionary<Document, ScoreDoc> dictPager = new Dictionary<Document, ScoreDoc>(pageSize);

            string key = string.Format(CACHE_KEY, query.ToString(), string.Join("_", sortFields.Select(item => item.ToString())), filter == null ? string.Empty : filter.ToString(), culture.ToUpper());

            TopDocs docs = MemCache.MemoryCacheBus.Get(key) as TopDocs;
            if (docs == null)
            {
                if (sortFields.Length > 0)
                {//支持排序
                    Sort sort = new Sort();
                    sort.SetSort(sortFields);
                    docs = indexSearcher.Search(query, filter, int.MaxValue, sort);//只返回前int.MaxValue条记录
                }
                else
                {//不支持排序
                    docs = indexSearcher.Search(query, filter, int.MaxValue);
                }
                if (docs != null)
                {
                    MemCache.MemoryCacheBus.Insert(key, docs, TimeSpan.FromMinutes(CACHE_TIME));
                }
            }
            if (docs != null)
            {
                recordCount = docs.TotalHits;//搜索结果总数量
                //执行分页操作
                ScoreDoc[] scoreDocs = docs.ScoreDocs;//搜索的结果集合
                if (scoreDocs != null)
                {
                    #region Linq
                    //Linq慢 wyp
                    //ScoreDoc[] scoreDocPages = scoreDocs.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToArray();
                    //foreach (ScoreDoc scoreDoc in scoreDocPages)
                    //{
                    //  if (scoreDoc.Doc != int.MaxValue && scoreDoc.Score != System.Single.NegativeInfinity)
                    //  {
                    //     dictPager.Add(indexSearcher.Doc(scoreDoc.Doc), scoreDoc);
                    //  }
                    //}
                    //Array.Clear(scoreDocPages, 0, scoreDocPages.Length);
                    //scoreDocPages = null;
                    #endregion
                    #region index
                    //index快 wyp
                    int start = (pageIndex - 1) * pageSize;
                    int end = pageIndex * pageSize;
                    if (end > recordCount)
                    {
                        end = recordCount;
                    }
                    ScoreDoc scoreDoc = null;
                    for (int index = start; index < end; index++)
                    {
                        scoreDoc = scoreDocs[index];
                        if (scoreDoc.Doc != int.MaxValue && scoreDoc.Score != System.Single.NegativeInfinity)
                        {
                            dictPager.Add(indexSearcher.Doc(scoreDoc.Doc), scoreDoc);
                        }
                    }
                    #endregion
                }
            }
            if (dictPager.Count == 0)
            {//如果没有取出符合条件的结果删除缓存。wyp
                MemCache.MemoryCacheBus.Delete(key);
            }
            return dictPager;
        }
        #endregion

        #region 分页查询（高效率）
        /// <summary>
        /// 分页查询(高效率)。wyp
        /// http://blog.csdn.net/smallearth/article/details/7980226
        /// </summary>
        /// <param name="indexSearcher"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="query"></param>
        /// <param name="recordCount"></param>
        /// <param name="filter"></param>
        /// <param name="sortFields"></param>
        /// <returns></returns>
        public static Dictionary<Document, ScoreDoc> SelectAfter(IndexSearcher indexSearcher, int pageSize, int pageIndex, Query query, out int recordCount,string culture="", Filter filter = null, params SortField[] sortFields)
        {
            recordCount = 0;
            Dictionary<Document, ScoreDoc> dictPager = new Dictionary<Document, ScoreDoc>(pageSize);
            int maxDoc = indexSearcher.IndexReader.MaxDoc;
            if (maxDoc == 0)
            {//返回索引可用的最大的索引ID
                return dictPager;
            }
            TopDocs docs = null;
            string key = string.Format(CACHE_KEY, query.ToString(), string.Join("_", sortFields.Select(item => item.ToString())), filter == null ? string.Empty : filter.ToString(), culture.ToUpper());
#if LAST_SCORE_DOC
            #region 先取出某（PageIndex）页文档结果中的最后一个文档，然后在从这个文档开始继续往下取出PageSize大小的文档记录
            //http://blog.csdn.net/smallearth/article/details/7980226
            ScoreDoc lastScoreDoc = GetLastScoreDoc(indexSearcher, pageSize, pageIndex, query, filter, culture,sortFields);
            if (lastScoreDoc == null)
            {//lastScoreDoc等于null，相当于需要取第一页的数据。wyp
                key += string.Format(",PAGE_SIZE:{0}", pageSize);
                docs = MemCache.MemoryCacheBus.Get(key) as TopDocs;
                if (docs == null)
                {
                    if (sortFields.Length > 0)
                    {//支持排序
                        Sort sort = new Sort();
                        sort.SetSort(sortFields);
                        docs = indexSearcher.Search(query, filter, pageSize, sort);//只返回前pageSize条记录
                    }
                    else
                    {//不支持排序
                        docs = indexSearcher.Search(query, filter, pageSize);//只返回前pageSize条记录
                    }
                    if (docs != null)
                    {
                        MemCache.MemoryCacheBus.Insert(key, docs, TimeSpan.FromMinutes(CACHE_TIME));
                    }
                }
            }
            else
            {
                if (lastScoreDoc.Doc < maxDoc)
                {
                    key += string.Format(",DOC:{0},PAGE_INDEX:{1},PAGE_SIZE:{2}", lastScoreDoc.Doc,pageIndex, pageSize);
                    docs = MemCache.MemoryCacheBus.Get(key) as TopDocs;
                    if (docs == null)
                    {
                        if (sortFields.Length > 0)
                        {//先排序，后分页
                            int start = pageIndex * pageSize;
                            start = Math.Min(start, maxDoc);
                            Sort sort = new Sort();
                            sort.SetSort(sortFields);
                            TopFieldCollector topFieldCollector = TopFieldCollector.Create(sort, start, true, false, false, !query.CreateWeight(indexSearcher).GetScoresDocsOutOfOrder());
                            indexSearcher.Search(query, filter, topFieldCollector);
                            start = start - pageSize;
                            if (start < 0)
                            {
                                start = 0;
                            }
                            docs = topFieldCollector.TopDocs(start, pageSize);//只返回前start条记录
                        }
                        else
                        {//不支持排序，只有分页
                            //http://search-lucene.com/c/Lucene:core/src/java/org/apache/lucene/search/IndexSearcher.java||IndexSearcher 482行
                            TopScoreDocCollectorEx topScoreDocCollectorEx = TopScoreDocCollectorEx.Create(pageSize, lastScoreDoc, !query.CreateWeight(indexSearcher).GetScoresDocsOutOfOrder());
                            indexSearcher.Search(query, filter, topScoreDocCollectorEx);
                            docs = topScoreDocCollectorEx.TopDocs();
                        }
                        if (docs != null)
                        {
                            MemCache.MemoryCacheBus.Insert(key, docs, TimeSpan.FromMinutes(CACHE_TIME));
                        }
                    }
                }
            }
            #endregion
#else
            #region 先取出前（PageIndex+1）页的文档数，然后在从文档结果中取出最后一页的文档记录。
            key += string.Format(",PAGE_INDEX:{0},PAGE_SIZE:{1}", pageIndex, pageSize);
            docs = WebCache.DataCacheBus.Get(key) as TopDocs;
            if (docs == null)
            {
                //https://searchcode.com/codesearch/view/7233825/
                int start = pageIndex * pageSize;
                start = Math.Min(start, maxDoc);
                if (sortFields.Length > 0)
                {//先排序，后分页
                    Sort sort = new Sort();
                    sort.SetSort(sortFields);
                    TopFieldCollector topFieldCollector = TopFieldCollector.Create(sort, start, true, false, false, !query.CreateWeight(indexSearcher).GetScoresDocsOutOfOrder());
                    indexSearcher.Search(query, filter, topFieldCollector);
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
                    indexSearcher.Search(query, filter, topScoreDocCollector);
                    start = start - pageSize;
                    if (start < 0)
                    {
                        start = 0;
                    }
                    docs = topScoreDocCollector.TopDocs(start, pageSize);//只返回前start条记录
                }
                if (docs != null)
                {
                    WebCache.DataCacheBus.Insert(key, docs, TimeSpan.FromMinutes(CACHE_TIME));
                }
            }
            #endregion
#endif
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
#if LAST_SCORE_DOC
                            lastScoreDoc = scoreDoc;//获取搜索结果中当前页中最后一个ScoreDoc对象
#endif
                            dictPager.Add(indexSearcher.Doc(scoreDoc.Doc), scoreDoc);
                        }
                    }
                }
            }
            if (dictPager.Count == 0)
            {//如果没有取出符合条件的结果删除缓存。wyp
                MemCache.MemoryCacheBus.Delete(key);
            }
#if LAST_SCORE_DOC
            else if (lastScoreDoc != null)
            {//提前设置好当用户搜索下一页时，需要用到当前这一页的最后一个ScoreDoc对象用于下次搜索使用。wyp
                key = string.Format(CACHE_KEY, query.ToString(), string.Join("_", sortFields.Select(item => item.ToString())), filter == null ? string.Empty : filter.ToString(), culture.ToUpper()) + string.Format(",PAGE_INDEX:{0},PAGE_SIZE:{1}", pageIndex + 1, pageSize);//提前把下一页用到的LastScoreDoc放入缓存中。wyp
                MemCache.MemoryCacheBus.Insert(key, lastScoreDoc, TimeSpan.FromMinutes(CACHE_TIME));
            }
#endif
            #endregion

            return dictPager;
        }
        /// <summary>
        /// 得到某一页的分页结果里面的最后一条记录。wyp
        /// </summary>
        /// <param name="indexSearcher"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <param name="sortFields"></param>
        /// <returns></returns>
        private static ScoreDoc GetLastScoreDoc(IndexSearcher indexSearcher, int pageSize, int pageIndex, Query query, Filter filter = null, string culture="", params SortField[] sortFields)
        {
            ScoreDoc lastScoreDoc = null;
            if (pageIndex < 2)
            {
                return lastScoreDoc;
            }

            string key = string.Format(CACHE_KEY, query.ToString(), string.Join("_", sortFields.Select(item => item.ToString())), filter == null ? string.Empty : filter.ToString(),culture.ToUpper()) + string.Format(",PAGE_INDEX:{0},PAGE_SIZE:{1}", pageIndex, pageSize);

            lastScoreDoc = MemCache.MemoryCacheBus.Get(key) as ScoreDoc;
            if (lastScoreDoc == null)
            {//如果用户直接输入打开的页数或者，用户在页面上是跳着点页数的链接地址，还是需要从Lucene中使用Top的方式进行查询。wyp
                int top = pageSize * (pageIndex - 1);
                TopDocs topDocs = null;
                if (top > 0)
                {
                    if (sortFields.Length > 0)
                    {//支持排序
                        Sort sort = new Sort();
                        sort.SetSort(sortFields);
                        topDocs = indexSearcher.Search(query, filter, top, sort);
                    }
                    else
                    {//不支持排序
                        topDocs = indexSearcher.Search(query, filter, top);
                    }
                }
                if (topDocs != null)
                {
                    ScoreDoc[] scoreDocs = topDocs.ScoreDocs;
                    if (scoreDocs != null)
                    {
                        if (scoreDocs.Length > 0)
                        {
                            int index = 1;
                            do
                            {
                                lastScoreDoc = scoreDocs[scoreDocs.Length - index];
                                if (lastScoreDoc.Doc != int.MaxValue && lastScoreDoc.Score != System.Single.NegativeInfinity)
                                {
                                    break;
                                }
                                index++;
                            }
                            while (index < scoreDocs.Length);
                        }
                        Array.Clear(scoreDocs, 0, scoreDocs.Length);
                        scoreDocs = null;
                    }
                    topDocs = null;
                }
            }
            return lastScoreDoc;
        }

        #endregion
    }
}
