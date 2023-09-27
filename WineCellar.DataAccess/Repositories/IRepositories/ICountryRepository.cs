using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories.IRepositories
{
    public interface ICountryRepository : IRepository<Country>
    {
        void Update(Country country);
    }
}
