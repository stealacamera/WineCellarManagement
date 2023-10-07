using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;

namespace WineCellar.DataAccess.Repositories
{
    public class AppRoleRepository : Repository<AppRole>, IAppRoleRepository
    {
        private readonly AppDbContext db;

        public AppRoleRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }
    }
}
