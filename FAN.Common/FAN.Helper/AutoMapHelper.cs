using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Helper
{
    /// <summary>
    /// AutoMapper扩展帮助类
    /// </summary>
    public class AutoMapperHelper
    {
        /// <summary>
        /// 获取映射值
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination Map<TDestination>(object source) where TDestination : class
        {
            if (source == null)
            {
                return default(TDestination);
            }

            return Mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 获取集合映射值
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TDestination> Map<TDestination>(IEnumerable source) where TDestination : class, new()
        {
            if (source == null)
            {
                return default(IEnumerable<TDestination>);
            }

            return Mapper.Map<IEnumerable<TDestination>>(source);
        }

        /// <summary>
        /// 获取映射值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(TSource source) where TSource : class, new() where TDestination : class, new()
        {
            if (source == null)
            {
                return default(TDestination);
            }

            return Mapper.Map<TSource, TDestination>(source);
        }

        /// <summary>
        /// 获取集合映射值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source) where TSource : class, new() where TDestination : class, new()
        {
            if (source == null)
            {
                return default(IEnumerable<TDestination>);
            }
            return Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
        }

        /// <summary>
        /// 读取DataReader内容
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<TDestination> Map<TDestination>(IDataReader reader)
        {
            if (reader == null)
            {
                return new List<TDestination>();
            }

            var result = Mapper.Map<IEnumerable<TDestination>>(reader);

            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return result;
        }
    }
}
