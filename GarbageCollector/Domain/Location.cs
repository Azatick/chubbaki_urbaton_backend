using System;
using GeoAPI.Geometries;

namespace GarbageCollector.Domain
{
    public class Location
    {
        public string Address { get; set; }
        public IPoint Coordinates { get; set; }
    }
}