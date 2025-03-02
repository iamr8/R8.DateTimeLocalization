using System.Globalization;
using NodaTime;
using NodaTime.Extensions;

namespace R8.DateTimeLocalization.Tests.TimezoneMappers;

public class TurkeyTimezone : LocalTimezoneInfo
{
    public TurkeyTimezone()
    {
        IanaId = "Europe/Istanbul";
        Clock = SystemClock.Instance.InZone(DateTimeZoneProviders.Tzdb[IanaId], CalendarSystem.Gregorian);
        Culture = CultureInfo.GetCultureInfo("tr-TR");
    }

    public override string IanaId { get; }
    public override CultureInfo Culture { get; }
}