#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2017 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-JLFFS1KMVG 
     * 文件名：  XXTEACryptoHelper 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间： 2017/3/1 10:20:19 
     * 描述    : TEA（Tiny Encryption Algorithm）是一種小型的對稱加密解密算法，支持128位密碼，與BlowFish一样TEA每次只能加密/解密8字節數據。TEA特點是速度快、效率高，實現也非常簡單。由於針對TEA的攻擊不斷出現，所以TEA也發展出幾個版本，分別是XTEA、Block TEA和XXTEA。 
     * =====================================================================
     * 修改时间：2017/3/1 10:20:19 
     * 修改人  ： wangyunpeng
     * 版本号  ： V1.0.0.0 
     * 描述    ： http://fanli7.net/a/JAVAbiancheng/JAVAzonghe/20130802/405910.html
*/
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace FAN.Helper
{
    /// <summary>
    /// TEA（Tiny Encryption Algorithm）是一種小型的對稱加密解密算法，支持128位密碼，與BlowFish一样TEA每次只能加密/解密8字節數據。TEA特點是速度快、效率高，實現也非常簡單。由於針對TEA的攻擊不斷出現，所以TEA也發展出幾個版本，分別是XTEA、Block TEA和XXTEA。 
    /// http://fanli7.net/a/JAVAbiancheng/JAVAzonghe/20130802/405910.html
    /// </summary>
    public static class XXTEA1CryptoHelper
    {
        public static string Encrypt(string plainText, string key)
        {
            byte[] plainTexts = Encoding.UTF8.GetBytes(plainText.PadRight(MIN_LENGTH, SPECIAL_CHAR));
            byte[] keys = Encoding.UTF8.GetBytes(key.PadRight(MIN_LENGTH, SPECIAL_CHAR));
            return TEAEncrypt(plainTexts.ToLongArray(), keys.ToLongArray()).ToHexString();
        }
        public static string Decrypt(string cryptoText, string key)
        {
            if (string.IsNullOrWhiteSpace(cryptoText))
            {
                return cryptoText;
            }
            byte[] keys = Encoding.UTF8.GetBytes(key.PadRight(MIN_LENGTH, SPECIAL_CHAR));
            byte[] code = TEADecrypt(cryptoText.ToHexString(), keys.ToLongArray()).ToByteArray();
            return Encoding.UTF8.GetString(code, 0, code.Length);
        }

        private static long[] TEAEncrypt(long[] datas, long[] key)
        {
            int n = datas.Length;
            if (n < 1) { return datas; }
            long z = datas[datas.Length - 1], y = datas[0], sum = 0, e, p, q;
            q = 6 + 52 / n;
            while (q-- > 0)
            {
                sum += DELTA;
                e = (sum >> 2) & 3;
                for (p = 0; p < n - 1; p++)
                {
                    y = datas[p + 1];
                    z = datas[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (key[p & 3 ^ e] ^ z);
                }
                y = datas[0];
                z = datas[n - 1] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (key[p & 3 ^ e] ^ z);
            }
            return datas;
        }

        private static long[] TEADecrypt(long[] datas, long[] key)
        {
            int n = datas.Length;
            if (n < 1) { return datas; }
            long z = datas[datas.Length - 1], y = datas[0], sum = 0, e, p, q;
            q = 6 + 52 / n;
            sum = q * DELTA;
            while (sum != 0)
            {
                e = (sum >> 2) & 3;
                for (p = n - 1; p > 0; p--)
                {
                    z = datas[p - 1];
                    y = datas[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (key[p & 3 ^ e] ^ z);
                }
                z = datas[n - 1];
                y = datas[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (key[p & 3 ^ e] ^ z);
                sum -= DELTA;
            }
            return datas;
        }

        private static long[] ToLongArray(this byte[] datas)
        {
            int n = (datas.Length % 8 == 0 ? 0 : 1) + datas.Length / 8;
            long[] results = new long[n];
            for (int i = 0; i < n - 1; i++)
            {
                results[i] = BitConverter.ToInt64(datas, i * 8);
            }
            byte[] buffers = new byte[8];
            Array.Copy(datas, (n - 1) * 8, buffers, 0, datas.Length - (n - 1) * 8);
            results[n - 1] = BitConverter.ToInt64(buffers, 0);
            return results;
        }

        private static byte[] ToByteArray(this long[] datas)
        {
            List<byte> resultList = new List<byte>(datas.Length * 8);
            for (int i = 0; i < datas.Length; i++)
            {
                resultList.AddRange(BitConverter.GetBytes(datas[i]));
            }
            while (resultList[resultList.Count - 1] == SPECIAL_CHAR)
            {
                resultList.RemoveAt(resultList.Count - 1);
            }
            return resultList.ToArray();
        }

        private static string ToHexString(this long[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2").PadLeft(16, '0'));
            }
            return sb.ToString();
        }

        private static long[] ToHexString(this string data)
        {
            int len = data.Length / 16;
            long[] result = new long[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = Convert.ToInt64(data.Substring(i * 16, 16), 16);
            }
            return result;
        }

        private const long DELTA = 0x9E3779B9;
        private const int MIN_LENGTH = 32;
        private const char SPECIAL_CHAR = '\0';
    }
}
