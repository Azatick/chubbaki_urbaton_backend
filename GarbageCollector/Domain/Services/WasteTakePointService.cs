using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using Microsoft.EntityFrameworkCore;

namespace GarbageCollector.Domain.Services
{
    public class WasteTakePointService
    {
        private GarbageCollectorContext context;
        private IMapper mapper;

        public WasteTakePointService(GarbageCollectorContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<WasteTakePoint[]> GetNearestTakePoinsAsync(Location location)
        {
            var takePointDbos = await context.WasteTakePoints
                .Where(x => x.Location.Coordinates.IsWithinDistance(location.Coordinates, 0.15))
                .OrderBy(x => x.Location.Coordinates.Distance(location.Coordinates))
                .ToArrayAsync().ConfigureAwait(false);
            return mapper.Map<WasteTakePoint[]>(takePointDbos);
        }
    }
}