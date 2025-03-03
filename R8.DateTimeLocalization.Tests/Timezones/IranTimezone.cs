using System.Globalization;
using NodaTime;

namespace R8.DateTimeLocalization.Tests.Timezones;

public class IranTimezone : LocalTimezoneInfo
{
    public override string IanaId => "Asia/Tehran";
    public override CultureInfo Culture => CultureInfo.GetCultureInfo("fa-IR");
    public override CalendarSystem Calendar => CalendarSystem.PersianSimple;
}