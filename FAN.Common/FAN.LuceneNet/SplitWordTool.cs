#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  SplitTool 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 分隔词的工具类
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Analysis.Tokenattributes;
using System.Collections.Generic;
using System.IO;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 分隔词的工具类
    /// </summary>
    public class SplitWordTool
    {

        /// <summary>
        /// 将字分成字的数组
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string[] SplitWord(string word)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(word))
            {
                return list.ToArray();
            }
            using (SymbolAnalyzer simple = new SymbolAnalyzer())
            {
                using (TokenStream ts = simple.ReusableTokenStream("", new StringReader(word)))//只显示分词信息,不需要使用FieldName
                {
                    while (ts.IncrementToken())
                    {
                        ITermAttribute attribute = ts.GetAttribute<ITermAttribute>();
                        list.Add(attribute.Term);
                    }
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 将word取出词干，支持停用词
        /// </summary>
        /// <param name="word"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string SnowballWord(string word, string language)
        {
            string result = null;
            string stemmer = SnowballDict.GetStemmer(language);
            if (stemmer == null)
            {
                result = word;
            }
            else
            {
                using (SnowballAnalyzer snowball = new SnowballAnalyzer(Lucene.Net.Util.Version.LUCENE_30, stemmer, StopWord.StopWordList))
                {
                    using (TokenStream ts = snowball.ReusableTokenStream("", new StringReader(word)))//只显示分词信息,不需要使用FieldName
                    {
                        while (ts.IncrementToken())
                        {
                            ITermAttribute attribute = ts.GetAttribute<ITermAttribute>();
                            result = attribute.Term;
                        }
                    }
                }
            }
            return result;
        }
    }
}
