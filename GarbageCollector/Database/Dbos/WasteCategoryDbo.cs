using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GarbageCollector.Domain;

namespace GarbageCollector.Database.Dbos
{
    public class WasteCategoryDbo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public Material Material { get; set; }
    }
}