using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hanlin.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToLowerOrDefault(this string str)
        {
            if (str == null) return null;

            return str.ToLowerInvariant();
        }

        private static string ToLowerOrUpper(this string str, bool isLower)
        {
            if (str == null) return null;
            if (str.Length > 0)
            {
                var firstChar = (isLower ? char.ToLower(str[0]) : char.ToUpper(str[0]));

                return str.Length > 1 ? firstChar + str.Substring(1) : firstChar.ToString();
            }
            return str;
        }

        public static string ToFirstLowerOrDefault(this string str)
        {
            return ToLowerOrUpper(str, true);
        }

        public static string ToFirstUpperOrDefault(this string str)
        {
           return ToLowerOrUpper(str, false);
        }
    }
}
