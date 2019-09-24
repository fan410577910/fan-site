#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2018 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-TI50KE6KO4 
     * 文件名：  serLogin 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan
     * 创建时间： 2018/5/12 11:22:02 
     * 描述    :
     * =====================================================================
     * 修改时间：2018/5/12 11:22:02 
     * 修改人  ： Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using FAN.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FAN.WebSite.Code
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class UserLogin
    {
        private const string _ID_ = "id";
        private const string _USER_TYPE_ = "usertype";
        private const string _EMAIL_ = "email";
        private const string _GUID_ = "guid";
        
        /// <summary> 
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 帐户类别
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// GUID
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 用户会员级别
        /// </summary>
        public int UserRanks { get; set; }

        /// <summary>
        /// 设置登陆的Cookie
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="email"></param>
        /// <param name="userType"></param>
        /// <param name="response"></param>
        public static void SetLoginCookie(int userID, string email, int userType, string guid, HttpResponse response)
        {
            NameValueCollection parameters = new NameValueCollection();
            if (userID > 0)
            {
                parameters.Add(UserLogin._ID_, userID.ToString());
            }
            if (userType > 0)
            {
                parameters.Add(UserLogin._USER_TYPE_, userType.ToString());
            }
            if (!string.IsNullOrEmpty(email))
            {
                parameters.Add(UserLogin._EMAIL_, email);
            }
            if (!string.IsNullOrEmpty(guid))
            {
                parameters.Add(UserLogin._GUID_, guid);
            }
            string value = UrlRoutingBus.BuildQueryString(parameters);
            DateTime expireDays = DateTime.Now.AddDays(30);
            UserLogin.SetAuthCookie(email, value, expireDays, response);
        }

        /// <summary>
        /// 设置一个加密Cookies
        /// </summary>
        /// <param name="value"></param>
        /// <param name="expireDays"></param>
        private static void SetAuthCookie(string name, string value, DateTime expireDays, HttpResponse response)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, name, DateTime.Now, expireDays, true, value, FormsAuthentication.FormsCookiePath);
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            CookieHelper.AddCookie(FormsAuthentication.FormsCookieName, encryptedTicket, ticket.Expiration, response, true, FormsAuthentication.RequireSSL, FormsAuthentication.FormsCookiePath, FormsAuthentication.CookieDomain);
        }

        /// <summary>
        /// 是否已经登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsLogin(HttpContext context)
        {
            return UserLogin.IsLogin(context.Request);
        }

        /// <summary>
        /// 是否已经登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsLogin(HttpRequest request)
        {
            return request.IsAuthenticated;
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 读取用户邮箱
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUserEmail(HttpContext context)
        {
            string email = string.Empty;
            if (UserLogin.IsLogin(context))
            {
                UserLogin userLogin = UserLogin.GetUserLogin(context.User.Identity as FormsIdentity, context.Items);
                if (userLogin != null)
                {
                    email = userLogin.Email;
                }
            }
            return email;
        }

        /// <summary>
        /// 获取用户登陆信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static UserLogin GetUserLogin(HttpContext context)
        {
            return UserLogin.GetUserLogin(context.User.Identity as FormsIdentity, context.Items);
        }

        /// <summary>
        /// 获得用户登陆信息
        /// </summary>
        /// <param name="formsIdentity"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private static UserLogin GetUserLogin(FormsIdentity formsIdentity, IDictionary items)
        {
            UserLogin userLogin = null;
            if (formsIdentity == null || !formsIdentity.IsAuthenticated)
            {
                return userLogin;
            }
            const string USER_LOGIN_COOKIE_ITEMS = "USER_LOGIN_COOKIE_ITEMS";
            userLogin = items[USER_LOGIN_COOKIE_ITEMS] as UserLogin;
            if (userLogin == null)
            {
                FormsAuthenticationTicket ticket = formsIdentity.Ticket;
                string userIdentityName = ticket.UserData;
                if (!String.IsNullOrEmpty(userIdentityName))
                {
                    userLogin = new UserLogin();
                    string[] strQuery = userIdentityName.Split('&');
                    foreach (string parameters in strQuery)
                    {
                        string[] parameter = parameters.Split('=');
                        if (parameter.Length == 2)
                        {
                            string attributeValue = parameter[1];
                            if (!string.IsNullOrEmpty(attributeValue))
                            {
                                attributeValue = HttpUtility.UrlDecode(attributeValue);
                                switch (parameter[0])
                                {
                                    case UserLogin._ID_://用户数字ID
                                        userLogin.UserID = TypeParseHelper.StrToInt32(attributeValue);
                                        break;
                                    case UserLogin._EMAIL_://用户Email
                                        userLogin.Email = attributeValue;
                                        break;
                                    case UserLogin._GUID_://用户Email
                                        userLogin.Guid = attributeValue;
                                        break;
                                    case UserLogin._USER_TYPE_://帐户类别
                                        userLogin.UserType = TypeParseHelper.StrToInt32(attributeValue);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            userLogin = null;
                            break;
                        }
                    }
                }
                items[USER_LOGIN_COOKIE_ITEMS] = userLogin;
            }
            return userLogin;
        }
    }
}