using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace FAN.Helper
{
    /// <summary>
    /// DateTime扩展
    /// </summary>
    /// <remarks>
    /// [2013-12-14] Develop by SunLiang.
    /// </remarks>
    public static class DateTimeExtensions
    {
        public static string ToString_yyyyMMddHHmmssffff(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
        }
        public static string ToString_yyyyMMddHHmmss(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ToString_yyyyMMddHHmm(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm");
        }
        public static string ToString_yyyyMMddHH(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH");
        }
        public static string ToString_yyyyMMdd(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
        public static string ToString_yyyyMM(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM");
        }
        public static string ToString_HHmmssffff(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss.ffff");
        }
        public static string ToString_HHmmss(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }
        public static string ToString_HHmm(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }
        /// <summary>
        /// 获取两个日期间隔的总月
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static int MonthDifference(this DateTime dateTime, DateTime other)
        {
            return Math.Abs((dateTime.Year - other.Year - 1) * 12 + (12 - other.Month) + dateTime.Month);
        }
    }
    public static class ObjectExtensions
    {
        /// <summary>
        /// 对象克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(this object obj) where T:class
        {
            T clone = default(T);
            using (MemoryStream ms = new MemoryStream(1024))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                // 反序列化至另一个对象(即创建了一个原对象的深表副本)
                clone = (T)binaryFormatter.Deserialize(ms);
            }
            return clone;
        }
    }
    /// <summary>
    /// String类扩展
    /// </summary>
    /// <remarks>
    /// [2013-12-14] Develop by SunLiang.
    /// </remarks>
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        public static string Formater(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
        public static string Formater(this string format, object arg0)
        {
            return string.Format(format, arg0);
        }
        public static string Formater(this string format, string arg0, object arg1)
        {
            return string.Format(format, arg0, arg1);
        }
        public static string Formater(this string format, object arg0, object arg1)
        {
            return string.Format(format, arg0, arg1);
        }
        public static string Formater(this string format, object arg0, object arg1, object arg2)
        {
            return string.Format(format, arg0, arg1, arg2);
        }
        public static string Formater(this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, format, args);
        }
        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null) return false;
            else return Regex.IsMatch(s, pattern);
        }
        public static bool IsMatch(this string s, string pattern, RegexOptions options)
        {
            if (s == null) return false;
            else return Regex.IsMatch(s, pattern, options);
        }
        public static string Match(this string s, string pattern)
        {
            if (s == null) return "";
            return Regex.Match(s, pattern).Value;
        }
        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }
        public static string ToCamel(this string s)
        {
            if (s.IsNullOrEmpty()) return s;
            return s[0].ToString().ToLower() + s.Substring(1);
        }
        public static string ToPascal(this string s)
        {
            if (s.IsNullOrEmpty()) return s;
            return s[0].ToString().ToUpper() + s.Substring(1);
        }
        /// <summary>
        /// 将字符串表示形式转换为它的等效 32 位有符号整数，如转换失败返回 0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int TryConvertInt32(this string s)
        {
            int result = 0;
            int.TryParse(s, out result);
            return result;
        }
        /// <summary>
        /// 将字符串表示形式转换为它的等效Decimal类型，如转换失败返回 0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal TryConvertDecimal(this string s)
        {
            decimal result = 0;
            decimal.TryParse(s, out result);
            return result;
        }
        /// <summary>
        /// 将字符串表示形式转换为它的等效Double类型，如转换失败返回 0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double TryConvertDouble(this string s)
        {
            double result = 0;
            double.TryParse(s, out result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static short TryConvertShort(this string s)
        {
            short result = 0;
            short.TryParse(s, out result);
            return result;
        }
        /// <summary>
        /// 将字符串按逗号分割成数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T[] SplitToArray<T>(this string s) where T : IConvertible
        {
            if (s.IsNullOrEmpty()) return new T[0];
            string[] array = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<T> ids = new List<T>();
            Type t = typeof(T);
            foreach (string item in array)
            {
                ids.Add((T)Convert.ChangeType(item, t));
            }
            return ids.ToArray();
        }
        /// <summary>
        /// 将字符串按<see cref="seperator"/>分割成数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="seperator">seperator</param>
        /// <returns></returns>
        public static T[] SplitToArray<T>(this string s, params char[] seperator) where T : IConvertible
        {
            if (s.IsNullOrEmpty()) return new T[0];
            string[] array = s.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
            Type t = typeof(T);
            return array.Select(item => (T)Convert.ChangeType(item, t)).ToArray();
        }
        /// <summary>
        /// 将字符串按<see cref="seperator"/>分割成数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="seperator">seperator</param>
        /// <returns></returns>
        public static T[] SplitToArray<T>(this string s, char seperator) where T : IConvertible
        {
            if (s.IsNullOrEmpty()) return new T[0];
            string[] array = s.Split(new[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
            List<T> ids = new List<T>();
            Type t = typeof(T);
            foreach (string item in array)
            {
                ids.Add((T)Convert.ChangeType(item, t));
            }
            return ids.ToArray();
        }
        /// <summary>
        /// 将字符串按逗号分割成数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<T> SplitToList<T>(this string s) where T : IConvertible
        {
            if (s.IsNullOrEmpty()) return new List<T>(0);
            string[] array = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<T> ids = new List<T>();
            Type t = typeof(T);
            foreach (string item in array)
            {
                ids.Add((T)Convert.ChangeType(item, t));
            }
            return ids;
        }
        /// <summary>
        /// 将字符串按<see cref="seperator"/>分割成数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="seperator">seperator</param>
        /// <returns></returns>
        public static List<T> SplitToList<T>(this string s, params char[] seperator) where T : IConvertible
        {
            if (s.IsNullOrEmpty()) return new List<T>(0);
            string[] array = s.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
            List<T> ids = new List<T>();
            Type t = typeof(T);
            foreach (string item in array)
            {
                ids.Add((T)Convert.ChangeType(item, t));
            }
            return ids;
        }
        /// <summary>
        /// 将字符串按<see cref="seperator"/>分割成数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="seperator">seperator</param>
        /// <returns></returns>
        public static List<T> SplitToList<T>(this string s, char seperator) where T : IConvertible
        {
            if (s.IsNullOrEmpty()) return new List<T>(0);
            string[] array = s.Split(new[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
            List<T> ids = new List<T>();
            Type t = typeof(T);
            foreach (string item in array)
            {
                ids.Add((T)Convert.ChangeType(item, t));
            }
            return ids;
        }
        /// <summary>
        /// 转半角(DBC case)
        /// </summary>
        /// <param name="target">任意字符串</param>
        /// <returns>半角字符串</returns>
        public static string ToDbc(this string target)
        {
            char[] c = target.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// http://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HashStringToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
    public static class ResponseExtensions
    {
        public static void WriteJSON(this HttpResponse httpResponse,  object data)
        {
            httpResponse.WriteJSON(0, data, string.Empty);
        }
        public static void WriteJSON(this HttpResponse httpResponse, int code, object data=null)
        {
            httpResponse.WriteJSON(code, data, string.Empty);
        }
        public static void WriteJSON(this HttpResponse httpResponse, int code, string message)
        {
            httpResponse.WriteJSON(code, null, message);
        }
        public static void WriteJSON(this HttpResponse httpResponse,int code,object data,string message) {
            var obj = new {
                Code = code,
                Data=data,
                Message=message
            };            
            httpResponse.Write(JsonHelper.ConvertJsonToStr(obj));
        }
    }
    public static class JObjectExtension
    {
        /// <summary>
        /// 获得指定Key的值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="jObject">JSON对象</param>
        /// <param name="key">键名称</param>
        /// <returns>JSON对象的值</returns>
        public static T Get<T>(this JObject jObject, string key)
        {
            T result = default(T);
            if (jObject == null)
                return result;
            try
            {
                if (jObject.ContainsKey(key))
                {
                    JToken jToken = jObject[key];
                    if (jToken != null)
                    {
                        result = jToken.Value<T>();
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }
            return result;
        }

        /// <summary>
        /// JSON对象是否存在指定键
        /// </summary>
        /// <param name="jObject">JSON对象</param>
        /// <param name="key">键名称</param>
        /// <returns>是否存在</returns>
        public static bool ContainsKey(this JObject jObject, string key)
        {
            bool result = false;
            if (jObject == null)
                return result;
            try
            {
                if (jObject[key] != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }
            return result;
        }
        /// <summary>
        /// 往JSON对象中添加或更新一个属性
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="jProperty"></param>
        public static void AddOrUpdate(this JObject jObject, JProperty jProperty)
        {
            if (jObject == null || jProperty == null)
                return;

            try
            {
                if (jObject[jProperty.Name] == null)
                {
                    jObject.Add(jProperty);
                }
                else
                {
                    jObject[jProperty.Name] = jProperty.Value;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }
        }
        private static IEnumerable<char> GetCharsInRange(string text, int min, int max)
        {
            return text.Where(e => e >= min && e <= max);
        }

        //var romaji = GetCharsInRange(searchKeyword, 0x0020, 0x007E);
        //var hiragana = GetCharsInRange(searchKeyword, 0x3040, 0x309F);
        //var katakana = GetCharsInRange(searchKeyword, 0x30A0, 0x30FF);
        //var kanji = GetCharsInRange(searchKeyword, 0x4E00, 0x9FBF);
    }
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举值的名称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }
        /// <summary>
        ///  获取该枚举的显示值（如果使用了DescriptionAttribute 标签则显示描述中的别名，否则使用 Enum 的名称。）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetText(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }
        /// <summary>
        ///  获取该枚举的  Description 标记值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return null;
        }

        /// <summary>
        /// 获取枚举代表的值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetValue(this Enum value)
        {
            return Convert.ToInt32(value);
        }
        public static T Parse<T>(this Enum enumThis, int value)
        {
            return (T)Enum.Parse(enumThis.GetType(), value.ToString());
        }
        public static T Parse<T>(this Enum enumThis, string value)
        {
            return (T)Enum.Parse(enumThis.GetType(), value);
        }
    }
}
