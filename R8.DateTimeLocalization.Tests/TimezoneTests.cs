using System.Globalization;
using FluentAssertions;
using NodaTime;
using R8.DateTimeLocalization.Tests.Timezones;

namespace R8.DateTimeLocalization.Tests;

public class TimezoneTests
{
    public TimezoneTests()
    {
        // LocalTimezone.Mappings.Clear();
        LocalTimezone.Mappings.Add<IranTimezone>();
        LocalTimezone.Mappings.Add<TurkeyTimezone>();
        LocalTimezone.Mappings.Add<UKTimezone>();
        LocalTimezone.Mappings.Add<LosAngelesTimezone>();
    }

    [Fact]
    public void should_return_utc_timezone()
    {
        var timezone = LocalTimezone.Utc;
        var currentTimezone = DateTimeZone.Utc;

        timezone.IanaId.Should().Be(currentTimezone.Id);
    }

    [Fact]
    public void should_return_timezone_from_resolver()
    {
        // Act
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();

        // Assert
        timezone.Should().NotBeNull();
        timezone.IanaId.Should().Be("Asia/Tehran");
        timezone.GetSystemTimeZone().StandardName.Should().BeOneOf("Iran Standard Time", "Iran Daylight Time");
        timezone.ToString().Should().Be("GMT+03:30");
        timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("fa-IR"));
        timezone.Calendar.Should().Be(CalendarSystem.PersianSimple);

