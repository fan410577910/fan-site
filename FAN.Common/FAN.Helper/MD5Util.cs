#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：YANGHUANWEN 
     * 文件名：  MD5Util 
     * 版本号：  V1.0.0.0 
     * 创建人：  杨焕文 
     * 创建时间：2014/6/11 15:30:59 
     * 描述    : 本类从之前的项目中复制过来的。
     * =====================================================================
     * 修改时间：2014/6/11 15:30:59 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion

using System;
using System.Security.Cryptography;
using System.Text;

namespace FAN.Helper
{
    public class MD5Util
    {
        /// <summary>
        /// 返回以MD5方式加密的密文
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>返回以MD5方式加密的密文</returns>
        public static string Get(string plainText)
        {
            StringBuilder sb = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                byte[] plainTexts = Encoding.UTF8.GetBytes(plainText);
                if (plainTexts != null)
                {
                    int plainTextLength = plainTexts.Length;
                    if (plainTextLength > 0)
                    {
                        byte[] hashTexts = md5.ComputeHash(plainTexts);
                        if (hashTexts != null)
                        {
                            int hashTextLength = hashTexts.Length;
                            if (hashTextLength > 0)
                            {
                                for (int i = 0; i < hashTextLength; i++)
                                {
                                    sb.Append(hashTexts[i].ToString("X2"));
                                }
                                Array.Clear(hashTexts, 0, hashTextLength);
                            }
                            hashTexts = null;
                        }
                        Array.Clear(plainTexts, 0, plainTextLength);
                    }
                    plainTexts = null;
                }
            }
            string md5Text = sb.ToString();
            sb.Clear();
            sb = null;
            return md5Text;
        }


        /// <summary>
        /// Bi Dianqing 在做百度翻译的时候新加的MD5加密方法 上面那个跟百度翻译对接不上
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Md5(string src)
        {
            byte[] buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(src));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in buffer)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 调用CMS接口的MD5方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CMSMd5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
    }
}
