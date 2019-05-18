using GarbageCollector.Domain;
using Microsoft.EntityFrameworkCore;

namespace GarbageCollector.Database.Dbos
{
    public class GarbageCollectorContext : DbContext
    {
        public GarbageCollectorContext(DbContextOptions<GarbageCollectorContext> options) : base(options)
        {
        }

        public DbSet<LocationDbo> Locations { get; set; }
        public DbSet<WasteCategoryDbo> WasteCategories { get; set; }
        public DbSet<WasteTakePointDbo> WasteTakePoints { get; set; }
        public DbSet<GarbageAppUserDbo> AppUsers { get; set; }
    }
}