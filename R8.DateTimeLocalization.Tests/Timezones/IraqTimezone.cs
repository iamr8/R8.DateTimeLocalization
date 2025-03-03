using System.Globalization;
using NodaTime;

namespace R8.DateTimeLocalization.Tests.Timezones;

public class IraqTimezone : LocalTimezoneInfo
{
    public override string IanaId => "Asia/Baghdad";
    public override CultureInfo Culture => CultureInfo.GetCultureInfo("ar-IQ");
    public override CalendarSystem Calendar => CalendarSystem.Gregorian;
}