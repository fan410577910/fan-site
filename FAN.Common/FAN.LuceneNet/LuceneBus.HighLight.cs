#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Highlight 
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
using Lucene.Net.Analysis;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using System.Text.RegularExpressions;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        const string PRE_TAG = "<font color=\"#FF0000\">";
        const string END_TAG = "</font>";
        /// <summary>
        /// 搜索结果高亮显示
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="content">搜索结果</param>
        /// <returns>高亮后显示的结果</returns> 
        public static string HighLight(string keyword,string content)
        {
            string[] keywords = SplitWordTool.SplitWord(keyword);
            bool isInclude = false;
            if (keywords.Length > 1)
            {
                content = HighLight(keyword,content,out isInclude);
            }
            if (!isInclude)
            {
                foreach (string word in keywords)
                {
                    content = HighLight(word,content,out isInclude);
                }
            }
            return content;
        }
        /// <summary>
        /// 搜索结果高亮显示
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="content">搜索结果</param>
        /// <param name="isInclude">是否包含关键字，True表示包含，False表示不包含</param>
        /// <returns>高亮后显示的结果</returns>
        private static string HighLight(string keyword,string content,out bool isInclude)
        {
            isInclude = false;
            Regex regex = new Regex(keyword,RegexOptions.IgnoreCase);
            int index = 0;
            for (Match m = regex.Match(content); m.Success; m = m.NextMatch())
            {
                isInclude = true;
                content = content.Insert(m.Groups[0].Index + index,PRE_TAG);
                index += PRE_TAG.Length;
                content = content.Insert(m.Index + keyword.Length + index,END_TAG);
                index += END_TAG.Length;
            }
            return content;
        }
        /// <summary>
        /// 搜索结果高亮显示
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="content">搜索结果</param>
        /// <param name="analyzer">new SimpleAnalyzer()</param>
        /// <returns></returns>
        public static string HighLight(string keyword,string content,Analyzer analyzer)
        {
            const string FIELD_NAME = "keyword";
            Query query = new QueryParserEx(Lucene.Net.Util.Version.LUCENE_30,FIELD_NAME,analyzer).Parse(keyword);
            QueryScorer scorer = new QueryScorer(query);
            SimpleHTMLFormatter formatter = new SimpleHTMLFormatter(PRE_TAG,END_TAG);
            SimpleSpanFragmenter fragment = new SimpleSpanFragmenter(scorer);
            Highlighter highlighter = new Highlighter(formatter,scorer);
            highlighter.TextFragmenter = fragment;
            return highlighter.GetBestFragment(analyzer,FIELD_NAME,content) ?? content;
        }
    }
}
