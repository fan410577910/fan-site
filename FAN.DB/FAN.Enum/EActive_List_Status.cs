#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：YANGHUANWEN 
     * 文件名：  EActive_List_Status 
     * 版本号：  V1.0.0.0 
     * 创建人：  杨焕文 
     * 创建时间：2014/11/30 16:29:45 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/11/30 16:29:45 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.ComponentModel;

namespace FAN.Enum
{
    public enum EActive_List_Status
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Description("未开始")]
        未开始 = 0,
        /// <summary>
        /// 暂停
        /// </summary>
        [Description("暂停中")]
        暂停中 = 1,
        /// <summary>
        /// 进行中(不存数据库，只是显示使用)
        /// </summary>
        [Description("进行中")]
        进行中 = 2,
        /// <summary>
        /// 结束(不存数据库，只是显示使用)
        /// </summary>
        [Description("已结束")]
        已结束 = 3,
        /// <summary>
        /// 结束(不存数据库，只是显示使用)
        /// </summary>
        [Description("已删除")]
        已删除 = 4

    }
}
