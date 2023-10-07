using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories.IRepositories
{
    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        void Update(Establishment establishment);
    }
}
