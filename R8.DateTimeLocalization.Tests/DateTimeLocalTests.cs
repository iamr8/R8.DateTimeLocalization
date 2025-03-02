using FluentAssertions;
using R8.DateTimeLocalization.Tests.TimezoneMappers;
using Xunit.Abstractions;

namespace R8.DateTimeLocalization.Tests;

public class DateTimeLocalTests : IAsyncLifetime
{
    private readonly ITestOutputHelper _outputHelper;

    public DateTimeLocalTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    public Task InitializeAsync()
    {
        LocalTimezone.Mappings.GetOrCreate<IranTimezone>();
        LocalTimezone.Mappings.GetOrCreate<TurkeyTimezone>();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void should_add()
    {
        var timezone = LocalTimezone.Mappings.GetOrCreate<IranTimezone>();
        timezone.Should().NotBeNull();
    }

    [Fact]
    public void should()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var tzdt1 = new DateTimeLocal(1402, 12, 1, 0, 0, 0, timezone);
        var tzdt2 = new DateTimeLocal(1402, 12, 29, 23, 59, 59, timezone);

        _outputHelper.WriteLine(tzdt1.GetUtcDateTime().ToString());
        _outputHelper.WriteLine(tzdt2.GetUtcDateTime().ToString());
    }

    [Fact]
    public void should_return_tzdt_from_datetime()
    {
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();


        var result = dateTime.ToDateTimeLocal(timezone);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);

