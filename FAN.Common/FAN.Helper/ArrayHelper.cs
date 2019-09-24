using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Helper
{
    public class ArrayHelper
    {
        /// <summary>
        /// 获取两个数组或集合存在的交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ones"></param>
        /// <param name="twos"></param>
        /// <returns>返回交集部分</returns>
        public static List<T> HasSameItem<T>(IEnumerable<T> ones, IEnumerable<T> twos)
        {
            return ones.Intersect<T>(twos).ToList<T>();
        }

        /// <summary>
        /// 获取两个数组或集合存在的差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ones"></param>
        /// <param name="twos"></param>
        /// <returns>返回交集部分</returns>
        public static List<T> HasNoSameItem<T>(IEnumerable<T> ones, IEnumerable<T> twos)
        {
            return ones.Except<T>(twos).ToList<T>();
        }

        /// <summary>
        /// 获取两个数组或集合存在的并集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ones"></param>
        /// <param name="twos"></param>
        /// <returns>返回并集部分</returns>
        public static List<T> HasUnionItem<T>(IEnumerable<T> ones, IEnumerable<T> twos)
        {
            return ones.Union<T>(twos).ToList<T>();
        }
    }
}
