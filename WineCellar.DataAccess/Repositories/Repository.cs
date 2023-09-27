using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WineCellar.DataAccess.Repositories.IRepositories;

namespace WineCellar.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext db;
        protected DbSet<T> set;

        public Repository(AppDbContext db)
        {
            this.db = db;
            set = this.db.Set<T>();
        }

        public void Add(T instance)
        {
            set.Add(instance);
        }

        public int Count()
        {
            return set.Count();
        }

        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null, 
            Expression<Func<T, object>>? orderBy = null, bool descendingOrder = true, 
            int? pageNumber = null, int pageSize = 10, 
            params string[] includeProps
        )
        {
            IQueryable<T> query = set;

            if(filter != null)
                query = query.Where(filter);

            foreach(var prop in includeProps)
                query = query.Include(prop);

            if(orderBy != null)
                query = descendingOrder ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

            if(pageNumber != null)
                query = query.Skip((int)(pageSize * (pageNumber - 1))).Take(pageSize);

            return query.ToList();
        }

        public T? GetFirstOrDefault(Expression<Func<T, bool>> filter, params string[] includeProps)
        {
            IQueryable<T> query = set;

            foreach (var prop in includeProps)
                query = query.Include(prop);

            return query.FirstOrDefault(filter);
        }

        public void Remove(T instance)
        {
            set.Remove(instance);
        }

        public void RemoveRange(IEnumerable<T> instances)
        {
            set.RemoveRange(instances);
        }
    }
}
