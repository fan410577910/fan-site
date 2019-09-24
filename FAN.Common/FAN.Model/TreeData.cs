using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Model
{
    /// <summary>
    /// JQuery EasyUI里面的Tree控件数据类型
    /// </summary>
    public class TreeData
    {
        public object id { get; set; }
        public string text { get; set; }
        //状态：open 或者 closed
        public string state { get; set; }
        public bool @checked { get; set; }
        public string iconCls { get; set; }
        public object attributes { get; set; }
        public string description { get; set; }
        public List<TreeData> children { get; set; }
    }
}
