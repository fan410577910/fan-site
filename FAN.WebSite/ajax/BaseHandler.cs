#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2018 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-TI50KE6KO4 
     * 文件名：  BaseHandler 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan
     * 创建时间： 2018/5/9 14:17:01 
     * 描述    :
     * =====================================================================
     * 修改时间：2018/5/9 14:17:01 
     * 修改人  ： Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FAN.Helper;
using System.Text;
using System.Reflection;

namespace FAN.WebSite.ajax
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UploadCheckAttribute : Attribute
    {
        private HttpContext _context = HttpContext.Current;
        private string[] _limitFileExtensions = null;
        public UploadCheckAttribute() { }
        public UploadCheckAttribute(params string[] limitFileExtensions)
        {
            _limitFileExtensions = limitFileExtensions;
        }
        /// <summary>
        /// 检查文件后缀
        /// </summary>
        /// <returns></returns>
        public bool CheckFileExtension()
        {
            if (this._limitFileExtensions == null || this._limitFileExtensions.Length == 0)
            {
                return false;
            }
            HttpFileCollection fileCollection = this._context.Request.Files;
            if (fileCollection.Count == 0)
            {
                return false;
            }
            string fileExtentsion = null;
            foreach (HttpPostedFile file in fileCollection)
            {
                fileExtentsion = System.IO.Path.GetExtension(file.FileName);
                if (!this._limitFileExtensions.Contains(fileExtentsion))
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// ShopCartHandler 的摘要说明
    /// </summary>
    public class BaseHandler : IHttpHandler
    {
        private HttpContext _Context = null;
        protected readonly Dictionary<string, Action> DictAction = new Dictionary<string, Action>(StringComparer.CurrentCultureIgnoreCase);
        public void ProcessRequest(HttpContext context)
        {
            this._Context = context;
            try {
                this.ActionName = this.Request.Params["action"];


                this.DoAction(this.ActionName);
            }
            catch (Exception ex) {
                //TODO:记录日志,向客户端返回提示
                string exceptionInfo = GetExceptionInfo(ex);
                this.Response.Clear();
                this.Response.WriteJSON(-500,ex.Message);
            }
            
        }
        protected void DoAction(string actionName)
        {
            Action action = null;
            if (!this.DictAction.TryGetValue(actionName, out action))
            {
                throw new Exception(string.Format("action is not found,actionName:{0}",actionName));
            }


            Type classType = action.Target.GetType();
            MethodInfo methodInfo = classType.GetMethod(action.Method.Name);

            //Attribute test = Attribute.GetCustomAttribute(classType, typeof(UploadCheckAttribute));
            UploadCheckAttribute uploadCheckAttribute = Attribute.GetCustomAttribute(classType, typeof(UploadCheckAttribute)) as UploadCheckAttribute;

            action();
        }
        protected HttpRequest Request
        {
            get {
                return this._Context.Request;
            }
        }
        protected HttpResponse Response
        {
            get {
                return this._Context.Response;
            }
        }
        protected HttpContext Context
        {
            get {
                return this._Context;
            }
        }
        /// <summary>
        /// 处理方法名
        /// </summary>
        protected string ActionName
        {
            get;set;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string GetExceptionInfo(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("exceptionName:{0}", e.GetType().Name);
            sb.Append(Environment.NewLine);
            sb.AppendFormat("Message:{0}", HttpUtility.HtmlEncode(e.Message));
            sb.Append(Environment.NewLine);
            sb.AppendFormat("StackTrace:{0}", HttpUtility.HtmlEncode(e.StackTrace));
            sb.Append(Environment.NewLine);
            sb.AppendFormat("Source:{0}", e.Source);
            sb.Append(Environment.NewLine);
            sb.AppendFormat("LogDateTime:{0}", DateTime.Now.ToString_yyyyMMddHHmmss());
            sb.Append(Environment.NewLine);
            sb.AppendFormat("rawUrl:{0}", this.Request.RawUrl == null
                           ? string.Empty : HttpUtility.HtmlEncode(this.Request.RawUrl.ToString()));
            sb.Append(Environment.NewLine);
            sb.AppendFormat("innerException:{0}", e.InnerException == null ? string.Empty : HttpUtility.HtmlEncode(e.InnerException.ToString()));
            sb.Append(Environment.NewLine);
            sb.AppendFormat("queryString:{0}", this.Request.QueryString == null ? string.Empty : HttpUtility.HtmlEncode(this.Request.QueryString));
            sb.Append(Environment.NewLine);
            sb.AppendFormat("formString:{0}", this.Request.Form == null ? string.Empty : this.Request.Form.ToString());
            sb.Append(Environment.NewLine);
            sb.AppendFormat("urlReferrer:{0}", this.Request.UrlReferrer == null
                           ? string.Empty : HttpUtility.HtmlEncode(this.Request.UrlReferrer.ToString()));
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}