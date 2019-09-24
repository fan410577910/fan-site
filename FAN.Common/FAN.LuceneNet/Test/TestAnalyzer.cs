#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  TestAnalyzer 
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
using Lucene.Net.Analysis.Tokenattributes;
using System;
using System.Collections.Generic;
using System.IO;

namespace TLZ.LuceneNet
{
    [Obsolete("wangyunpeng，测试专用")]
    public class TestAnalyzer
    {
        /// <summary>
        /// 显示分词信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="analzyer"></param>
        /// <returns></returns>
        public static List<string> TestTerm(string content, Analyzer analzyer)
        {
            List<string> list = new List<string>();
            using (TokenStream tokenStream = analzyer.ReusableTokenStream("", new StringReader(content)))
            {
                //tokenStream.AddAttribute<ITermAttribute>();
                while (tokenStream.IncrementToken())
                {
                    ITermAttribute termAttribute = tokenStream.GetAttribute<ITermAttribute>();
                    list.Add(termAttribute.Term);
                }
            }
            return list;
        }
        /// <summary>
        /// 显示分词完整信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="analzyer"></param>
        /// <returns></returns>
        public static List<TermInfo> TestTermAll(string content, Analyzer analzyer)
        {
            List<TermInfo> list = new List<TermInfo>();
            using (TokenStream tokenStream = analzyer.ReusableTokenStream("", new StringReader(content)))
            {
                //tokenStream.AddAttribute<ITermAttribute>();
                while (tokenStream.IncrementToken())
                {
                    ITermAttribute termAttribute = tokenStream.GetAttribute<ITermAttribute>();
                    IPositionIncrementAttribute postionIncrementAttribute = tokenStream.GetAttribute<IPositionIncrementAttribute>();
                    ITypeAttribute typeAttribute = tokenStream.GetAttribute<ITypeAttribute>();
                    IOffsetAttribute offsetAttribute = tokenStream.GetAttribute<IOffsetAttribute>();
                    TermInfo obj = new TermInfo();
                    //obj.FlagsAttribute = tokenStream.GetAttribute<IFlagsAttribute>().Flags.ToString();
                    //obj.PayloadAttribute = tokenStream.GetAttribute<IPayloadAttribute>().Payload.Length.ToString();
                    obj.TermAttribute = termAttribute.Term;
                    obj.OffsetAttribute = offsetAttribute.StartOffset.ToString() + "---" + offsetAttribute.EndOffset.ToString();
                    obj.PositionIncrementAttribute = postionIncrementAttribute.PositionIncrement.ToString();
                    obj.TypeAttribute = typeAttribute.Type;
                    obj.TokenStream = tokenStream;
                    list.Add(obj);
                }
            }
            return list;
        }
    }

    public class TermInfo
    {
        //public string PayloadAttribute { get; set; }
        //public string FlagsAttribute { get; set; }
        public string TermAttribute { get; set; }
        public string OffsetAttribute { get; set; }
        public string PositionIncrementAttribute { get; set; }
        public string TypeAttribute { get; set; }

        public TokenStream TokenStream { get; set; }
    }
}
