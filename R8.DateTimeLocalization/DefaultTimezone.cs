using System.Globalization;
using NodaTime;

namespace R8.DateTimeLocalization;

public class DefaultTimezone : LocalTimezoneInfo
{
    public DefaultTimezone(DateTimeZone zone, CultureInfo culture)
    {
        IanaId = zone.Id;
        Culture = culture;
        Calendar = CalendarSystem.Iso;
    }

    public override string IanaId { get; }
    public override CultureInfo Culture { get; }
    public override CalendarSystem Calendar { get; }
}