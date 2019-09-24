using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Entity
{
    public interface IEntity
    {
        DateTime CreateTime { get; set; }
        string CreateUserName { get; set; }
        DateTime UpdateTime { get; set; }
        string UpdateUserName { get; set; }
    }
}
