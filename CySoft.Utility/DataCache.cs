using System;
using System.Web;
using System.Web.Caching;

namespace CySoft.Utility
{
    /// <summary>
    /// 数据缓存类
    /// </summary>
    public class DataCache
    {
        public static readonly DateTime NoAbsoluteExpiration = Cache.NoAbsoluteExpiration;
        public static readonly TimeSpan NoSlidingExpiration = Cache.NoSlidingExpiration;

        public static Cache cache
        {
            get
            {
                return HttpContext.Current.Cache;
            }
        }

        public static int Count
        {
            get
            {
                return cache.Count;
            }
        }

        public static object Add(string key, object value)
        {
            return Add(key, value, CacheItemPriority.Default);
        }

        public static object Add(string key, object value, DateTime absoluteExpiration)
        {
            return Add(key, value, absoluteExpiration, CacheItemPriority.Default);
        }

        public static object Add(string key, object value, DateTime absoluteExpiration, CacheItemPriority priority)
        {
            return Add(key, value, absoluteExpiration, priority, null);
        }

        public static object Add(string key, object value, DateTime absoluteExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            return cache.Add(key, value, null, absoluteExpiration, NoSlidingExpiration, priority, onRemoveCallback);
        }

        public static object Add(string key, object value, TimeSpan slidingExpiration)
        {
            return Add(key, value, slidingExpiration, CacheItemPriority.Default);
        }

        public static object Add(string key, object value, TimeSpan slidingExpiration, CacheItemPriority priority)
        {
            return Add(key, value, slidingExpiration, priority, null);
        }

        public static object Add(string key, object value, CacheItemPriority priority)
        {
            return Add(key, value, NoSlidingExpiration, priority, null);
        }

        public static object Add(string key, object value, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            return cache.Add(key, value, null, NoAbsoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }

        public static bool ContainsKey(string key)
        {
            if (cache.Get(key) != null)
            {
                return true;
            }
            return false;
        }

        public static void Remove(string key)
        {
            cache.Remove(key);
        }

        public static void Remove(params string[] keys)
        {
            if (keys != null && keys.Length > 0)
            {
                foreach (string key in keys)
                {
                    cache.Remove(key);
                }
            }

        }

        public static object Get(string key)
        {
            return cache.Get(key);
        }

        public static T Get<T>(string key)
        {
            object obj = cache.Get(key);
            if (obj != null)
            {
                return (T)obj;
            }
            return default(T);
        }

        public static object Set(string key, object value)
        {
            return cache[key] = value;
        }
    }
}