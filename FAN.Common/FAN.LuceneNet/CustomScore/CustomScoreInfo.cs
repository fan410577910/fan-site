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
     * 描述    : 根据某一个域的值进行自定义评分的设置信息（持久化使用）
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 根据某一个域的值进行自定义评分的设置信息（持久化使用）
    /// </summary>
    public class CustomScoreInfo
    {
        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段值数组
        /// </summary>
        [XmlElement("WordScores")]
        public List<WordScore> WordScoreList { get; set; }
        /// <summary>
        /// 从Lucene里面读取出来的值数组
        /// </summary>
        [XmlIgnore]
        public Array FieldValues { get; set; }
    }
    /// <summary>
    /// 每个词的值和系数
    /// </summary>
    public class WordScore
    {
        /// <summary>
        /// 词的内容
        /// </summary>
        public string Word { get; set; }
        private float _Quotiety = 1.0f;
        /// <summary>
        /// 排序系数
        /// </summary>
        public float Quotiety
        {
            get { return _Quotiety; }
            set { _Quotiety = value; }
        }
        
    }
}