        var daysOfWeek = timezone.DaysOfWeek;
        daysOfWeek[0].Should().Be(DayOfWeek.Saturday);
        daysOfWeek[1].Should().Be(DayOfWeek.Sunday);
        daysOfWeek[2].Should().Be(DayOfWeek.Monday);
        daysOfWeek[3].Should().Be(DayOfWeek.Tuesday);
        daysOfWeek[4].Should().Be(DayOfWeek.Wednesday);
        daysOfWeek[5].Should().Be(DayOfWeek.Thursday);
        daysOfWeek[6].Should().Be(DayOfWeek.Friday);
    }

    [Fact]
    public void should_return_timezone_of_iran()
    {
        // Act
        var timezone = LocalTimezone.Get("Asia/Tehran");

        // Assert
        timezone.IanaId.Should().Be("Asia/Tehran");
        timezone.GetSystemTimeZone().StandardName.Should().BeOneOf("Iran Standard Time", "Iran Daylight Time");
        timezone.ToString().Should().Be("GMT+03:30");
        timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("fa-IR"));
        timezone.Calendar.Should().Be(CalendarSystem.PersianSimple);

        var daysOfWeek = timezone.DaysOfWeek;
        daysOfWeek[0].Should().Be(DayOfWeek.Saturday);
        daysOfWeek[1].Should().Be(DayOfWeek.Sunday);
        daysOfWeek[2].Should().Be(DayOfWeek.Monday);
        daysOfWeek[3].Should().Be(DayOfWeek.Tuesday);
        daysOfWeek[4].Should().Be(DayOfWeek.Wednesday);
        daysOfWeek[5].Should().Be(DayOfWeek.Thursday);
        daysOfWeek[6].Should().Be(DayOfWeek.Friday);
    }

    [Fact]
    public void should_return_timezone_of_turkey()
    {
        // Act
        var timezone = LocalTimezone.Get("Europe/Istanbul");

        // Assert
        timezone.IanaId.Should().Be("Europe/Istanbul");
        timezone.ToString().Should().Be("GMT+03:00");
        timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("tr-TR"));
        timezone.Calendar.Should().Be(CalendarSystem.Gregorian);

        var daysOfWeek = timezone.DaysOfWeek;
        daysOfWeek[0].Should().Be(DayOfWeek.Monday);
        daysOfWeek[1].Should().Be(DayOfWeek.Tuesday);
        daysOfWeek[2].Should().Be(DayOfWeek.Wednesday);
        daysOfWeek[3].Should().Be(DayOfWeek.Thursday);
        daysOfWeek[4].Should().Be(DayOfWeek.Friday);
        daysOfWeek[5].Should().Be(DayOfWeek.Saturday);
        daysOfWeek[6].Should().Be(DayOfWeek.Sunday);
    }

    [Fact]
    public void should_return_timezone_of_los_angeles()
    {
        // Act
        var timezone = LocalTimezone.Get("America/Los_Angeles");

        // Assert
        timezone.IanaId.Should().Be("America/Los_Angeles");
        timezone.GetSystemTimeZone().StandardName.Should().BeOneOf("Pacific Standard Time", "Pacific Daylight Time");
        timezone.ToString().Should().BeOneOf("GMT-08:00", "GMT-07:00");
        timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("en-US"));
        timezone.Calendar.Should().Be(CalendarSystem.Gregorian);

        var daysOfWeek = timezone.DaysOfWeek;
        daysOfWeek[0].Should().Be(DayOfWeek.Sunday);
        daysOfWeek[1].Should().Be(DayOfWeek.Monday);
        daysOfWeek[2].Should().Be(DayOfWeek.Tuesday);
        daysOfWeek[3].Should().Be(DayOfWeek.Wednesday);
        daysOfWeek[4].Should().Be(DayOfWeek.Thursday);
        daysOfWeek[5].Should().Be(DayOfWeek.Friday);
        daysOfWeek[6].Should().Be(DayOfWeek.Saturday);
    }

    [Fact]
    public void should_return_timezone_of_uk()
    {
        // Act
        var timezone = LocalTimezone.Get("Europe/London");

        // Assert
        timezone.IanaId.Should().Be("Europe/London");
        timezone.ToString().Should().BeOneOf("GMT+00:00", "GMT+01:00");
        timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("en-GB"));
        timezone.Calendar.Should().Be(CalendarSystem.Gregorian);

        var daysOfWeek = timezone.DaysOfWeek;
        daysOfWeek[0].Should().Be(DayOfWeek.Monday);
        daysOfWeek[1].Should().Be(DayOfWeek.Tuesday);
        daysOfWeek[2].Should().Be(DayOfWeek.Wednesday);
        daysOfWeek[3].Should().Be(DayOfWeek.Thursday);
        daysOfWeek[4].Should().Be(DayOfWeek.Friday);
        daysOfWeek[5].Should().Be(DayOfWeek.Saturday);
        daysOfWeek[6].Should().Be(DayOfWeek.Sunday);
    }

    [Fact]
    public void should_return_cached_timezone()
    {
        // Act
        var timezone = LocalTimezone.Get("Asia/Tehran");

        // Assert
        timezone.IanaId.Should().Be("Asia/Tehran");
        timezone.GetSystemTimeZone().StandardName.Should().BeOneOf("Iran Standard Time", "Iran Daylight Time");
        timezone.ToString().Should().Be("GMT+03:30");
        timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("fa-IR"));
        timezone.Calendar.Should().Be(CalendarSystem.PersianSimple);

        var daysOfWeek = timezone.DaysOfWeek;
        daysOfWeek[0].Should().Be(DayOfWeek.Saturday);
        daysOfWeek[1].Should().Be(DayOfWeek.Sunday);
        daysOfWeek[2].Should().Be(DayOfWeek.Monday);
        daysOfWeek[3].Should().Be(DayOfWeek.Tuesday);
        daysOfWeek[4].Should().Be(DayOfWeek.Wednesday);
        daysOfWeek[5].Should().Be(DayOfWeek.Thursday);
        daysOfWeek[6].Should().Be(DayOfWeek.Friday);
    }

    [Fact]
    public void should_return_timezone_without_resolver()
    {
        // Act
        var timezone = LocalTimezone.Get("Europe/Paris");

        // Assert
        timezone.IanaId.Should().Be("Europe/Paris");
        timezone.GetSystemTimeZone().StandardName.Should().Be("Central European Standard Time");
        timezone.ToString().Should().BeOneOf("GMT+01:00", "GMT+02:00");
        timezone.Culture.Should().Be(CultureInfo.InvariantCulture);
        timezone.Calendar.Should().Be(CalendarSystem.Iso);
    }

    [Fact]
    public void should_be_equal()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var timezone2 = LocalTimezone.Get("Asia/Tehran");

        timezone.Should().Be(timezone2);
    }

    [Fact]
    public void should_not_be_equal()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var timezone2 = LocalTimezone.Get("Europe/Istanbul");

        timezone.Should().NotBe(timezone2);
    }

    [Fact]
    public void should_not_be_equal_with_null()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");

        timezone.Should().NotBeNull();
    }

    [Fact]
    public void should_be_equal_by_operator()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var timezone2 = LocalTimezone.Get("Asia/Tehran");

        (timezone == timezone2).Should().BeTrue();
    }

    [Fact]
    public void should_not_be_equal_by_operator()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var timezone2 = LocalTimezone.Get("Europe/Istanbul");

        (timezone != timezone2).Should().BeTrue();
    }

    [Fact]
    public void should_be_casted_to_string()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        ((string)timezone).Should().Be("Asia/Tehran");
    }

    [Fact]
    public void should_be_casted_to_timezone()
    {
        var timezone = (LocalTimezone)"Asia/Tehran";
        timezone.IanaId.Should().Be("Asia/Tehran");
    }

    [Fact]
    public void should_return_offset_from_timezone()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var offset = timezone.Offset;

        offset.Should().Be(Offset.FromHoursAndMinutes(3, 30));
    }

    [Fact]
    public void should_be_comparable_greater_than()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var timezone2 = LocalTimezone.Get("Europe/Istanbul");

        (timezone > timezone2).Should().BeTrue();
    }

    [Fact]
    public void should_be_comparable_greater_than_or_equal()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var timezone2 = LocalTimezone.Get("Asia/Tehran");

        (timezone >= timezone2).Should().BeTrue();
    }

    [Fact]
    public void should_be_comparable_less_than()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var timezone2 = LocalTimezone.Get("Europe/Istanbul");

        (timezone2 < timezone).Should().BeTrue();
    }

    [Fact]
    public void should_be_comparable_less_than_or_equal()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var timezone2 = LocalTimezone.Get("Asia/Tehran");

        (timezone2 <= timezone).Should().BeTrue();
    }

    [Fact]
    public void should_be_comparable_hash_code()
    {
        var timezone = LocalTimezone.Get("Asia/Tehran");
        var timezone2 = LocalTimezone.Get("Asia/Tehran");

        timezone2.GetHashCode().Should().Be(timezone.GetHashCode());
    }
}