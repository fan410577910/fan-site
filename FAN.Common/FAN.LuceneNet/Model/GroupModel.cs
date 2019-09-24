#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2015 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  ValueDocCount 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2015/5/8 14:01:11 
     * 描述    : 分组之后的键、值、对应的文档数的集合类型
     * =====================================================================
     * 修改时间：2015/5/8 14:01:11 
     * 修改人  ：Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：分组之后的键、值、对应的文档数的集合类型
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 分组之后的键、值、对应的文档数的集合类型
    /// </summary>
    [Serializable]
    public class GroupKeyValueList : List<GroupKeyValue>
    {
        public GroupKeyValueList()
            : base()
        {

        }
        public GroupKeyValueList(int capacity)
            : base(capacity)
        {

        }
        new public void Add(GroupKeyValue groupKeyValue)
        {
            if (!base.Contains(groupKeyValue))
            {
                base.Add(groupKeyValue);
            }
        }
        public bool Contains(string key)
        {
            return this.Count(item => item.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)) > 0;
        }
        public GroupKeyValue this[string key]
        {
            get
            {
                GroupKeyValue groupKeyValue = null;
                if (!string.IsNullOrEmpty(key))
                {
                    groupKeyValue = base.Find(item => item.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
                }
                return groupKeyValue;
            }
        }
        /// <summary>
        /// 自定义排序
        /// </summary>
        new public void Sort()
        {
            foreach (GroupKeyValue groupKeyValue in this)
            {
                groupKeyValue.GroupValueDocCountList.Sort();
            }
            base.Sort(new Comparison<GroupKeyValue>((obj0, obj1) =>
            {
                return obj0.Key.CompareTo(obj1.Key);//升序
            }));
        }
    }
    /// <summary>
    /// 分组之后的键、值、对应的文档数类型
    /// </summary>
    [Serializable]
    public class GroupKeyValue
    {
        public GroupKeyValue(string key, GroupValueDocCountList groupValueDocCountList)
        {
            this.Key = key;
            this._GroupValueDocCountList = groupValueDocCountList;
        }
        /// <summary>
        /// 分组之后的键名称
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 分组之后的值以及它所对应的文档数集合类型
        /// </summary>
        private GroupValueDocCountList _GroupValueDocCountList = null;
        public GroupValueDocCountList GroupValueDocCountList
        {
            get { return this._GroupValueDocCountList ?? new GroupValueDocCountList(0); }
            set { this._GroupValueDocCountList = value; }
        }
        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is GroupKeyValue)
            {
                result = (obj as GroupKeyValue).Key.Equals(this.Key, StringComparison.CurrentCultureIgnoreCase);
            }
            return result;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    /// <summary>
    /// 分组之后某一个键它所有对应的值以及值所对应的文档数集合类型
    /// </summary>
    [Serializable]
    public class GroupValueDocCountList : List<GroupValueDocCount>
    {
        public GroupValueDocCountList()
            : base()
        {

        }
        public GroupValueDocCountList(int capacity)
            : base(capacity)
        {

        }
        new public void Add(GroupValueDocCount groupValueDocCount)
        {
            if (!base.Contains(groupValueDocCount))
            {
                base.Add(groupValueDocCount);
            }
        }
        public bool Contains(string value)
        {
            return this.Count(item => item.Value.Equals(value, StringComparison.CurrentCultureIgnoreCase)) > 0;
        }
        public GroupValueDocCount this[string key]
        {
            get
            {
                GroupValueDocCount groupValueDocCount = null;
                if (!string.IsNullOrEmpty(key))
                {
                    groupValueDocCount = base.Find(item => item.Value.Equals(key, StringComparison.CurrentCultureIgnoreCase));
                }
                return groupValueDocCount;
            }
        }

        /// <summary>
        /// 自定义排序
        /// </summary>
        new public void Sort()
        {
            base.Sort(new Comparison<GroupValueDocCount>((obj0, obj1) =>
                {
                    return obj1.DocCount.CompareTo(obj0.DocCount);//降序
                }));
        }
    }
    /// <summary>
    /// 分组之后的值以及它所对应的文档数类型
    /// </summary>
    [Serializable]
    public class GroupValueDocCount
    {
        public GroupValueDocCount(string value, int docCount)
        {
            this.Value = value;
            this.DocCount = docCount;
        }
        /// <summary>
        /// 分组之后的值名称
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 分组之后的值以及它所对应的文档数
        /// </summary>
        public int DocCount { get; set; }
        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is GroupValueDocCount)
            {
                result = (obj as GroupValueDocCount).Value.Equals(this.Value, StringComparison.CurrentCultureIgnoreCase);
            }
            return result;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
