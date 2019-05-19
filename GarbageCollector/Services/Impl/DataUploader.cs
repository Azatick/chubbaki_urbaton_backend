using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Domain;
using GarbageCollector.Domain.Services;
using GarbageCollector.Extensions;
using Microsoft.AspNetCore.Mvc.Formatters;
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
            var wasteCategoryDbosToAdd = categoriesByMateria.SelectMany(x => x.Categories.Select(c =>
                new WasteCategoryDbo()
                {
                    Id = Guid.NewGuid(),
                    Material = GetEnumValueFromDescription<Material>(x.Material),
                    Name = c
                })).Where(x => !_dbContext.WasteCategories.Any(c => c.Name == x.Name)).ToArray();
            _dbContext.WasteCategories.AddRange(wasteCategoryDbosToAdd);
            _dbContext.SaveChanges();
        }

        public async Task MapPointsToCategoriesAsync()
        {
            var file = File.ReadAllLines(_options.Value.PointsToCatMapPath);
            var pointsWithCategories = file
                .Select(
                    x => x.Split("_", StringSplitOptions.RemoveEmptyEntries).Select(y => y.Trim()).ToArray()
                )
                .Where(x =>
                {
                    if (!x.IsNullOrEmpty() && x.Count() > 1)
                    {
                        return true;
                    }

                    Console.WriteLine("Bad Line: " + string.Join(" _ ", x));
                    return false;
                })
                .Select(
                    x => (PointName: x[0], Categories: x[1]
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(y => y.Trim()).Distinct().ToArray())
                ).ToArray();
            foreach (var pointWithCategories in pointsWithCategories)
            {
                var points = await
                    _dbContext.WasteTakePoints
                        .Where(p => p.Name == pointWithCategories.PointName).ToListAsync().ConfigureAwait(false);
                var categories = await _dbContext.WasteCategories.Where(c => pointWithCategories.Categories.Contains(c
                    .Name)).ToListAsync().ConfigureAwait(false);
                if (!points.IsNullOrEmpty() && categories.Any())
                {
                    points.Select(point => point.LinksToCategories = categories.Select(c => new
                        WasteTakePointToCategoryLinkDbo()
                        {
                            Id = Guid.NewGuid(),
                            CategoryId = c.Id,
                            WasteTakePointId = point.Id
                        }).ToList()).ToList();
                }
                else
                {
                    Console.WriteLine(
                        $"Bad Line: {pointWithCategories.PointName} _ {string.Join(',', pointWithCategories.Categories)}");
                }
            }

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CreateDefaultUser()
        {
            const string userJson =
                "{\r\n  \"id\": \"7a0939c4-2233-46a5-a7f1-7927c9ea133b\",\r\n  \"login\": \"Pupkin\",\r\n  \"trashCans\":[\r\n    {}\r\n  ],\r\n  \"currentLocation\": {\r\n     \"address\": \"string\",\r\n    \"longitude\": 49.145088,\r\n    \"latitude\": 55.775596\r\n  }\r\n}";
            using (var cli = new HttpClient())
            {
                await cli.PostAsync("http://localhost:5010/user/signup", new StringContent(userJson, Encoding.UTF8,
                    "application/json")).ConfigureAwait(false);
            }
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            MemberInfo[] fis = typeof(T).GetFields();

            foreach (var fi in fis)
            {
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0 && attributes[0].Description == description)
                    return (T) Enum.Parse(typeof(T), fi.Name);
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