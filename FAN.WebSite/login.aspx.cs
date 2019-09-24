using FAN.WebSite.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FAN.WebSite
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserLogin.SetLoginCookie(1, "fanwenjian@tidebuy.net", 1, "lalala", base.Response);
            base.Response.Redirect("/index.html", true);
        }
    }
}