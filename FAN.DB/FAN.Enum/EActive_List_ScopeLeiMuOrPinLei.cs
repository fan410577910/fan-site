#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  EActive_List_ScopeLeiMuOrPinLei 
     * 版本号：  V1.0.0.0 
     * 创建人：  Administrator 
     * 创建时间：2014/11/29 9:54:22 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/11/29 9:54:22 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.ComponentModel;

namespace FAN.Enum
{
    public enum EActive_List_ScopeLeiMuOrPinLei
    {
        /// <summary>
        /// 类目
        /// </summary>
        [Description("类目")]
        类目 = 0,
        /// <summary>
        /// 品类
        /// </summary>
        [Description("品类")]
        品类 = 1,
        /// <summary>
        /// 品牌
        /// </summary>
        [Description("品牌")]
        品牌 = 2,
        /// <summary>
        /// 风格
        /// </summary>
        [Description("风格")]
        风格 = 3

    }
}
