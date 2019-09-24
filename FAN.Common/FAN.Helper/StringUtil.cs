#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2017 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-JLFFS1KMVG 
     * 文件名：  StringUtil 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间： 2017/10/27 15:21:44 
     * 描述    :
     * =====================================================================
     * 修改时间：2017/10/27 15:21:44 
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
    /// https://github.com/KevinWG/OSS.Common/blob/master/OSS.Common/ComUtils/StringUtil.cs
    /// 字符串通用功能
    /// </summary>
    public static class StringUtil
    {

        private static readonly Random _Random = new Random();
        private static readonly char[] _CharArray =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n', 'p', 'r', 'q', 's', 't', 'u',
            'v', 'w', 'z', 'y', 'x',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V',
            'U', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// 生成随机串
        /// </summary>
        /// <returns></returns>
        public static string RandomStr(int length = 8)
        {
            var num = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                num.Append(_CharArray[_Random.Next(0, 59)]);
            }
            return num.ToString();
        }

        /// <summary>
        /// 随机数字
        /// </summary>
        /// <returns></returns>
        public static string RandomNum(int length = 4)
        {
            var num = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                num.Append(_Random.Next(0, 9));
            }
            return num.ToString();
        }


        /// <summary>
        /// 排除【0，O】I 4 这类
        /// </summary>
        private const string _CodeStr = "12356789ABCDEFGHJKLMNPQRSTUVWXYZ";

        /// <summary>
        /// 数字转化为短码
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToCode(this long num)
        {
            const long codeTemp = 0x1F;
            var code = new StringBuilder(13);

            while (num > 0)
            {
                var index = num & codeTemp;
                code.Append(_CodeStr[(int)index]);

                num >>= 5;
            }
            return code.ToString();
        }

        /// <summary>
        /// 根据短码反推数字
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static long ToCodeNum(this string code)
        {
            if (string.IsNullOrEmpty(code))
                return 0;
            var count = code.Length;
            if (count > 13)
                throw new ArgumentOutOfRangeException("code", "the code is not from [ToCode] method !");

            long value = 0;
            for (var i = count - 1; i >= 0; i--)
            {
                var num = _CodeStr.IndexOf(code[i]);
                if (num < 0)
                    throw new ArgumentOutOfRangeException("code", "the code is not from [ToCode] method !");

                value <<= 5;
                if (count == 13 && i == 12)
                    value = value ^ (num & 0x03);     // 最高位只有两位
                else
                    value = value ^ num;

            }
            return value;

        }
    }
}
