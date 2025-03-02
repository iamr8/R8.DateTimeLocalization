using System.Globalization;
using NodaTime;
using NodaTime.Extensions;

namespace R8.DateTimeLocalization.Tests.TimezoneMappers;

public class UKTimezone : LocalTimezoneInfo
{
    public UKTimezone()
    {
        IanaId = "Europe/London";
        Clock = SystemClock.Instance.InZone(DateTimeZoneProviders.Tzdb[IanaId], CalendarSystem.Gregorian);
        Culture = CultureInfo.GetCultureInfo("en-GB");
    }

    public override string IanaId { get; }
    public override CultureInfo Culture { get; }
}