using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;

namespace GarbageCollector.Database.Dbos
{
    [Owned]
    public class LocationDbo
    {

        public string Address { get; set; }
        public IPoint Coordinates { get; set; }
    }
}