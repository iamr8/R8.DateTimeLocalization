using System.Globalization;
using NodaTime;
using NodaTime.Extensions;

namespace R8.DateTimeLocalization.Tests.TimezoneMappers;

public class IranTimezone : LocalTimezoneInfo
{
    public IranTimezone()
    {
        IanaId = "Asia/Tehran";
        Clock = SystemClock.Instance.InZone(DateTimeZoneProviders.Tzdb[IanaId], CalendarSystem.PersianSimple);
        Culture = CultureInfo.GetCultureInfo("fa-IR");
    }

    public override string IanaId { get; }
    public override CultureInfo Culture { get; }
}