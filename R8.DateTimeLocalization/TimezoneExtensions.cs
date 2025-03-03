using System;
using System.Runtime.CompilerServices;

namespace R8.DateTimeLocalization;

public static class TimezoneExtensions
{
    /// <summary>
    ///     Returns a <see cref="TimezoneDateTime" /> from the specified <see cref="DateTime" /> according to the specified
    ///     <see cref="LocalTimezone" />.
    /// </summary>
    /// <param name="dateTime">A <see cref="DateTime" /> object.</param>
    /// <param name="timeZone">A <see cref="LocalTimezone" /> object.</param>
    /// <returns>A <see cref="TimezoneDateTime" /> object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimezoneDateTime ToTimezoneDateTime(this DateTime dateTime, LocalTimezone timeZone)
    {
        return new TimezoneDateTime(dateTime, timeZone);
    }
}