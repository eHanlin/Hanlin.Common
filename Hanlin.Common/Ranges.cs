using System;

namespace Hanlin.Common
{
    /// <summary>
    /// Straight from http://stackoverflow.com/a/4781727/494297
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRange<T>
    {
        T Start { get; }
        T End { get; }
        bool Includes(T value);
        bool Includes(IRange<T> range);
    }

    public class DateRange : IRange<DateTime>
    {
        public DateRange(DateTime start, DateTime end)
        {
            Start = start.Date;
            End = end.Date;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public bool Includes(DateTime value)
        {
            return (Start <= value) && (value <= End);
        }

        public bool Includes(IRange<DateTime> range)
        {
            return (Start <= range.Start) && (range.End <= End);
        }
    }

    public static class DateRangeUtils
    {
        public static DateRange RangeForMonth(int year, int month)
        {
            return new DateRange(
                new DateTime(year, month, 1),
                new DateTime(year, month, DateTime.DaysInMonth(year, month))
            );
        }
    }
}
