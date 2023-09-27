using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly AppDbContext db;

        public CountryRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(Country country)
        {
            db.Update(country);
        }
    }
}
