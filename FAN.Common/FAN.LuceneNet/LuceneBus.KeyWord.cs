#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.KeyWord 
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
using System.Collections.Generic;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 得到关键字
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string GetKeyWords(string keyword)
        {
            string[] keywords = SplitWordTool.SplitWord(keyword);
            return GetKeyWords(keywords);
        }
        /// <summary>
        /// 得到关键词
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static string GetKeyWords(IEnumerable<string> keywords)
        {
            return string.Join(" ", keywords);
        }
        /// <summary>
        /// 获取同义词，相关词
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static List<string> GetKeyWordsWithSynonyms(string keyWord, string language = "EN")
        {
            List<string> wordList = new List<string>();
            string[] keywords = SplitWordTool.SplitWord(keyWord);
            foreach (string word in keywords)
            {
                string item = SplitWordTool.SnowballWord(word, language);
                wordList.AddRange(SynonymDict.GetSynonymsWord(language, item));
            }
            return wordList;
        }
    }
}
