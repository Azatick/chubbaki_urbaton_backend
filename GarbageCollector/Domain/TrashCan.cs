using System;
using System.Collections.Generic;

namespace GarbageCollector.Domain
{
    public class TrashCan
    {
        public Guid Id { get; set; }
        public WasteTakePoint WasteTakePoint { get; set; }
        public ICollection<WasteCategory> WasteCategories { get; set; }
    }
}