using System;
using System.Collections;
using System.Collections.Generic;

namespace R8.DateTimeLocalization
{
    /// <summary>
    ///     A collection of <see cref="ITimezone" />.
    /// </summary>
    public class LocalTimezoneMapCollection : IEnumerable<LocalTimezoneInfo>
    {
        private readonly Dictionary<string, LocalTimezoneInfo> _dictionary = new();

        private readonly object _syncRoot = new();

        internal LocalTimezoneMapCollection()
        {
            if (!_dictionary.ContainsKey(UtcTimezone.UtcIanaId)) Create<UtcTimezone>();
        }

        /// <summary>Gets or sets the element with the specified key.</summary>
        /// <param name="ianaId">The key of the element to get or set.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="ianaId" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="ianaId" /> is not found.</exception>
        /// <returns>A <see cref="ITimezone" /> object.</returns>
        public LocalTimezoneInfo this[string ianaId]
        {
            get
            {
                if (ianaId == null)
                    throw new ArgumentNullException(nameof(ianaId));
                lock (_syncRoot)
                {
                    return _dictionary[ianaId];
                }
            }
        }

        public LocalTimezoneInfo this[ushort index]
        {
            get
            {
                lock (_syncRoot)
                {
                    foreach (var timezone in _dictionary.Values)
                        if (timezone._index == index)
                            return timezone;
                }

                throw new KeyNotFoundException($"The timezone with index '{index}' was not found.");
            }
        }

        public IEnumerator<LocalTimezoneInfo> GetEnumerator()
        {
            lock (_syncRoot)
            {
                return _dictionary.Values.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Add the specified mapper to the collection.
        /// </summary>
        /// <param name="value">A mapper that implements <see cref="ITimezone" />.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value" /> is <see langword="null" />.</exception>
        /// <typeparam name="TMap">Any type that implements <see cref="ITimezone" />.</typeparam>
        public LocalTimezone GetOrCreate<TMap>() where TMap : LocalTimezoneInfo, new()
        {
            lock (_syncRoot)
            {
                var map = Create<TMap>();
                return map.GetTimezone()!; // force to create cache
            }
        }

        private T Create<T>() where T : LocalTimezoneInfo, new()
        {
            var map = new T();
            if (!_dictionary.TryAdd(map.IanaId, map))
                return (T)_dictionary[map.IanaId];

            map._index = checked((ushort)(_dictionary.Count - 1));
            return map;
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.TryGetValue" />
        public bool TryGetValue(string key, out LocalTimezoneInfo? value)
        {
            lock (_syncRoot)
            {
                return _dictionary.TryGetValue(key, out value);
            }
        }

        public ushort GetIndex(string ianaId)
        {
            foreach (var timezone in this)
                if (timezone.IanaId == ianaId)
                    return timezone._index;

            throw new KeyNotFoundException($"The timezone '{ianaId}' was not found.");
        }
    }
}