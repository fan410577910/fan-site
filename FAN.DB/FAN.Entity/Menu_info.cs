using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Entity
{
    public partial class Menu_info
    {
        public string Description { get; set; }

        public int ID { get; set; }

        public string Name { get; set; }

        public int ParentID { get; set; }

        public string Href { get; set; }
    }
}
