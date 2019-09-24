using FAN.Admin.Components;
using FAN.Entity;
using FAN.Helper;
using FAN.Helper.HttpRequests;
using FAN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using FAN.Admin.Components;

namespace FAN.Admin.Areas.Active.Controllers
{
    public class ActiveController : Controller
    {
        public ActionResult ActiveList()
        {
            System.Collections.Generic.List<Active_info> activeList = new List<Active_info> {
                new Active_info() { ID=1,Name="活动1"},
                new Active_info() { ID=2,Name="活动2"},
                new Active_info() { ID=3,Name="活动3"},
                new Active_info() { ID=4,Name="活动4"}
            };
            ViewBag.ActiveList = activeList;
            return View();
        }
        //JsonBinder案例
        //public ActionResult InsertActive([ModelBinder(typeof(JsonBinder<Active_info>))]Active_info activeInfo)//[ModelBinder(typeof(DMLModelBinder))]Active_info activeInfo
        //{
        //    //Active_list activeList = new Active_list();
        //    //base.UpdateModel<Active_list>(activeList);
        //    throw new Exception("err");
        //    return new ContentResult() { Content = "success" };
        //}
        //TryUpdateModel案例
        //public ActionResult InsertActive(int ID,FormCollection formValue)
        //{
        //    //FormCollection
        //    Active_info activeInfo = new Active_info();
        //    base.TryUpdateModel<Active_info>(activeInfo,"", formValue.AllKeys,new string[] { Active_info._ID_ });
        //    return JsonManager.GetSuccess();
        //    //new ContentResult() { Content = "success" };
        //}

        public ActionResult GetActiveMenuData()
        {
            System.Collections.Generic.List<ActiveMenu_info> activeList = new List<ActiveMenu_info> {
                new ActiveMenu_info() { ID=1,Name="日常活动",ParentID=0},
                new ActiveMenu_info() { ID=2,Name="长期活动",ParentID=0},
                new ActiveMenu_info() { ID=3,Name="黑五",ParentID=1},
            };
            List<TreeData> tree = CommonTree.GetTreeData(activeList, "全部活动");
            return this.Json(tree);
        }
        /// <summary>
        /// 新增活动菜单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ActiveMenuAdd()
        {
            //ActiveMenu_info menu = new ActiveMenu_info {
            //    Name="新活动",
            //    ParentID=1
            //};
            return this.View();
        }
        /// <summary>
        /// 活动菜单信息保存
        /// </summary>
        /// <param name="activeMenuInfo"></param>
        /// <returns></returns>
        public ActionResult ActiveMenuAddSave(ActiveMenu_info activeMenuInfo)
        {
            //TODO...
           return JsonManager.GetSuccess();
        }
        public ActionResult GetActiveListData(FormCollection formValue)
        {
            List<Active_info> activeList = new List<Active_info> {
                new Active_info { ID=1,Name="满减活动1",ActiveNameID=3},
                 new Active_info { ID=1,Name="满减活动2",ActiveNameID=3},
                  new Active_info { ID=1,Name="满减活动3",ActiveNameID=3},
                   new Active_info { ID=1,Name="满减活动4",ActiveNameID=3},
                    new Active_info { ID=1,Name="满减活动5",ActiveNameID=3}
            };
            return JsonManager.GetSuccess(new {total  = activeList.Count,rows=activeList });
        }



    }

         
}