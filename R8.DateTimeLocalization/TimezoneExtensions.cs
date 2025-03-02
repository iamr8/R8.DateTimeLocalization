using System;
using System.Runtime.CompilerServices;

namespace R8.DateTimeLocalization
{
    public static class TimezoneExtensions
    {
        /// <summary>
        ///     Converts Time of Day to a specific timezone.
        /// </summary>
        /// <param name="timeOfDay">A <see cref="TimeSpan" /> object.</param>
        /// <param name="sourceTimezone">A Timezone to convert from.</param>
        /// <param name="targetTimezone">A Timezone to convert to.</param>
        /// <returns>A localized <see cref="TimeSpan" /> object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan WithTimezone(this TimeSpan timeOfDay, LocalTimezone sourceTimezone, LocalTimezone targetTimezone)
        {
            if (sourceTimezone == targetTimezone)
                return timeOfDay;

            var currentTime = DateTime.UtcNow;
            var tempDate = currentTime.Date.Add(timeOfDay);
            if (sourceTimezone != LocalTimezone.Utc)
                tempDate = tempDate.WithKind(DateTimeKind.Unspecified);
            var srcTz = new DateTimeLocal(tempDate, sourceTimezone);
            var trgTz = srcTz.WithTimezone(targetTimezone);
            return trgTz.GetDateTime().TimeOfDay;
        }

        /// <summary>
        ///     Returns a <see cref="LocalTimezone" /> from the specified <see cref="ITimezone" />.
        /// </summary>
        /// <param name="options">A <see cref="LocalTimezoneInfo" /> object.</param>
        /// <returns>A <see cref="LocalTimezone" /> object.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="options" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LocalTimezone? GetTimezone<T>(this T? options) where T : LocalTimezoneInfo
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            return LocalTimezone.GetOrCreate(options.IanaId);
        }

        /// <summary>
        ///     Returns a <see cref="DateTimeLocal" /> from the specified <see cref="DateTime" /> according to the specified
        ///     <see cref="LocalTimezone" />.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime" /> object.</param>
        /// <param name="timeZone">A <see cref="LocalTimezone" /> object.</param>
        /// <returns>A <see cref="DateTimeLocal" /> object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTimeLocal ToDateTimeLocal(this DateTime dateTime, LocalTimezone timeZone)
        {
            return new DateTimeLocal(dateTime, timeZone);
        }

        /// <summary>
        ///     Returns a <see cref="DateTimeLocal" /> from the specified <see cref="DateTime" /> according to the specified
        ///     <see cref="LocalTimezone" />.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime" /> object.</param>
        /// <returns>A <see cref="DateTimeLocal" /> object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTimeLocal ToDateTimeLocal(this DateTime dateTime)
        {
            return dateTime.ToDateTimeLocal(LocalTimezone.Current);
        }
    }
}