using System;
using GarbageCollector.Domain;

namespace GarbageCollector.ViewModels
{
    public class TrashCanViewModel
    {
        public Guid Id { get; set; }
        public WasteTakePointViewModel WasteTakePoint { get; set; }
        public WasteCategory[] WasteCategories { get; set; }
    }
}