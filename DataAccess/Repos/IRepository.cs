using System.Linq.Expressions;

namespace DataAccess.Repos
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> FindAllAsync();
        Task<T?> FindAsync(int id);
        Task<T> AddAsync(T region);
        Task<T?> DeleteAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
    }
}
