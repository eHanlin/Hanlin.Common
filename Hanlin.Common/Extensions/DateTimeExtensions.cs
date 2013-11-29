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

        // Straight from: http://stackoverflow.com/a/250400
        public static DateTime FromUnixTimestamp(double timestampMillis)
        {
            var seconds = timestampMillis / 1000d;

            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(seconds).ToLocalTime();
            return dtDateTime;
        }
    }
}
