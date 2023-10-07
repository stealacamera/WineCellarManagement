using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories
{
    public class EstablishmentRepository : Repository<Establishment>, IEstablishmentRepository
    {
        private readonly AppDbContext db;

        public EstablishmentRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(Establishment establishment)
        {
            db.Update(establishment);
        }
    }
}
