
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using Tool.Utilities;

namespace Domain.Interface
{
    public interface IService<T> where T : class, IEntity
    {
        Task<T> Insert(T entity);
        Task InsertRange(IEnumerable<T> entities);

        Task<T> Find(params object[] id);
        Task<IEnumerable<T>> List(Func<IQueryable<T>, IOrderedQueryable<T>> order = null, params string[] includeProperties);
        Task<IEnumerable<T>> List(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, params string[] includeProperties);
        Task<IQueryable<T>> GetQueryable(Expression<Func<T, bool>> filter = null, params string[] includeProperties);
        Task<Pagination<T>> GetQueryablePagination(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> order = null, EDirection direction = EDirection.Descending, int page = 1, int count = int.MaxValue, params string[] includeProperties);

        Task Update(T entity);
        Task UpdateRange(IEnumerable<T> entities);

        Task<bool> Delete(params object[] id);
        Task Delete(T entity);
        Task DeleteRange(IEnumerable<T> entities);
    }
}
