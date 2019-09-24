#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  SymbolTokenzier 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 符号分词器
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Analysis;
using System.IO;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 符号分词器
    /// </summary>
    class SymbolTokenzier : CharTokenizer
    {
        public SymbolTokenzier(TextReader reader)
            : base(reader)
        {
        }
        protected override bool IsTokenChar(char c)
        {
            return !(c == ' ' || c == '-' || c == '_' || c == ',' || c == '，' || c == '|' || c == '.' || c == '。' || c == '=' || c == '&' || c == '/' || c == '\\' || c == ';' || c == '；');
        }

    }
}
