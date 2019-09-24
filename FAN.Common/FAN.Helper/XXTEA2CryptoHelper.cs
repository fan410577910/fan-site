#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2017 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-JLFFS1KMVG 
     * 文件名：  XXTEA2CryptoHelper 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间： 2017/3/1 13:27:35 
     * 描述    :
     * =====================================================================
     * 修改时间：2017/3/1 13:27:35 
     * 修改人  ： wangyunpeng
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Text;

namespace FAN.Helper
{
    /// <summary>
    /// http://www.cnblogs.com/luminji/p/3406407.html
    /// </summary>
    public class XXTEA2CryptoHelper
    {
        public static string Encrypt(string source, string key)
        {
            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            //UTF8==>BASE64==>XXTEA==>BASE64 
            byte[] datas = encoder.GetBytes(Base64Encode(source));
            byte[] keys = encoder.GetBytes(key);
            if (datas.Length == 0)
            {
                return "";
            }
            return System.Convert.ToBase64String(ToByteArray(Encrypt(ToUInt32Array(datas, true), ToUInt32Array(keys, false)), false));
        }
        public static string Decrypt(string source, string key)
        {
            if (source.Length == 0)
            {
                return "";
            }
            // reverse 
            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            byte[] datas = System.Convert.FromBase64String(source);
            byte[] keys = encoder.GetBytes(key);

            return Base64Decode(encoder.GetString(ToByteArray(Decrypt(ToUInt32Array(datas, false), ToUInt32Array(keys, false)), true)));
        }

        private static uint[] Encrypt(uint[] values, uint[] keys)
        {
            int n = values.Length - 1;
            if (n < 1)
            {
                return values;
            }
            if (keys.Length < 4)
            {
                uint[] keyTemp = new uint[4];
                keys.CopyTo(keyTemp, 0);
                keys = keyTemp;
            }
            uint z = values[n], y = values[0], delta = 0x9E3779B9, sum = 0, e;
            int p, q = 6 + 52 / (n + 1);
            while (q-- > 0)
            {
                sum = unchecked(sum + delta);
                e = sum >> 2 & 3;
                for (p = 0; p < n; p++)
                {
                    y = values[p + 1];
                    z = unchecked(values[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (keys[p & 3 ^ e] ^ z));
                }
                y = values[0];
                z = unchecked(values[n] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (keys[p & 3 ^ e] ^ z));
            }
            return values;
        }

        private static uint[] Decrypt(uint[] values, uint[] keys)
        {
            int n = values.Length - 1;
            if (n < 1)
            {
                return values;
            }
            if (keys.Length < 4)
            {
                uint[] Key = new uint[4];
                keys.CopyTo(Key, 0);
                keys = Key;
            }
            uint z = values[n], y = values[0], delta = 0x9E3779B9, sum, e;
            int p, q = 6 + 52 / (n + 1);
            sum = unchecked((uint)(q * delta));
            while (sum != 0)
            {
                e = sum >> 2 & 3;
                for (p = n; p > 0; p--)
                {
                    z = values[p - 1];
                    y = unchecked(values[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (keys[p & 3 ^ e] ^ z));
                }
                z = values[n];
                y = unchecked(values[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (keys[p & 3 ^ e] ^ z));
                sum = unchecked(sum - delta);
            }
            return values;
        }

        private static uint[] ToUInt32Array(byte[] datas, bool includeLength)
        {
            int n = (((datas.Length & 3) == 0) ? (datas.Length >> 2) : ((datas.Length >> 2) + 1));
            uint[] results;
            if (includeLength)
            {
                results = new uint[n + 1];
                results[n] = (uint)datas.Length;
            }
            else
            {
                results = new uint[n];
            }
            n = datas.Length;
            for (int i = 0; i < n; i++)
            {
                results[i >> 2] |= (uint)datas[i] << ((i & 3) << 3);
            }
            return results;
        }

        private static byte[] ToByteArray(uint[] datas, bool includeLength)
        {
            int n;
            if (includeLength)
            {
                n = (int)datas[datas.Length - 1];
            }
            else
            {
                n = datas.Length << 2;
            }
            byte[] results = new byte[n];
            for (int i = 0; i < n; i++)
            {
                results[i] = (byte)(datas[i >> 2] >> ((i & 3) << 3));
            }
            return results;
        }

        private static string Base64Decode(string data)
        {
            string result = null;
            try
            {
                byte[] datas = Convert.FromBase64String(data);
                result = Encoding.UTF8.GetString(datas);
                Array.Clear(datas, 0, datas.Length);
                datas = null;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
            return result;
        }

        private static string Base64Encode(string data)
        {
            string result = null;
            try
            {
                byte[] datas = Encoding.UTF8.GetBytes(data);
                result = Convert.ToBase64String(datas);
                Array.Clear(datas, 0, datas.Length);
                datas = null;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
            return result;
        }

    }
}
