using System.Globalization;
using NodaTime;
using NodaTime.Extensions;

namespace R8.DateTimeLocalization
{
    public class DefaultTimezone : LocalTimezoneInfo
    {
        public DefaultTimezone(string ianaId, CultureInfo culture)
        {
            IanaId = ianaId;
            Culture = culture;
            Clock = SystemClock.Instance.InZone(DateTimeZoneProviders.Tzdb[IanaId], CalendarSystem.Iso);
        }

        public DefaultTimezone(DateTimeZone zone, CultureInfo culture)
        {
            IanaId = zone.Id;
            Culture = culture;
            Clock = SystemClock.Instance.InZone(zone, CalendarSystem.Iso);
        }

        public override string IanaId { get; }
        public override CultureInfo Culture { get; }
    }
}