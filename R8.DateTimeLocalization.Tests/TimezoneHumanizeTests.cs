using FluentAssertions;
using R8.DateTimeLocalization.Tests.TimezoneMappers;

namespace R8.DateTimeLocalization.Tests;

public class TimezoneHumanizeTests : IAsyncLifetime
{
    public Task InitializeAsync()
    {
        LocalTimezone.Mappings.GetOrCreate<IranTimezone>();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Yesterday_sometime()
    {
        var dateTime = new DateTimeLocal(1403, 9, 9, 12, 25, 43, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 11, 9, 23, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("yesterday at 12:25 بعدازظهر");
    }

    [Fact]
    public void AFewSecondsAgo()
    {
        var dateTime = new DateTimeLocal(1403, 9, 10, 12, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 12, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("a few seconds ago");
    }

    [Fact]
    public void AnHourAgo()
    {
        var dateTime = new DateTimeLocal(1403, 9, 10, 11, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 12, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("an hour ago");
    }

    [Fact]
    public void SomeHoursAgo()
    {
        var dateTime = new DateTimeLocal(1403, 9, 10, 8, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 12, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("4 hours ago");
    }

    [Fact]
    public void AFewHoursAgo()
    {
        var dateTime = new DateTimeLocal(1403, 9, 10, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 12, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("a few hours ago");
    }

    [Fact]
    public void Today()
    {
        var dateTime = new DateTimeLocal(1403, 9, 10, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 18, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("today at 03:58 قبل‌ازظهر");
    }

    [Fact]
    public void AFewDaysAgo()
    {
        var dateTime = new DateTimeLocal(1403, 9, 5, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 18, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("a few days ago");
    }

    [Fact]
    public void SomeDaysAgo()
    {
        var dateTime = new DateTimeLocal(1403, 9, 8, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 18, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("2 days ago");
    }

    [Fact]
    public void LastWeek()
    {
        var dateTime = new DateTimeLocal(1403, 9, 3, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 18, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("last week");
    }

    [Fact]
    public void LastMonth()
    {
        var dateTime = new DateTimeLocal(1403, 8, 12, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 18, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("last month");
    }

    [Fact]
    public void SomeMonthsAgo()
    {
        var dateTime = new DateTimeLocal(1403, 7, 12, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 18, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("2 months ago");
    }

    [Fact]
    public void LastYear_between_two_years()
    {
        var dateTime = new DateTimeLocal(1402, 7, 12, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 18, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("last year");
    }

    [Fact]
    public void TwoMonthsAgo()
    {
        var dateTime = new DateTimeLocal(1402, 11, 12, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 2, 10, 18, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("2 months ago");
    }

    [Fact]
    public void LastYear()
    {
        var dateTime = new DateTimeLocal(1402, 3, 12, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 2, 10, 18, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("last year");
    }

    [Fact]
    public void AFewWeeksAgo2()
    {
        var dateTime = new DateTimeLocal(1403, 9, 2, 3, 58, 20, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 30, 18, 58, 55, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("a few weeks ago");
    }

    [Fact]
    public void SomeMinutesAgo_between_two_months()
    {
        var dateTime = new DateTimeLocal(1403, 9, 9, 23, 10, 59, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 0, 0, 0, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("49 minutes ago");
    }

    [Fact]
    public void AFewMinutesAgo_between_two_months()
    {
        var dateTime = new DateTimeLocal(1403, 9, 9, 23, 58, 59, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 0, 0, 0, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("a few minutes ago");
    }

    [Fact]
    public void JustNow_between_two_months()
    {
        var dateTime = new DateTimeLocal(1403, 9, 9, 23, 59, 59, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 9, 10, 0, 0, 0, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("just now");
    }

    [Fact]
    public void JustNow_between_two_days()
    {
        var dateTime = new DateTimeLocal(1403, 9, 30, 23, 59, 59, LocalTimezone.GetOrCreate("Asia/Tehran"));
        var comparerDate = new DateTimeLocal(1403, 10, 1, 0, 0, 0, LocalTimezone.GetOrCreate("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("just now");
    }
}