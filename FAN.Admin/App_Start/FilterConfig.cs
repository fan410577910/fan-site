using FAN.Admin.Components;
using FAN.Helper;
using Microsoft.AspNet.Identity;
using System;
using System.IO.Compression;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FAN.Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());//异常处理
            //filters.Add(new PermissionFilterAttribute());//授权控制
            filters.Add(new CompressionFilter());//内容压缩
        }
    }
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            if (exception != null)
            {
                HttpRequestBase request = filterContext.HttpContext.Request;
                if (request.IsAjaxRequest())
                {//request.Headers["X-Requested-With"] == "XMLHttpRequest"
                    filterContext.Result = JsonManager.GetError((int)HttpStatusCode.InternalServerError, exception);
                    filterContext.HttpContext.Response.StatusCode = HttpStatusCode.OK.GetValue();
                    filterContext.ExceptionHandled = true;//不走global
                }
                else
                {
                    filterContext.ExceptionHandled = false;//走Global处理
                }
            }

            //TODO:记录日志

        }
    }
    /// <summary>
    /// Gzip 压缩
    /// </summary>
    public sealed class CompressionFilter : IResultFilter
    {

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string acceptEncoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (!String.IsNullOrEmpty(acceptEncoding))
            {
                acceptEncoding = acceptEncoding.ToUpperInvariant();
                HttpResponseBase response = filterContext.HttpContext.Response;
                if (acceptEncoding.Contains("GZIP"))
                {
                    response.AppendHeader("Content-encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                }
                else if (acceptEncoding.Contains("DEFLATE"))
                {
                    response.AppendHeader("Content-encoding", "deflate");
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                }
            }
        }
    }
    /// <summary>
    /// 具体判断用户访问权限的业务逻辑
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PermissionFilterAttribute :AuthorizeAttribute
    {
        #region Constructors
        //public PermissionFilterAttribute(params string[] permissions)
        //    : base(permissions)
        //{

        //}
        //public PermissionFilterAttribute(string permission)
        //    : base(permission)
        //{

        //}
        #endregion

        #region Methods
        /// <summary>
        /// 具体判断方法
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            //User user = httpContext.User.Identity as User;
            //if (user == null)
            //{
            //    return false;
            //}
            //string[] permissions = base.Permissions;
            //if (permissions.Length == 0)
            //{//没有指定访问权限地址的，读取请求URL作为权限地址。wangyunpeng。2017-4-21
            //    string path = httpContext.Request.Url.LocalPath;
            //    if (this.IsSkipValidate(path))
            //    {//首页，欢迎页排除
            //        permissions = new string[] { path };
            //    }
            //}
            //return AccessBiz.HasPermission(user.ID, permissions, GlobalValueProvider.Time);
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            ActionResult result = null;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                result = JsonManager.GetError(304, "没有获取数据的访问权限!");
            }
            else
            {
                if (!this.IsSkipValidate(filterContext.HttpContext.Request.Url.LocalPath))
                {//首页，欢迎页直接跳转登录页面
                    result = new HttpUnauthorizedResult();
                }
                else
                {
                    // 输出当前的结果
                    ContentResult contentresult = new ContentResult();
                    contentresult.Content = "没有页面访问权限!";
                    contentresult.ContentType = filterContext.HttpContext.Response.ContentType;
                    result = contentresult;
                }
            }
            filterContext.Result = result ?? new HttpUnauthorizedResult();
        }

        private bool IsSkipValidate(String path)
        {
            return "/" != path && !path.StartsWith("/Home/Welcome", StringComparison.CurrentCultureIgnoreCase) && !path.StartsWith("/Home/Index", StringComparison.CurrentCultureIgnoreCase);
        }
        #endregion
    }
    public sealed class UserFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.ViewBag.__USER != null
                || !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }
            if (filterContext.HttpContext.User.Identity is IUser)
            {//传统Form的验证信息。
                //filterContext.Controller.ViewBag.__USER = filterContext.HttpContext.User.Identity as User;
            }
            else if (filterContext.HttpContext.User.Identity is FormsIdentity)
            {//增加OAuth2.0的验证信息。wangyunpeng。2016-6-24
                //filterContext.Controller.ViewBag.__USER = new User((filterContext.HttpContext.User.Identity as FormsIdentity).Ticket);
            }
        }
    }
}
