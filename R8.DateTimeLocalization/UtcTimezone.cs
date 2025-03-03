using System.Globalization;
using NodaTime;

namespace R8.DateTimeLocalization;

internal sealed class UtcTimezone : LocalTimezoneInfo
{
    public const string UtcIanaId = "UTC";

    public override string IanaId => UtcIanaId;
    public override CultureInfo Culture { get; } = CultureInfo.InvariantCulture;
    public override CalendarSystem Calendar => CalendarSystem.Iso;
}