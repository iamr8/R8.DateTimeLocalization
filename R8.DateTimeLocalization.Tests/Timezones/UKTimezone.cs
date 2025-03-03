using System.Globalization;
using NodaTime;

namespace R8.DateTimeLocalization.Tests.Timezones;

public class UKTimezone : LocalTimezoneInfo
{
    public override string IanaId => "Europe/London";
    public override CultureInfo Culture => CultureInfo.GetCultureInfo("en-GB");
    public override CalendarSystem Calendar => CalendarSystem.Gregorian;
}