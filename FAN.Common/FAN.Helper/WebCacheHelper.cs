using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace FAN.Helper
{
    public class WebCacheHelper
    {

    }
    /// <summary>
    /// 数据缓存总线(主要用在Web项目)
    /// </summary>
    public partial class DataCacheBus
    {
        /// <summary>
        /// GenerateFile 生成文件
        /// </summary>
        /// <param name="filePath"></param>
        private static void GenerateFile(string filePath)
        {
            if (!IOHelper.IsExistFilePath(filePath))
            {
                IOHelper.GenerateFile(filePath, "author:fan");
            }
        }
        /// <summary>
        /// 插入缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Insert(string key, object value)
        {
            Insert(key, value, (CacheDependency)null);
        }
        /// <summary>
        /// 插入缓存对象(缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencyFile">文件依赖</param>
        public static void Insert(string key, object value, string dependencyFile)
        {
            GenerateFile(dependencyFile);
            Insert(key, value, new CacheDependency(dependencyFile));
        }
        /// <summary>
        /// 插入缓存对象(缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies">缓存依赖</param>
        public static void Insert(string key, object value, CacheDependency dependencies)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies);
        }
        /// <summary>
        /// 插入缓存对象(缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencyFile">文件依赖</param>
        /// <param name="onRemoveCallBack">缓存消失之后的处理方法</param>
        public static void Insert(string key, object value, string dependencyFile, CacheItemRemovedCallback onRemoveCallBack)
        {
            GenerateFile(dependencyFile);
            HttpRuntime.Cache.Insert(key, value, new CacheDependency(dependencyFile), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, onRemoveCallBack);
        }
        /// <summary>
        /// 插入缓存对象(缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies">缓存依赖</param>
        /// <param name="onRemoveCallBack">缓存消失之后的处理方法</param>
        public static void Insert(string key, object value, CacheDependency dependencies, CacheItemRemovedCallback onRemoveCallBack)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, onRemoveCallBack);
        }
        /// <summary>
        /// 插入缓存对象(绝对过期时间)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        public static void Insert(string key, object value, DateTime absoluteExpiration)
        {
            Insert(key, value, absoluteExpiration, (CacheDependency)null);
        }
        /// <summary>
        /// 插入缓存对象(绝对过期时间)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="dependencyFile">文件依赖</param>
        public static void Insert(string key, object value, DateTime absoluteExpiration, string dependencyFile)
        {
            GenerateFile(dependencyFile);
            HttpRuntime.Cache.Insert(key, value, new CacheDependency(dependencyFile), absoluteExpiration.ToUniversalTime(), Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 插入缓存对象(绝对过期时间,缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="dependencies">缓存依赖</param>
        public static void Insert(string key, object value, DateTime absoluteExpiration, CacheDependency dependencies)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration.ToUniversalTime(), Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 插入缓存对象(绝对过期时间,缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="dependencyFile">文件依赖</param>
        /// <param name="onRemoveCallBack">缓存消失之后的处理方法</param>
        public static void Insert(string key, object value, DateTime absoluteExpiration, string dependencyFile, CacheItemRemovedCallback onRemoveCallBack)
        {
            GenerateFile(dependencyFile);
            HttpRuntime.Cache.Insert(key, value, new CacheDependency(dependencyFile), absoluteExpiration.ToUniversalTime(), Cache.NoSlidingExpiration, CacheItemPriority.High, onRemoveCallBack);
        }
        /// <summary>
        /// 插入缓存对象(绝对过期时间,缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="dependencies">缓存依赖</param>
        /// <param name="onRemoveCallBack">缓存消失之后的处理方法</param>
        public static void Insert(string key, object value, DateTime absoluteExpiration, CacheDependency dependencies, CacheItemRemovedCallback onRemoveCallBack)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration.ToUniversalTime(), Cache.NoSlidingExpiration, CacheItemPriority.High, onRemoveCallBack);
        }
        /// <summary>
        /// 插入缓存对象(相对过期时间)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration">相对过期时间</param>
        public static void Insert(string key, object value, TimeSpan slidingExpiration)
        {
            Insert(key, value, slidingExpiration, (CacheDependency)null);
        }
        /// <summary>
        /// 插入缓存对象(相对过期时间,缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration">相对过期时间</param>
        /// <param name="dependencyFile">文件依赖</param>
        public static void Insert(string key, object value, TimeSpan slidingExpiration, string dependencyFile)
        {
            GenerateFile(dependencyFile);
            HttpRuntime.Cache.Insert(key, value, new CacheDependency(dependencyFile), Cache.NoAbsoluteExpiration, slidingExpiration);
        }
        /// <summary>
        /// 插入缓存对象(相对过期时间,缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration">相对过期时间</param>
        /// <param name="dependencies">缓存依赖</param>
        public static void Insert(string key, object value, TimeSpan slidingExpiration, CacheDependency dependencies)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, Cache.NoAbsoluteExpiration, slidingExpiration);
        }
        /// <summary>
        /// 插入缓存对象(相对过期时间,缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration">相对过期时间</param>
        /// <param name="dependencyFile">文件依赖</param>
        /// <param name="onRemoveCallBack">缓存消失之后的处理方法</param>
        public static void Insert(string key, object value, TimeSpan slidingExpiration, string dependencyFile, CacheItemRemovedCallback onRemoveCallBack)
        {
            GenerateFile(dependencyFile);
            HttpRuntime.Cache.Insert(key, value, new CacheDependency(dependencyFile), Cache.NoAbsoluteExpiration, slidingExpiration, CacheItemPriority.High, onRemoveCallBack);
        }
        /// <summary>
        /// 插入缓存对象(相对过期时间,缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration">相对过期时间</param>
        /// <param name="dependencies">缓存依赖</param>
        /// <param name="onRemoveCallBack">缓存消失之后的处理方法</param>
        public static void Insert(string key, object value, TimeSpan slidingExpiration, CacheDependency dependencies, CacheItemRemovedCallback onRemoveCallBack)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, Cache.NoAbsoluteExpiration, slidingExpiration, CacheItemPriority.High, onRemoveCallBack);
        }
        /// <summary>
        /// 插入缓存对象(绝对过期时间，相对过期时间，缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="dependencyFile"></param>
        public static void Insert(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration, string dependencyFile)
        {
            GenerateFile(dependencyFile);
            HttpRuntime.Cache.Insert(key, value, new CacheDependency(dependencyFile), absoluteExpiration.ToUniversalTime(), slidingExpiration);
        }
        /// <summary>
        /// 插入缓存对象(绝对过期时间，相对过期时间，缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="dependencies"></param>
        public static void Insert(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheDependency dependencies)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration.ToUniversalTime(), slidingExpiration);
        }
        /// <summary>
        /// 插入缓存对象(绝对过期时间，相对过期时间，缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="dependencyFile"></param>
        /// <param name="onRemoveCallBack"></param>
        public static void Insert(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration, string dependencyFile, CacheItemRemovedCallback onRemoveCallBack)
        {
            GenerateFile(dependencyFile);
            HttpRuntime.Cache.Insert(key, value, new CacheDependency(dependencyFile), absoluteExpiration.ToUniversalTime(), slidingExpiration, CacheItemPriority.High, onRemoveCallBack);
        }
        /// <summary>
        /// 插入缓存对象(绝对过期时间，相对过期时间，缓存依赖)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="dependencies"></param>
        /// <param name="onRemoveCallBack"></param>
        public static void Insert(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheDependency dependencies, CacheItemRemovedCallback onRemoveCallBack)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration.ToUniversalTime(), slidingExpiration, CacheItemPriority.High, onRemoveCallBack);
        }
        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns>不等于null表示返回删除的缓存对象,等于null表示缓存中没有指定Key的缓存对象</returns>
        public static object Delete(string key)
        {
            return HttpRuntime.Cache.Remove(key);
        }
        /// <summary>
        /// 得到缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns>未找到该Key时为 null</returns>
        public static object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }
        /// <summary>
        /// 获取所有缓存信息(不包含已过期的缓存项)
        /// </summary>
        /// <returns></returns>
        public static IDictionaryEnumerator GetAll()
        {
            return HttpRuntime.Cache.GetEnumerator();
        }
        /// <summary>
        /// 获取满足查询条件的Key的内容
        /// </summary>
        /// <param name="keyContent">要查询的key（可以写的不完整）</param>
        /// <param name="count">取出满足条件的最大数量</param>
        /// <returns></returns>
        public static List<string> GetKey(string keyContent = "_OLD_", bool contains = true, int count = 1000)
        {
            if (string.IsNullOrWhiteSpace(keyContent))
            {
                throw new ArgumentException(nameof(keyContent));
            }
            if (count < 1)
            {
                throw new ArgumentException(nameof(count));
            }
            object @object = null;
            string key = null;
            keyContent = keyContent.ToLower();
            List<string> list = new List<string>(count);
            IDictionaryEnumerator enumerator = GetAll();
            while (enumerator.MoveNext())
            {
                @object = enumerator.Key;
                if (@object is string)
                {
                    key = (@object as string).ToLower();
                    if (contains)
                    {
                        if (key.Contains(keyContent))
                        {
                            list.Add(key);
                        }
                    }
                    else
                    {
                        if (!key.Contains(keyContent) && !key.EndsWith(".html"))
                        {
                            list.Add(key);
                        }
                    }
                    if (list.Count == count)
                    {
                        break;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取满足查询条件的Key的数量
        /// </summary>
        /// <param name="keyContent">要查询的key（可以写的不完整）</param>
        /// <returns></returns>
        public static long GetKeyCount(string keyContent = "_OLD_", bool contains = true)
        {
            if (string.IsNullOrWhiteSpace(keyContent))
            {
                throw new ArgumentException(nameof(keyContent));
            }
            object @object = null;
            string key = null;
            long count = 0L;
            keyContent = keyContent.ToLower();
            IDictionaryEnumerator enumerator = GetAll();
            while (enumerator.MoveNext())
            {
                @object = enumerator.Key;
                if (@object is string)
                {
                    key = (@object as string).ToLower();
                    if (contains)
                    {
                        if (key.Contains(keyContent))
                        {
                            System.Threading.Interlocked.Increment(ref count);
                        }
                    }
                    else
                    {
                        if (!key.Contains(keyContent) && !key.EndsWith(".html"))
                        {
                            System.Threading.Interlocked.Increment(ref count);
                        }
                    }
                }
            }
            return count;
        }
    }

    partial class DataCacheBus
    {
        private const int SECONDS = 60;
        private const string OLD = "_OLD_";
        /// <summary>
        /// 得到缓存列表对象(绝对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="lock"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        private static List<T> GetOrInsertList<T>(string key, Func<List<T>> func, DateTime absoluteExpiration, object @lock, string dependencyFile)
        {
#if SOURCE
            List<T> list = Get(key) as List<T>;
            if (list == null || list.Count == 0)
            {
                list = func() ?? new List<T>(0);
                if (list.Count > 0)
                {
                    list.TrimExcess();
                    Insert(key, list, absoluteExpiration, dependencyFile);//绝对过期时间
                }
            }
            return list;
#else
            List<T> list = Get(key) as List<T>;
            if (list == null || list.Count == 0)
            {
                string oldKey = OLD + key;
                list = Get(oldKey) as List<T>;
                if (list == null || list.Count == 0)
                {
                    list = Get(key) as List<T>;
                    if (list == null || list.Count == 0)
                    {
                        list = func() ?? new List<T>(0);
                        if (list.Count > 0)
                        {
                            list.TrimExcess();
                            Insert(key, list, absoluteExpiration, dependencyFile, (cacheKey, value, reason) =>
                            {
                                if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                {
                                    Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                }
                            });//绝对过期时间
                        }
                    }
                }
                else
                {
                    System.Threading.Tasks.Task.Factory.StartNew(@object =>
                    {
                        State state = @object as State;
                        List<T> other = null;
                        other = Get(state.Key) as List<T>;
                        if (other == null || other.Count == 0)
                        {
                            other = func() ?? new List<T>(0);
                            if (other.Count > 0)
                            {
                                other.TrimExcess();
                                Insert(state.Key, other, absoluteExpiration, dependencyFile, (cacheKey, value, reason) =>
                                {
                                    if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                    {
                                        Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                    }
                                });//绝对过期时间
                                Delete(state.OldKey);
                            }
                        }
                    }, new State() { Key = key, OldKey = oldKey });
                }
            }
            return list ?? new List<T>(0);
#endif
        }

        /// <summary>
        /// 得到缓存列表对象(相对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="lock"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        private static List<T> GetOrInsertList<T>(string key, Func<List<T>> func, TimeSpan slidingExpiration, object @lock, string dependencyFile)
        {
#if SOURCE
            List<T> list = Get(key) as List<T>;
            if (list == null || list.Count == 0)
            {
                list = func() ?? new List<T>(0);
                if (list.Count > 0)
                {
                    list.TrimExcess();
                    Insert(key, list, slidingExpiration, dependencyFile);//相对过期时间
                }
            }
            return list;
#else
            List<T> list = Get(key) as List<T>;
            if (list == null || list.Count == 0)
            {
                string oldKey = OLD + key;
                list = Get(oldKey) as List<T>;
                if (list == null || list.Count == 0)
                {
                    list = Get(key) as List<T>;
                    if (list == null || list.Count == 0)
                    {
                        list = func() ?? new List<T>(0);
                        if (list.Count > 0)
                        {
                            list.TrimExcess();
                            Insert(key, list, slidingExpiration, dependencyFile, (cacheKey, value, reason) =>
                            {
                                if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                {
                                    Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                }
                            });//相对过期时间
                        }
                    }
                }
                else
                {
                    System.Threading.Tasks.Task.Factory.StartNew(@object =>
                    {
                        State state = @object as State;
                        List<T> other = null;
                        other = Get(state.Key) as List<T>;
                        if (other == null || other.Count == 0)
                        {
                            other = func() ?? new List<T>(0);
                            if (other.Count > 0)
                            {
                                other.TrimExcess();
                                Insert(state.Key, other, slidingExpiration, dependencyFile, (cacheKey, value, reason) =>
                                {
                                    if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                    {
                                        Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                    }
                                });//相对过期时间
                                Delete(state.OldKey);
                            }
                        }
                    }, new State() { Key = key, OldKey = oldKey });
                }
            }
            return list ?? new List<T>(0);
#endif
        }

        /// <summary>
        /// 得到缓存列表对象(绝对过期时间，相对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="lock"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        private static List<T> GetOrInsertList<T>(string key, Func<List<T>> func, DateTime absoluteExpiration, TimeSpan slidingExpiration, object @lock, string dependencyFile)
        {
#if SOURCE
            List<T> list = Get(key) as List<T>;
            if (list == null || list.Count == 0)
            {
                list = func() ?? new List<T>(0);
                if (list.Count > 0)
                {
                    list.TrimExcess();
                    Insert(key, list, absoluteExpiration, slidingExpiration, dependencyFile);//相对过期时间
                }
            }
            return list;
#else
            List<T> list = Get(key) as List<T>;
            if (list == null || list.Count == 0)
            {
                string oldKey = OLD + key;
                list = Get(oldKey) as List<T>;
                if (list == null || list.Count == 0)
                {
                    list = Get(key) as List<T>;
                    if (list == null || list.Count == 0)
                    {
                        list = func() ?? new List<T>(0);
                        if (list.Count > 0)
                        {
                            list.TrimExcess();
                            Insert(key, list, absoluteExpiration, slidingExpiration, dependencyFile, (cacheKey, value, reason) =>
                            {
                                if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                {
                                    Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                }
                            });//相对过期时间
                        }
                    }
                }
                else
                {
                    System.Threading.Tasks.Task.Factory.StartNew(@object =>
                    {
                        State state = @object as State;
                        List<T> other = null;
                        other = Get(state.Key) as List<T>;
                        if (other == null || other.Count == 0)
                        {
                            other = func() ?? new List<T>(0);
                            if (other.Count > 0)
                            {
                                other.TrimExcess();
                                Insert(state.Key, other, absoluteExpiration, slidingExpiration, dependencyFile, (cacheKey, value, reason) =>
                                {
                                    if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                    {
                                        Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                    }
                                });//相对过期时间
                                Delete(state.OldKey);
                            }
                        }
                    }, new State() { Key = key, OldKey = oldKey });
                }
            }
            return list ?? new List<T>(0);
#endif
        }

        /// <summary>
        /// 得到缓存单个对象(绝对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="lock"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        private static T GetOrInsertItem<T>(string key, Func<T> func, DateTime absoluteExpiration, object @lock, string dependencyFile) where T : class
        {
#if SOURCE
            T item = Get(key) as T;
            if (item == null)
            {
                item = func();
                if (item != null)
                {
                    Insert(key, item, absoluteExpiration, dependencyFile);//绝对过期时间
                }
            }
            return item;
#else
            T item = Get(key) as T;
            if (item == null)
            {
                string oldKey = OLD + key;
                item = Get(oldKey) as T;
                if (item == null)
                {
                    item = Get(key) as T;
                    if (item == null)
                    {
                        item = func();
                        if (item != null)
                        {
                            Insert(key, item, absoluteExpiration, dependencyFile, (cacheKey, value, reason) =>
                            {
                                if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                {
                                    Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                }
                            });//绝对过期时间
                        }
                    }
                }
                else
                {
                    System.Threading.Tasks.Task.Factory.StartNew(@object =>
                    {
                        State state = @object as State;
                        T other = null;
                        other = Get(state.Key) as T;
                        if (other == null)
                        {
                            other = func();
                            if (other != null)
                            {
                                Insert(state.Key, other, absoluteExpiration, dependencyFile, (cacheKey, value, reason) =>
                                {
                                    if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                    {
                                        Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                    }
                                });//绝对过期时间
                                Delete(state.OldKey);
                            }
                        }
                    }, new State() { Key = key, OldKey = oldKey });
                }
            }
            return item;
#endif
        }

        /// <summary>
        /// 得到缓存单个对象(相对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="lock"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        private static T GetOrInsertItem<T>(string key, Func<T> func, TimeSpan slidingExpiration, object @lock, string dependencyFile) where T : class
        {
#if SOURCE
            T item = Get(key) as T;
            if (item == null)
            {
                item = func();
                if (item != null)
                {
                    Insert(key, item, slidingExpiration, dependencyFile);//相对过期时间
                }
            }
            return item;
#else
            T item = Get(key) as T;
            if (item == null)
            {
                string oldKey = OLD + key;
                item = Get(oldKey) as T;
                if (item == null)
                {
                    item = Get(key) as T;
                    if (item == null)
                    {
                        item = func();
                        if (item != null)
                        {
                            Insert(key, item, slidingExpiration, dependencyFile, (cacheKey, value, reason) =>
                            {
                                if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                {
                                    Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                }
                            });//相对过期时间
                        }
                    }
                }
                else
                {
                    System.Threading.Tasks.Task.Factory.StartNew(@object =>
                    {
                        State state = @object as State;
                        T other = null;
                        other = Get(state.Key) as T;
                        if (other == null)
                        {
                            other = func();
                            if (other != null)
                            {
                                Insert(state.Key, other, slidingExpiration, dependencyFile, (cacheKey, value, reason) =>
                                {
                                    if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                    {
                                        Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                    }
                                });//相对过期时间
                                Delete(state.OldKey);
                            }
                        }
                    }, new State() { Key = key, OldKey = oldKey });
                }
            }
            return item;
#endif
        }
        /// <summary>
        /// 得到缓存单个对象(绝对过期时间，相对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="lock"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        private static T GetOrInsertItem<T>(string key, Func<T> func, DateTime absoluteExpiration, TimeSpan slidingExpiration, object @lock, string dependencyFile) where T : class
        {
#if SOURCE
            T item = Get(key) as T;
            if (item == null)
            {
                item = func();
                if (item != null)
                {
                    Insert(key, item, absoluteExpiration, slidingExpiration, dependencyFile);//相对过期时间
                }
            }
            return item;
#else
            T item = Get(key) as T;
            if (item == null)
            {
                string oldKey = OLD + key;
                item = Get(oldKey) as T;
                if (item == null)
                {
                    item = Get(key) as T;
                    if (item == null)
                    {
                        item = func();
                        if (item != null)
                        {
                            Insert(key, item, absoluteExpiration, slidingExpiration, dependencyFile, (cacheKey, value, reason) =>
                            {
                                if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                {
                                    Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                }
                            });//相对过期时间
                        }
                    }
                }
                else
                {
                    System.Threading.Tasks.Task.Factory.StartNew(@object =>
                    {
                        State state = @object as State;
                        T other = null;
                        other = Get(state.Key) as T;
                        if (other == null)
                        {
                            other = func();
                            if (other != null)
                            {
                                Insert(state.Key, other, absoluteExpiration, slidingExpiration, dependencyFile, (cacheKey, value, reason) =>
                                {
                                    if (reason == System.Web.Caching.CacheItemRemovedReason.Expired)
                                    {
                                        Insert(OLD + cacheKey, value, DateTime.Now.AddSeconds(SECONDS), dependencyFile);//绝对过期时间
                                    }
                                });//相对过期时间
                                Delete(state.OldKey);
                            }
                        }
                    }, new State() { Key = key, OldKey = oldKey });
                }
            }
            return item;
#endif
        }
    }

    /// <summary>
    /// Efficient, Thread-Safe Caching with ASP.NET
    /// </summary>
    partial class DataCacheBus
    {
#if LOCK
        /// <summary>
        /// 缓存锁的key的字典集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, Lock> _CacheLockKeyDictionary =
           new ConcurrentDictionary<string, Lock>();
        /// <summary>
        /// 获取缓存key的锁对象
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private static Lock GetOrAddCacheLockObject(string cacheKey)
        {
            return _CacheLockKeyDictionary.GetOrAdd(cacheKey, new Lock(cacheKey, RemoveCacheLockObject));
        }
        /// <summary>
        /// 删除缓存key对应的锁对象
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private static bool RemoveCacheLockObject(string cacheKey)
        {
            //return true;
            //wangyunpeng 删掉没用的Key，IIS定时回收内存导致这里的很多Key无效，所以用完就删掉。2017-9-7
            Lock @object = null;
            return _CacheLockKeyDictionary.TryRemove(cacheKey, out @object);
        }
#endif
        /// <summary>
        /// 得到缓存列表对象(绝对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        public static List<T> GetOrInsertList<T>(string key, Func<List<T>> func, DateTime absoluteExpiration, string dependencyFile)
        {
#if LOCK
            using (Lock @lock = GetOrAddCacheLockObject(key))
            {
                return GetOrInsertList<T>(key, func, absoluteExpiration, @lock, dependencyFile);
            }
#else
            return GetOrInsertList<T>(key, func, absoluteExpiration, null, dependencyFile);
#endif
        }
        /// <summary>
        /// 得到缓存列表对象(相对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        public static List<T> GetOrInsertList<T>(string key, Func<List<T>> func, TimeSpan slidingExpiration, string dependencyFile)
        {
#if LOCK
            using (Lock @lock = GetOrAddCacheLockObject(key))
            {
                return GetOrInsertList<T>(key, func, slidingExpiration, @lock, dependencyFile);
            }
#else
            return GetOrInsertList<T>(key, func, slidingExpiration, null, dependencyFile);
#endif
        }
        /// <summary>
        /// 得到缓存列表对象(绝对过期时间,相对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        public static List<T> GetOrInsertList<T>(string key, Func<List<T>> func, DateTime absoluteExpiration, TimeSpan slidingExpiration, string dependencyFile)
        {
#if LOCK
            using (Lock @lock = GetOrAddCacheLockObject(key))
            {
                return GetOrInsertList<T>(key, func, absoluteExpiration, slidingExpiration, @lock, dependencyFile);
            }
#else
            return GetOrInsertList<T>(key, func, absoluteExpiration, slidingExpiration, null, dependencyFile);
#endif
        }
        /// <summary>
        /// 得到缓存单个对象(绝对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        public static T GetOrInsertItem<T>(string key, Func<T> func, DateTime absoluteExpiration, string dependencyFile) where T : class
        {
#if LOCK
            using (Lock @lock = GetOrAddCacheLockObject(key))
            {
                return GetOrInsertItem<T>(key, func, absoluteExpiration, @lock, dependencyFile);
            }
#else
            return GetOrInsertItem<T>(key, func, absoluteExpiration, null, dependencyFile);
#endif
        }
        /// <summary>
        /// 得到缓存单个对象(相对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="lock"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        public static T GetOrInsertItem<T>(string key, Func<T> func, TimeSpan slidingExpiration, string dependencyFile) where T : class
        {
#if LOCK
            using (Lock @lock = GetOrAddCacheLockObject(key))
            {
                return GetOrInsertItem<T>(key, func, slidingExpiration, @lock, dependencyFile);
            }
#else
            return GetOrInsertItem<T>(key, func, slidingExpiration, null, dependencyFile);
#endif
        }
        /// <summary>
        /// 得到缓存单个对象(绝对过期时间,相对过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="dependencyFile"></param>
        /// <returns></returns>
        public static T GetOrInsertItem<T>(string key, Func<T> func, DateTime absoluteExpiration, TimeSpan slidingExpiration, string dependencyFile) where T : class
        {
#if LOCK
            using (Lock @lock = GetOrAddCacheLockObject(key))
            {
                return GetOrInsertItem<T>(key, func, absoluteExpiration, slidingExpiration, @lock, dependencyFile);
            }
#else
            return GetOrInsertItem<T>(key, func, absoluteExpiration, slidingExpiration, null, dependencyFile);
#endif
        }
    }

    /// <summary>
    /// 锁对象
    /// </summary>
    internal class Lock : IDisposable
    {
        private string _key = null;
        private Func<string, bool> _func = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="dispose">删除锁对象的方法</param>
        internal Lock(string key, Func<string, bool> dispose)
        {
            this._key = key;
            this._func = dispose;
        }
        public void Dispose()
        {
            this._func(this._key);
        }
    }
    internal class State
    {
        internal string Key { get; set; }
        internal string OldKey { get; set; }
    }
}
