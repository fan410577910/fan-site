#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  SynonymsFilter 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 过滤器
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using System.Collections.Generic;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 过滤器
    /// 自定义同义词
    /// </summary>
    public class SynonymsFilter : TokenFilter
    {
        private string _Language = null;
        /// <summary>
        /// 同义词集合
        /// </summary>
        private Queue<string> _SynonymsQueue = null;
        private ITermAttribute _TermAttribute = null;
        private IPositionIncrementAttribute _PostionIncrementAttribute = null;
        private List<Dictionary<string, string[]>> _synonymsDictList = null;
        /// <summary>
        /// 有同义词的词的状态
        /// </summary>
        private State _Current = null;

        public SynonymsFilter(string language, TokenStream input)
            : base(input)
        {
            this._Language = language;
            this._SynonymsQueue = new Queue<string>();
            this._TermAttribute = base.AddAttribute<ITermAttribute>();
            this._PostionIncrementAttribute = base.AddAttribute<IPositionIncrementAttribute>();
        }
        public SynonymsFilter(TokenStream input,List<Dictionary<string, string[]>> synonymsDictList)
            : base(input)
        {
            this._SynonymsQueue = new Queue<string>();
            this._TermAttribute = base.AddAttribute<ITermAttribute>();
            this._PostionIncrementAttribute = base.AddAttribute<IPositionIncrementAttribute>();
            this._synonymsDictList = synonymsDictList;
        }

        /// <summary>
        /// 取出下一个词
        /// </summary>
        /// <returns>True表示跳到下一个要添加到索引里面的词，False表示没有后面的词了</returns>
        public override bool IncrementToken()
        {
            if (this._SynonymsQueue.Count > 0)
            {//如果上一个索引的词出现在同义词列表里面了
                //将同义词集合出栈，并获取同义词
                string buffer = this._SynonymsQueue.Dequeue();
                base.RestoreState(this._Current);//还原上一个要索引的词(有同义词的词)的状态
                this._TermAttribute.SetTermBuffer(buffer);//设置同义词
                this._PostionIncrementAttribute.PositionIncrement = 0;//设置位置为0,表示同义词
                return true;
            }
            if (!base.input.IncrementToken())
            {
                return false;//如果已经没有下一个词了，到了末尾了返回False
            }
            if (this.AddSynonymsWordToQueue())
            {//如果有同义词，将当前词的状态保存
                this._Current = base.CaptureState();
            }
            return true;//跳到下一个要添加到索引里面的词了
        }

        /// <summary>
        /// 添加同义词
        /// </summary>
        /// <returns>False表示没有同义词,True表示有同义词</returns>
        private bool AddSynonymsWordToQueue()
        {
            List<string> synonymsWordList = new List<string>();
          
            if (this._synonymsDictList != null)
            { //如果有外部传过来的近义词，就取外部的近义词列表
                foreach (Dictionary<string, string[]> dict in _synonymsDictList)
                {
                    foreach (KeyValuePair<string, string[]> pair in dict)
                    {
                        if (pair.Key.ToLower().Contains(this._TermAttribute.Term))
                        {
                            synonymsWordList.AddRange(pair.Value);
                        }
                    }
                    if (!synonymsWordList.Contains(this._TermAttribute.Term))
                    {
                        synonymsWordList.Add(this._TermAttribute.Term);
                    }
                }
            }
            else
            { //如果没有外部传过来的近义词，就取Synonym.txt的近义词列表
                synonymsWordList = SynonymDict.GetSynonymsWord(this._Language, this._TermAttribute.Term);
            }

            if (synonymsWordList == null || synonymsWordList.Count <= 0)
            {
                return false;
            }
            foreach (string synonymsWord in synonymsWordList)
            {
                if (!this._TermAttribute.Term.ToLower().Equals(synonymsWord))
                {//取出同义词，不包含已经被添加到索引里面的词
                    this._SynonymsQueue.Enqueue(synonymsWord);
                }
            }
            return true;
        }
    }
}
