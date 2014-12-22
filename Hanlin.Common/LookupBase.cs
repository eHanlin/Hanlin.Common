using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;

namespace Hanlin.Common
{
    public abstract class LookupBase<TKey, TType>
    {
        protected IDictionary<TKey, TType> Lookup = new Dictionary<TKey, TType>();

        public TType GetByKey(TKey key)
        {
            TType value;
            if (TryGetByKey(key, out value))
            {
                return value;
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot lookup with key: {0}. Available keys: {1} ({2} keys in total)", key, GetKeyList(50), Lookup.Keys.Count));
            }
        }

        public bool TryGetByKey(TKey key, out TType value)
        {
            return Lookup.TryGetValue(key, out value);
        }

        protected void TryAdd(TKey key, TType value)
        {
            if (Lookup.ContainsKey(key))
            {
                throw new ArgumentException("Lookup already contains a key: " + key);
            }

            Lookup[key] = value;
        }

        protected string GetKeyList(int howMany = int.MaxValue)
        {
            return string.Join(", ", Lookup.Keys.Take(howMany));
        }
    }

    public class StringLookupBase<T> : LookupBase<string, T>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public const string KeyPartsSeparator = "_";

        protected readonly ISet<string> LoggedLookup = new HashSet<string>();

        protected static string Key(params string[] components)
        {
            return string.Join(KeyPartsSeparator, components);
        }

        public T GetByKeyParts(params string[] keyParts)
        {
            var key = Key(keyParts);

            T value;
            if (TryGetByKey(key, out value))
            {
                return value;
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot lookup with key: {0}. Available keys: {1} ({2} keys in total)", key, GetKeyList(50), Lookup.Count()));
            }
        }

        public bool TryGetByKey(out T value, params string[] keyParts)
        {
            return Lookup.TryGetValue(Key(keyParts), out value);
        }

        protected void LogOnce(string format, params object[] args)
        {
            var message = string.Format(format, args);

            if (LoggedLookup.Contains(message)) return;

            LoggedLookup.Add(message);
            Log.Debug(message);
        }

        public bool Contains(params string[] keyParts)
        {
            return Lookup.ContainsKey(Key(keyParts));
        }
    }
}