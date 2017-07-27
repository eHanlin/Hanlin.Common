using System;
using System.Runtime.Caching;

namespace Hanlin.Common.Caching
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly MemoryCache _cache = MemoryCache.Default;

        public void Set(string cacheKey, object objectToCache, TimeSpan slidingExpiration, Action<object> updateDelegate = null)
        {
            var policy = new CacheItemPolicy();

            CacheEntryUpdateCallback cacheEntryUpdateCallback = null;
            if (updateDelegate != null)
            {
                cacheEntryUpdateCallback = arguments =>
                {
                    var cache = arguments.Source;
                    var key = arguments.Key;
                    var removingItem = cache.Get(key);
                    updateDelegate(removingItem);
                };
            }

            policy.UpdateCallback = cacheEntryUpdateCallback;
            policy.SlidingExpiration = slidingExpiration;
            _cache.Set(cacheKey, objectToCache, policy);
        }

        public void Set<T>(ICachingDefinition<T> cachingDefinition, T value)
        {
            var policy = new CacheItemPolicy
            {
                SlidingExpiration = cachingDefinition.SlidingExpiration
            };
            _cache.Set(cachingDefinition.Key, value, policy);
        }

        public T Get<T>(string cacheKey)
        {
            return (T)GetOrAdd<object>(cacheKey);
        }

        public T GetOrAdd<T>(ICachingDefinition<T> cachingDefinition)
        {
            return GetOrAdd(cachingDefinition.Key, cachingDefinition.ValueFactory, cachingDefinition.SlidingExpiration);
        }

        public T GetOrAdd<T>(string cacheKey, Func<T> factoryMethod = null, TimeSpan? slidingExpiration = null)
        {
            var entry = _cache.Get(cacheKey);

            if (entry == null && factoryMethod != null)
            {
                entry = factoryMethod();
                if (entry != null)
                {
                    Set(cacheKey, entry, slidingExpiration ?? BaseCachingDefinition.DefaultSlidingExpiration);
                }
            }
            return (T)entry;
        }

        public object Remove(string cacheKey)
        {
            return _cache.Remove(cacheKey);
        }

        public bool Contains(string cacheKey)
        {
            return _cache.Contains(cacheKey);
        }
    }
}