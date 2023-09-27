using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories
{
    public class VarietalRepository : Repository<Varietal>, IVarietalRepository
    {
        private readonly AppDbContext db;

        public VarietalRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(Varietal varietal)
        {
            db.Update(varietal);
        }
    }
}
