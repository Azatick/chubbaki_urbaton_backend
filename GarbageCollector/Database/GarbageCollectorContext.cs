using GarbageCollector.Domain;
using Microsoft.EntityFrameworkCore;

namespace GarbageCollector.Database.Dbos
{
    public class GarbageCollectorContext : DbContext
    {
        public GarbageCollectorContext(DbContextOptions<GarbageCollectorContext> options) : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<WasteCategory> WasteCategories { get; set; }
        public DbSet<WasteTakePoint> WasteTakePoints { get; set; }
        public DbSet<GarbageAppUser> AppUsers { get; set; }
    }
}