#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2015 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  GroupCollectorField 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2015/5/7 15:52:45 
     * 描述    : 用于保存分组统计后每个字段的分组结果
     * =====================================================================
     * 修改时间：2015/5/7 15:52:45 
     * 修改人  ：Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 用于保存分组统计后每个字段的分组结果
    /// </summary>
    public class GroupCollectorField : IDisposable
    {
        #region 属性
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 键、值、文档数量集合
        /// </summary>
        private GroupKeyValueList _GroupKeyValueDocCountList = new GroupKeyValueList();
        /// <summary>
        /// 键、值、文档数量集合
        /// </summary>
        public GroupKeyValueList GroupKeyValueDocCountList
        {
            get { return this._GroupKeyValueDocCountList; }
            set { this._GroupKeyValueDocCountList = value; }
        }

        #endregion

        public GroupCollectorField(string fieldName)
        {
            this.FieldName = fieldName;
        }
        public void AddValue(string nameValuePair)
        {
            if (string.IsNullOrEmpty(nameValuePair))
            {
                return;
            }
            NameValueCollection keyValueCollection = NameValueCollectionTool.GetParameterKeyValueCollection(nameValuePair);
            if (keyValueCollection != null)
            {
                GroupValueDocCountList groupValueDocCountList = null;
                string[] values = null;
                foreach (string key in keyValueCollection)
                {
                    if (this._GroupKeyValueDocCountList.Contains(key))
                    {
                        groupValueDocCountList = this._GroupKeyValueDocCountList[key].GroupValueDocCountList;
                    }
                    else
                    {
                        groupValueDocCountList = new GroupValueDocCountList();
                        this._GroupKeyValueDocCountList.Add(new GroupKeyValue(key, groupValueDocCountList));
                    }
                    values = keyValueCollection[key].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string value in values)
                    {
                        if (groupValueDocCountList.Contains(value))
                        {
                            groupValueDocCountList[value].DocCount += 1;
                        }
                        else
                        {
                            groupValueDocCountList.Add(new GroupValueDocCount(value, 1));
                        }
                    }
                    Array.Clear(values, 0, values.Length);
                    values = null;
                }
                keyValueCollection.Clear();
                keyValueCollection = null;
            }
        }

        public void Dispose()
        {
            foreach (GroupKeyValue groupKeyValue in this._GroupKeyValueDocCountList)
            {
                groupKeyValue.GroupValueDocCountList.Clear();
            }
            this._GroupKeyValueDocCountList.Clear();
            this._GroupKeyValueDocCountList = null;
        }
        class NameValueCollectionTool
        {
            private const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Compiled;
            private const string PARAMETER_KEY_VALUE = "([^=&]+)=([^&]*)";
            private static readonly Regex _RegexParameterKeyValue = new Regex(PARAMETER_KEY_VALUE, OPTIONS);

            /// <summary>
            /// 通过SKU的SpecificationText获取必选区键、值文本对集合
            /// </summary>
            /// <param name="nameValuePair">SKU的SpecificationText</param>
            /// <returns>必选区键、值文本对集合</returns>
            internal static NameValueCollection GetParameterKeyValueCollection(string nameValuePair)
            {
                NameValueCollection keyValueCollection = null;
                if (string.IsNullOrWhiteSpace(nameValuePair))
                {
                    return keyValueCollection;
                }
                MatchCollection mc = _RegexParameterKeyValue.Matches(nameValuePair);
                if (mc.Count > 0)
                {
                    keyValueCollection = new NameValueCollection(mc.Count);
                    foreach (Match m in mc)
                    {
                        for (int i = 0; i < m.Groups.Count; )
                        {
                            if (i % 3 == 0)
                            {
                                i++;
                                continue;
                            }
                            string key = m.Groups[i++].Value;
                            string value = m.Groups[i++].Value;
                            keyValueCollection.Add(key, value);
                        }
                    }
                }
                return keyValueCollection;
            }
        }

    }
}
