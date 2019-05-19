using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarbageCollector.Database.Dbos
{
    public class WasteTakePointDbo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public LocationDbo Location { get; set; }
        public List<WasteTakePointToCategoryLinkDbo> LinksToCategories { get; set; }
    }
}