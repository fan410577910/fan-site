using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAN.Admin.Areas.Account.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Account
        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}