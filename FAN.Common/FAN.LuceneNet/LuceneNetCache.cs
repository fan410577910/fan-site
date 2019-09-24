#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneNetCache
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 缓存搜索数据
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 缓存搜索数据
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TObject"></typeparam>
    public class LuceneNetCache<TKey, TObject> : IDisposable
    {
        /// <summary>
        /// 缓存对象的容器
        /// </summary>
        private ConcurrentDictionary<TKey, TObject> _dictionary = null;
        public LuceneNetCache()
        {
            this._dictionary = new ConcurrentDictionary<TKey, TObject>();
            LuceneNetConfig.ConfigChangedEvent += this.Clear;
        }
        ~LuceneNetCache()
        {
            LuceneNetConfig.ConfigChangedEvent -= this.Clear;
        }
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void Clear()
        {
            //DataCacheBus
            //IDictionaryEnumerator enumerator = WebCache.DataCacheBus.GetAll();
            //while (enumerator.MoveNext())
            //{
            //    TLZ.WebCache.DataCacheBus.Delete(enumerator.Key.ToString());
            //}

            //MemoryCacheBus
            IEnumerator<KeyValuePair<string, object>> enumerator = MemCache.MemoryCacheBus.GetAll();
            while (enumerator.MoveNext())
            {
                MemCache.MemoryCacheBus.Delete(enumerator.Current.Key);
            }

            this._dictionary.Clear();
        }
        /// <summary>
        /// 获取缓存中的对象
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>缓存数据</returns>
        public TObject GetTObject(TKey key)
        {
            TObject tObject = default(TObject);
            this._dictionary.TryGetValue(key, out tObject);
            return tObject;
        }
        /// <summary>
        /// 进入缓存对象
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="newTObject">要缓存数据</param>
        /// <returns>True表示进入缓存，False表示没有进入缓存</returns>
        public bool AddOrUpdateTObject(TKey key, TObject newTObject)
        {
            bool result = false;
            TObject oldTObject = default(TObject);
            if (this._dictionary.TryGetValue(key, out oldTObject))
            {
                result = this._dictionary.TryUpdate(key, newTObject, oldTObject);
            }
            else
            {
                result = this._dictionary.TryAdd(key, newTObject);
            }
            return result;
        }
        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="key">主键</param>
        public void DeleteTObject(TKey key)
        {
            TObject tObject = default(TObject);
            this._dictionary.TryRemove(key, out tObject);
        }

        public void Dispose()
        {
            this.Clear();
        }
    }
}
