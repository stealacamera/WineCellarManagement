using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories.IRepositories
{
    public interface IRegionRepository : IRepository<Region>
    {
        void Update(Region region);
    }
}
