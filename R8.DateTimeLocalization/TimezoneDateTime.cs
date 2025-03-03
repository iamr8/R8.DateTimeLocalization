using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using NodaTime;
using NodaTime.Extensions;

namespace R8.DateTimeLocalization;

/// <summary>
///     Represents an instant in time, typically expressed as a date and time of day according to a particular calendar and
///     time zone.
/// </summary>
[JsonConverter(typeof(DateTimeLocalJsonConverter))]
public readonly struct TimezoneDateTime : IComparable, IComparable<TimezoneDateTime>, IEquatable<TimezoneDateTime>, IFormattable
{
    private readonly ushort _timezone;

    /// <summary>
    ///     Represents an instant in time, typically expressed as a date and time of day according to a particular calendar and
    ///     time zone.
    /// </summary>
    public TimezoneDateTime()
    {
        _timezone = 0;
        Ticks = 0;
    }

    private TimezoneDateTime(long ticks, ITimezone timezone)
    {
        _timezone = LocalTimezone.Mappings.GetIndex(timezone.IanaId);
        Ticks = ticks;
    }

    private TimezoneDateTime(DateTime utcDateTime, ushort timezone)
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
    public TimezoneDateTime(DateTime utcDateTime, ITimezone timezone) :
        this(utcDateTime, LocalTimezone.Mappings.GetIndex(timezone.IanaId))
    {
    }

    private TimezoneDateTime(ZonedDateTime zonedDateTime, ushort timezone)
    {
        _timezone = timezone;
        Ticks = zonedDateTime.ToDateTimeUtc().Ticks;
    }

    public TimezoneDateTime(ZonedDateTime zonedDateTime) :
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
    public TimezoneDateTime(int year, int month, int day, int hour, int minute, int second, ITimezone timezone) :
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
    public TimezoneDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, ITimezone timezone)
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
    public TimezoneDateTime(int year, int month, int day, ITimezone timezone)
        : this(year, month, day, 0, 0, 0, 0, timezone)
    {
    }

    /// <summary>
    ///     Gets the current date and time.
    /// </summary>
    public static TimezoneDateTime Now => new(DateTime.UtcNow, LocalTimezone.Current);

    /// <inheritdoc cref="DateTime.Ticks" />
    public long Ticks { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetDatePart(int part)
    {
        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(LocalTimezone.Mappings[_timezone].Clock.Zone, LocalTimezone.Mappings[_timezone].Clock.Calendar) : (ZonedDateTime?)null;
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

    public static TimezoneDateTime Empty => new();

    public DateTime GetUtcDateTime()
    {
        return new DateTime(Ticks, DateTimeKind.Utc);
    }

    public DateTime GetDateTime()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var utcDateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var localDateTime = Instant.FromDateTimeUtc(utcDateTime)
            .InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar)
            .ToDateTimeUnspecified();
        return localDateTime;
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
        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(LocalTimezone.Mappings[_timezone].Clock.Zone, LocalTimezone.Mappings[_timezone].Clock.Calendar) : (ZonedDateTime?)null;
        var currentYear = zoned?.Year ?? dateTime.Year;
        var currentMonth = zoned?.Month ?? dateTime.Month;

        return LocalTimezone.Mappings[_timezone].Clock.Calendar.GetDaysInMonth(currentYear, currentMonth);
    }

    /// <summary>
    ///     Returns a new object with the same date as this instance, and the hour value set to 00:00. For instance, if the
    ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
    ///     "2019-01-01 12:00:00".
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetStartOfHour()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentDay = zoned?.Day ?? dateTime.Day;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;
        var currentHour = zoned?.Hour ?? dateTime.Hour;

        return new TimezoneDateTime(currentYear, currentMonth, currentDay, currentHour, 0, 0, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new object with the same date as this instance, and the hour value set to 23:59. For instance, if the
    ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
    ///     "2019-01-01 23:59:59".
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetEndOfHour()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentDay = zoned?.Day ?? dateTime.Day;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;
        var currentHour = zoned?.Hour ?? dateTime.Hour;

        return new TimezoneDateTime(currentYear, currentMonth, currentDay, currentHour, 59, 59, 9999999, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new object with the same date as this instance, and the minute value set to 00. For instance, if the
    ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
    ///     "2019-01-01 12:33:00".
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetStartOfMinute()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentDay = zoned?.Day ?? dateTime.Day;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;
        var currentHour = zoned?.Hour ?? dateTime.Hour;
        var currentMinute = zoned?.Minute ?? dateTime.Minute;

        return new TimezoneDateTime(currentYear, currentMonth, currentDay, currentHour, currentMinute, 0, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new object with the same date as this instance, and the minute value set to 59. For instance, if the
    ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
    ///     "2019-01-01 12:33:59".
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetEndOfMinute()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentDay = zoned?.Day ?? dateTime.Day;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;
        var currentHour = zoned?.Hour ?? dateTime.Hour;
        var currentMinute = zoned?.Minute ?? dateTime.Minute;

        return new TimezoneDateTime(currentYear, currentMonth, currentDay, currentHour, currentMinute, 59, 9999999, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new object with the same date as this instance, and the time value set to 00:00:00. For instance, if the
    ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
    ///     "2019-01-01 00:00:00".
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    /// <remarks>This method works similar to <c>DateTime.Date</c></remarks>
    public TimezoneDateTime GetStartOfDay()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentDay = zoned?.Day ?? dateTime.Day;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;

        return new TimezoneDateTime(currentYear, currentMonth, currentDay, 0, 0, 0, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a object for first moment of the next day.
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetStartOfNextDay()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentDay = zoned?.Day ?? dateTime.Day;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;
        var daysInMonth = timezoneInfo.Clock.Calendar.GetDaysInMonth(currentYear, currentMonth);
        if (currentDay == daysInMonth)
        {
            if (currentMonth == 12) return new TimezoneDateTime(currentYear + 1, 1, 1, 0, 0, 0, timezoneInfo.GetTimezone()!);

            return new TimezoneDateTime(currentYear, currentMonth + 1, 1, 0, 0, 0, timezoneInfo.GetTimezone()!);
        }

        return new TimezoneDateTime(currentYear, currentMonth, currentDay + 1, 0, 0, 0, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new object with the same date as this instance, and the time value set to 23:59:59. For instance, if the
    ///     current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing
    ///     "2019-01-01 23:59:59".
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetEndOfDay()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentDay = zoned?.Day ?? dateTime.Day;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;

        return new TimezoneDateTime(currentYear, currentMonth, currentDay, 23, 59, 59, 9999999, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new object similar to this instance, and the day value set to 1. For instance, if the current instance
    ///     represents the date "2019-01-15 12:33:25", this method will return a new instance representing "2019-01-01
    ///     00:00:00".
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetStartOfMonth()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;

        return new TimezoneDateTime(currentYear, currentMonth, 1, 0, 0, 0, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new object similar to this instance, and the day value set to the last day of the month. For instance, if
    ///     the current instance represents the date "2019-01-15 12:33:25", this method will return a new instance representing
    ///     "2019-01-31 23:59:59".
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetEndOfMonth()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;
        var daysInMonth = timezoneInfo.Clock.Calendar.GetDaysInMonth(currentYear, currentMonth);

        return new TimezoneDateTime(currentYear, currentMonth, daysInMonth, 23, 59, 59, 9999999, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new object of the first moment of the next month.
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetStartOfNextMonth()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;
        if (currentMonth == 12)
            return new TimezoneDateTime(currentYear + 1, 1, 1, 0, 0, 0, timezoneInfo.GetTimezone()!);

        return new TimezoneDateTime(currentYear, currentMonth + 1, 1, 0, 0, 0, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new object similar to this instance, and represents the first moment of the current week.
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetStartOfWeek()
    {
        var firstDayOfWeek = GetFirstDayOfWeek(out var timezoneInfo);
        return new TimezoneDateTime(firstDayOfWeek.Year, firstDayOfWeek.Month, firstDayOfWeek.Day, 0, 0, 0, 0, timezoneInfo.GetTimezone()!);
    }

    private ZonedDateTime GetFirstDayOfWeek(out LocalTimezoneInfo timezoneInfo)
    {
        const int firstDayIndex = 0; // 0-based index = 0 | 1-based index = 1
        const int lastDayIndex = 6; // 0-based index = 6 | 1-based index = 7

        timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar);
        var currentDayOfWeek = zoned.DayOfWeek.ToDayOfWeek();
        var dayInWeek = (lastDayIndex + 1 + (currentDayOfWeek - timezoneInfo.Culture.DateTimeFormat.FirstDayOfWeek)) % (lastDayIndex + 1);
        var firstDayOfWeek = dayInWeek switch
        {
            firstDayIndex => zoned,
            lastDayIndex => zoned.Minus(Duration.FromDays(lastDayIndex)),
            _ => zoned.Minus(Duration.FromDays(lastDayIndex - dayInWeek))
        };
        return firstDayOfWeek;
    }

    /// <summary>
    ///     Returns a new object similar to this instance, and represents the last moment of the current week.
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetEndOfWeek()
    {
        const int lastDayIndex = 6;

        var firstDayOfWeek = GetFirstDayOfWeek(out var timezone);
        var lastDayOfWeek = firstDayOfWeek.Plus(Duration.FromDays(lastDayIndex));
        return new TimezoneDateTime(lastDayOfWeek.Year, lastDayOfWeek.Month, lastDayOfWeek.Day, 23, 59, 59, 9999999, timezone.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new object of the first moment of the next week.
    /// </summary>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    public TimezoneDateTime GetStartOfNextWeek()
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
        var currentDayOfWeek = zoned?.DayOfWeek.ToDayOfWeek() ?? dateTime.DayOfWeek;
        var currentDay = zoned?.Day ?? dateTime.Day;
        var currentMonth = zoned?.Month ?? dateTime.Month;
        var currentYear = zoned?.Year ?? dateTime.Year;
        var daysInMonth = timezoneInfo.Clock.Calendar.GetDaysInMonth(currentYear, currentMonth);

        var dayNoInWeek = timezoneInfo.GetDayNumberInWeek(currentDayOfWeek);
        var remainedDaysOfCurrentWeek = 6 - dayNoInWeek;
        var firstDayOfNextWeek = remainedDaysOfCurrentWeek + currentDay + 1;
        if (firstDayOfNextWeek > daysInMonth)
        {
            var dayInNextMonth = firstDayOfNextWeek - daysInMonth;
            var nextMonth = currentMonth + 1;
            if (nextMonth == 13)
                return new TimezoneDateTime(currentYear + 1, 1, dayInNextMonth, 0, 0, 0, timezoneInfo.GetTimezone()!);

            return new TimezoneDateTime(currentYear, currentMonth + 1, dayInNextMonth, 0, 0, 0, timezoneInfo.GetTimezone()!);
        }

        return new TimezoneDateTime(currentYear, currentMonth, firstDayOfNextWeek, 0, 0, 0, timezoneInfo.GetTimezone()!);
    }

    /// <summary>
    ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of years to the value of this
    ///     instance.
    /// </summary>
    /// <param name="value">A number of years. The value parameter can be negative or positive.</param>
    /// <remarks>This API has slow performance.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimezoneDateTime AddYears(int value)
    {
        if (value == 0)
            return this;

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        if (_timezone == 0)
            return new TimezoneDateTime(dateTime.AddYears(value), _timezone);

        var timezoneInfo = LocalTimezone.Mappings[_timezone];
        var zoned = Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar);
        zoned = zoned.Date.PlusYears(value).At(zoned.TimeOfDay).InZoneLeniently(zoned.Zone).WithCalendar(zoned.Calendar);
        return new TimezoneDateTime(zoned, _timezone);
    }

    /// <summary>
    ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of months to the value of this
    ///     instance.
    /// </summary>
    /// <param name="value">A number of months. The value parameter can be negative or positive.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimezoneDateTime AddMonths(int value)
    {
        if (value == 0)
            return this;

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        if (_timezone == 0)
            return new TimezoneDateTime(dateTime.AddMonths(value), _timezone);

        var timezoneInfo = LocalTimezone.Mappings[_timezone];
        var zoned = Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar);
        zoned = zoned.Date.PlusMonths(value).At(zoned.TimeOfDay).InZoneLeniently(zoned.Zone).WithCalendar(zoned.Calendar);
        return new TimezoneDateTime(zoned, _timezone);
    }

    /// <summary>
    ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of days to the value of this instance.
    /// </summary>
    /// <param name="value">A number of days. The value parameter can be negative or positive.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimezoneDateTime AddDays(int value)
    {
        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        if (_timezone == 0)
            return new TimezoneDateTime(dateTime.AddDays(value), _timezone);

        var timezoneInfo = LocalTimezone.Mappings[_timezone];
        var zoned = Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar);
        zoned = zoned.Date.PlusDays(value).At(zoned.TimeOfDay).InZoneLeniently(zoned.Zone).WithCalendar(zoned.Calendar);
        return new TimezoneDateTime(zoned, _timezone);
    }

    /// <summary>
    ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of hours to the value of this
    ///     instance.
    /// </summary>
    /// <param name="value">A number of hours. The value parameter can be negative or positive.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimezoneDateTime AddHours(int value)
    {
        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        return new TimezoneDateTime(dateTime.AddHours(value), _timezone);
    }

    /// <summary>
    ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of minutes to the value of this
    ///     instance.
    /// </summary>
    /// <param name="value">A number of minutes. The value parameter can be negative or positive.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimezoneDateTime AddMinutes(int value)
    {
        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        return new TimezoneDateTime(dateTime.AddMinutes(value), _timezone);
    }

    /// <summary>
    ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of seconds to the value of this
    ///     instance.
    /// </summary>
    /// <param name="value">A number of seconds. The value parameter can be negative or positive.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimezoneDateTime AddSeconds(int value)
    {
        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        return new TimezoneDateTime(dateTime.AddSeconds(value), _timezone);
    }

    /// <summary>
    ///     Returns a new object with the specified timezone.
    /// </summary>
    /// <param name="timezone">A timezone to be compared with the current data</param>
    /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TimezoneDateTime WithTimezone(LocalTimezone timezone)
    {
        return new TimezoneDateTime(Ticks, timezone);
    }

    public override string ToString()
    {
        return ToString("G");
    }

    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        if (string.IsNullOrEmpty(format))
            format = "G";

        var timezoneInfo = LocalTimezone.Mappings[_timezone];
        var culture = formatProvider as CultureInfo ?? timezoneInfo.Culture;

        var utcDateTime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = _timezone > 0 ? Instant.FromDateTimeUtc(utcDateTime).InZone(LocalTimezone.Mappings[_timezone].Clock.Zone, LocalTimezone.Mappings[_timezone].Clock.Calendar) : (ZonedDateTime?)null;
        var currentYear = zoned?.Year ?? utcDateTime.Year;
        var currentMonth = zoned?.Month ?? utcDateTime.Month;
        var currentDay = zoned?.Day ?? utcDateTime.Day;
        var currentHour = zoned?.Hour ?? utcDateTime.Hour;
        var currentMinute = zoned?.Minute ?? utcDateTime.Minute;
        var currentSecond = zoned?.Second ?? utcDateTime.Second;
        var currentMillisecond = zoned?.Millisecond ?? utcDateTime.Millisecond;

        var localDateTime = new LocalDateTime(currentYear, currentMonth, currentDay, currentHour, currentMinute, currentSecond, currentMillisecond, timezoneInfo.Clock.Calendar);
        var zonedDateTime = localDateTime.InZoneLeniently(timezoneInfo.Clock.Zone).WithCalendar(timezoneInfo.Clock.Calendar);
        var dateTime = zonedDateTime.ToDateTimeUnspecified();
        var str = format.Equals("dddddT", StringComparison.InvariantCulture)
            ? $"{dateTime.ToString("dddd", culture)} {dateTime.ToString("d", culture)} — {dateTime.ToString("T", culture)}"
            : dateTime.ToString(format, culture);
        if (culture.TextInfo.IsRightToLeft)
            str = str.RemoveRlmChar();
        return str;
    }

    public static int Compare(TimezoneDateTime left, TimezoneDateTime right)
    {
        if (left._timezone != right._timezone)
            throw new ArgumentException($"Cannot compare {nameof(TimezoneDateTime)} with different timezones");

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
        if (obj is TimezoneDateTime tz)
            return Compare(this, tz);
        throw new ArgumentException("Argument must be DateTimeLocal");
    }

    public int CompareTo(TimezoneDateTime value)
    {
        return Compare(this, value);
    }

    public static bool operator >(TimezoneDateTime a, TimezoneDateTime b)
    {
        return a.CompareTo(b) > 0;
    }

    public TimezoneDateTime Add(TimeSpan timeSpan)
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];
        var datetime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = Instant.FromDateTimeUtc(datetime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar).Plus(timeSpan.ToDuration());
        return new TimezoneDateTime(zoned, _timezone);
    }

    public TimezoneDateTime Subtract(TimeSpan timeSpan)
    {
        var timezoneInfo = LocalTimezone.Mappings[_timezone];
        var datetime = new DateTime(Ticks, DateTimeKind.Utc);
        var zoned = Instant.FromDateTimeUtc(datetime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar).Minus(timeSpan.ToDuration());
        return new TimezoneDateTime(zoned, _timezone);
    }

    public TimeSpan Subtract(TimezoneDateTime timezoneDateTime)
    {
        return GetUtcDateTime().Subtract(timezoneDateTime.GetUtcDateTime());
    }

    public static TimeSpan operator -(TimezoneDateTime a, TimezoneDateTime b)
    {
        return a.Subtract(b);
    }

    public static TimezoneDateTime operator +(TimezoneDateTime d, TimeSpan t)
    {
        return d.Add(t);
    }

    public static bool operator <(TimezoneDateTime a, TimezoneDateTime b)
    {
        return a.CompareTo(b) < 0;
    }

    public static bool operator >=(TimezoneDateTime a, TimezoneDateTime b)
    {
        return a.CompareTo(b) >= 0;
    }

    public static bool operator <=(TimezoneDateTime a, TimezoneDateTime b)
    {
        return a.CompareTo(b) <= 0;
    }

    public static bool Equals(TimezoneDateTime left, TimezoneDateTime right)
    {
        return left.CompareTo(right) == 0;
    }

    public static bool operator ==(TimezoneDateTime a, TimezoneDateTime b)
    {
        return Equals(a, b);
    }

    public bool Equals(TimezoneDateTime other)
    {
        return Equals(this, other);
    }

    public override bool Equals(object? obj)
    {
        if (obj is TimezoneDateTime tz)
            return Equals(this, tz);
        return false;
    }

    public static bool operator !=(TimezoneDateTime a, TimezoneDateTime b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return Ticks.GetHashCode();
    }

    /// <summary>
    ///     Converts a memory span that contains string representation of a date and time to its
    ///     <see cref="TimezoneDateTime" /> equivalent by using culture-specific format information and a formatting style.
    /// </summary>
    /// <param name="s">The memory span that contains the string to parse. See The string to parse for more information.</param>
    /// <param name="timezone">A <see cref="LocalTimezone" /> object.</param>
    /// <returns>A <see cref="TimezoneDateTime" /> object equivalent to the date and time contained in <paramref name="s" />.</returns>
    public static TimezoneDateTime Parse(ReadOnlySpan<char> s, LocalTimezone timezone)
    {
        var dateTime = DateTime.Parse(s, timezone.Culture.DateTimeFormat);
        return new TimezoneDateTime(dateTime, timezone);
    }

    /// <summary>
    ///     Converts a <see cref="DateTime" /> to a <see cref="TimezoneDateTime" />.
    /// </summary>
    /// <param name="dateTime">A <see cref="DateTime" /> to convert.</param>
    /// <param name="timezone">A <see cref="LocalTimezone" /> to convert.</param>
    /// <returns>A <see cref="TimezoneDateTime" />.</returns>
    public static TimezoneDateTime FromDateTime(DateTime dateTime, LocalTimezone timezone)
    {
        return new TimezoneDateTime(dateTime, timezone);
    }

    /// <summary>
    ///     Returns a string that represents the current <see cref="TimezoneDateTime" /> instance in a relative time format.
    /// </summary>
    /// <param name="compareAgainst">
    ///     A <see cref="TimezoneDateTime" /> object to be compared as Current Datetime. If null, <c>DateTime.UtcNow</c> will
    ///     be used.
    /// </param>
    /// <param name="maxRelativity">The maximum time span to consider as relative. If null, all time spans will be considered.</param>
    /// <param name="format">The format to use when the time is not relative.</param>
    /// <exception cref="ArgumentException">When <paramref name="compareAgainst" /> is not UTC.</exception>
    /// <returns>A string that represents the current <see cref="TimezoneDateTime" /> instance in a relative time format.</returns>
    public string Humanize(TimezoneDateTime? compareAgainst = null, TimeSpan? maxRelativity = null, string format = "G")
    {
        return Humanize(compareAgainst?.GetUtcDateTime(), maxRelativity, format);
    }

    /// <summary>
    ///     Returns a string that represents the current <see cref="TimezoneDateTime" /> instance in a relative time format.
    /// </summary>
    /// <param name="compareAgainst">
    ///     A <see cref="DateTime" /> object to be compared as Current Datetime. If null, <c>DateTime.UtcNow</c> will be used.
    /// </param>
    /// <param name="maxRelativity">The maximum time span to consider as relative. If null, all time spans will be considered.</param>
    /// <param name="format">The format to use when the time is not relative.</param>
    /// <exception cref="ArgumentException">When <paramref name="compareAgainst" /> is not UTC.</exception>
    /// <returns>A string that represents the current <see cref="TimezoneDateTime" /> instance in a relative time format.</returns>
    public string Humanize(DateTime? compareAgainst = null, TimeSpan? maxRelativity = null, string format = "G")
    {
        compareAgainst ??= DateTime.UtcNow;
        if (compareAgainst.Value.Kind != DateTimeKind.Utc)
            throw new ArgumentException("compareAgainst must be UTC", nameof(compareAgainst));

        var comparingTzdt = new TimezoneDateTime(compareAgainst.Value, _timezone);
        if (comparingTzdt < this)
        {
            var duration = this - comparingTzdt;

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
            var timezoneInfo = LocalTimezone.Mappings[_timezone];
            var utcDateTime = new DateTime(Ticks, DateTimeKind.Utc);

            var current = _timezone > 0 ? Instant.FromDateTimeUtc(utcDateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
            var currentYear = current?.Year ?? utcDateTime.Year;
            var currentMonth = current?.Month ?? utcDateTime.Month;
            var currentDay = current?.Day ?? utcDateTime.Day;

            var compare = _timezone > 0 ? Instant.FromDateTimeUtc(compareAgainst.Value).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar) : (ZonedDateTime?)null;
            var compareYear = compare?.Year ?? compareAgainst.Value.Year;
            var compareMonth = compare?.Month ?? compareAgainst.Value.Month;
            var compareDay = compare?.Day ?? compareAgainst.Value.Day;

            var duration = comparingTzdt - this;
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

                if (currentYear == compareYear && currentMonth == compareMonth)
                {
                    if (currentDay == compareDay)
                        return $"today at {this:hh:mm tt}";

                    if (currentDay == compareDay - 1)
                        return $"yesterday at {this:hh:mm tt}";
                }

                if (currentYear == compareYear)
                {
                    if (currentMonth == compareMonth)
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

                    if (currentMonth == compareMonth - 1)
                        return "last month";

                    var months = compareMonth - currentMonth;
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
                            if (currentYear == compareYear - 1)
                                return "last year";

                        return $"{months} months ago";
                    }
                }

                if (currentYear == compareYear - 1)
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
        var timezoneInfo = LocalTimezone.Mappings[_timezone];

        var dateTime = new DateTime(Ticks, DateTimeKind.Utc);
        if (_timezone == 0)
            return DateTime.Today.Date == dateTime.Date;

        var current = Instant.FromDateTimeUtc(dateTime).InZone(timezoneInfo.Clock.Zone, timezoneInfo.Clock.Calendar);
        var currentDay = current.Day;
        var currentMonth = current.Month;
        var currentYear = current.Year;

        var today = timezoneInfo.Clock.GetCurrentDate();
        return currentYear == today.Year && currentMonth == today.Month && currentDay == today.Day;
    }

    public class DateTimeLocalJsonConverter : JsonConverter<TimezoneDateTime>
    {
        public override TimezoneDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return Empty;

            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException($"The value is expected to be a string, but was {reader.TokenType}");

            var str = reader.GetString();
            if (string.IsNullOrWhiteSpace(str) || !DateTime.TryParse(str, out var dt))
                throw new JsonException($"Invalid date format: \"{str}\"");

            dt = dt.ToUniversalTime();
            return new TimezoneDateTime(dt, LocalTimezone.Utc);
        }

        public override void Write(Utf8JsonWriter writer, TimezoneDateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.GetUtcDateTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }
}