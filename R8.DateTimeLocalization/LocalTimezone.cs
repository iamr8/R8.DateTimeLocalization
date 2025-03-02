using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using NodaTime;

namespace R8.DateTimeLocalization
{
    /// <summary>
    ///     Initializes a new instance of <see cref="LocalTimezone" />.
    /// </summary>
    [DebuggerDisplay("{" + nameof(IanaId) + "}")]
    public class LocalTimezone : ITimezone, IEquatable<LocalTimezone>, IComparable<LocalTimezone>, IFormattable
    {
        /// <summary>
        ///     A collection of <see cref="ITimezone" /> that can be used to resolve timezone information.
        /// </summary>
        public static LocalTimezoneMapCollection Mappings = new();

        private static readonly object SyncRoot = new();
        private static ConcurrentDictionary<string, LocalTimezone> Cached = new();
        private static readonly AsyncLocal<LocalTimezone?> CurrentLocal = new();

        private static LocalTimezone? _current;

        private LocalTimezoneInfo _info;

        static LocalTimezone()
        {
            lock (SyncRoot)
            {
                _current ??= CurrentLocal.Value ?? GetOrCreate(GetDefaultZone());
            }
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="LocalTimezone" />.
        /// </summary>
        private LocalTimezone()
        {
        }

        /// <summary>
        ///     Gets or sets the current timezone.
        /// </summary>
        /// <remarks>If a scope is started using <see cref="StartScope" />, this property will return the timezone of the scope.</remarks>
        public static LocalTimezone Current
        {
            get
            {
                lock (SyncRoot)
                {
                    if (CurrentLocal.Value != null)
                        return CurrentLocal.Value;
                    if (_current != null)
                        return _current;

                    var defaultZone = GetDefaultZone();
                    _current = GetOrCreate(defaultZone);
                    return _current;
                }
            }

            set
            {
                lock (SyncRoot)
                {
                    _current = value;
                }
            }
        }

        /// <summary>
        ///     Gets a <see cref="LocalTimezone" /> object representing the UTC timezone.
        /// </summary>
        public static LocalTimezone Utc => GetOrCreate("UTC");


        public int CompareTo(LocalTimezone? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Offset.CompareTo(other.Offset);
        }

        public bool Equals(LocalTimezone? other)
        {
            if (other is null)
                return false;
            return IanaId.Equals(other.IanaId, StringComparison.Ordinal);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (format == null)
                return ToString();

            if (format.Equals("G", StringComparison.OrdinalIgnoreCase))
                return ToString();

            if (format.Equals("N", StringComparison.OrdinalIgnoreCase))
                return IanaId;

            if (format.Equals("O", StringComparison.OrdinalIgnoreCase))
                return Offset.ToString("m", formatProvider ?? Culture);

            if (format.Equals("C", StringComparison.OrdinalIgnoreCase))
                return Culture.Name;

            if (format.Equals("A", StringComparison.OrdinalIgnoreCase))
                return Calendar.Id;

            if (format.Equals("F", StringComparison.OrdinalIgnoreCase))
                return Culture.DateTimeFormat.FirstDayOfWeek.ToString();

            throw new FormatException("Invalid format string");
        }

        /// <summary>
        ///     Gets the ID of the timezone
        /// </summary>
        public TimeZoneInfo GetSystemTimeZone()
        {
            return TimeZoneInfo.FindSystemTimeZoneById(IanaId);
        }

        public string IanaId { get; private init; }

        public CultureInfo Culture => _info.Culture;

        public DayOfWeek[] DaysOfWeek => _info.DaysOfWeek;
        public ZonedClock Clock => _info.Clock;
        public Offset Offset => _info.Clock.GetCurrentOffsetDateTime().Offset;
        public CalendarSystem Calendar => _info.Clock.Calendar;

        public bool Equals(ITimezone? other)
        {
            if (other is null)
                return false;
            return IanaId.Equals(other.IanaId, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="LocalTimezone" />.
        /// </summary>
        /// <param name="ianaId">A valid IANA ID.</param>
        /// <returns>A <see cref="LocalTimezone" /> object</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="ianaId" /> is null.</exception>
        public static LocalTimezone GetOrCreate(string? ianaId)
        {
            ArgumentNullException.ThrowIfNull(ianaId);
            return GetOrCreate(DateTimeZoneProviders.Tzdb[ianaId]);
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="LocalTimezone" />.
        /// </summary>
        /// <param name="zone">A Zone to use.</param>
        /// <returns>A <see cref="LocalTimezone" /> object</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="zone" /> is null.</exception>
        public static LocalTimezone GetOrCreate(DateTimeZone? zone)
        {
            ArgumentNullException.ThrowIfNull(zone);
            Cached ??= new ConcurrentDictionary<string, LocalTimezone>();
            Mappings ??= new LocalTimezoneMapCollection();

            if (Cached.TryGetValue(zone.Id, out var cachedTimezone))
                return cachedTimezone!;

            cachedTimezone = new LocalTimezone
            {
                IanaId = zone.Id,
                _info = Mappings.TryGetValue(zone.Id, out var resolver) && resolver != null ? resolver : new DefaultTimezone(zone, CultureInfo.InvariantCulture)
            };
            Cached.TryAdd(cachedTimezone.IanaId, cachedTimezone);
            return cachedTimezone;
        }

        private static LocalTimezoneInfo GetUtcMap()
        {
            if (Mappings.TryGetValue("UTC", out var map) && map != null)
                return map;

            return new UtcTimezone();
        }

        private static DateTimeZone GetDefaultZone()
        {
            return DateTimeZoneProviders.Tzdb.GetSystemDefault();
        }

        public static implicit operator LocalTimezone(string? ianaId)
        {
            return GetOrCreate(ianaId);
        }

        public static implicit operator string(LocalTimezone timezone)
        {
            return timezone.IanaId;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("GMT");
            sb.Append(Offset.ToString("m", CultureInfo.CurrentCulture));
            return sb.ToString();
        }

        public static int CompareTo(LocalTimezone? left, LocalTimezone? right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left is null) return -1;
            if (right is null) return 1;
            return left.CompareTo(right);
        }

        public static bool Equal(LocalTimezone? left, LocalTimezone? right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public override bool Equals(object? obj)
        {
            return obj is LocalTimezone other && Equals(other);
        }

        public override int GetHashCode()
        {
            return IanaId.GetHashCode();
        }

        public static bool operator ==(LocalTimezone? left, LocalTimezone? right)
        {
            return Equal(left, right);
        }

        public static bool operator !=(LocalTimezone? left, LocalTimezone? right)
        {
            return !Equal(left, right);
        }

        public static bool operator <(LocalTimezone? left, LocalTimezone? right)
        {
            return CompareTo(left, right) < 0;
        }

        public static bool operator >(LocalTimezone? left, LocalTimezone? right)
        {
            return CompareTo(left, right) > 0;
        }

        public static bool operator <=(LocalTimezone? left, LocalTimezone? right)
        {
            return CompareTo(left, right) <= 0;
        }

        public static bool operator >=(LocalTimezone? left, LocalTimezone? right)
        {
            return CompareTo(left, right) >= 0;
        }
    }
}