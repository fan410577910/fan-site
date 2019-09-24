using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Helper
{
    /// <summary> 
    /// 实现Base64加密解密 
    /// </summary>
    public static class Base64Helper
    {
        /// <summary> 
        /// Base64加密 
        /// </summary> 
        /// <param name="text">待加密的明文</param> 
        /// <returns></returns> 
        public static string Encode(string text, Encoding encode)
        {
            string result = null;
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bytes = encode.GetBytes(text);
                try
                {
                    result = Convert.ToBase64String(bytes);
                }
                catch
                {
                    result = null;
                }
                Array.Clear(bytes, 0, bytes.Length);
                bytes = null;
            }
            return result;
        }

        /// <summary> 
        /// Base64加密，采用utf8编码方式加密 
        /// </summary> 
        /// <param name="text">待加密的明文</param> 
        /// <returns>加密后的字符串</returns> 
        public static string Encode(string text)
        {
            return Encode(text, Encoding.UTF8);
        }

        /// <summary> 
        /// Base64解密 
        /// </summary> 
        /// <param name="result">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static string Decode(string text, Encoding encode)
        {
            string result = null;
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bytes = Convert.FromBase64String(text);
                try
                {
                    result = encode.GetString(bytes);
                }
                catch
                {
                    result = null;
                }
                Array.Clear(bytes, 0, bytes.Length);
                bytes = null;
            }
            return result;
        }

        /// <summary> 
        /// Base64解密，采用utf8编码方式解密 
        /// </summary> 
        /// <param name="text">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static string Decode(string text)
        {
            return Decode(text, Encoding.UTF8);
        }
    }
}
