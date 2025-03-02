using System;
using System.Runtime.CompilerServices;
using NodaTime;
using NodaTime.Extensions;

namespace R8.DateTimeLocalization
{
    public static class ZonedDateTimeExtensions
    {
        /// <summary>
        ///     Converts a <see cref="DateTime" /> to a <see cref="ZonedDateTime" /> according to the specified
        ///     <see cref="dateTime" />.
        /// </summary>
        /// <param name="dateTime">A <see cref="timezone" /> object.</param>
        /// <param name="timezone">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="LocalTimezone" /> object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ZonedDateTime ToZonedDateTime(this DateTime dateTime, ITimezoneInfo timezone)
        {
            ZonedDateTime zoned;
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                var instant = Instant.FromDateTimeUtc(dateTime);
                zoned = instant.InZone(timezone.Clock.Zone, timezone.Clock.Calendar);
            }
            else
            {
                var local = LocalDateTime.FromDateTime(dateTime);
                zoned = local.InZoneLeniently(timezone.Clock.Zone).WithCalendar(timezone.Clock.Calendar);
            }

            return zoned;
        }

        /// <summary>
        ///     Converts a <see cref="LocalDateTime" /> to a <see cref="ZonedDateTime" /> according to the specified
        ///     <see cref="LocalTimezone" />.
        /// </summary>
        /// <param name="localDateTime">A <see cref="LocalDateTime" /> object.</param>
        /// <param name="timezone">A <see cref="LocalTimezone" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ZonedDateTime ToZonedDateTime(this LocalDateTime localDateTime, LocalTimezone timezone)
        {
            var zonedDateTime = localDateTime.InZoneLeniently(timezone.Clock.Zone).WithCalendar(timezone.Calendar);
            return zonedDateTime;
        }

        /// <summary>
        ///     Returns a <see cref="LocalTimezone" /> from the specified <see cref="ZonedDateTime" />.
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="LocalTimezone" /> object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LocalTimezone GetTimezone(this ZonedDateTime zonedDateTime)
        {
            return LocalTimezone.GetOrCreate(zonedDateTime.Zone.Id);
        }

        /// <summary>
        ///     Returns a <see cref="LocalTimezone" /> from the specified <see cref="DateTimeZone" />.
        /// </summary>
        /// <param name="dateTimeZone">A <see cref="DateTimeZone" /> object.</param>
        /// <returns>A <see cref="LocalTimezone" /> object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LocalTimezone GetTimezone(this DateTimeZone dateTimeZone)
        {
            return LocalTimezone.GetOrCreate(dateTimeZone.Id);
        }

