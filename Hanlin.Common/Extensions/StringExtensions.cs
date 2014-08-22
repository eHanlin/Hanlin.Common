using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
