using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FAN.Helper;
using Newtonsoft.Json;

namespace FAN.WebSite.ajax
{
    /// <summary>
    /// ShopCartHandler 的摘要说明
    /// </summary>
    [UploadCheckAttribute]
    public class ShopCartHandler : BaseHandler
    {
        public ShopCartHandler()
        {
            base.DictAction.Add("GetShopCartList",this.GetShopCartList);
        }
        private void GetShopCartList()
        {
            //Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //base.Response.WriteJSON(new { ShopCartList = new string[] { "p1","p2","p3" } });
            //throw new Exception("出错了");
            //Response.Headers.Add("Content-Type", "application/javascript");
            WriteJson(new { a=1});
            //Response.Write("fn({a:'hahaha'})");
        }
        protected void WriteJson(object jsonObj)
        {
            string jsonpCallback = Request["callback"],
                json = JsonConvert.SerializeObject(jsonObj);
            if (String.IsNullOrWhiteSpace(jsonpCallback))
            {
                Response.AddHeader("Content-Type", "text/plain");
                Response.Write(json);
            }
            else
            {
                Response.AddHeader("Content-Type", "application/javascript");
                Response.Write(String.Format("{0}({1});", jsonpCallback, json));
            }
        }
    }
}