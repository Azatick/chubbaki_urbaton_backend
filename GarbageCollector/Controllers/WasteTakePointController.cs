using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Domain;
using GarbageCollector.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarbageCollector.Controllers
{
    [Route("api/[controller]"), EnableCors("MyPolicy")]
    public class WasteTakePointController : Controller
    {
        private GarbageCollectorContext _dbcontext;
        private IMapper _mapper;
        
        public WasteTakePointController(GarbageCollectorContext dbContext, IMapper mapper)
        {
            _dbcontext = dbContext;
            _mapper = mapper;
        }
        
        [HttpGet]
        public List<WasteTakePointViewModel> List()
        {
            var points = _mapper.Map<List<WasteTakePointViewModel>>(_dbcontext.WasteTakePoints.Include(x => x.Location));
            return points;
        }
    }
}