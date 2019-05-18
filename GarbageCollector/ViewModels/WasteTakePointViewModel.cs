using System;

namespace GarbageCollector.ViewModels
{
    public class WasteTakePointViewModel
    {
        public Guid Id { get; set; }
        public LocationViewModel Location { get; set; }
        public string Name { get; set; }
    }
}