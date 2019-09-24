#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  TokenizerDict
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 分词器字典
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.AR;
using Lucene.Net.Analysis.CJK;
using Lucene.Net.Analysis.Cn;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Ru;
using Lucene.Net.Analysis.Standard;
using System.Collections.Generic;
using System.IO;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 分词器字典
    /// 每个Analyzer都有一个辅助类，一般以”“Tokenizer”结尾，分词的逻辑大都在辅助类完成。
    /// </summary>
    static class TokenizerDict
    {
        private static Dictionary<string, Tokenizer> _dict = new Dictionary<string, Tokenizer>();
        static TokenizerDict()
        {
            _dict.Add("EN", new StandardTokenizer(global::Lucene.Net.Util.Version.LUCENE_30, new StringReader("")));//英语
            _dict.Add("AR", new ArabicLetterTokenizer(new StringReader("")));//阿拉伯语
            _dict.Add("PT", new StandardTokenizer(global::Lucene.Net.Util.Version.LUCENE_30, new StringReader("")));//葡萄牙语
            _dict.Add("KO", new CJKTokenizer(Lucene.Net.Util.AttributeSource.AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY, new StringReader("")));//韩语
            _dict.Add("JA", new PanGuTokenizer(new StringReader("")));//日语
            _dict.Add("ZH", new ChineseTokenizer(new StringReader("")));//汉语
            _dict.Add("CS", new StandardTokenizer(global::Lucene.Net.Util.Version.LUCENE_30, new StringReader("")));//捷克语
            _dict.Add("DE", new StandardTokenizer(global::Lucene.Net.Util.Version.LUCENE_30, new StringReader("")));//德语
            _dict.Add("EL", new StandardTokenizer(global::Lucene.Net.Util.Version.LUCENE_30, new StringReader("")));//希腊语
            //_dict.Add("波斯语", new ArabicLetterTokenizer(new StringReader("")));//波斯语暂时有问题
            _dict.Add("FR", new StandardTokenizer(global::Lucene.Net.Util.Version.LUCENE_30, new StringReader("")));//法语
            _dict.Add("NL", new StandardTokenizer(global::Lucene.Net.Util.Version.LUCENE_30, new StringReader("")));//荷兰语
            _dict.Add("RU", new RussianLetterTokenizer(new StringReader("")));//俄语
            _dict.Add("TH", new StandardTokenizer(global::Lucene.Net.Util.Version.LUCENE_30, new StringReader("")));//泰语
           
            //_dict.Add("西班牙语", new SnowballAnalyzer(global::Lucene.Net.Util.Version.LUCENE_30, "Spanish"));
        }
        /// <summary>
        /// 根据语言获取分词器
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns>返回语言对应的分词器</returns>
        public static Tokenizer GetTokenStream(string language)
        {
            if (_dict.ContainsKey(language))
                return _dict[language];
            return _dict["EN"];
        }
    }
}
