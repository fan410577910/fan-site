using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    public enum EOrder_Master_IsDisputedOrder
    {
        [Description("非争议")]
        非争议 = 0,
        [Description("争议")]
        争议 = 1
    }
}
