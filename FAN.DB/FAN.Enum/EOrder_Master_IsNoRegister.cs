#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：YANGHUANWEN 
     * 文件名：  EOrder_Master_IsNoRegister 
     * 版本号：  V1.0.0.0 
     * 创建人：  杨焕文 
     * 创建时间：2015/10/29 11:55:42 
     * 描述    :
     * =====================================================================
     * 修改时间：2015/10/29 11:55:42 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.ComponentModel;

namespace FAN.Enum
{
    public enum EOrder_Master_IsNoRegister
    {
        [Description("正常订单")]
        正常订单 = 0,
        [Description("无注册订单")]
        无注册订单 = 1
    }
}
