using System;
using System.Runtime.Caching;

namespace AutocorApi.Repositorios.Cache
{
    public class MemoryCacher : IMemoryCacher
    {
        public object GetValue(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }

        public T GetValue<T>(string key) where T : class
        {
            return GetValue(key) as T;
        }

        public bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, absExpiration);
        }

        public void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }

        public T GetOrAddAs<T>(string cacheKey, Func<T> method, DateTimeOffset absExpiration) where T : class
        {
            return GetOrAdd(cacheKey, method, absExpiration) as T;
        }

        public object GetOrAdd(string cacheKey, Func<object> method, DateTimeOffset absExpiration)
        {
            // buscar el recurso en caché
            object res = GetValue(cacheKey);

            if (res != null)
                return res;

            // buscar el recurso real
            res = method();

            if (res == null)
                return res;

            Add(cacheKey, res, absExpiration);

            return res;
        }
    }
}