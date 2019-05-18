using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarbageCollector.Database.Dbos
{
    public class WasteTakePointToCategoryLinkDbo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } 
        public Guid WasteTakePointId { get; set; }
        [ForeignKey("WasteTakePointId")] 
        public WasteTakePointDbo WasteTakePoint { get; set; }
        
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")] 
        public WasteCategoryDbo Category { get; set; }
    }
}