        /// <summary>
        ///     Returns the number of days in the month and year of the specified date.
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>An integer from 28 through 31.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDaysInMonth(this ZonedDateTime zonedDateTime)
        {
            return zonedDateTime.Calendar.GetDaysInMonth(zonedDateTime.Year, zonedDateTime.Month);
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the hour value set to 00:00. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 12:00:00".
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime GetStartOfHour(this ZonedDateTime zonedDateTime)
        {
            if (zonedDateTime is { Minute: 0, Second: 0 })
                return zonedDateTime;

            var dateTime = new LocalDateTime(zonedDateTime.Year, zonedDateTime.Month, zonedDateTime.Day, zonedDateTime.Hour, 0, 0, zonedDateTime.Calendar);
            var output = dateTime
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the hour value set to 23:59. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 23:59:59".
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime GetEndOfHour(this ZonedDateTime zonedDateTime)
        {
            if (zonedDateTime is { Minute: 59, Second: 59 })
                return zonedDateTime;

            var dateTime = new LocalDateTime(zonedDateTime.Year, zonedDateTime.Month, zonedDateTime.Day, zonedDateTime.Hour, 59, 59, zonedDateTime.Calendar);
            var output = dateTime
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the minute value set to 00. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 12:33:00".
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime GetStartOfMinute(this ZonedDateTime zonedDateTime)
        {
            if (zonedDateTime.Second == 0)
                return zonedDateTime;

            var dateTime = new LocalDateTime(zonedDateTime.Year, zonedDateTime.Month, zonedDateTime.Day, zonedDateTime.Hour, zonedDateTime.Minute, 0, zonedDateTime.Calendar);
            var output = dateTime
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the minute value set to 59. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 12:33:59".
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime GetEndOfMinute(this ZonedDateTime zonedDateTime)
        {
            if (zonedDateTime.Second == 59)
                return zonedDateTime;

            var dateTime = new LocalDateTime(zonedDateTime.Year, zonedDateTime.Month, zonedDateTime.Day, zonedDateTime.Hour, zonedDateTime.Minute, 59, zonedDateTime.Calendar);
            var output = dateTime
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the time value set to 00:00:00. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 00:00:00".
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime GetStartOfDay(this ZonedDateTime zonedDateTime)
        {
            if (zonedDateTime is { Hour: 0, Minute: 0, Second: 0 })
                return zonedDateTime;

            var dateTime = new LocalDateTime(zonedDateTime.Year, zonedDateTime.Month, zonedDateTime.Day, 0, 0, 0, zonedDateTime.Calendar);
            var output = dateTime
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the time value set to 23:59:59. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 23:59:59".
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime GetEndOfDay(this ZonedDateTime zonedDateTime)
        {
            if (zonedDateTime is { Hour: 23, Minute: 59, Second: 59 })
                return zonedDateTime;

            var dateTime = new LocalDateTime(zonedDateTime.Year, zonedDateTime.Month, zonedDateTime.Day, 23, 59, 59, zonedDateTime.Calendar);
            var output = dateTime
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and the day value set to 1. For instance, if the current instance
        ///     represents the date "2019-01-15 12:33:25", this method will return a new instance representing "2019-01-01
        ///     00:00:00".
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime GetStartOfMonth(this ZonedDateTime zonedDateTime)
        {
            if (zonedDateTime is { Day: 1, Hour: 0, Minute: 0, Second: 0 })
                return zonedDateTime;

            var dateTime = new LocalDateTime(zonedDateTime.Year, zonedDateTime.Month, 1, 0, 0, 0, zonedDateTime.Calendar);
            var output = dateTime
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and the day value set to the last day of the month. For instance, if
        ///     the current instance represents the date "2019-01-15 12:33:25", this method will return a new instance representing
        ///     "2019-01-31 23:59:59".
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime GetEndOfMonth(this ZonedDateTime zonedDateTime)
        {
            var lastDay = zonedDateTime.GetDaysInMonth();
            if (zonedDateTime is { Hour: 23, Minute: 59, Second: 59 } && zonedDateTime.Day == lastDay)
                return zonedDateTime;

            var dateTime = new LocalDateTime(zonedDateTime.Year, zonedDateTime.Month, lastDay, 23, 59, 59, zonedDateTime.Calendar);
            var output = dateTime
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and represents the first moment of the current week.
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime GetStartOfWeek(this ZonedDateTime zonedDateTime)
        {
            var timezone = zonedDateTime.GetTimezone();
            var output = zonedDateTime
                .Plus(TimeSpan.FromDays(-zonedDateTime.GetDayInWeek()).ToDuration())
                .Date
                .AtStartOfDayInZone(timezone.Clock.Zone);
            return output;
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and represents the last moment of the current week.
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime GetEndOfWeek(this ZonedDateTime zonedDateTime)
        {
            var output = zonedDateTime.GetStartOfWeek().Plus(Duration.FromDays(6)).PlusHours(23).PlusMinutes(59).PlusSeconds(59);
            return output;
        }

        private static int GetDayInWeek(this ZonedDateTime zonedDateTime)
        {
            var timezone = zonedDateTime.Zone.GetTimezone();
            return (7 + (zonedDateTime.DayOfWeek.ToDayOfWeek() - timezone.Culture.DateTimeFormat.FirstDayOfWeek)) % 7;
        }

        /// <summary>
        ///     Returns a new <see cref="ZonedDateTime" /> that adds the specified number of years to the value of this instance.
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <param name="value">A number of years. The value parameter can be negative or positive.</param>
        /// <remarks>This API has slow performance.</remarks>
        public static ZonedDateTime PlusYears(this ZonedDateTime zonedDateTime, int value)
        {
            var output = zonedDateTime
                .LocalDateTime
                .PlusYears(value)
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new <see cref="ZonedDateTime" /> that adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <param name="value">A number of months. The value parameter can be negative or positive.</param>
        public static ZonedDateTime PlusMonths(this ZonedDateTime zonedDateTime, int value)
        {
            var output = zonedDateTime
                .LocalDateTime
                .PlusMonths(value)
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new <see cref="ZonedDateTime" /> that adds the specified number of days to the value of this instance.
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <param name="value">A number of days. The value parameter can be negative or positive.</param>
        public static ZonedDateTime PlusDays(this ZonedDateTime zonedDateTime, int value)
        {
            var output = zonedDateTime
                .LocalDateTime
                .PlusDays(value)
                .InZoneLeniently(zonedDateTime.Zone)
                .WithCalendar(zonedDateTime.Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a new object with the specified timezone.
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object</param>
        /// <param name="timezone">A timezone to be compared with the current data</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public static ZonedDateTime WithTimezone(this ZonedDateTime zonedDateTime, LocalTimezone timezone)
        {
            var output = zonedDateTime
                .WithZone(timezone.Clock.Zone)
                .WithCalendar(timezone.Calendar);
            return output;
        }

        /// <summary>
        ///     Converts the value of the current <see cref="ZonedDateTime" /> object to its equivalent string representation using
        ///     the 'G' format.
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <returns>A string representation of value of the current <see cref="ZonedDateTime" /> object as 'G' format.</returns>
        public static string ToOrdinalString(this ZonedDateTime zonedDateTime)
        {
            return zonedDateTime.ToOrdinalString("G");
        }

        /// <summary>
        ///     Converts the value of the current <see cref="ZonedDateTime" /> object to its equivalent string representation using
        ///     the specified format.
        /// </summary>
        /// <param name="zonedDateTime">A <see cref="ZonedDateTime" /> object.</param>
        /// <param name="format">A standard or custom date and time format string.</param>
        /// <returns>A string representation of value of the current <see cref="ZonedDateTime" /> object as specified by format.</returns>
        public static string ToOrdinalString(this ZonedDateTime zonedDateTime, string? format)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";

            var timezone = zonedDateTime.GetTimezone();
            var dateTime = zonedDateTime.ToDateTimeUnspecified();
            return dateTime.ToString(format, timezone.Culture);
        }
    }
}