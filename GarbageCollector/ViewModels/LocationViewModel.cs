using System;

namespace GarbageCollector.ViewModels
{
    public class LocationViewModel
    {
        public Guid Id { get; set; }
        
        public string Address { get; set; }
        
        public double Longitude { get; set; }
        
        public double Latitude { get; set; }
    }
}