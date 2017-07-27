using System;

namespace Hanlin.Common.Caching
{
    public interface ICachingDefinition<TValue>
    {
        string Key { get; }
        TimeSpan SlidingExpiration { get; set; }
        Func<TValue> ValueFactory { get; set; }
    }

    public abstract class BaseCachingDefinition
    {
        public static readonly TimeSpan DefaultSlidingExpiration = new TimeSpan(0, 30, 0);
    }

    public abstract class BaseCachingDefinition<TValue> : BaseCachingDefinition, ICachingDefinition<TValue>
    {
        protected BaseCachingDefinition(string key)
        {
            Key = key;
            SlidingExpiration = DefaultSlidingExpiration;
        }

        public string Key { get; private set; }
        public TimeSpan SlidingExpiration { get; set; }
        public Func<TValue> ValueFactory { get; set; }
    }
}