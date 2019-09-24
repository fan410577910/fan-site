using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FAN.Enum
{
    public enum EOrder_Master_IsDangerOrder
    {
        [Description("非风控")]
        非风控 = 0,
        [Description("风控")]
        风控 = 1
    }
}
