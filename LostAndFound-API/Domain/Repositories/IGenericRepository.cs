using System.Linq.Expressions;

namespace LostAndFound_API.Domain.Repositories
{
    public interface IGenericRepository
    {
        Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate, bool ignoreQueryFilters = false) where T : class;

        Task<ICollection<T>> ListAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task<ICollection<T>> ListAsync<T>() where T : class;

        Task AddAsync<T>(T entity) where T : class;

        Task AddRangeAsync<T>(List<T> entity) where T : class;

        void UpdateAsync<T>(T entity) where T : class;

        void Remove<T>(T entity) where T : class;
    }
}
