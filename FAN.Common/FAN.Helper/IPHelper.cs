using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace FAN.Helper
{
    public class IPHelper
    {
        private static Regex _Regex = new Regex(@"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// 是否是IP地址
        /// </summary>
        /// <param name="str1"></param>
        /// <returns></returns>
        public static bool IsIpAddress(string str1)
        {
            if (string.IsNullOrEmpty(str1) || str1.Length < 7 || str1.Length > 15)
            {
                return false;
            }
            return _Regex.IsMatch(str1);
        }
        /// <summary>
        /// 用下面的方法获取IP（WebAPI）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetUserIpAddress(HttpRequestBase request)
        {
            string result = request.Headers["X-Forwarded-For"];//获取负载转发之后用户真实的IP地址.wangyp
                if (!string.IsNullOrWhiteSpace(result))
                {
                    int index = result.IndexOf(".", StringComparison.Ordinal);
                    //可能有代理   
                    if (index == -1)//没有"."肯定是非IPv4格式   
                    {
                        if (!IsIpAddress(result))//代理不是IP格式
                        {
                            result = null;
                        }
                    }
                    else
                    {
                        //有","，估计多个代理。取第一个不是内网的IP。   
                        result = result.Replace(" ", "").Replace("\"", "");
                        string[] tempIps = result.Split(",;".ToCharArray());
                        foreach (string temp in tempIps)
                        {
                            if (IsIpAddress(temp)
                                && temp.Substring(0, 3) != "10."
                                && temp.Substring(0, 7) != "192.168"
                                && temp.Substring(0, 7) != "172.16.")
                            {
                                return temp;//找到不是内网的地址   
                            }
                        }
                    }
                }
            if (string.IsNullOrWhiteSpace(result))
            {
                result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                result = request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                result = request.UserHostAddress;
            }
            if (result == "::1")
            {
                result = "127.0.0.1";
            }
            return result;
        }
        /// <summary>
        /// 用下面的方法获取IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetUserIpAddress(HttpRequest request)
        {
            string result = request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrWhiteSpace(result))
            {
                int index = result.IndexOf(".", StringComparison.Ordinal);
                //可能有代理   
                if (index == -1)//没有"."肯定是非IPv4格式   
                {
                    if (!IsIpAddress(result))//代理不是IP格式
                    {
                        result = null;
                    }
                }
                else
                {
                    //有","，估计多个代理。取第一个不是内网的IP。   
                    result = result.Replace(" ", "").Replace("\"", "");
                    string[] tempIps = result.Split(",;".ToCharArray());
                    foreach (string temp in tempIps)
                    {
                        if (IsIpAddress(temp)
                            && temp.Substring(0, 3) != "10."
                            && temp.Substring(0, 7) != "192.168"
                            && temp.Substring(0, 7) != "172.16.")
                        {
                            return temp;//找到不是内网的地址   
                        }
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                result = request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                result = request.UserHostAddress;
            }
            if (result == "::1")
            {
                result = "127.0.0.1";
            }
            return result;
        }
        /// <summary>
        /// 是否是公网IP
        /// </summary>
        public static bool IsPublicIp(string ipAddress)
        {
            bool isPublic = true;
            try
            {
                string[] ips = ipAddress.Split('.');
                if (ips.Length < 4)
                {
                    return false;
                }
                int w = 0;
                int x = 0;
                int y = 0;
                int z = 0;
                int.TryParse(ips[0], out w);
                int.TryParse(ips[1], out x);
                int.TryParse(ips[2], out y);
                int.TryParse(ips[3], out z);

                if (w == 127 && x == 0 && y == 0 && z == 1) // 127.0.0.1
                {
                    isPublic = false;
                }
                else if (w == 10) // 10.0.0.0 - 10.255.255.255
                {
                    isPublic = false;
                }
                else if (w == 172 && (x >= 16 || x <= 31)) // 172.16.0.0 - 172.31.255.255
                {
                    isPublic = false;
                }
                else if (w == 192 && x == 168) // 192.168.0.0 - 192.168.255.255
                {
                    isPublic = false;
                }
            }
            catch
            {
                isPublic = false;
            }
            return isPublic;
        }
        /// <summary>
        /// 将int型表示的IP还原成正常IPv4格式。
        /// </summary>
        /// <param name="intIpAddress">uint型表示的IP</param>
        /// <returns></returns>
        public static string NumberToIp(uint intIpAddress)
        {
            byte[] bytes = BitConverter.GetBytes(intIpAddress);
            return string.Format("{0}.{1}.{2}.{3}", bytes[3], bytes[2], bytes[1], bytes[0]);
        }
        /// <summary>
        /// 将正常IPv4格式还原成int型表示的IP
        /// </summary>
        /// <param name="strIpAddress"></param>
        /// <returns></returns>
        public static uint IpToNumber(string strIpAddress)
        {
            uint dwIp = 0;
            try
            {
                if (strIpAddress.Count(m => m == '.') != 3)
                {
                    return 0;
                }
                string[] strIpArray = strIpAddress.Split('.');
                uint[] iIp = new uint[strIpArray.Length];
                for (int i = 0; i < strIpArray.Length; ++i)
                {
                    iIp[i] = uint.Parse(strIpArray[i]);
                }
                dwIp = (iIp[0] << 24) | (iIp[1] << 16) | (iIp[2] << 8) | iIp[3];
            }
            catch
            {
            }
            return dwIp;
        }

    }
}
