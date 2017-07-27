using System;

namespace Hanlin.Common.Caching
{
    public interface ICacheManager
    {
        void Set(string cacheKey, object objectToCache, TimeSpan slidingExpiration, Action<object> updateDelegate = null);
        void Set<T>(ICachingDefinition<T> cachingDefinition, T value);

        T Get<T>(string cacheKey);
        T GetOrAdd<T>(ICachingDefinition<T> cachingDefinition);
        T GetOrAdd<T>(string cacheKey, Func<T> factoryMethod, TimeSpan? slidingExpiration = null);
        object Remove(string cacheKey);
        bool Contains(string cacheKey);
    }
}