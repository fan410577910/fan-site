#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  Language 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2014/12/29 10:17:59 
     * 描述    : 多语言信息
     * =====================================================================
     * 修改时间：2014/12/29 10:17:59 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion

using System;
namespace TLZ.LuceneNet
{
    /// <summary>
    /// 多语言信息
    /// </summary>
    [Serializable]
    public class Language : IComparable<Language>
    {
        /// <summary>
        /// 多语言ID
        /// </summary>
        public int ID { get; set; }
        ///<summary>
        ///语种简称（简码），也是二级域名
        ///</summary>
        public string Code { get; set; }

        ///<summary>
        ///用中文的方式显示语言的名称
        ///</summary>
        public string CNText { get; set; }

        ///<summary>
        ///按语言本身的方式显示语言的名称
        ///</summary>
        public string Text { get; set; }

        public int CompareTo(Language other)
        {
            if (other.Code.Equals("EN", StringComparison.CurrentCultureIgnoreCase))
                return 1;
            return this.Code.CompareTo(other.Code);
        }
    }
}
