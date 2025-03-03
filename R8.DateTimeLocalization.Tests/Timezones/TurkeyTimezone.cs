using System.Globalization;
using NodaTime;

namespace R8.DateTimeLocalization.Tests.Timezones;

public class TurkeyTimezone : LocalTimezoneInfo
{
    public override string IanaId => "Europe/Istanbul";
    public override CultureInfo Culture => CultureInfo.GetCultureInfo("tr-TR");
    public override CalendarSystem Calendar => CalendarSystem.Gregorian;
}