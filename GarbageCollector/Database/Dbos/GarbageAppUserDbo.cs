using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarbageCollector.Database.Dbos
{
    public class GarbageAppUserDbo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Login { get; set; }
        public Guid CurrentLocationId { get; set; }
        [ForeignKey("CurrentLocationId")]
        public LocationDbo CurrentLocation { get; set; }

        public TrashCanDbo[] TrashCans { get; set; }
    }
}