using System;
using System.Runtime.CompilerServices;

namespace R8.DateTimeLocalization
{
    public static class DateTimeExtensions
    {
        /// <inheritdoc cref="DateTime.SpecifyKind" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime? WithKind(this DateTime? dateTime, DateTimeKind kind)
        {
            return dateTime?.WithKind(kind);
        }

        /// <inheritdoc cref="DateTime.SpecifyKind" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime WithKind(this DateTime dateTime, DateTimeKind kind)
        {
            if (dateTime == DateTime.MinValue)
                return dateTime;

            if (dateTime.Kind == kind)
                return dateTime;

            return DateTime.SpecifyKind(dateTime, kind);
        }

        /// <summary>
        ///     Converts a DateTime to Unix Timestamp in seconds
        /// </summary>
        /// <returns>An integer representing the number of seconds that have elapsed since 00:00:00 UTC, Thursday, 1 January 1970.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToUnixTimeSeconds(this DateTime datetime)
        {
            return (long)datetime.Subtract(DateTime.UnixEpoch).TotalSeconds;
        }

        /// <summary>
        ///     Converts a DateTime to Unix Timestamp in milliseconds
        /// </summary>
        /// <returns>
        ///     An integer representing the number of milliseconds that have elapsed since 00:00:00 UTC, Thursday, 1 January
        ///     1970.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToUnixTimeMilliseconds(this DateTime datetime)
        {
            return (long)datetime.Subtract(DateTime.UnixEpoch).TotalMilliseconds;
        }

        /// <summary>
        ///     Converts a Unix Timestamp in seconds to DateTime
        /// </summary>
        /// <returns>A DateTime representing the number of seconds that have elapsed since 00:00:00 UTC, Thursday, 1 January 1970.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime FromUnixTimeSeconds(long seconds)
        {
            return DateTime.UnixEpoch.AddSeconds(seconds);
        }

        /// <summary>
        ///     Converts a Unix Timestamp in milliseconds to DateTime
        /// </summary>
        /// <returns>
        ///     A DateTime representing the number of milliseconds that have elapsed since 00:00:00 UTC, Thursday, 1 January
        ///     1970.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime FromUnixTimeMilliseconds(long milliseconds)
        {
            return DateTime.UnixEpoch.AddMilliseconds(milliseconds);
        }

        public static string ToReadableString(this TimeSpan span)
        {
            var formatted = $"{(span.Duration().Days > 0 ? $"{span.Days:0} day{(span.Days == 1 ? string.Empty : "s")}, " : string.Empty)}{(span.Duration().Hours > 0 ? $"{span.Hours:0} hour{(span.Hours == 1 ? string.Empty : "s")}, " : string.Empty)}{(span.Duration().Minutes > 0 ? $"{span.Minutes:0} minute{(span.Minutes == 1 ? string.Empty : "s")}, " : string.Empty)}{(span.Duration().Seconds > 0 ? $"{span.Seconds:0} second{(span.Seconds == 1 ? string.Empty : "s")}" : string.Empty)}";

            if (formatted.EndsWith(", "))
                formatted = formatted[..^2];

            if (string.IsNullOrEmpty(formatted))
                formatted = "0 seconds";

            return formatted;
        }
    }
}