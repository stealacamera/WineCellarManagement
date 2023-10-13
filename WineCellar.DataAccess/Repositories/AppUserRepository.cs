using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private readonly AppDbContext db;

        public AppUserRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }
    }
}
