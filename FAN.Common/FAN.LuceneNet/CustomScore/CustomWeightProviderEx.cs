using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Search.Function;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TLZ.LuceneNet
{
    public class CustomWeightProviderEx : CustomScoreProvider
    {
        private string _language = null;
        private static Dictionary<string, List<CustomWeightInfo>> _customWeightInfoDict = new Dictionary<string, List<CustomWeightInfo>>();
        private List<CustomWeightInfo> _customScoreInfoList = null;
        private IndexReader _indexReader = null;
        private string[] _fieldCache = null;
        private string _key = null;
        private List<CustomWeightInfo> CustomWeightInfoList
        {
            get
            {
                if (_customWeightInfoDict.ContainsKey(this._language))
                {
                    this._customScoreInfoList = _customWeightInfoDict[this._language];
                }
                if (this._customScoreInfoList == null)
                {
                    _customWeightInfoDict[this._language] = this._customScoreInfoList = new List<CustomWeightInfo>(0);
                }
                return this._customScoreInfoList;
            }
        }

        private string[] FieldCache
        {
            get
            {
                if (this._fieldCache == null)
                {
                    this._fieldCache = FieldCache_Fields.DEFAULT.GetStrings(this._indexReader, "TitleNotAnalyzed");
                }
                return this._fieldCache;
            }

        }
        public CustomWeightProviderEx(string language,string key, IndexReader reader, List<CustomWeightInfo> customWeightInfo)
            : base(reader)
        {
            this._language = language;
            this._customScoreInfoList = customWeightInfo;
            this._indexReader = reader;
            this._key = key;
        }
        /// <summary>
        /// 自定义评分操作的实现方式
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="subQueryScore">原来文档的打分</param>
        /// <param name="valSrcScore">评分列的打分（不区分是哪一种评分方式）</param>
        /// <returns></returns>
        public override float CustomScore(int doc, float subQueryScore, float valSrcScore)
        {
            float score = subQueryScore;
            object fieldDoc = null;
            fieldDoc = this.FieldCache.GetValue(doc);
            if (fieldDoc == null)
            {
                return subQueryScore;
            }
            string fieldValue = fieldDoc.ToString();
            foreach (CustomWeightInfo customWeightInfo in this.CustomWeightInfoList)
            {
                if (fieldValue.IndexOf(customWeightInfo.KeyWord, StringComparison.CurrentCultureIgnoreCase) > -1 
                    && this._key.IndexOf(customWeightInfo.KeyWord, StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    if (customWeightInfo.Weight <= 0.0f)
                    {
                        continue;
                    }
                    score = score * customWeightInfo.Weight;
                    break;
                }
            }
            return score;
        }
    }
}
