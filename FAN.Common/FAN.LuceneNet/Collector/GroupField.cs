#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  GroupField 
     * 版本号：  V1.0.0.0 
     * 创建人：  wyp 
     * 创建时间：2014/9/13 15:16:49 
     * 描述    : 用于保存分组统计后每个字段的分组结果
     * =====================================================================
     * 修改时间：2014/9/13 15:16:49 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：用于保存分组统计后每个字段的分组结果
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 用于保存分组统计后每个字段的分组结果
    /// </summary>
    [Obsolete("wangyunpeng，测试专用")]
    public class GroupField
    {
        /// <summary>
        /// 是否先按空格分隔字段值，后统计
        /// </summary>
        public bool IsSplitSpace { get; private set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; private set; }
        /// <summary>
        /// 缓存字段值
        /// </summary>
        public string[] FieldValueCaches { get; private set; }

        public GroupField(string fieldName,bool isSplitSpace,string[] fieldValueCaches)
        {
            this.FieldName = fieldName;
            this.IsSplitSpace = isSplitSpace;
            this.FieldValueCaches = fieldValueCaches;
        }

        /// <summary>
        /// 所有可能的分组字段值，排序按每个字段值的文档个数大小排序
        /// </summary>
        private List<string> _FieldValueList = new List<string>();
        /// <summary>
        /// 保存字段值和文档个数的对应关系
        /// </summary>
        private Dictionary<string, int> _ValueCountDict = new Dictionary<string, int>();
        /// <summary>
        /// 所有可能的分组字段值，排序按每个字段值的文档个数大小排序
        /// </summary>
        public List<string> FieldValueList
        {
            get
            {
                this._FieldValueList.Sort(new Comparison<string>((key0, key1) =>
                {
                    int value0 = this._ValueCountDict[key0];
                    int value1 = this._ValueCountDict[key1];
                    return value1.CompareTo(value0);
                }));
                return this._FieldValueList;
            }
            set { this._FieldValueList = value; }
        }
        /// <summary>
        /// 保存字段值和文档个数的对应关系
        /// </summary>
        public Dictionary<string, int> ValueCountDict
        {
            get { return this._ValueCountDict; }
            set { this._ValueCountDict = value; }
        }

        public void AddValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            string[] values = null;
            if (this.IsSplitSpace)
            {
                values = value.Split(' ');
            }
            if (values == null)
            {
                values = new string[] { value };
            }
            foreach (string item in values)
            {
                if (this._ValueCountDict.ContainsKey(item))
                {
                    this._ValueCountDict[item] += 1;
                }
                else
                {
                    this._ValueCountDict[item] = 1;
                    this._FieldValueList.Add(item);
                }
            }
        }

    }
}
