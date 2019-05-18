using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace GarbageCollector.Services.Impl
{
    public class DataUploader : IDataUploader
    {
        private IOptions<DomainOptions> _options;

        private GarbageCollectorContext _dbContext;

        private IMapper _mapper;
        

        public DataUploader(IOptions<DomainOptions> options,
            GarbageCollectorContext dbContext, IMapper mapper)
        {
            _options = options;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<ImportModel> Upload()
        {
            var file = File.ReadAllText(_options.Value.JsonPath);
            var points = JsonConvert.DeserializeObject<List<ImportModel>>(file);

            var dboWasteTakePoints = points.Select(x => new WasteTakePointDbo
            {
                Id = Guid.NewGuid(),
                Location = new LocationDbo
                {
                    Id = Guid.NewGuid(),
                    Coordinates = new Point(x.Longitude, x.Latitude)

                },
                Name = x.Type
            }).ToList();
            
            _dbContext.WasteTakePoints.AddRange(dboWasteTakePoints);

            _dbContext.SaveChanges();

            return points;
        }
    }

    public class ImportModel
    {
        public string Name { get; set; }
        
        public double Longitude { get; set; }
        
        public double Latitude { get; set; }
        
        public string Type { get; set; }
    }
}