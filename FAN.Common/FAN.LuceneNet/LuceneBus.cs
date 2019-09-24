#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : Lucenen操作总线
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// Lucenen操作总线
    /// </summary>
    public partial class LuceneBus
    {
        static LuceneBus()
        {
            CACHE_TIME = StrToInt32(LuceneNetConfig.GetAppSettingValue("CACHE_TIME"));
            CACHE_TIME = Math.Max(CACHE_TIME, 20);
            LuceneNetConfig.ConfigChangedEvent += LuceneNetConfig_ConfigChangedEvent;
        }

        static void LuceneNetConfig_ConfigChangedEvent()
        {
            Close();
            ClearDirectory();
        }
    }
}
