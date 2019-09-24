#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2015 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  Ex 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2015/5/8 14:48:03 
     * 描述    :
     * =====================================================================
     * 修改时间：2015/5/8 14:48:03 
     * 修改人  ：Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 将一个实体对象的值复制到另一个对象
    /// </summary>
    public class ObjectExtensions
    {
        /// <summary>
        /// 对象克隆,wangyunpeng.2015-2-4.本方法是wangyunpeng写的。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(T obj) where T : class
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
}
