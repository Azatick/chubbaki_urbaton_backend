using System.Threading.Tasks;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using Microsoft.EntityFrameworkCore;

namespace GarbageCollector.Domain.Services
{
    public class CategoriesService
    {
        private GarbageCollectorContext dbContext;
        private IMapper mapper;

        public CategoriesService(GarbageCollectorContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<WasteCategory[]> GetAllCategoriesAsync()
        {
            return mapper.Map<WasteCategory[]>((await dbContext.WasteCategories.ToArrayAsync().ConfigureAwait(false)));
        }
    }
}