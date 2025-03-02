using System.Collections.Generic;

namespace R8.DateTimeLocalization
{
    public class GroupedTimezones
    {
        public string Offset { get; init; }
        public Dictionary<string, Timezone> IanaIds { get; init; }

        public class Timezone
        {
            public string DisplayName { get; init; }
            public bool IsActive { get; set; }
            public bool FullySupported { get; init; }
        }
    }
}