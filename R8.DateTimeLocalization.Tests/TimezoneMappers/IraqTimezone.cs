using System.Globalization;
using NodaTime;
using NodaTime.Extensions;

namespace R8.DateTimeLocalization.Tests.TimezoneMappers;

public class IraqTimezone : LocalTimezoneInfo
{
    public IraqTimezone()
    {
        IanaId = "Asia/Baghdad";
        Clock = SystemClock.Instance.InZone(DateTimeZoneProviders.Tzdb[IanaId], CalendarSystem.Gregorian);
        Culture = CultureInfo.GetCultureInfo("ar-IQ");
    }

    public override string IanaId { get; }
    public override CultureInfo Culture { get; }
}