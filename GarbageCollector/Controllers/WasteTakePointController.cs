using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarbageCollector.Controllers
{
    [Route("api/[controller]")]
    public class WasteTakePointController : Controller
    {
        private GarbageCollectorContext _dbcontext;
        
        public WasteTakePointController(GarbageCollectorContext dbContext)
        {
            _dbcontext = dbContext;
        }
        
        [HttpGet]
        public List<WasteTakePointDbo> List()
        {
           return _dbcontext.WasteTakePoints.Include(x => x.Location).ToList();
        }
    }
}