#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  EPlatform 
     * 版本号：  V1.0.0.0 
     * 创建人：  Administrator 
     * 创建时间：2014/12/5 12:26:28 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/12/5 12:26:28 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.ComponentModel;

namespace FAN.Enum
{
    /// <summary>
    /// 使用平台枚举，用到的地方太多了，所以不分表的方式建立了。
    /// </summary>
    public enum EPlatform
    {
        [Description("PC端")]
        PC端 = 1,
        [Description("M端")]
        M端 = 2,
        [Description("APP")]
        APP = 3
    }

}
