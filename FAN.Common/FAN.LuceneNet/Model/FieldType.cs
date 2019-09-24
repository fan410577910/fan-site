#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  FieldType 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/15
     * 描述    : Lucene里面存储数据的类型
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
    /// Lucene里面存储数据的类型
    /// </summary>
    public class FieldType
    {
        public const string STRING = "String";
        public const string INT32 = "Int32";
        public const string INT64 = "Int64";
        public const string SINGLE = "Single";
        public const string DOUBLE = "Double";
        public const string DECIMAL = "Decimal";
        public const string DATETIME = "DateTime";
    }
}
