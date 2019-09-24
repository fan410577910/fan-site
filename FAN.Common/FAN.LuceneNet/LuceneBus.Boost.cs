#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Boost 
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
using Lucene.Net.Documents;
using Lucene.Net.Search;
using System;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 设置文档权重
        /// http://www.cnblogs.com/o-andy-o/p/3877562.html
        /// </summary>
        /// <param name="document">文档</param>
        /// <param name="boost">权重</param>
        public static void SetBoost(Document document,float boost)
        {
            //读取文档时Boost的值总是1.
            //http://www.cnblogs.com/jinzhao/archive/2012/05/15/2501991.html
            document.Boost = boost;
        }
        /// <summary>
        /// 设置字段权重
        /// </summary>
        /// <param name="field">文档</param>
        /// <param name="boost">权重</param>
        public static void SetBoost(Field field, float boost)
        {
            field.Boost = boost;
        }
        /// <summary>
        /// 设置Query权重
        /// http://blog.csdn.net/shirdrn/article/details/6785385
        /// </summary>
        /// <param name="query">Query</param>
        /// <param name="boost">权重</param>
        public static void SetBoost(Query query ,float boost)
        {
            query.Boost = boost;
        }
        /// <summary>
        /// http://www.cnblogs.com/jinzhao/archive/2012/05/22/2513398.html
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static sbyte FloatToByte315(float f)
        {
            int num = BitConverter.ToInt32(BitConverter.GetBytes(f), 0);
            int num2 = num >> 0x15;
            if (num2 < 0x180)
            {
                if (num > 0) return 1;
                return 0;
            }
            if (num2 >= 640) return -1;
            return (sbyte)(num2 - 0x180);
        }


        public static float Byte315ToFloat(byte b)
        {
            if (b == 0) return 0f;
            int num = (b & 0xff) << 0x15;
            num += 0x30000000;
            return BitConverter.ToSingle(BitConverter.GetBytes(num), 0);
        }
    }
}
