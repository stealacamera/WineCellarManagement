using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WineCellar.Models;

namespace WineCellar.DataAccess.Seeder
{
    public class DbSeeder : IDbSeeder
    {
        private readonly AppDbContext db;
        private readonly IHostEnvironment hostEnv;
        private readonly string dirPath;

        public DbSeeder(AppDbContext db, IHostEnvironment hostEnv)
        {
            this.db = db;
            this.hostEnv = hostEnv;

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

            SeedVarietals();
            SeedWineProducers();
            SeedCountries();
            SeedRegions();
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
