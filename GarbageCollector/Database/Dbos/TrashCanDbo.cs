using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarbageCollector.Database.Dbos
{
    public class TrashCanDbo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        [ForeignKey("UserId")] 
        public GarbageAppUserDbo User { get; set; }
        public string Name { get; set; } 

        public Guid WasteTakePointId { get; set; }
        [ForeignKey("WasteTakePointId")] 
        public WasteTakePointDbo WasteTakePoint { get; set; }
        public List<TrashCanToCategoryLinkDbo> LinksToCategories { get; set; }
    }
}