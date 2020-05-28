using System;

namespace AutocorApi.Repositorios.Cache
{
    public interface IMemoryCacher
    {
        bool Add(string key, object value, DateTimeOffset absExpiration);

        void Delete(string key);

        object GetValue(string key);

        T GetValue<T>(string key) where T : class;

        object GetOrAdd(string cacheKey, Func<object> method, DateTimeOffset absExpiration);

        T GetOrAddAs<T>(string cacheKey, Func<T> method, DateTimeOffset absExpiration) where T : class;
    }
}