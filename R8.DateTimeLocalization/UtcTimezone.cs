using System.Globalization;
using NodaTime;
using NodaTime.Extensions;

namespace R8.DateTimeLocalization
{
    internal sealed class UtcTimezone : LocalTimezoneInfo
    {
        public const string UtcIanaId = "UTC";

        public UtcTimezone()
        {
            IanaId = UtcIanaId;
            Clock = SystemClock.Instance.InZone(DateTimeZoneProviders.Tzdb[IanaId], CalendarSystem.Iso);
            Culture = CultureInfo.InvariantCulture;
        }

        public override string IanaId { get; }
        public override CultureInfo Culture { get; }
    }
}