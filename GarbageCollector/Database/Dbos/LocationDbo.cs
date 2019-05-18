using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoAPI.Geometries;

namespace GarbageCollector.Database.Dbos
{
    public class LocationDbo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Address { get; set; }
        public IPoint Coordinates { get; set; }
    }
}