        result.DayOfWeek.Should().Be(DayOfWeek.Friday);

        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_from_datetime_according_to_current_timezone()
    {
        LocalTimezone.Current = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone()!;

        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var result = dateTime.ToDateTimeLocal();

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        result.DayOfWeek.Should().Be(DayOfWeek.Friday);

        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_from_start_of_week_when_current_day_is_in_middle_of_week()
    {
        var result = new DateTimeLocal(1402, 11, 24, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.GetStartOfWeek();

        Assert.Equal(1402, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(21, result.Day);

        Assert.Equal(0, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);
    }

    [Fact]
    public void should_return_tzdt_from_start_of_week_when_current_day_is_first_day_of_week()
    {
        var result = new DateTimeLocal(1402, 11, 21, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.GetStartOfWeek();

        Assert.Equal(1402, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(21, result.Day);

        Assert.Equal(0, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);
    }

    [Fact]
    public void should_return_tzdt_from_start_of_week_when_current_day_is_last_day_of_week()
    {
        var result = new DateTimeLocal(1402, 11, 27, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.GetStartOfWeek();

        Assert.Equal(1402, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(21, result.Day);

        Assert.Equal(0, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);
    }

    [Fact]
    public void should_return_tzdt_from_end_of_week_when_current_day_is_first_day_of_week()
    {
        var result = new DateTimeLocal(1402, 11, 21, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.GetEndOfWeek();

        Assert.Equal(1402, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(27, result.Day);

        Assert.Equal(23, result.Hour);
        Assert.Equal(59, result.Minute);
        Assert.Equal(59, result.Second);
    }

    [Fact]
    public void should_return_tzdt_from_end_of_week_when_current_day_is_last_day_of_week()
    {
        var result = new DateTimeLocal(1402, 11, 27, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.GetEndOfWeek();

        Assert.Equal(1402, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(27, result.Day);

        Assert.Equal(23, result.Hour);
        Assert.Equal(59, result.Minute);
        Assert.Equal(59, result.Second);
    }

    [Fact]
    public void should_return_tzdt_from_end_of_week_when_current_day_is_in_middle_of_week()
    {
        var result = new DateTimeLocal(1402, 11, 24, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
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
        var result = new DateTimeLocal(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
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
        var result = new DateTimeLocal(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
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
        var result = new DateTimeLocal(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
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
        var result = new DateTimeLocal(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
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
        var result = new DateTimeLocal(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
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
        var result = new DateTimeLocal(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
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
        var result = new DateTimeLocal(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
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
        var result = new DateTimeLocal(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
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
        var result = new DateTimeLocal(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

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
        var result = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

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
        var dateTime = new DateTime(2021, 3, 21, 12, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

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
        var dateTime = new DateTime(2021, 3, 21, 23, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        Assert.Equal(1400, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(2, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(31, result.GetDaysInMonth());
    }

    [Fact]
    public void should_subtract_minute()
    {
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddMinutes(-1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(29, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_minute()
    {
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddMinutes(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(31, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_subtract_second()
    {
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddSeconds(-1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(29, result.Minute);
        Assert.Equal(59, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_second()
    {
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddSeconds(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(1, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_second_resulted_to_next_minute()
    {
        var result = new DateTimeLocal(1399, 10, 12, 4, 0, 59, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddSeconds(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(4, result.Hour);
        Assert.Equal(1, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_return_tzdt_with_different_timezone()
    {
        var result = new DateTimeLocal(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.WithTimezone(LocalTimezone.Mappings["Europe/Istanbul"].GetTimezone());

        Assert.Equal(2021, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);
    }

    [Fact]
    public void should_add_minute_resulted_to_next_hour()
    {
        var result = new DateTimeLocal(1399, 10, 12, 3, 59, 0, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddMinutes(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(4, result.Hour);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_compare_to_other_while_are_equal()
    {
        var result = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        Assert.Equal(0, result.CompareTo(result2));
    }

    [Fact]
    public void should_add_month()
    {
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddMonths(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_month2()
    {
        var dateTime = new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddMonths(1);

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
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddYears(2);

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
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddYears(-2);

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
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone()!;
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var expected = dateTime.ToDateTimeLocal(timezone);
        var actual = expected.AddDays(1);

        Assert.Equal(1399, actual.Year);
        Assert.Equal(10, actual.Month);
        Assert.Equal(13, actual.Day);

        Assert.Equal(3, actual.Hour);
        Assert.Equal(30, actual.Minute);
        Assert.Equal(0, actual.Second);


        Assert.Equal(30, actual.GetDaysInMonth());
    }

    [Fact]
    public void should_add_days_resulted_to_new_month()
    {
        var dateTime = new DateTime(2021, 1, 20, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddMonths(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(1, result.Day);

        Assert.Equal(3, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_hour()
    {
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddHours(1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(4, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_subtract_hour()
    {
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddHours(-1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(12, result.Day);

        Assert.Equal(2, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(0, result.Second);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_be_same_as_local_values()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();

        var actual = new DateTimeLocal(1399, 10, 12, 23, 30, 0, timezone);
        actual.Year.Should().Be(1399);
        actual.Month.Should().Be(10);
        actual.Day.Should().Be(12);
        actual.Hour.Should().Be(23);
        actual.Minute.Should().Be(30);
        actual.Second.Should().Be(0);
    }

    [Fact]
    public void should_add_hour_resulted_to_next_day()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();

        var actual = new DateTimeLocal(1399, 10, 12, 23, 30, 0, timezone);
        actual = actual.AddHours(1);

        var expected = new DateTimeLocal(1399, 10, 13, 0, 30, 0, timezone);
        actual.Should().Be(expected);

        actual.Year.Should().Be(expected.Year);
        actual.Month.Should().Be(expected.Month);
        actual.Day.Should().Be(expected.Day);
        actual.Hour.Should().Be(expected.Hour);
        actual.Minute.Should().Be(expected.Minute);
        actual.Second.Should().Be(expected.Second);
        actual.GetDaysInMonth().Should().Be(expected.GetDaysInMonth());
        actual.DayOfWeek.Should().Be(expected.DayOfWeek);
        actual.GetUtcDateTime().Should().Be(expected.GetUtcDateTime());
    }

    [Fact]
    public void should_return_underlying_datetime()
    {
        var result = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        var underlyingDateTime = result.GetUtcDateTime();
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
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddMonths(-1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(9, result.Month);
        Assert.Equal(12, result.Day);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_subtract_twelve_months()
    {
        var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddMonths(-12);

        Assert.Equal(1400, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(14, result.Day);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_add_month3()
    {
        var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddMonths(12);

        Assert.Equal(1402, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(14, result.Day);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_subtract_day()
    {
        var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddDays(-1);

        Assert.Equal(1399, result.Year);
        Assert.Equal(10, result.Month);
        Assert.Equal(11, result.Day);


        Assert.Equal(30, result.GetDaysInMonth());
    }

    [Fact]
    public void should_be_equal()
    {
        var result = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        Assert.True(result2 == result);
    }

    [Fact]
    public void should_not_be_equal()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        Assert.True(result2 != result);
    }

    [Fact]
    public void should_be_greater_than()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        Assert.True(result > result2);
    }

    [Fact]
    public void should_be_greater_than_or_equal_to()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        Assert.True(result >= result2);
    }

    [Fact]
    public void should_be_less_than_or_equal_to()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        Assert.True(result2 <= result);
    }

    [Fact]
    public void should_be_less_than()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        Assert.True(result2 < result);
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


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var resultString = result.ToString(format);

        Assert.Equal(expected, resultString);
    }

    [Fact]
    public void should_return_formatted_ordinal_string()
    {
        var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);


        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var resultString = result.ToString();

        resultString.Should().Be("1401/11/14 3:30:00");
    }

    [Fact]
    public void should_subtract_two_tzdt_by_operator()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        var result3 = result - result2;

        Assert.Equal(1, result3.Days);
    }

    [Fact]
    public void should_subtract_two_tzdt_by_operator2()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        var result3 = result2 - result;

        Assert.Equal(-1, result3.Days);
    }

    [Fact]
    public void should_subtract_two_tzdt_by_Subtract_method()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        var result3 = result2.Subtract(result);

        Assert.Equal(-1, result3.Days);
    }

    [Fact]
    public void should_subtract_tzdt_and_TimeSpan_by_Subtract_method()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = result.Subtract(TimeSpan.FromDays(1));

        Assert.Equal(1399, result2.Year);
        Assert.Equal(10, result2.Month);
        Assert.Equal(12, result2.Day);
    }

    [Fact]
    public void should_add_tzdt_and_TimeSpan_by_Add_method()
    {
        var original = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var actual = original.Add(TimeSpan.FromDays(1));

        Assert.Equal(1399, actual.Year);
        Assert.Equal(10, actual.Month);
        Assert.Equal(14, actual.Day);
    }

    [Theory]
    [InlineData(1399, 10, 13, 1, 1399, 10, 14)]
    [InlineData(1399, 10, 30, 1, 1399, 11, 1)]
    public void should_add_tzdt_and_TimeSpan_by_operator(int year, int month, int day, int addingDays, int resultYear, int resultMonth, int resultDay)
    {
        var result = new DateTimeLocal(year, month, day, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = result + TimeSpan.FromDays(addingDays);

        Assert.Equal(resultYear, result2.Year);
        Assert.Equal(resultMonth, result2.Month);
        Assert.Equal(resultDay, result2.Day);
    }

    [Fact]
    public void should_returns_ticks()
    {
        var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);
        var tzdt = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        dateTime.Ticks.Should().Be(tzdt.Ticks);
    }

    [Fact]
    public void should_be_equal_by_Equal_method()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        Assert.True(result2.Equals(result));
    }

    [Fact]
    public void should_not_be_equal_by_Equal_method()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        Assert.False(result2.Equals(result));
    }

    [Fact]
    public void should_subtract_two_tzdt_by_operator3()
    {
        var result = new DateTimeLocal(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        var result2 = new DateTimeLocal(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());

        var result3 = result - result2;

        Assert.Equal(1, result3.Days);
    }

    [Fact]
    public void should_add_days()
    {
        var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);
        var result = dateTime.ToDateTimeLocal(LocalTimezone.Mappings["Asia/Tehran"].GetTimezone());
        result = result.AddDays(1);
        Assert.Equal(1401, result.Year);
        Assert.Equal(11, result.Month);
        Assert.Equal(15, result.Day);
    }
}