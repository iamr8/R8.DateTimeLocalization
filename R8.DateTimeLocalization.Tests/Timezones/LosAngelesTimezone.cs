using System.Globalization;
using NodaTime;

namespace R8.DateTimeLocalization.Tests.Timezones;

public class LosAngelesTimezone : LocalTimezoneInfo
{
    public override string IanaId => "America/Los_Angeles";
    public override CultureInfo Culture => CultureInfo.GetCultureInfo("en-US");
    public override CalendarSystem Calendar => CalendarSystem.Gregorian;
}