using System;
using System.Globalization;
using NodaTime;

namespace R8.DateTimeLocalization
{
    public interface ITimezoneInfo
    {
        /// <summary>
        ///     Gets the IANA timezone identifier.
        /// </summary>
        string IanaId { get; }

        /// <summary>
        ///     Returns the culture info of the timezone.
        /// </summary>
        CultureInfo Culture { get; }

        /// <summary>
        ///     Gets the ordered days of the week according to the timezone
        /// </summary>
        DayOfWeek[] DaysOfWeek { get; }

        ZonedClock Clock { get; }
    }
}