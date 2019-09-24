using FAN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Entity
{
    public partial class Active_info : IEntity, ITreeData
    {
        public int ParentID { get; set; }
        public string Description { get; set; }
    }
    public partial class Menu_info : ITreeData
    {
    }
    public partial class ActiveMenu_info:ITreeData
    {
    }
}
