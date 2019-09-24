using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    /// <summary>
    /// 是否支持指定的运输方式（根据国家）
    /// </summary>
    public enum EActive_ShipState
    {
        /// <summary>
        /// 不支持
        /// </summary>
        [Description("不支持")]
        不支持 = 0,

        /// <summary>
        /// 支持
        /// </summary>
        [Description("支持")]
        支持 = 1
    }
}
