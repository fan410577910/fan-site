using FAN.WebSite.Code;
using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FAN.WebSite
{
    public partial class index : BasePage
    {
        protected string UserEmail { get; set; }
        protected override string TemplateName
        {
            get
            {
                return "index.vm";
            }
        }
        public index() : base(false, false)
        {
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void InitDict()
        {
            base.Dict.Add("USER_NAME", UserLogin.GetUserEmail(base.Context));
            base.Dict.Add("CONTENT", Encoder.JavaScriptEncode("<script>alert(1)</script>",false));
            
            base.InitDict();
        }
    }
}