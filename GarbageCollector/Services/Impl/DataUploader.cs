using System.Collections.Generic;
using System.IO;
using GarbageCollector.Domain;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GarbageCollector.Services.Impl
{
    public class DataUploader : IDataUploader
    {
        private IOptions<DomainOptions> _options;

        public DataUploader(IOptions<DomainOptions> options)
        {
            _options = options;
        }
        public IEnumerable<ImportModel> Upload()
        {
            var file = File.ReadAllText(_options.Value.JsonPath);
            var points = JsonConvert.DeserializeObject<List<ImportModel>>(file);

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