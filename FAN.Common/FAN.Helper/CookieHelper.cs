using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Security.Application;

namespace FAN.Helper
{
    public class CookieHelper
    {
        public static void AddCookie(string cookieName, string value, HttpResponse response)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = Encoder.UrlEncode(value);
            response.Cookies.Add(cookie);
        }

        public static void AddCookie(string cookieName, string value, DateTime expireDays, HttpResponse response)
        {
            HttpCookie cookie = new HttpCookie(cookieName, Encoder.UrlEncode(value));
            cookie.Expires = expireDays;
            response.Cookies.Add(cookie);
        }
        public static void AddCookie(string cookieName, string value, DateTime expireDays, HttpResponse response, string path)
        {
            HttpCookie cookie = new HttpCookie(cookieName, Encoder.UrlEncode(value));
            cookie.Expires = expireDays;
            cookie.Path = path;
            response.Cookies.Add(cookie);
        }

        public static void AddCookie(string cookieName, string value, DateTime expireDays, HttpResponse response, string path, string domain)
        {
            HttpCookie cookie = new HttpCookie(cookieName, Encoder.UrlEncode(value));
            cookie.Expires = expireDays;
            cookie.Path = path;
            cookie.Domain = domain;
            response.Cookies.Add(cookie);
        }
        public static void AddCookie(string cookieName, string value, HttpResponse response, bool httpOnly, bool secure, string path, string domain)
        {
            HttpCookie cookie = new HttpCookie(cookieName, Encoder.UrlEncode(value));
            cookie.HttpOnly = httpOnly;
            cookie.Secure = secure;
            cookie.Path = path;
            cookie.Domain = domain;
            response.Cookies.Add(cookie);
        }
        public static void AddCookie(string cookieName, string value, DateTime expireDays, HttpResponse response, bool httpOnly, bool secure, string path, string domain)
        {
            HttpCookie cookie = new HttpCookie(cookieName, Encoder.UrlEncode(value));
            cookie.HttpOnly = httpOnly;
            cookie.Secure = secure;
            cookie.Path = path;
            cookie.Domain = domain;
            cookie.Expires = expireDays;
            response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookieValue(HttpRequest request, string cookieName)
        {
            string value = null;
            HttpCookie cookie = request.Cookies[cookieName];
            if (cookie != null)
            {
                value = cookie.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    value = System.Web.HttpUtility.UrlDecode(value);
                }
            }
            return value;
        }

        public static void DeleteCookie(string cookieName, HttpResponse response)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.UtcNow.AddYears(-1);
            response.Cookies.Add(cookie);
        }
        public static void DeleteCookie(string cookieName, string domainName, HttpResponse response)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.UtcNow.AddYears(-1);
            cookie.Domain = domainName;
            response.Cookies.Add(cookie);
        }
    }
}
