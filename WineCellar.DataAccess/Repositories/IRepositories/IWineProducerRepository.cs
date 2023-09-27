using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories.IRepositories
{
    public interface IWineProducerRepository : IRepository<WineProducer>
    {
        void Update(WineProducer wineProducer);
    }
}
