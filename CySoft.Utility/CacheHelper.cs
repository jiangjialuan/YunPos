using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace CySoft.Utility
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    public sealed class CacheHelper
    {
        // YZQ修改 读写缓存要用线程锁
        private static ReaderWriterLock _CacheLock = new ReaderWriterLock();
        private static Cache _cache;

        private CacheHelper() { }

        /// <summary>
        /// Static initializer should ensure we only have to look up the current cache
        /// instance once.
        /// </summary>
        static CacheHelper()
        {
            // 如果当前上下文已经创建，则使用HttpContext.Cache,否则使用HttpRuntime.Cache
            // 两者其实同一个Cache
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                _cache = context.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// Removes all items from the Cache
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }

            _CacheLock.AcquireWriterLock(Timeout.Infinite);

            foreach (string key in al)
            {
                _cache.Remove(key);
            }

            _CacheLock.ReleaseWriterLock();
        }

        /// <summary>
        /// Removes the items the by pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        public static void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            while (CacheEnum.MoveNext())
            {
                if (regex.IsMatch(CacheEnum.Key.ToString()))
                {
                    _CacheLock.AcquireWriterLock(Timeout.Infinite);

                    _cache.Remove(CacheEnum.Key.ToString());

                    _CacheLock.ReleaseWriterLock();
                }
            }
        }

        /// <summary>
        /// Removes the specified key from the cache
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            _CacheLock.AcquireWriterLock(Timeout.Infinite);

            _cache.Remove(key);

            _CacheLock.ReleaseWriterLock();
        }

        /// <summary>
        /// Insert the current "obj" into the cache. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Insert(string key, object obj)
        {
            Insert(key, obj, null, 10);
        }

        //public static void Insert(string key, object obj, CacheDependency dep)
        //{
        //    Insert(key, obj, dep, HourFactor * 12);
        //}

        public static void Insert(string key, object obj, int seconds)
        {
            Insert(key, obj, null, seconds);
        }

        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                _CacheLock.AcquireWriterLock(Timeout.Infinite);

                //_cache.Insert(key, obj, dep, DateTime.Now.AddSeconds(Factor * SecondFactor * seconds), TimeSpan.Zero, priority, null);            //YZQ
                _cache.Insert(key, obj, dep, DateTime.Now.AddSeconds(seconds), TimeSpan.Zero, priority, null);

                _CacheLock.ReleaseWriterLock();
            }

        }
        /// <summary>
        /// 插入缓存，使用相对过期时间
        /// </summary>
        /// <param name="key">缓存名称</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="ts">时间间隔</param>
        public static void Insert_sliding(string key, object obj, TimeSpan ts)
        {
            if (obj != null)
            {
                _CacheLock.AcquireWriterLock(Timeout.Infinite);

                _cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, ts, CacheItemPriority.Normal, null);

                _CacheLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 插入缓存，使用相对过期时间（默认10秒）
        /// </summary>
        /// <param name="key">缓存名称</param>
        /// <param name="obj">缓存对象</param>
        public static void Insert_sliding(string key, object obj)
        {
            Insert_sliding(key, obj, new TimeSpan(0, 0, 0, 10, 0));
        }

        //public static void MicroInsert(string key, object obj, int secondFactor)
        //{
        //    if (obj != null)
        //    {
        //        _CacheLock.AcquireWriterLock(Timeout.Infinite);

        //        _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(Factor * secondFactor), TimeSpan.Zero);

        //        _CacheLock.ReleaseWriterLock();
        //    }
        //}

        /// <summary>
        /// Insert an item into the cache for the Maximum allowed time
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Max(string key, object obj)
        {
            Max(key, obj, null);
        }

        public static void Max(string key, object obj, CacheDependency dep)
        {
            if (obj != null)
            {
                _CacheLock.AcquireWriterLock(Timeout.Infinite);

                _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);

                _CacheLock.ReleaseWriterLock();
            }
        }

        public static object Get(string key)
        {
            _CacheLock.AcquireReaderLock(Timeout.Infinite);
            object obj = _cache[key];
            _CacheLock.ReleaseReaderLock();

            return obj;
        }

        /// <summary>
        /// 取缓存 为null返回default(T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            object obj = Get(key);
            if (obj != null)
            {
                return (T)obj;
            }
            return default(T);
        }

        ///// <summary>
        ///// Return int of seconds * SecondFactor
        ///// </summary>
        //public static int SecondFactorCalculate(int seconds)
        //{
        //    // Insert method below takes integer seconds, so we have to round any fractional values
        //    return Convert.ToInt32(Math.Round((double)seconds * SecondFactor));
        //}

    }
}
