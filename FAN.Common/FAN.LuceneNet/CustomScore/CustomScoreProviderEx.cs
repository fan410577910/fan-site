#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  CustomScoreInfo
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 自定义评分适配器扩展，支持文档增加自定义评分列、根据索引原来某一个列的值和默认评分三种方式进行自定义评分查询操作。
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Search.Function;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 自定义评分适配器扩展
    /// 支持文档增加自定义评分列、根据索引原来某一个列的值和默认评分三种方式进行自定义评分查询操作。
    /// </summary>
    public class CustomScoreProviderEx : CustomScoreProvider
    {
        private string _language = null;
        private static Dictionary<string, List<CustomScoreInfo>> _CustomScoreInfoDict = new Dictionary<string, List<CustomScoreInfo>>();
        private List<CustomScoreInfo> CustomScoreInfoList
        {
            get
            {
                List<CustomScoreInfo> customScoreInfoList = null;
                if (_CustomScoreInfoDict.ContainsKey(this._language))
                {
                    customScoreInfoList = _CustomScoreInfoDict[this._language];
                }
                if (customScoreInfoList == null)
                {
                    _CustomScoreInfoDict[this._language] = customScoreInfoList = CustomScoreDict.CustomScoreInfos(this._language);
                }
                return customScoreInfoList;
            }
        }
        public CustomScoreProviderEx(string language, IndexReader reader)
            : base(reader)
        {
            this._language = language;
            List<CustomScoreInfo> customScoreInfoList = this.CustomScoreInfoList;
            if (customScoreInfoList == null || customScoreInfoList.Count == 0)
            {
                return;
            }
            foreach (CustomScoreInfo customScoreInfo in customScoreInfoList)
            {
                if (customScoreInfo.FieldValues == null)
                {
                    string fieldName = customScoreInfo.FieldName;
                    if (string.IsNullOrWhiteSpace(fieldName))
                    {
                        continue;
                    }
                    Array fieldValues = null;
                    switch (customScoreInfo.FieldType)
                    {
                        case FieldType.INT64:
                            fieldValues = FieldCache_Fields.DEFAULT.GetLongs(reader, fieldName);//取出所有文档某一列的值
                            break;
                        case FieldType.INT32:
                            fieldValues = FieldCache_Fields.DEFAULT.GetInts(reader, fieldName);
                            break;
                        case FieldType.SINGLE:
                            fieldValues = FieldCache_Fields.DEFAULT.GetFloats(reader, fieldName);
                            break;
                        case FieldType.DOUBLE:
                            fieldValues = FieldCache_Fields.DEFAULT.GetDoubles(reader, fieldName);
                            break;
                        case FieldType.DATETIME:
                            fieldValues = FieldCache_Fields.DEFAULT.GetLongs(reader, fieldName);
                            break;
                        case FieldType.STRING:
                        default:
                            fieldValues = FieldCache_Fields.DEFAULT.GetStrings(reader, fieldName);
                            break;
                    }
                    customScoreInfo.FieldValues = fieldValues;
                }
            }
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
            #region 第一种评分方式：根据现有文档索引中的某一个列的值进行自定义评分方式，配合外部CustomScore.xml文件完成自定义评分
            List<CustomScoreInfo> customScoreInfoList = this.CustomScoreInfoList;
            if (customScoreInfoList != null && customScoreInfoList.Count > 0)
            {
                object fieldDoc = null;
                List<WordScore> wordScoreList = null;
                string scoreType = null;
                foreach (CustomScoreInfo customScoreInfo in customScoreInfoList)
                {
                    if (customScoreInfo.FieldValues == null)
                        continue;
                    fieldDoc = customScoreInfo.FieldValues.GetValue(doc);//获取某一字段的某一条记录信息
                    if (fieldDoc == null)
                        continue;
                    wordScoreList = customScoreInfo.WordScoreList;
                    scoreType = customScoreInfo.FieldType;

                    if (scoreType == FieldType.INT64)
                    {
                        long fieldValue = (long)fieldDoc;
                        foreach (WordScore wordScore in wordScoreList)
                        {
                            if (fieldValue == StrToT<long>(wordScore.Word))
                            {
                                if (wordScore.Quotiety <= 0.0f)
                                    return subQueryScore;
                                return subQueryScore * wordScore.Quotiety;
                            }
                        }
                    }
                    else if (scoreType == FieldType.INT32)
                    {
                        int fieldValue = (int)fieldDoc;
                        foreach (WordScore wordScore in wordScoreList)
                        {
                            if (!string.IsNullOrWhiteSpace(wordScore.Word))
                            {
                                if (fieldValue == StrToT<int>(wordScore.Word))
                                {
                                    if (wordScore.Quotiety <= 0.0f)
                                        return subQueryScore;
                                    return subQueryScore * wordScore.Quotiety;
                                }
                            }
                        }
                    }
                    else if (scoreType == FieldType.SINGLE)
                    {
                        float fieldValue = (float)fieldDoc;
                        foreach (WordScore wordScore in wordScoreList)
                        {
                            if (!string.IsNullOrWhiteSpace(wordScore.Word))
                            {
                                if (fieldValue == StrToT<float>(wordScore.Word))
                                {
                                    if (wordScore.Quotiety <= 0.0f)
                                        return subQueryScore;
                                    return subQueryScore * wordScore.Quotiety;
                                }
                            }
                        }
                    }
                    else if (scoreType == FieldType.DOUBLE)
                    {
                        double fieldValue = (double)fieldDoc;
                        foreach (WordScore wordScore in wordScoreList)
                        {
                            if (!string.IsNullOrWhiteSpace(wordScore.Word))
                            {
                                if (fieldValue == StrToT<double>(wordScore.Word))
                                {
                                    if (wordScore.Quotiety <= 0.0f)
                                        return subQueryScore;
                                    return subQueryScore * wordScore.Quotiety;
                                }
                            }
                        }
                    }
                    else if (scoreType == FieldType.DATETIME)
                    {
                        long fieldValue = (long)fieldDoc;
                        foreach (WordScore wordScore in wordScoreList)
                        {
                            if (!string.IsNullOrWhiteSpace(wordScore.Word))
                            {
                                if (fieldValue == StrToT<DateTime>(wordScore.Word).Ticks)
                                {
                                    if (wordScore.Quotiety <= 0.0f)
                                        return subQueryScore;
                                    return subQueryScore * wordScore.Quotiety;
                                }
                            }
                        }
                    }
                    else
                    {
                        string fieldValue = fieldDoc.ToString();
                        foreach (WordScore wordScore in wordScoreList)
                        {
                            if (!string.IsNullOrWhiteSpace(wordScore.Word))
                            {
                                if (fieldValue.IndexOf(wordScore.Word, StringComparison.CurrentCultureIgnoreCase) > -1)
                                {
                                    if (wordScore.Quotiety <= 0.0f)
                                        return subQueryScore;
                                    return subQueryScore * wordScore.Quotiety;
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region 第二种评分方式：原本的评分方式（默认的方式）
            if (valSrcScore == 0.0f)
            {//上面的代码中没有找到需要评分操作的域的内容，并且文档也没有单独定义一个自定义评分域，需要返回文档原来默认的评分方式
                return subQueryScore;
            }
            #endregion

            #region 第三种评分方式：使用文档单独定义的自定义评分列后进行自定义评分，
            return base.CustomScore(doc, subQueryScore, valSrcScore);//默认是subQueryScore * valSrcScore //升序
            //return subQueryScore / valSrcScore;//倒序排
            #endregion
        }

        /// <summary>
        /// 将字符串转换成T数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        private static T StrToT<T>(string data)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(data, typeof(T));
            }
            object[] parameters = new object[] { data, default(T) };
            Type type = typeof(T);
            MethodInfo methodInfo = type.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string), type.MakeByRefType() }, null);
            object result = methodInfo.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, parameters, null);
            if ((bool)result)
            {
                return (T)parameters[1];
            }
            return default(T);
        }
    }
}
