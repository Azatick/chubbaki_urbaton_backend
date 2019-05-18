using System;
using System.Threading.Tasks;

namespace GarbageCollector.Domain.Services
{
    public class WasteTakePointService
    {
        public  async Task<WasteTakePoint[]> GetNearestTakePoinsAsync(Location location)
        {
            return Array.Empty<WasteTakePoint>();
        }
    }
}