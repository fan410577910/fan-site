#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  SerializeHelper 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 序列化帮助类型
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 序列化帮助类型
    /// </summary>
    public class SerializeHelper
    {
        /// <summary>
        /// 序列化到对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        public static void XmlSerialize(string filePath, object obj)
        {
            XmlSerialize(filePath, obj, Encoding.UTF8);
        }
        /// <summary>
        /// 序列化到对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        public static void XmlSerialize(string filePath, object obj, Encoding encoding)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                string dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                using (StreamWriter writer = new StreamWriter(filePath, false, encoding))
                {
                    XmlSerializer xs = new XmlSerializer(obj.GetType());
                    xs.Serialize(writer, obj);
                    writer.Flush();
                    xs = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string filePath)
        {
            return XmlDeserialize<T>(filePath, Encoding.UTF8);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string filePath, Encoding encoding)
        {
            T t = default(T);
            if (!File.Exists(filePath))
            {
                return t;
            }
            try
            {
                using (StreamReader reader = new StreamReader(filePath, encoding, false))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    Object obj = xs.Deserialize(reader);
                    if (obj is T)
                    {
                        t = (T)obj;
                    }
                    xs = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }
    }
}
