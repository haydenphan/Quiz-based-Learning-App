using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
    }
}
