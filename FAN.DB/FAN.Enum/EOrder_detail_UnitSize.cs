#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2015 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  EOrder_detail_UnitSize 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2015/5/20 10:19:15 
     * 描述    :
     * =====================================================================
     * 修改时间：2015/5/20 10:19:15 
     * 修改人  ：Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.ComponentModel;

namespace FAN.Enum
{
    /// <summary>
    /// 产品定制项单位枚举类型(对应在order_detail表中的UnitSize值)
    /// </summary>
    public enum EOrder_detail_UnitSize
    {
        /// <summary>
        /// 厘米
        /// </summary>
        [Description("CM")]
        CM = 0,
        /// <summary>
        /// 英寸
        /// </summary>
        [Description("IN")]
        IN = 1
    }

    
}
