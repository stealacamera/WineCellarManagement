using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WineCellar.Models;

namespace WineCellar.DataAccess
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Varietal> Varietals { get; set; }
        public DbSet<WineProducer> WineProducers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Renaming tables to singular names
            builder.Entity<Varietal>().ToTable("Varietal");
            builder.Entity<WineProducer>().ToTable("WineProducer");
            builder.Entity<Country>().ToTable("Country");
            builder.Entity<Region>().ToTable("Region");
        }
    }
}
