using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanlin.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static double ToUnixTimestamp(this DateTime dateTime)
        {
            var unixTimeSpan = (dateTime.AddHours(-8) - new DateTime(1970, 1, 1, 0, 0, 0));
            return unixTimeSpan.TotalMilliseconds;
        }
    }
}
