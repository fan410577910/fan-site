#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  AnalyzerBus 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 分析器总线
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Snowball;
using SF.Snowball;
using System;
using System.Collections.Generic;
using System.IO;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 分析器总线（支持：同义词，近义词，相关词）
    /// </summary>
    public class AnalyzerBus : Analyzer
    {
        private Analyzer _Analyzer = null;
        private SymbolAnalyzer _SymbolAnalyzer = null;
        private ISet<string> _StopCharArraySet = null;
        private bool _UseIndexSynonyms = false;
        private string _Language = null;
        private readonly bool _EnableStopPositionIncrements = false;
        private string[] _NormalizeChars = new string[] { "-", "_", ",", "，", "|", ".", "。", "=", "&", "/", "\\", ";", "；", "#" };
        /// <summary>
        /// 创建分析器
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="useIndexSynonyms">true表示在创建索引时，将同义词，近义词，相关词存入索引；false表示不使用。</param>
        public AnalyzerBus(string language, bool useIndexSynonyms = false)
        {
            this._EnableStopPositionIncrements = StopFilter.GetEnablePositionIncrementsVersionDefault(global::Lucene.Net.Util.Version.LUCENE_30);
            this._Language = language;
            this._UseIndexSynonyms = useIndexSynonyms;
            this._SymbolAnalyzer = new SymbolAnalyzer();
            this._Analyzer = AnalyzerDict.GetAnalyzer(language.ToUpper());
            this._StopCharArraySet = StopWord.StopWordList;
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            if (!this._Language.Equals("JA", StringComparison.CurrentCultureIgnoreCase))
            {//不是日语的需要重建Reader。wangyunpeng
                reader = this.InitReader(reader);
            }
            TokenStream result = this._Analyzer.TokenStream(fieldName, reader);
            result = new StopFilter(this._EnableStopPositionIncrements, result, this._StopCharArraySet, true);
            SnowballProgram snowballProgram = SnowballDict.GetSnowball(this._Language);//词干。wangyunpeng，2015-8-17改成线程安全的调用方式。
            if (snowballProgram != null)
            {
                result = new SnowballFilter(result, snowballProgram);
            }
            if (_UseIndexSynonyms)
            {//在创建索引的时候，将同义词，近义词，相关词存入索引。
                result = new SynonymsFilter(this._Language, result);
            }
            return result;
        }
        /// <summary>
        /// http://stackoverflow.com/questions/15235126/lucene-4-1-how-split-words-that-contains-dots-when-indexing
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private TextReader InitReader(TextReader reader)
        {
            NormalizeCharMap normalizeCharMap = new NormalizeCharMap();
            foreach (string normalizeChar in this._NormalizeChars)
            {
                normalizeCharMap.Add(normalizeChar, " ");
            }
            return new MappingCharFilter(normalizeCharMap, reader);
        }
        public override void Dispose()
        {
            Array.Clear(this._NormalizeChars, 0, this._NormalizeChars.Length);
            this._NormalizeChars = null;
            base.Dispose();
        }
    }
}
