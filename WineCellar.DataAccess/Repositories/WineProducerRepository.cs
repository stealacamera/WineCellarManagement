using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories
{
    public class WineProducerRepository : Repository<WineProducer>, IWineProducerRepository
    {
        private readonly AppDbContext db;

        public WineProducerRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(WineProducer wineProducer)
        {
            db.Update(wineProducer);
        }
    }
}
