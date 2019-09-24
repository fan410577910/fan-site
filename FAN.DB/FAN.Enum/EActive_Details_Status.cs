#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：YANGHUANWEN 
     * 文件名：  EActive_Details_Status 
     * 版本号：  V1.0.0.0 
     * 创建人：  杨焕文 
     * 创建时间：2014/11/29 17:05:17 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/11/29 17:05:17 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.ComponentModel;

namespace FAN.Enum
{
    /// <summary>
    /// 活动明细状态,数据库Active_Details表的Status列的值
    /// </summary>
    public enum EActive_Details_Status
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        正常 = 0,
        /// <summary>
        /// 暂停
        /// </summary>
        [Description("暂停")]
        暂停 = 1,
        /// <summary>
        /// 已删除(不存数据库，只是显示使用)
        /// </summary>
        [Description("已删除")]
        已删除 = 2
    }
}
