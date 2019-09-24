using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Model
{
    /// <summary>
    /// 实体类需要用JQuery EasyUI里面的Tree控件显示数据时需要继承此接口
    /// </summary>
    public interface ITreeData
    {
        int ParentID { get; set; }
        int ID { get; set; }
        string Name { get; set; }
        string Description { get; set; }        
    }
}
