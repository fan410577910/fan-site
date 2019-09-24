#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  StopWord
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 停用词（忽略词）
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Analysis;
using System.IO;
using System.Text;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 停用词（忽略词）
    /// </summary>
    class StopWord
    {
        private static CharArraySet _StopWordList = null;

        static StopWord()
        {
            CharArraySet charArraySet = new CharArraySet(0, true);
            string applicationPath = Path.Combine(LuceneNetConfig.LuceneDictDirectory, "Stopword.txt");
            if (File.Exists(applicationPath))
            {
                Encoding encoding = EncodingType.GetType(applicationPath);
                using (StreamReader sr = new StreamReader(applicationPath, encoding))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line != null)
                        {
                            charArraySet.Add(line);
                        }
                    }
                }
            }
            //charArraySet.AddAll(StopAnalyzer.ENGLISH_STOP_WORDS_SET);//英语停用词，我们使用StandardAnalyzer分析器里面已经使用了英语停用词，所以就不需要在添加了。
            _StopWordList = CharArraySet.UnmodifiableSet(charArraySet);
        }

        public static CharArraySet StopWordList
        {
            get { return StopWord._StopWordList; }
        }

    }
}
