using FluentAssertions;
using R8.DateTimeLocalization.Tests.Timezones;

namespace R8.DateTimeLocalization.Tests;

public class TimezoneHumanizeTests : IAsyncLifetime
{
    public Task InitializeAsync()
    {
        LocalTimezone.Mappings.Add<IranTimezone>();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void should_return_YesterdayAtCertainTime_when_specified_date_is_1_day_bind_comparer_date_in_a_month()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 9, 12, 25, 43, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 11, 9, 23, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("yesterday at 12:25 بعدازظهر");
    }

    [Fact]
    public void should_return_AFewSecondsAgo_when_specified_date_is_lessthan_60_seconds_behind_comparer_date_in_a_day()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 10, 12, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 12, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("a few seconds ago");
    }

    [Fact]
    public void should_return_ToString_when_specified_date_is_lessthan_60_seconds_behind_comparer_date_in_a_day_but_MaxRelativity_is_lessthan_30_seconds()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 10, 12, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 12, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime(), TimeSpan.FromSeconds(30));

        result.Should().Be(dateTime.ToString());
    }

    [Fact]
    public void should_return_AFewSecondsAgo_when_specified_date_is_lessthan_60_seconds_behind_comparer_date_in_a_day_and_MaxRelativity_is_lessthan_60_seconds()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 10, 12, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 12, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime(), TimeSpan.FromSeconds(60));

        result.Should().Be("a few seconds ago");
    }

    [Fact]
    public void should_return_AnHourAgo_when_specified_date_is_1_hour_behind_comparer_date_in_a_day()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 10, 11, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 12, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("an hour ago");
    }

    [Fact]
    public void should_return_ACertainHoursAgo_when_specified_date_is_equalorlessthan_5_hours_behind_comparer_date_in_a_day()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 10, 8, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 12, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("4 hours ago");
    }

    [Fact]
    public void should_return_AFewHoursAgo_when_specified_date_is_more_than_5_hours_behind_comparer_date_in_a_day()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 10, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 12, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("a few hours ago");
    }

    [Fact]
    public void should_return_TodayAtCertainTime_when_specified_date_is_less_than_5_hours_behind_comparer_date_in_a_day()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 10, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("today at 03:58 قبل‌ازظهر");
    }

    [Fact]
    public void should_return_AFewDaysAgo_when_specified_date_is_5_days_behind_comparer_date_in_a_month()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 5, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate);

        result.Should().Be("a few days ago");
    }

    [Fact]
    public void should_return_AFewDaysAgo_when_specified_date_is_5_days_behind_comparer_date_but_within_2_different_years()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1402, 12, 24, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 1, 1, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate);

        result.Should().Be("a few days ago");
    }

    [Fact]
    public void should_return_CertainDaysAgo_when_specified_date_is_lessthan_3_days_behind_comparer_date_in_a_month()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 8, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("2 days ago");
    }

    [Fact]
    public void should_return_CertainDaysAgo_when_specified_date_is_lessthan_3_days_behind_comparer_date_but_in_2_different_years()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1402, 12, 28, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 1, 1, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("2 days ago");
    }

    [Fact]
    public void should_return_LastWeek_when_specified_date_is_less_than_1_week_behind_comparer_date_in_a_month()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 3, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("last week");
    }

    [Fact]
    public void should_return_LastWeek_when_specified_date_is_less_than_1_week_behind_comparer_date_but_within_2_different_years()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1402, 12, 20, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 1, 1, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("last week");
    }

    [Fact]
    public void should_return_LastMonth_when_specified_date_is_less_than_1_month_behind_comparer_date_in_a_year()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 8, 12, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("last month");
    }

    [Fact]
    public void should_return_LastMonth_when_specified_date_is_less_than_1_month_behind_comparer_date_but_within_2_different_years()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1402, 11, 20, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 1, 1, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("last month");
    }

    [Fact]
    public void should_return_CertainMonthsAgo_when_specified_date_is_less_than_12_months_behind_comparer_date_in_a_year()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 7, 12, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("2 months ago");
    }

    [Fact]
    public void should_return_LastYear_when_specified_date_is_1_year_behind_comparer_date_within_2_different_years()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1402, 7, 12, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("last year");
    }

    [Fact]
    public void should_return_CertainMonthsAgo_when_specified_date_is_some_months_behinds_comparer_date_but_within_2_different_years()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1402, 11, 12, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 2, 10, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("2 months ago");
    }

    [Fact]
    public void should_return_LastYear_when_specified_date_is_lessthan_1_year_behind_comparer_date_but_within_2_different_years()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1402, 3, 12, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 2, 10, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("last year");
    }

    [Fact]
    public void should_return_AFewWeeksAgo_when_specified_date_is_morethan_1_week_and_lessthan_1_month_behind_comparer_date_in_a_month()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 2, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 30, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("a few weeks ago");
    }

    [Fact]
    public void should_return_AFewWeeksAgo_when_specified_date_is_morethan_1_week_and_lessthan_1_month_behind_comparer_date_but_in_2_different_years()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1402, 12, 10, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 1, 1, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("a few weeks ago");
    }

    [Fact]
    public void should_return_CertainMinutesAgo_when_specified_date_is_lessthan_1_hour_behind_comparer_date_but_within_2_different_days()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 9, 23, 10, 59, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 0, 0, 0, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("49 minutes ago");
    }

    [Fact]
    public void should_return_AFewMinutesAgo_when_specified_date_is_equallessthan_10_minutes_behind_comparer_date_but_within_2_different_days()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1403, 9, 9, 23, 58, 59, timezone);
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 0, 0, 0, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("a few minutes ago");
    }

    [Fact]
    public void should_return_JustNow_when_specified_date_is_lessthan_30_seconds_behind_comparer_date_but_within_2_different_days()
    {
        var dateTime = new TimezoneDateTime(1403, 9, 9, 23, 59, 59, LocalTimezone.Get("Asia/Tehran"));
        var comparerDate = new TimezoneDateTime(1403, 9, 10, 0, 0, 0, LocalTimezone.Get("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("just now");
    }

    [Fact]
    public void should_return_JustNow_when_specified_date_is_lessthan_30_seconds_behind_comparer_date_but_within_2_different_months()
    {
        var dateTime = new TimezoneDateTime(1403, 9, 30, 23, 59, 59, LocalTimezone.Get("Asia/Tehran"));
        var comparerDate = new TimezoneDateTime(1403, 10, 1, 0, 0, 0, LocalTimezone.Get("Asia/Tehran"));

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be("just now");
    }

    [Fact]
    public void should_return_ToString_when_specified_date_is_lessthan_1_year_behind_comparer_date()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var dateTime = new TimezoneDateTime(1401, 11, 20, 3, 58, 20, timezone);
        var comparerDate = new TimezoneDateTime(1403, 1, 1, 18, 58, 55, timezone);

        var result = dateTime.Humanize(comparerDate.GetUtcDateTime());

        result.Should().Be(dateTime.ToString());
    }
}