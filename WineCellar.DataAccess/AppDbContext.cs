using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WineCellar.Models;

namespace WineCellar.DataAccess
{
    public class AppDbContext : IdentityDbContext<
        AppUser, AppRole, string, 
        IdentityUserClaim<string>, AppUserRole, 
        IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Varietal> Varietals { get; set; }
        public DbSet<WineProducer> WineProducers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }

        public DbSet<Establishment> Establishments { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Linking custom Identity and Role to default join table
            builder.Entity<AppUser>()
                .HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<AppUserRole>(
                    l => l.HasOne(y => y.Role).WithMany().HasForeignKey(y => y.RoleId),
                    r => r.HasOne(y => y.User).WithMany().HasForeignKey(y => y.UserId));

            // Renaming tables to singular names
            builder.Entity<Varietal>().ToTable("Varietal");
            builder.Entity<WineProducer>().ToTable("WineProducer");
            builder.Entity<Country>().ToTable("Country");
            builder.Entity<Region>().ToTable("Region");
            builder.Entity<Establishment>().ToTable("Establishment");
        }
    }
}
