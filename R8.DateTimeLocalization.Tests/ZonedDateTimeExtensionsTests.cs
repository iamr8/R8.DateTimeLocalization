using FluentAssertions;
using NodaTime;
using R8.DateTimeLocalization.Tests.TimezoneMappers;

namespace R8.DateTimeLocalization.Tests;

public class ZonedDateTimeExtensionsTests
{
    public ZonedDateTimeExtensionsTests()
    {
        LocalTimezone.Mappings.GetOrCreate<IranTimezone>();
        LocalTimezone.Mappings.GetOrCreate<TurkeyTimezone>();
    }

    [Fact]
    public void should_return_tzdt_from_datetime()
    {
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();

        var result = dateTime.ToZonedDateTime(timezone);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        result.DayOfWeek.Should().Be(IsoDayOfWeek.Friday);

        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_from_localdatetime()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);

        var result = dateTime.ToZonedDateTime(timezone);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        result.DayOfWeek.Should().Be(IsoDayOfWeek.Friday);

        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_from_datetime_according_to_current_timezone()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();

        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var result = dateTime.ToZonedDateTime(timezone);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        result.DayOfWeek.Should().Be(IsoDayOfWeek.Friday);

        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_from_start_of_week()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1402, 11, 24, 0, 0, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.GetStartOfWeek();

        Assert.Equal(1402, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(21, result.Day);

        Assert.Equal(0, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);
    }

    [Fact]
    public void should_return_tzdt_from_end_of_week()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1402, 11, 21, 0, 0, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.GetEndOfWeek();

        Assert.Equal(1402, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(27, result.Day);

        Assert.Equal(23, result.Hour);
        Assert.Equal(59, result.Minute);
        Assert.Equal(59, result.Second);
    }

    [Fact]
    public void should_return_tzdt_with_start_of_hour()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.GetStartOfHour();

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_with_end_of_hour()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.GetEndOfHour();

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(59, result.Minute);
        Assert.Equal(59, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_with_start_of_minute()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.GetStartOfMinute();

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_with_end_of_minute()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.GetEndOfMinute();

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(59, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_with_start_of_day()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.GetStartOfDay();

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(0, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_with_end_of_day()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.GetEndOfDay();

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(23, result.Hour);
        Assert.Equal(59, result.Minute);
        Assert.Equal(59, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_with_start_of_month()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.GetStartOfMonth();

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(1, result.Day);

        Assert.Equal(0, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_with_end_of_month()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.GetEndOfMonth();

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(30, result.Day);

        Assert.Equal(23, result.Hour);
        Assert.Equal(59, result.Minute);
        Assert.Equal(59, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_from_local_date_time()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_from_local_date()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 0, 0, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(0, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_from_datetime2()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2021, 3, 21, 12, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);

        Assert.Equal(1400, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);

        Assert.Equal(15, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(31, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_from_datetime3()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2021, 3, 21, 23, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);

        Assert.Equal(1400, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(2, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(31, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_with_different_timezone()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 3, 30, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);
        result = result.WithTimezone(LocalTimezone.Mappings["Europe/Istanbul"].GetTimezone());

        Assert.Equal(2021, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);
    }

    [Fact]
    public void should_add_month()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2021, 1, 1, 13, 12, 11, DateTimeKind.Utc);

        var result = dateTime.ToZonedDateTime(timezone);
        result = result.PlusMonths(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(16, result.Hour);
        Assert.Equal(42, result.Minute);
        Assert.Equal(11, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_month2()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);
        result = result.PlusMonths(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(13, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_year()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);
        result = result.PlusYears(2);

        Assert.Equal(1401, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_subtract_year()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);
        result = result.PlusYears(-2);

        Assert.Equal(1397, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_day()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);

        result = result.PlusDays(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(13, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_days_resulted_to_new_month()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2021, 1, 20, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);
        result = result.PlusMonths(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(1, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_underlying_datetime()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dt = new LocalDateTime(1399, 10, 12, 0, 0, 0, timezone.Calendar);
        var result = dt.ToZonedDateTime(timezone);

        var underlyingDateTime = result.ToDateTimeUtc();
        Assert.Equal(2020, underlyingDateTime.Year);
        Assert.Equal(12, underlyingDateTime.Month);
        Assert.Equal(31, underlyingDateTime.Day);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_subtract_month()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);
        result = result.PlusMonths(-1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(9, result.Month);
        Assert.Equal(12, result.Day);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_subtract_twelve_months()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);
        result = result.PlusMonths(-12);

        Assert.Equal(1400, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(14, result.Day);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_month3()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);
        result = result.PlusMonths(12);

        Assert.Equal(1402, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(14, result.Day);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_subtract_day()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);
        result = result.PlusDays(-1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(11, result.Day);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Theory]
    [InlineData(null, "1401/11/14 3:30:00")]
    [InlineData("d", "1401/11/14")]
    [InlineData("D", "1401 بهمن 14, جمعه")]
    [InlineData("f", "1401 بهمن 14, جمعه 3:30")]
    [InlineData("F", "1401 بهمن 14, جمعه 3:30:00")]
    [InlineData("g", "1401/11/14 3:30")]
    [InlineData("G", "1401/11/14 3:30:00")]
    [InlineData("m", "14 بهمن")]
    [InlineData("M", "14 بهمن")]
    [InlineData("MMMM", "بهمن")]
    public void should_return_formatted_string(string? format, string expected)
    {
        var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();

        var result = dateTime.ToZonedDateTime(timezone);
        var resultString = result.ToOrdinalString(format);

        Assert.Equal(expected, resultString);
    }

    [Fact]
    public void should_return_formatted_ordinal_string()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToZonedDateTime(timezone);
        var resultString = result.ToOrdinalString();

        Assert.Equal("1401/11/14 3:30:00", resultString);
    }

    [Fact]
    public void should_add_days()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);
        var result = dateTime.ToZonedDateTime(timezone);
        result = result.PlusDays(1);
        Assert.Equal(1401, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(15, result.Day);
    }
}