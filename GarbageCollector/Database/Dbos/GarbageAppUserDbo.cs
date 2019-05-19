using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarbageCollector.Database.Dbos
{
    public class GarbageAppUserDbo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Login { get; set; }

        public LocationDbo CurrentLocation { get; set; }

        public List<TrashCanDbo> TrashCans { get; set; }
    }
}