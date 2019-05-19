using System.Collections.Generic;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GarbageCollector.Controllers
{
    [Route("api/[controller]")]
    public class WasteCategoriesController : Controller
    {
        private GarbageCollectorContext _dbcontext;
        private IMapper _mapper;
        
        public WasteCategoriesController(GarbageCollectorContext dbContext, IMapper mapper)
        {
            _dbcontext = dbContext;
            _mapper = mapper;
        }
        
        [HttpGet]
        public List<WasteCategory> List()
        {
            return _mapper.Map<List<WasteCategory>>(_dbcontext.WasteCategories);
        }
    }
}