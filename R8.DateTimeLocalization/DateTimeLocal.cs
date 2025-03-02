using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using NodaTime;
using NodaTime.Extensions;

namespace R8.DateTimeLocalization
{
    /// <summary>
    ///     Represents an instant in time, typically expressed as a date and time of day according to a particular calendar and
    ///     time zone.
    /// </summary>
    [JsonConverter(typeof(DateTimeLocalJsonConverter))]
    public readonly struct DateTimeLocal : IComparable, IComparable<DateTimeLocal>, IEquatable<DateTimeLocal>, IFormattable
    {
        private readonly ushort _timezone;

        /// <summary>
        ///     Represents an instant in time, typically expressed as a date and time of day according to a particular calendar and
        ///     time zone.
        /// </summary>
        public DateTimeLocal()
        {
            _timezone = 0;
            Ticks = 0;
        }

        private DateTimeLocal(long ticks, ITimezone timezone)
        {
            _timezone = LocalTimezone.Mappings.GetIndex(timezone.IanaId);
            Ticks = ticks;
        }

        private DateTimeLocal(DateTime utcDateTime, ushort timezone)
        {
            _timezone = timezone;
            Ticks = utcDateTime.Ticks;
        }

        /// <summary>
        ///     Represents an instant in time, typically expressed as a date and time of day according to a particular calendar and
        ///     time zone.
        /// </summary>
        /// <param name="utcDateTime">A <see cref="DateTime" /> object.</param>
        /// <param name="timezone">A <see cref="LocalTimezone" /> object.</param>
        public DateTimeLocal(DateTime utcDateTime, ITimezone timezone) :
            this(utcDateTime, LocalTimezone.Mappings.GetIndex(timezone.IanaId))
        {
        }

        private DateTimeLocal(ZonedDateTime zonedDateTime, ushort timezone)
        {
            _timezone = timezone;
            Ticks = zonedDateTime.ToDateTimeUtc().Ticks;
        }

        private DateTimeLocal(ZonedDateTime zonedDateTime) :
            this(zonedDateTime, LocalTimezone.Mappings.GetIndex(zonedDateTime.Zone.Id))
        {
            Ticks = zonedDateTime.ToDateTimeUtc().Ticks;
        }

        /// <summary>
        ///     Represents an instant in time, typically expressed as a date and time of day according to a particular calendar and
        ///     time zone.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
        /// <param name="hour">The hour (0 through 23).</param>
        /// <param name="minute">The minute (0 through 59).</param>
        /// <param name="second">The second (0 through 59).</param>
        /// <param name="timezone">A <see cref="LocalTimezone" /> object.</param>
        public DateTimeLocal(int year, int month, int day, int hour, int minute, int second, ITimezone timezone) :
            this(year, month, day, hour, minute, second, 0, timezone)
        {
        }

        /// <summary>
        ///     Represents an instant in time, typically expressed as a date and time of day according to a particular calendar and
        ///     time zone.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
        /// <param name="hour">The hour (0 through 23).</param>
        /// <param name="minute">The minute (0 through 59).</param>
        /// <param name="second">The second (0 through 59).</param>
        /// <param name="millisecond">The millisecond (0 through 999).</param>
        /// <param name="timezone">A <see cref="LocalTimezone" /> object.</param>
        public DateTimeLocal(int year, int month, int day, int hour, int minute, int second, int millisecond, ITimezone timezone)
        {
            _timezone = LocalTimezone.Mappings.GetIndex(timezone.IanaId);
            var local = new LocalDateTime(year, month, day, hour, minute, second, millisecond > 999 ? 999 : millisecond, timezone.Clock.Calendar);
            var zoned = local.InZoneLeniently(timezone.Clock.Zone).WithCalendar(timezone.Clock.Calendar);
            Ticks = zoned.ToDateTimeUtc().Ticks;
        }

        /// <summary>
        ///     Represents an instant in time, typically expressed as a date and time of day according to a particular calendar and
        ///     time zone.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
        /// <param name="timezone">A <see cref="LocalTimezone" /> object.</param>
        public DateTimeLocal(int year, int month, int day, ITimezone timezone)
            : this(year, month, day, 0, 0, 0, 0, timezone)
        {
        }

        /// <summary>
        ///     Gets the current date and time.
        /// </summary>
        public static DateTimeLocal Now => DateTime.UtcNow.ToDateTimeLocal(LocalTimezone.Current);

        /// <inheritdoc cref="DateTime.Ticks" />
        public long Ticks { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetDatePart(int part)
        {
            var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
            var zoned = _timezone > 0 ? dateTime.ToZonedDateTime(LocalTimezone.Mappings[_timezone]) : (ZonedDateTime?)null;
            return part switch
            {
                0 => (int)(zoned?.DayOfWeek.ToDayOfWeek() ?? dateTime.DayOfWeek),
                1 => zoned?.Year ?? dateTime.Year,
                2 => zoned?.Month ?? dateTime.Month,
                3 => zoned?.Day ?? dateTime.Day,
                4 => zoned?.Hour ?? dateTime.Hour,
                5 => zoned?.Minute ?? dateTime.Minute,
                6 => zoned?.Second ?? dateTime.Second,
                7 => zoned?.Millisecond ?? dateTime.Millisecond,
                _ => throw new ArgumentOutOfRangeException(nameof(part), "Invalid part")
            };
        }

        /// <inheritdoc cref="DateTime.Year" />
        public int Year => GetDatePart(1);

        /// <inheritdoc cref="DateTime.Month" />
        public int Month => GetDatePart(2);

        /// <inheritdoc cref="DateTime.Day" />
        public int Day => GetDatePart(3);

        /// <inheritdoc cref="DateTime.Hour" />
        public int Hour => GetDatePart(4);

        /// <inheritdoc cref="DateTime.Minute" />
        public int Minute => GetDatePart(5);

        /// <inheritdoc cref="DateTime.Second" />
        public int Second => GetDatePart(6);

        /// <inheritdoc cref="DateTime.Second" />
        public int Millisecond => GetDatePart(7);

        /// <inheritdoc cref="DateTime.DayOfWeek" />
        public DayOfWeek DayOfWeek => (DayOfWeek)GetDatePart(0);

        public static DateTimeLocal Empty => new();

        public DateTime GetUtcDateTime()
        {
            return new DateTime(Ticks, DateTimeKind.Utc);
        }

        public DateTime GetDateTime()
        {
            var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
            return dateTime.ToZonedDateTime(LocalTimezone.Mappings[_timezone]).ToDateTimeUnspecified();
        }

        /// <summary>
        ///     Returns the timezone of this instance.
        /// </summary>
        /// <returns>A <see cref="LocalTimezone" /> object</returns>
        /// <exception cref="InvalidOperationException">Thrown when timezone is not available.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITimezone GetTimezone()
        {
            return LocalTimezone.Mappings[_timezone].GetTimezone()!;
        }

        /// <summary>
        ///     Returns the number of days in the month and year of the specified date.
        /// </summary>
        /// <returns>An integer from 28 through 31.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetDaysInMonth()
        {
            return LocalTimezone.Mappings[_timezone].Clock.Calendar.GetDaysInMonth(Year, Month);
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the hour value set to 00:00. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 12:00:00".
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetStartOfHour()
        {
            return new DateTimeLocal(Year, Month, Day, Hour, 0, 0, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the hour value set to 23:59. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 23:59:59".
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetEndOfHour()
        {
            return new DateTimeLocal(Year, Month, Day, Hour, 59, 59, 9999999, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the minute value set to 00. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 12:33:00".
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetStartOfMinute()
        {
            return new DateTimeLocal(Year, Month, Day, Hour, Minute, 0, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the minute value set to 59. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 12:33:59".
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetEndOfMinute()
        {
            return new DateTimeLocal(Year, Month, Day, Hour, Minute, 59, 9999999, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the time value set to 00:00:00. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 00:00:00".
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        /// <remarks>This method works similar to <c>DateTime.Date</c></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetStartOfDay()
        {
            return new DateTimeLocal(Year, Month, Day, 0, 0, 0, GetTimezone());
        }

        /// <summary>
        ///     Returns a object for first moment of the next day.
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetStartOfNextDay()
        {
            var daysInMonth = GetDaysInMonth();
            var timezone = LocalTimezone.Mappings[_timezone].GetTimezone();
            if (Day == daysInMonth)
            {
                if (Month == 12) return new DateTimeLocal(Year + 1, 1, 1, 0, 0, 0, timezone);

                return new DateTimeLocal(Year, Month + 1, 1, 0, 0, 0, timezone);
            }

            return new DateTimeLocal(Year, Month, Day + 1, 0, 0, 0, timezone);
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the time value set to 23:59:59. For instance, if the
        ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
        ///     "2019-01-01 23:59:59".
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetEndOfDay()
        {
            return new DateTimeLocal(Year, Month, Day, 23, 59, 59, 9999999, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and the day value set to 1. For instance, if the current instance
        ///     represents the date "2019-01-15 12:33:25", this method will return a new instance representing "2019-01-01
        ///     00:00:00".
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetStartOfMonth()
        {
            return new DateTimeLocal(Year, Month, 1, 0, 0, 0, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and the day value set to the last day of the month. For instance, if
        ///     the current instance represents the date "2019-01-15 12:33:25", this method will return a new instance representing
        ///     "2019-01-31 23:59:59".
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetEndOfMonth()
        {
            return new DateTimeLocal(Year, Month, GetDaysInMonth(), 23, 59, 59, 9999999, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object of the first moment of the next month.
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetStartOfNextMonth()
        {
            var timezone = GetTimezone();
            if (Month == 12)
                return new DateTimeLocal(Year + 1, 1, 1, 0, 0, 0, timezone);

            return new DateTimeLocal(Year, Month + 1, 1, 0, 0, 0, timezone);
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and represents the first moment of the current week.
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetStartOfWeek()
        {
            var firstDayOfWeek = GetFirstDayOfWeek(out var timezone);
            return new DateTimeLocal(firstDayOfWeek.Year, firstDayOfWeek.Month, firstDayOfWeek.Day, 0, 0, 0, 0, timezone.GetTimezone()!);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ZonedDateTime GetFirstDayOfWeek(out LocalTimezoneInfo timezone)
        {
            const int firstDayIndex = 0; // 0-based index = 0 | 1-based index = 1
            const int lastDayIndex = 6; // 0-based index = 6 | 1-based index = 7

            timezone = LocalTimezone.Mappings[_timezone];

            var dayInWeek = (lastDayIndex + 1 + (DayOfWeek - timezone.Culture.DateTimeFormat.FirstDayOfWeek)) % (lastDayIndex + 1);
            var zonedDateTime = new DateTime(Ticks, DateTimeKind.Utc).ToZonedDateTime(timezone);
            var firstDayOfWeek = dayInWeek switch
            {
                firstDayIndex => zonedDateTime,
                lastDayIndex => zonedDateTime.Minus(Duration.FromDays(lastDayIndex)),
                _ => zonedDateTime.Minus(Duration.FromDays(lastDayIndex - dayInWeek))
            };
            return firstDayOfWeek;
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and represents the last moment of the current week.
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetEndOfWeek()
        {
            const int lastDayIndex = 6;

            var firstDayOfWeek = GetFirstDayOfWeek(out var timezone);
            var lastDayOfWeek = firstDayOfWeek.Plus(Duration.FromDays(lastDayIndex));
            return new DateTimeLocal(lastDayOfWeek.Year, lastDayOfWeek.Month, lastDayOfWeek.Day, 23, 59, 59, 9999999, timezone.GetTimezone()!);
        }

        /// <summary>
        ///     Returns a new object of the first moment of the next week.
        /// </summary>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal GetStartOfNextWeek()
        {
            var timezone = LocalTimezone.Mappings[_timezone].GetTimezone();
            var daysInMonth = GetDaysInMonth();
            var sumDays = Day + 7;
            if (sumDays > daysInMonth)
            {
                var daysToNextMonth = sumDays - daysInMonth;
                var nextMonth = Month + 1;
                if (nextMonth == 13)
                    return new DateTimeLocal(Year + 1, 1, daysToNextMonth, 0, 0, 0, timezone);

                return new DateTimeLocal(Year, Month + 1, daysToNextMonth, 0, 0, 0, timezone);
            }

            return new DateTimeLocal(Year, Month, sumDays, 0, 0, 0, timezone);
        }

        /// <summary>
        ///     Returns a new <see cref="DateTimeLocal" /> that adds the specified number of years to the value of this
        ///     instance.
        /// </summary>
        /// <param name="value">A number of years. The value parameter can be negative or positive.</param>
        /// <remarks>This API has slow performance.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal AddYears(int value)
        {
            var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
            if (_timezone == 0)
                return new DateTimeLocal(dateTime.AddYears(value), _timezone);

            var timezone = LocalTimezone.Mappings[_timezone];
            var zoned = dateTime.ToZonedDateTime(timezone).PlusYears(value);
            return new DateTimeLocal(zoned, _timezone);
        }

        /// <summary>
        ///     Returns a new <see cref="DateTimeLocal" /> that adds the specified number of months to the value of this
        ///     instance.
        /// </summary>
        /// <param name="value">A number of months. The value parameter can be negative or positive.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal AddMonths(int value)
        {
            var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
            if (_timezone == 0)
                return new DateTimeLocal(dateTime.AddMonths(value), _timezone);

            var timezone = LocalTimezone.Mappings[_timezone];
            var zoned = dateTime.ToZonedDateTime(timezone).PlusMonths(value);
            return new DateTimeLocal(zoned, _timezone);
        }

        /// <summary>
        ///     Returns a new <see cref="DateTimeLocal" /> that adds the specified number of days to the value of this instance.
        /// </summary>
        /// <param name="value">A number of days. The value parameter can be negative or positive.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal AddDays(int value)
        {
            var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
            if (_timezone == 0)
                return new DateTimeLocal(dateTime.AddDays(value), _timezone);

            var timezone = LocalTimezone.Mappings[_timezone];
            var zoned = dateTime.ToZonedDateTime(timezone).PlusDays(value);
            return new DateTimeLocal(zoned, _timezone);
        }

        /// <summary>
        ///     Returns a new <see cref="DateTimeLocal" /> that adds the specified number of hours to the value of this
        ///     instance.
        /// </summary>
        /// <param name="value">A number of hours. The value parameter can be negative or positive.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal AddHours(int value)
        {
            var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
            return new DateTimeLocal(dateTime.AddHours(value), _timezone);
        }

        /// <summary>
        ///     Returns a new <see cref="DateTimeLocal" /> that adds the specified number of minutes to the value of this
        ///     instance.
        /// </summary>
        /// <param name="value">A number of minutes. The value parameter can be negative or positive.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal AddMinutes(int value)
        {
            var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
            return new DateTimeLocal(dateTime.AddMinutes(value), _timezone);
        }

        /// <summary>
        ///     Returns a new <see cref="DateTimeLocal" /> that adds the specified number of seconds to the value of this
        ///     instance.
        /// </summary>
        /// <param name="value">A number of seconds. The value parameter can be negative or positive.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal AddSeconds(int value)
        {
            var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
            return new DateTimeLocal(dateTime.AddSeconds(value), _timezone);
        }

        /// <summary>
        ///     Returns a new object with the specified timezone.
        /// </summary>
        /// <param name="timezone">A timezone to be compared with the current data</param>
        /// <returns>A <see cref="DateTimeLocal" /> object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeLocal WithTimezone(LocalTimezone timezone)
        {
            return new DateTimeLocal(Ticks, timezone);
        }

        public override string ToString()
        {
            return ToString("G");
        }

        public string ToString(string? format, IFormatProvider? formatProvider = null)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";

            var timezone = LocalTimezone.Mappings[_timezone];
            var culture = formatProvider as CultureInfo ?? timezone.Culture;

            var localDateTime = new LocalDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond, timezone.Clock.Calendar);
            var zonedDateTime = localDateTime.InZoneLeniently(timezone.Clock.Zone).WithCalendar(timezone.Clock.Calendar);
            var dateTime = zonedDateTime.ToDateTimeUnspecified();
            var str = format.Equals("dddddT", StringComparison.InvariantCulture)
                ? $"{dateTime.ToString("dddd", culture)} {dateTime.ToString("d", culture)} — {dateTime.ToString("T", culture)}"
                : dateTime.ToString(format, culture);
            if (culture.TextInfo.IsRightToLeft)
                str = str.RemoveRlmChar();
            return str;
        }

        public static int Compare(DateTimeLocal left, DateTimeLocal right)
        {
            var l = left.Ticks;
            var r = right.Ticks;

            if (l > r)
                return 1;
            if (l < r)
                return -1;
            return 0;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
                return 1;
            if (obj is DateTimeLocal tz)
                return Compare(this, tz);
            throw new ArgumentException("Argument must be DateTimeLocal");
        }

        public int CompareTo(DateTimeLocal value)
        {
            return Compare(this, value);
        }

        public static bool operator >(DateTimeLocal a, DateTimeLocal b)
        {
            return a.CompareTo(b) > 0;
        }

        public DateTimeLocal Add(TimeSpan timeSpan)
        {
            var timezone = LocalTimezone.Mappings[_timezone];
            var zoned = new DateTime(Ticks, DateTimeKind.Utc).ToZonedDateTime(timezone).Plus(timeSpan.ToDuration());
            return new DateTimeLocal(zoned, _timezone);
        }

        public DateTimeLocal Subtract(TimeSpan timeSpan)
        {
            var timezone = LocalTimezone.Mappings[_timezone];
            var zoned = new DateTime(Ticks, DateTimeKind.Utc).ToZonedDateTime(timezone).Minus(timeSpan.ToDuration());
            return new DateTimeLocal(zoned);
        }

        public TimeSpan Subtract(DateTimeLocal dateTimeLocal)
        {
            return GetUtcDateTime().Subtract(dateTimeLocal.GetUtcDateTime());
        }

        public static TimeSpan operator -(DateTimeLocal a, DateTimeLocal b)
        {
            return a.Subtract(b);
        }

        public static DateTimeLocal operator +(DateTimeLocal d, TimeSpan t)
        {
            return d.Add(t);
        }

        public static bool operator <(DateTimeLocal a, DateTimeLocal b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >=(DateTimeLocal a, DateTimeLocal b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static bool operator <=(DateTimeLocal a, DateTimeLocal b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool Equals(DateTimeLocal left, DateTimeLocal right)
        {
            return left.CompareTo(right) == 0;
        }

        public static bool operator ==(DateTimeLocal a, DateTimeLocal b)
        {
            return Equals(a, b);
        }

        public bool Equals(DateTimeLocal other)
        {
            return Equals(this, other);
        }

        public override bool Equals(object? obj)
        {
            if (obj is DateTimeLocal tz)
                return Equals(this, tz);
            return false;
        }

        public static bool operator !=(DateTimeLocal a, DateTimeLocal b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return GetUtcDateTime().GetHashCode();
        }

        /// <summary>
        ///     Converts a memory span that contains string representation of a date and time to its
        ///     <see cref="DateTimeLocal" /> equivalent by using culture-specific format information and a formatting style.
        /// </summary>
        /// <param name="s">The memory span that contains the string to parse. See The string to parse for more information.</param>
        /// <param name="timezone">A <see cref="LocalTimezone" /> object.</param>
        /// <returns>A <see cref="DateTimeLocal" /> object equivalent to the date and time contained in <paramref name="s" />.</returns>
        public static DateTimeLocal Parse(ReadOnlySpan<char> s, LocalTimezone timezone)
        {
            var dateTime = DateTime.Parse(s, timezone.Culture.DateTimeFormat);
            return new DateTimeLocal(dateTime, timezone);
        }

        /// <summary>
        ///     Returns a string that represents the current <see cref="DateTimeLocal" /> instance in a relative time format.
        /// </summary>
        /// <param name="compareAgainst">
        ///     A <see cref="DateTime" /> object to compare against. If null, the current time will be
        ///     used.
        /// </param>
        /// <param name="maxRelativity">The maximum time span to consider as relative. If null, all time spans will be considered.</param>
        /// <param name="format">The format to use when the time is not relative.</param>
        /// <exception cref="ArgumentException">When <paramref name="compareAgainst" /> is not UTC.</exception>
        /// <returns>A string that represents the current <see cref="DateTimeLocal" /> instance in a relative time format.</returns>
        public string Humanize(DateTime? compareAgainst = null, TimeSpan? maxRelativity = null, string format = "G")
        {
            if (compareAgainst.HasValue && compareAgainst.Value.Kind != DateTimeKind.Utc)
                throw new ArgumentException("compareAgainst must be UTC", nameof(compareAgainst));

            var compare = new DateTimeLocal(compareAgainst ?? DateTime.UtcNow, GetTimezone());
            if (compare < this)
            {
                var duration = this - compare;

                if (duration.TotalSeconds < 60)
                    return "in a few seconds";
                if (duration.TotalMinutes < 60)
                    return $"in {duration.Minutes} minutes";
                if (duration.TotalHours < 6)
                    return $"in {duration.Hours} hours";
                if (duration.TotalHours < 24)
                    return "tomorrow";
                if (duration.TotalDays < 7)
                    return "soon";
                if (duration.TotalDays < 30)
                    return "next month";
                if (duration.TotalDays < 365)
                    return "in future";
            }
            else
            {
                var duration = compare - this;
                if (maxRelativity == null || duration <= maxRelativity)
                {
                    var hours = (int)duration.TotalHours;
                    if (hours <= 1)
                    {
                        var minutes = (int)duration.TotalMinutes;
                        if (minutes < 60)
                        {
                            var seconds = (int)duration.TotalSeconds;
                            if (seconds <= 30)
                                return "just now";

                            if (seconds <= 60)
                                return "a few seconds ago";

                            if (minutes <= 10)
                                return "a few minutes ago";

                            return $"{minutes} minutes ago";
                        }

                        return "an hour ago";
                    }

                    if (hours <= 5)
                        return $"{hours} hours ago";

                    if (hours <= 12)
                        return "a few hours ago";

                    if (Year == compare.Year && Month == compare.Month)
                    {
                        if (Day == compare.Day)
                            return $"today at {this:hh:mm tt}";

                        if (Day == compare.Day - 1)
                            return $"yesterday at {this:hh:mm tt}";
                    }

                    if (Year == compare.Year)
                    {
                        if (Month == compare.Month)
                        {
                            var days = (int)duration.TotalDays;
                            var weeks = days / 7;
                            if (weeks == 0)
                            {
                                if (days <= 3)
                                    return $"{days} days ago";

                                return "a few days ago";
                            }

                            if (weeks == 1)
                                return "last week";

                            return "a few weeks ago";
                        }

                        if (Month == compare.Month - 1)
                            return "last month";

                        var months = compare.Month - Month;
                        if (months is > 0 and <= 12)
                            return $"{months} months ago";
                    }
                    else
                    {
                        var months = (int)(duration.TotalDays / 30);
                        if (months < 1)
                        {
                            var days = (int)duration.TotalDays;
                            var weeks = days / 7;
                            if (weeks == 0)
                            {
                                if (days <= 3)
                                    return $"{days} days ago";

                                return "a few days ago";
                            }

                            if (weeks == 1)
                                return "last week";

                            return "a few weeks ago";
                        }

                        if (months == 1)
                            return "last month";

                        if (months <= 12)
                        {
                            if (months >= 6)
                                if (Year == compare.Year - 1)
                                    return "last year";

                            return $"{months} months ago";
                        }
                    }

                    if (Year == compare.Year - 1)
                        return "last year";
                }
            }

            return ToString(format);
        }

        /// <summary>
        ///     Checks if the current instance is today.
        /// </summary>
        public bool IsToday()
        {
            var now = Now;
            return Year == now.Year && Month == now.Month && Day == now.Day;
        }
    }

    public class DateTimeLocalJsonConverter : JsonConverter<DateTimeLocal>
    {
        public override DateTimeLocal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            if (string.IsNullOrEmpty(str))
                return DateTimeLocal.Empty;

            var datetime = DateTime.Parse(str, CultureInfo.InvariantCulture).WithKind(DateTimeKind.Utc);
            return new DateTimeLocal(datetime, LocalTimezone.Utc);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeLocal value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.GetUtcDateTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }
}