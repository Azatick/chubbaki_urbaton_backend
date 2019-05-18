using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarbageCollector.Domain
{
    public class WasteTakePoint
    {
        public Guid Id { get; set; }

        public Location Location { get; set; }
        public ICollection<WasteCategory> AcceptingCategories { get; set; }

        public static async Task<WasteTakePoint[]> GetNearestTakePoinsAsync(Location location)
        {
            return Array.Empty<WasteTakePoint>();
        }
    }
}