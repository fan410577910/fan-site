#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  SymbolAnalyzer 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 符号分析器
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Analysis;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 符号分析器
    /// http://blog.hamny.com/articles/2013/01/07/1357545955452.html
    /// </summary>
    class SymbolAnalyzer : Analyzer
    {
        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            return new SymbolTokenzier(reader);
        }
    }
}
