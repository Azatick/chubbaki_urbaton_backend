using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Domain;
using GarbageCollector.Domain.Services;
using GarbageCollector.Extensions;
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

        public void ImportCategories()
        {
            var file = File.ReadAllLines(_options.Value.CategoriesPath);
            var categoriesByMateria = file.Select(x => x.Split("_", StringSplitOptions.RemoveEmptyEntries).Select(y 
            => y.Trim()).ToArray()).Where(x =>
                {
                    if (!x.IsNullOrEmpty() && x.Count() > 1)
                    {
                        return true;
                    }

                    Console.WriteLine("Bad Line: " + string.Join(" _ ", x));
                    return false;
                })
                .ToLookup(x => x[0], x => x[1].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(y => y.Trim()
                ).ToArray()).Select(group =>
                {
                    var materialName = @group.Key;
                    var categoriesNames = @group.SelectMany(x => x).Distinct().ToArray();
                    return (Material: materialName, Categories: categoriesNames);
                }).ToArray();
            var wasteCategoryDbosToAdd = categoriesByMateria.SelectMany(x => x.Categories.Select(c => new WasteCategoryDbo()
            {
                Id = Guid.NewGuid(),
                Material = GetEnumValueFromDescription<Material>(x.Material),
                Name = c
            })).Where(x => !_dbContext.WasteCategories.Any(c => c.Name == x.Name)).ToArray();
            _dbContext.WasteCategories.AddRange(wasteCategoryDbosToAdd);
            _dbContext.SaveChanges();
        }
        public static T GetEnumValueFromDescription<T>(string description)
        {
            MemberInfo[] fis = typeof(T).GetFields();

            foreach (var fi in fis)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0 && attributes[0].Description == description)
                    return (T)Enum.Parse(typeof(T), fi.Name);
            }

            throw new Exception("Not found");
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