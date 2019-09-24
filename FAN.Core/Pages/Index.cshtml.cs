using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FAN.Core.Pages
{
    public class IndexModel : PageModel
    {
        public IConfiguration Configuration = null;
        public IndexModel(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public void OnGet()
        {
            //Uri uri = new Uri("https://www.taobao.com/1/2/3.html?q1=1&q2=2#head");
            //string[] segments = uri.Segments;



            string d = this.Configuration["Logging:LogLevel:Default"];
            base.ViewData["d"] = d;
        }
        
        
    }
}
