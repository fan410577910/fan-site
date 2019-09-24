#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  AnalyzerDict
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
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.AR;
using Lucene.Net.Analysis.BR;
using Lucene.Net.Analysis.CJK;
using Lucene.Net.Analysis.Cn;
using Lucene.Net.Analysis.Cz;
using Lucene.Net.Analysis.De;
using Lucene.Net.Analysis.El;
using Lucene.Net.Analysis.Fr;
using Lucene.Net.Analysis.Nl;
using Lucene.Net.Analysis.Ru;
using Lucene.Net.Analysis.Th;
using System.Collections.Generic;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Es;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 分析器字典
    /// </summary>
    public static class AnalyzerDict
    {
        private static Dictionary<string, Analyzer> _dict = new Dictionary<string, Analyzer>();
        static AnalyzerDict()
        {
            //http://www.cnblogs.com/jinzhao/archive/2012/02/13/2348908.html盘古分词的Luke.net

            _dict.Add("EN", new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//英语
            _dict.Add("ES", new SpanishAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//西班牙语，从java中拿出来的code.wangyunpeng
            _dict.Add("AR", new ArabicAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//阿拉伯语
            _dict.Add("PT", new BrazilianAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//葡萄牙语
            _dict.Add("KO", new CJKAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//韩语
            _dict.Add("JA", new PanGuAnalyzer());//日语
            _dict.Add("ZH", new ChineseAnalyzer());//中文
            _dict.Add("CS", new CzechAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//捷克语
            _dict.Add("DE", new GermanAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//德语
            _dict.Add("EL", new GreekAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//希腊语
            //_dict.Add("波斯语", new PersianAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//波斯语需要stopwords.txt，暂时不用
            _dict.Add("FR", new FrenchAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//法语
            _dict.Add("NL", new DutchAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//荷兰语
            _dict.Add("RU", new RussianAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//俄语
            _dict.Add("TH", new ThaiAnalyzer(Lucene.Net.Util.Version.LUCENE_30));//泰语
        }
        /// <summary>
        /// 根据语言获取分析器
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns>返回语言对应的分析器</returns>
        public static Analyzer GetAnalyzer(string language)
        {
            if (_dict.ContainsKey(language))
                return _dict[language];
            return _dict["EN"];
        }
    }
}
