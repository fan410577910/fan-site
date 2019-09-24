using FAN.Admin.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.Text.RegularExpressions;
using System.Text;
using FAN.Helper;
using FAN.Admin.Components;
namespace FAN.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //全局ModelBinder
            //ModelBinders.Binders.Add(typeof(string),new StringModelBuilder());//处理字符串
        }
        /// <summary>
        /// 在 ASP.NET 响应请求时作为 HTTP 执行管道中的第一个事件发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #region 360
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            HttpRequest request = context.Request;
            string ip = IPHelper.GetUserIpAddress(request);//提交请求的IP地址
            if (request.Url.AbsolutePath.StartsWith("/admin/"))
            {
                return;
            }
            bool isSafe = true;
            StringBuilder sbr = new StringBuilder("<h1>请勿非法提交！系统已经对你进行记录:</h1><br/>");
            sbr.Append("<h2>操 作 I  P  ：" + ip + "<br/>");
            sbr.Append("操 作 时 间：" + DateTime.Now + "<br/>");
            sbr.Append("操 作 页 面：" + request.ServerVariables["URL"] + "<br/>");
            if (safe_360.CookieData(request))
            {
                isSafe = false;
                sbr.Append("提 交 方 式： Cookie<br/>");
                foreach (HttpCookie cookie in request.Cookies)
                {
                    sbr.Append("提 交 参 数：" + cookie.Name + "<br/>");
                    sbr.Append("提 交 数 据：" + cookie.Value + "<br/>");
                }
            }

            if (request.UrlReferrer != null && safe_360.Referer(request))
            {
                isSafe = false;
                sbr.Append("提 交 方 式：" + request.UrlReferrer.AbsoluteUri + "<br/>");
            }
            if (request.HttpMethod == "POST" && safe_360.PostData(request))
            {
                isSafe = false;
                sbr.Append("提 交 方 式： POST<br/>");
                foreach (string key in request.Form)
                {
                    sbr.Append("提 交 参 数：" + key + "<br/>");
                    sbr.Append("提 交 数 据：" + request.Form[key] + "<br/>");
                }
            }
            if (request.HttpMethod == "GET" && safe_360.GetData(request))
            {
                isSafe = false;
                sbr.Append("提 交 方 式： GET<br/>");
                foreach (string key in request.QueryString)
                {
                    sbr.Append("提 交 参 数：" + key + "<br/>");
                    sbr.Append("提 交 数 据：" + request.QueryString[key] + "<br/>");
                }
            }
            if (!isSafe)
            {
                sbr.Append("</h2>");
                HttpResponse response = context.Response;
                response.ContentType = "text/html;charset=utf-8;";
                response.Write(sbr.ToString());
                response.End();
                return;
            }
            #endregion
        }
        /// <summary>
        /// 当引发未处理的异常时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception error = this.Server.GetLastError();
            Exception exception = error.InnerException ?? error;
            //TODO:记录日志
        }
    }

    /// <summary>
    /// 360安全
    /// </summary>
    public class safe_360
    {
        private const string StrRegex = @"\b(alert|confirm|prompt)\b|^\+/v(8|9)|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|<\s*script\b|\bEXEC\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";
        public static bool PostData(HttpRequest request)
        {
            bool result = false;
            int count = request.Form.Count;
            for (int i = 0; i < count; i++)
            {
                result = CheckData(request.Form[i]);
                if (result)
                {
                    break;
                }
            }
            return result;
        }

        public static bool GetData(HttpRequest request)
        {
            bool result = false;
            int count = request.QueryString.Count;
            for (int i = 0; i < count; i++)
            {
                result = CheckData(request.QueryString[i]);
                if (result)
                {
                    break;
                }
            }
            return result;
        }

        public static bool CookieData(HttpRequest request)
        {
            bool result = false;
            int count = request.Cookies.Count;
            for (int i = 0; i < count; i++)
            {
                string value = request.Cookies[i].Value;
                if (!string.IsNullOrEmpty(value))
                {
                    result = CheckData(value);
                    if (result)
                    {
                        break;
                    }
                }
            }
            return result;

        }

        public static bool Referer(HttpRequest request)
        {
            return request.UrlReferrer != null && CheckData(request.UrlReferrer.ToString());
        }

        public static bool CheckData(string inputData)
        {
            return Regex.IsMatch(inputData, StrRegex, RegexOptions.IgnoreCase);
        }
    }
}
