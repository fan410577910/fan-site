using FAN.Admin.Components;
using FAN.Entity;
using FAN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAN.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Welcome()
        {
            return View();
        }
        public JsonResult GetMenuData()
        {
            System.Collections.Generic.List<Menu_info> activeList = new List<Menu_info> {
                new Menu_info() { ID=1,Name="活动管理",ParentID=0,Description=""},
                new Menu_info() { ID=2,Name="产品管理",ParentID=0,Description=""},
                new Menu_info() { ID=3,Name="网站活动管理",ParentID=1,Description="/Active/Active/activeList"},
                new Menu_info() { ID=5,Name="temp",ParentID=2,Description=""},
                new Menu_info() { ID=6,Name="temp",ParentID=2,Description=""},
            };
            
            List<TreeData> tree = CommonTree.GetTreeData(activeList, "后台系统");
            return this.Json(tree);

        }
        
    }
}