using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using WineCellar.Models;
using WineCellar.Utilities;

namespace WineCellar.DataAccess.Seeder
{
    public class DbSeeder : IDbSeeder
    {
        private readonly AppDbContext db;
        private readonly IHostEnvironment hostEnv;
        private readonly string dirPath;

        private readonly RoleManager<AppRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public DbSeeder(
            AppDbContext db, 
            IHostEnvironment hostEnv,
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager)
        {
            this.db = db;
            this.hostEnv = hostEnv;
            this.roleManager = roleManager;
            this.userManager = userManager;

            // Gets path to data files folder
            dirPath = Path.Combine(this.hostEnv.ContentRootPath, @"..\WineCellar.DataAccess\Seeder\Data");
        }

        public void Initialize()
        {
            db.Database.EnsureCreated();

            try
            {
                if (db.Database.GetPendingMigrations().Count() > 0)
                    db.Database.Migrate();
            }
            catch (Exception)
            {
                throw;
            }

            SeedRoles();
            SeedAdmin();

            SeedDummyData();

            SeedVarietals();
            SeedWineProducers();
            SeedCountries();
            SeedRegions();
        }

        private void SeedDummyData()
        {
            if(db.Establishments.Count() == 0)
            {
                for(int i = 1; i <= 3; i++)
                {
                    Establishment establishment = new Establishment { Name = $"Establishment{i}" };
                    db.Establishments.Add(establishment);
                    db.SaveChanges();

                    // Creates manager
                    createSimpleUser($"Manager{establishment.Id}", $"manager{establishment.Id}@gmail.com", establishment.Id, Consts.Role_Manager);

                    // Creates 3 employees per manager
                    createSimpleUser($"Emp{establishment.Id}a", $"emp{establishment.Id}a@gmail.com", establishment.Id, Consts.Role_Employee);
                    createSimpleUser($"Emp{establishment.Id}b", $"emp{establishment.Id}b@gmail.com", establishment.Id, Consts.Role_Employee);
                    createSimpleUser($"Emp{establishment.Id}c", $"emp{establishment.Id}c@gmail.com", establishment.Id, Consts.Role_Employee);
                }
            }
        }

        private AppUser createSimpleUser(string username, string email, int establishmentId, string role)
        {
            AppUser user = new AppUser { UserName = username, Email = email, EmailConfirmed = true, EstablishmentId = establishmentId };

            userManager.CreateAsync(user, "Password123//").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();

            return user;
        }

        private void SeedRoles()
        {
            if (!roleManager.RoleExistsAsync(Consts.Role_Admin).GetAwaiter().GetResult())
            {
                string[] roles = { Consts.Role_Admin, Consts.Role_Manager, Consts.Role_Employee };

                foreach(string role in roles)
                    roleManager.CreateAsync(new AppRole(role)).GetAwaiter().GetResult();
            }
        }

        private void SeedAdmin()
        {
            string email = "admin@gmail.com";

            if(db.Users.FirstOrDefault(x => x.Email == email) == null)
            {
                AppUser admin = new AppUser { UserName = "Administrator", Email = email, EmailConfirmed = true };

                userManager.CreateAsync(admin, "Password123//").GetAwaiter().GetResult();
                userManager.AddToRoleAsync(admin, Consts.Role_Admin).GetAwaiter().GetResult();
            }
        }

        private void SeedVarietals()
        {
            if (db.Varietals.Any())
                return;

            foreach (string line in File.ReadLines(Path.Combine(dirPath, @"varietals.txt")))
                db.Varietals.Add(new Varietal { Name = line });

            db.SaveChanges();
        }

        private void SeedWineProducers()
        {
            if (db.WineProducers.Any())
                return;

            foreach (string line in File.ReadLines(Path.Combine(dirPath, @"wineproducers.txt")))
                db.WineProducers.Add(new WineProducer { Name = line });

            db.SaveChanges();
        }

        private void SeedCountries()
        {
            if (db.Countries.Any())
                return;

            foreach (string line in File.ReadLines(Path.Combine(dirPath, @"countries.txt")))
                db.Countries.Add(new Country { Name = line });

            db.SaveChanges();
        }

        private void SeedRegions()
        {
            if (db.Regions.Any())
                return;

            Country? parent = new Country();

            foreach (string line in File.ReadLines(Path.Combine(dirPath, @"regions.txt")))
            {
                // # denotes a country name, followed by its regions
                if (line[0] == '#')
                {
                    string countryName = line.Substring(1);
                    parent = db.Countries.FirstOrDefault(x => x.Name == countryName);

                    if (parent == null)
                        throw new InvalidDataException($"Country name ({countryName}) doesn't exist");

                    continue;
                }

                db.Regions.Add(new Region { Name = line, CountryId = parent.Id });
            }

            db.SaveChanges();
        }
    }
}
