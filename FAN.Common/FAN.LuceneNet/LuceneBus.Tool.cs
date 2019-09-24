#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Tool 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 将字符串转换成Int32数据类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static int StrToInt32(string str)
        {
            int result = 0;
            Int32.TryParse(str, out result);
            return result;
        }

        #region 把标准时间转换为UNIX时间戳
        /// <summary>
        /// 把标准时间转换为UNIX时间戳
        /// </summary>
        /// <param name="winTime">要转换的Windows时间</param>
        /// <returns>UNIX时间戳</returns>
        private static int ToUnixTime(DateTime winTime)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(winTime.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return System.Convert.ToInt32(timeStamp);
        }
        #endregion

        #region 把UNIX时间戳转换为标准时间
        /// <summary>
        /// 把UNIX时间戳转换为标准时间
        /// </summary>
        /// <param name="unixTime">UNIX时间戳</param>
        /// <returns>Windows时间</returns>
        private static DateTime FromUnixTime(int unixTime)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(unixTime.ToString() + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion

        #region 过滤html,js,css代码
        // <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content">含有html标签的网页内容</param>
        /// <returns>没有html标签的网页内容</returns>
        public static string RemoveHtml(string html)
        {
            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", "").Replace("&nbsp;", " ");
        }

        /// <summary>
        /// 过滤html,js,css代码
        /// </summary>
        /// <param name="html">含有html标签的网页内容，包括js，css都过滤掉</param>
        /// <returns>没有html，js，css标签的网页内容</returns>
        public static string RemoveHtmlAll(string html)
        {
            System.Text.RegularExpressions.Regex regScript = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regHref = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regOn = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regIframe = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regFrameset = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regImg = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regEndP = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regPreP = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regHtml = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regScript.Replace(html, "");                 //过滤<script></script>标记
            html = regHref.Replace(html, "");                   //过滤href=java script: (<A>) 属性
            html = regOn.Replace(html, " _disibledevent=");     //过滤其它控件的on...事件
            html = regIframe.Replace(html, "");                 //过滤iframe
            html = regFrameset.Replace(html, "");               //过滤frameset
            html = regImg.Replace(html, "");                    //过滤img
            html = regEndP.Replace(html, "");                   //过滤frameset
            html = regPreP.Replace(html, "");                   //过滤frameset
            html = regHtml.Replace(html, "");
            html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");

            html = html.Replace("&nbsp;", "");
            html = html.Replace(" ", "");
            html = html.Replace("　", "");
            return html;
        }
        #endregion

    }
}
