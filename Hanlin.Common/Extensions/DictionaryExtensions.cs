using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hanlin.Common.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// From: http://stackoverflow.com/a/538751/494297
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue ret;
            // Ignore return value
            dictionary.TryGetValue(key, out ret);
            return ret;
        }

        // Original source: http://stackoverflow.com/a/2819566
        public static string AsString(this Dictionary<String, String> hash)
        {
            return String.Join(", ", hash.Select(kvp => kvp.Key + ":" + kvp.Value));
        }

        public static bool GetBoolean<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            var rawValue = dictionary.GetValueOrDefault(key);
            var result = false;

            try
            {
                result = Convert.ToBoolean(rawValue);
            }
            catch (Exception e)
            {
                // ignored
            }

            return result;
        }
    }
}
