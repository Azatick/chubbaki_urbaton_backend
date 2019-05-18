using System;

namespace GarbageCollector.Domain
{
    public class WasteCategory
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public Material Material { get; set; }
    }
}