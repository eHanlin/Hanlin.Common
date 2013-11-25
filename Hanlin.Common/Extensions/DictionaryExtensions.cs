using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
