#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  FilterInfo 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/15
     * 描述    : 过滤信息
     * =====================================================================
     * 修改时间：2014/7/15
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 过滤信息
    /// </summary>
    public class FilterInfo
    {
        /// <summary>
        /// 是否使用自定义过滤器
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// True表示放入，False表示清除
        /// </summary>
        public bool SetOrClear { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// 字段值数组
        /// </summary>
        public string[] Values { get; set; }
    }
}
