using System;
using NodaTime;

namespace R8.DateTimeLocalization
{
    public interface ITimezone : ITimezoneInfo, IEquatable<ITimezone>
    {
        CalendarSystem Calendar { get; }
        Offset Offset { get; }
        TimeZoneInfo GetSystemTimeZone();
    }
}