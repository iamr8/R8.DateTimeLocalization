using System.Globalization;
using NodaTime;
using NodaTime.Extensions;

namespace R8.DateTimeLocalization.Tests.TimezoneMappers;

public class LosAngelesTimezone : LocalTimezoneInfo
{
    public LosAngelesTimezone()
    {
        IanaId = "America/Los_Angeles";
        Clock = SystemClock.Instance.InZone(DateTimeZoneProviders.Tzdb[IanaId], CalendarSystem.Gregorian);
        Culture = CultureInfo.GetCultureInfo("en-US");
    }

    public override string IanaId { get; }
    public override CultureInfo Culture { get; }
}