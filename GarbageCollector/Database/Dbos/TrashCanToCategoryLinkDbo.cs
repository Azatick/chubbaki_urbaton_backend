using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarbageCollector.Database.Dbos
{
    public class TrashCanToCategoryLinkDbo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } 
        public Guid TrashCanId { get; set; }
        [ForeignKey("TrashCanId")] 
        public TrashCanDbo TrashCan { get; set; }
        
        
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")] 
        public WasteCategoryDbo Category { get; set; }
    }
}