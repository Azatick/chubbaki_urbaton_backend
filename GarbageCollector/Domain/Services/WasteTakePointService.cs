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
            var magicValue = GetKilometers(1);
            var takePointDbos = await context.WasteTakePoints.Include(x => x.Location).Include(x => x
            .LinksToCategories).ThenInclude(x => x.WasteTakePoint)
                .Where(x => x.Location.Coordinates.Distance(location.Coordinates) < magicValue)
                .OrderBy(x => x.Location.Coordinates.Distance(location.Coordinates))
                .ToArrayAsync().ConfigureAwait(false);
            return mapper.Map<WasteTakePoint[]>(takePointDbos);
        }

        private double GetKilometers(int kilometers)
        {
            return 0.015 * kilometers;
        }
    }
}