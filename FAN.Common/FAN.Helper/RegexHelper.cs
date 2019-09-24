#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  RegexHelper 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyp 
     * 创建时间：2014/12/12 13:15:05 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/12/12 13:15:05 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace FAN.Helper
{
    /// <summary>
    /// 正则表达式辅助类
    /// </summary>
    public static class RegexHelper
    {
        private const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Compiled;

        public const string HTML_COLOR = @"^#?([a-f]|[A-F]|[0-9]){3}(([a-f]|[A-F]|[0-9]){3})?$";
        //private static readonly Regex _RegexHtmlColor = new Regex(HTML_COLOR, OPTIONS);//wangyunpeng

        public const string NUMBER = @"^[0-9]+$";
        //private static readonly Regex _RegexNumber = new Regex(NUMBER, OPTIONS);//wangyunpeng

        public const string ENGLISH = @"^[A-Za-z0-9]+$";
        //private static readonly Regex _RegexEnglish = new Regex(ENGLISH, OPTIONS);//wangyunpeng

        public const string EMAIL = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
        //private static readonly Regex _RegexEmail = new Regex(EMAIL, OPTIONS);//wangyunpeng

        public const string IP = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
        //private static readonly Regex _RegexIp = new Regex(IP, OPTIONS);//wangyunpeng

        public const string URL = @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$";
        //private static readonly Regex _RegexUrl = new Regex(URL, OPTIONS);//wangyunpeng

        public const string CHINESE = @"^[\u4e00-\u9fa5]{2,}$";
        //private static readonly Regex _RegexChinese = new Regex(CHINESE, OPTIONS);//wangyunpeng

        //private static readonly Regex _RegexMobile = new Regex("AppleWebKit", OPTIONS);//wangyunpeng

        public const string PARAMETER_KEY_VALUE = "([^=&]+)=([^&]*)";
        //private static readonly Regex _RegexParameterKeyValue = new Regex(PARAMETER_KEY_VALUE, OPTIONS);//wangyunpeng

        public const string IMG_SRC = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
        public const string SRC = @"src[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[\s\t\r\n]*[""']?";
        /// <summary>
        /// 判断是否网页颜色 如:#ffffff 不包含透明度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsHtmlColor(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            //return _RegexHtmlColor.IsMatch(str);//wangyunpeng
            return Regex.IsMatch(input, HTML_COLOR, OPTIONS);//wangyunpeng
        }
        /// <summary>
        /// 判断是否为数组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            //return _RegexNumber.IsMatch(input);//wangyunpeng
            return Regex.IsMatch(input, NUMBER, OPTIONS);//wangyunpeng
        }
        /// <summary>
        /// 判断是否为电子邮件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            //return _RegexEmail.IsMatch(input);//wangyunpeng
            return Regex.IsMatch(input, EMAIL, OPTIONS);//wangyunpeng
        }
        /// <summary>
        /// 判断是否为IP地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIP(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            //return _RegexIp.IsMatch(input);//wangyunpeng
            return Regex.IsMatch(input, IP, OPTIONS);//wangyunpeng
        }
        /// <summary>
        /// 判断是否为URL
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUrl(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            //return _RegexUrl.IsMatch(input);//wangyunpeng
            return Regex.IsMatch(input, URL, OPTIONS);//wangyunpeng
        }
        /// <summary>
        /// 判断是否为中文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChinese(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            //return _RegexChinese.IsMatch(input);//wangyunpeng
            return Regex.IsMatch(input, CHINESE, OPTIONS);//wangyunpeng
        }
        /// <summary>
        /// 判断是否为英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEnglish(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            //return _RegexEnglish.IsMatch(input);//wangyunpeng
            return Regex.IsMatch(input, ENGLISH, OPTIONS);//wangyunpeng
        }
        /// <summary>
        /// 判断是否为移动端上网的
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns>True表示移动端，False表示PC</returns>
        public static bool IsMobile(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return false;
            //return _RegexMobile.IsMatch(userAgent);//wangyunpeng
            return Regex.IsMatch(userAgent, "AppleWebKit", OPTIONS);//wangyunpeng
        }
        /// <summary>
        /// 获取23=716&amp;24=750里面的所有键和值的集合
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static NameValueCollection GetParameterKeyValueCollection(string parameter)
        {
            NameValueCollection nameValueCollection = null;
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return nameValueCollection;
            }
            //MatchCollection mc = _RegexParameterKeyValue.Matches(parameter);//wangyunpeng
            MatchCollection mc = Regex.Matches(parameter, PARAMETER_KEY_VALUE, OPTIONS);//wangyunpeng
            if (mc.Count > 0)
            {
                nameValueCollection = new NameValueCollection(mc.Count);
                foreach (Match m in mc)
                {
                    for (int i = 0; i < m.Groups.Count;)
                    {
                        if (i % 3 == 0)
                        {
                            i++;
                            continue;
                        }
                        string key = m.Groups[i++].Value;
                        string value = m.Groups[i++].Value;
                        nameValueCollection.Add(key, value);
                    }
                }
            }
            return nameValueCollection;
        }

        // <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content">含有html标签的网页内容</param>
        /// <returns>没有html标签的网页内容</returns>
        public static string RemoveHtml(string html)
        {
            return Regex.Replace(html, "<[^>]*>", "").Replace("&nbsp;", " ");
        }

        /// <summary>
        /// 取得HTML中所有图片的 URL。 
        /// </summary>
        /// <param name="html">HTML代码</param>
        /// <returns>图片的URL列表</returns>
        public static List<string> GetHtmlImageUrlList(string html)
        {
            // 搜索匹配的字符串 
            Regex regImg = new Regex(IMG_SRC, OPTIONS);
            MatchCollection matches = regImg.Matches(html);
            List<string> imageUrlList = new List<string>(matches.Count);
            // 取得匹配项列表 
            foreach (Match match in matches)
            {
                imageUrlList.Add(match.Groups["imgUrl"].Value);
            }
            matches = null;
            return imageUrlList;
        }
        /// <summary>
        /// 将html里面图片src属性增加域名开头
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ReplaceHtmlImageUrl(string imageUrl, string html)
        {
            Regex regImg = new Regex(IMG_SRC, OPTIONS);
            Regex regImgSrc = new Regex(SRC, OPTIONS);
            return regImg.Replace(html, match =>
            {
                string src = match.Groups["imgUrl"].Value;
                src = string.Concat(imageUrl, src.Replace(imageUrl, string.Empty));
                src = ImageHelper.GetImageUrl(src, "watermark", "0", "0");
                src = string.Format("src=\"{0}\"", src);
                return regImgSrc.Replace(match.Value, src);
            });
        }
        /// <summary>
        /// 将html里面图片src属性替换掉域名开头
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ReplaceHtmlImageNonUrl(string imageUrl, string html)
        {
            Regex regImg = new Regex(IMG_SRC, OPTIONS);
            Regex regImgSrc = new Regex(SRC, OPTIONS);
            return regImg.Replace(html, match =>
            {
                string src = match.Groups["imgUrl"].Value;
                src = string.Concat("src=\"", src.Replace(imageUrl, string.Empty).Replace("/watermark/", "/"), "\"");
                return regImgSrc.Replace(match.Value, src);
            });
        }

    }
}
