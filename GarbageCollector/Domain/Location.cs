using System;
using GeoAPI.Geometries;

namespace GarbageCollector.Domain
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public IPoint Coordinates { get; set; }
    }
}