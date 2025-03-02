using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using NodaTime;

namespace R8.DateTimeLocalization
{
    /// <summary>
    ///     A class representing a local timezone options.
    /// </summary>
    public abstract class LocalTimezoneInfo : ITimezoneInfo
    {
        private static readonly Lazy<DayOfWeek[]> UnorderedDaysOfWeek = new(() => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToArray(), LazyThreadSafetyMode.ExecutionAndPublication);

        internal ushort _index;

        public DayOfWeek[] DaysOfWeek
        {
            get
            {
                var daysOfWeek = UnorderedDaysOfWeek.Value;
                if (daysOfWeek is not { Length: 7 })
                    throw new InvalidOperationException("Days of week are not valid.");

                var dayOfWeekAdjustment = Math.Abs(0 - (int)Culture.DateTimeFormat.FirstDayOfWeek);
                if (dayOfWeekAdjustment == 0)
                    return daysOfWeek;

                Span<DayOfWeek> orderedArray = stackalloc DayOfWeek[7];
                for (var i = 0; i < daysOfWeek.Length; i++)
                {
                    var adjustment = (i + dayOfWeekAdjustment - 1) % 7;
                    var index = adjustment >= 6
                        ? 7 - adjustment - 1
                        : adjustment + 1;
                    orderedArray[i] = daysOfWeek[index];
                }

                return orderedArray.ToArray();
            }
        }

        public abstract string IanaId { get; }
        public abstract CultureInfo Culture { get; }
        public ZonedClock Clock { get; protected init; }
    }
}