using Lucene.Net.Search;
using Lucene.Net.Search.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 根据权重扩展自定义评分
    /// </summary>
    public class CustomWeightQueryEx : CustomScoreQuery
    {
        private string _language = null;
        private List<CustomWeightInfo> _customWeightInfoList = null;
        private string _key = null;
        public CustomWeightQueryEx(string language,string key,Query subQuery, List<CustomWeightInfo> customScoreInfoList = null)
            : base(subQuery)
        {
            this._language = language;
            this._customWeightInfoList = customScoreInfoList;
            this._key = key;
        }

        protected override CustomScoreProvider GetCustomScoreProvider(Lucene.Net.Index.IndexReader reader)
        {
            //默认情况实现的评分是通过原有的评分 * 传入进来的评分列所获取的评分来确定最终打分。
            //return base.GetCustomScoreProvider(reader);
            //为了根据不同的需求进行评分，需要自己进行评分的设定。
            /*
             * 自定义评分的步骤
             * 创建一个类继承于CustomScoreProvider
             * 覆盖CustomScore方法
             */
            return new CustomWeightProviderEx(this._language, this._key, reader, this._customWeightInfoList);
        }
    }
}
