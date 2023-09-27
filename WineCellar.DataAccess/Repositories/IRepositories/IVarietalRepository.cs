using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories.IRepositories
{
    public interface IVarietalRepository : IRepository<Varietal>
    {
        void Update(Varietal varietal);
    }
}
