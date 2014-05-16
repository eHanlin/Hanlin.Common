using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Hanlin.Common.Extensions
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Wraps this object instance into an IEnumerable&lt;T&gt;
        /// consisting of a single item.
        /// Copied straight from http://stackoverflow.com/q/1577822/494297
        /// </summary>
        /// <typeparam name="T"> Type of the wrapped object.</typeparam>
        /// <param name="item"> The object to wrap.</param>
        /// <returns>
        /// An IEnumerable&lt;T&gt; consisting of a single item.
        /// </returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        // Straight from: http://stackoverflow.com/a/6252351/494297
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            var array = values as double[] ?? values.ToArray();
            double avg = array.Average();
            return Math.Sqrt(array.Average(v => Math.Pow(v - avg, 2)));
        }

        public static string AsString<T>(this IEnumerable<T> list)
        {
            var str = string.Empty;
            if (list != null) str = string.Join(", ", list);
            return "[" + str + "]";
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count)
        {
            var total = source.Count();

            var randomIndices = new HashSet<int>();

            var gen = new Random();

            for (int i = 0; i < count; i++)
            {
                int next;
                do
                {
                    next = gen.Next(0, total);
                } while (randomIndices.Contains(next));
                randomIndices.Add(next);
            }

            return randomIndices.Select(source.ElementAt).ToList();
        }
    }
}
