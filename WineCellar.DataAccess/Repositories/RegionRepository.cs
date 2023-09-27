using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        private readonly AppDbContext db;

        public RegionRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(Region region)
        {
            db.Update(region);
        }
    }
}
