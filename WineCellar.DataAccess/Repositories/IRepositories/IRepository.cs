using System.Linq.Expressions;

namespace WineCellar.DataAccess.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>? orderBy = null, bool descendingOrder = true,
            int? pageNumber = null, int pageSize = 10,
            params string[] includeProps
        );

        T? GetFirstOrDefault(Expression<Func<T, bool>> filter, params string[] includeProps);

        T GetFirst(Expression<Func<T, bool>> filter, params string[] includeProps);

        void Add(T instance);

        void Remove(T instance);

        void RemoveRange(IEnumerable<T> instances);

        int Count();
    }
}
