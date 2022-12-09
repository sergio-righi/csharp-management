using Domain;
using Domain.Base;
using Domain.Entity;
using Tool.Extensions;
using Domain.Interface;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Base
{
    public class BaseRepository<T> : SpecificMethod<T>, IRepository<T> where T : class, IEntity
    {
        protected readonly DbSet<T> DbSet;
        protected readonly DBContext DbContext;

        protected BaseRepository(DBContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<T>();
        }

        public void Dispose()
        {
            DbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual async Task<T> Insert(T entity)
        {
            var r = await DbSet.AddAsync(entity);
            await CommitAsync();
            return r.Entity;
        }

        public virtual async Task<int> InsertRange(IEnumerable<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
            return await CommitAsync();
        }

        public virtual async Task<IEnumerable<T>> List(Func<IQueryable<T>, IOrderedQueryable<T>> order = null, params string[] includeProperties)
        {
            IQueryable<T> query = DbSet;

            query = GenerateIncludeProperties(query, includeProperties);

            if (order != null)
                return await Task.FromResult(order(query.AsNoTracking()));

            return await Task.FromResult(query.AsNoTracking());
        }

        public virtual async Task<IEnumerable<T>> List(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, params string[] includeProperties)
        {
            IQueryable<T> query = DbSet;

            query = GenerateQueryableWhereExpression(query, filter);
            query = GenerateIncludeProperties(query, includeProperties);

            if (order != null)
                return await Task.FromResult(order(query.AsNoTracking()).AsEnumerable());

            return await Task.FromResult(query.AsNoTracking().AsEnumerable());
        }

        public virtual async Task<IQueryable<T>> GetQueryable(Expression<Func<T, bool>> filter = null, params string[] includeProperties)
        {
            IQueryable<T> query = DbSet;

            query = GenerateQueryableWhereExpression(query, filter);
            query = GenerateIncludeProperties(query, includeProperties);

            return await Task.FromResult(query.AsQueryable().AsNoTracking());
        }

        public virtual async Task<Pagination<T>> GetQueryablePagination(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> order = null, EDirection direction = EDirection.Descending, int page = 1, int count = int.MaxValue, params string[] includeProperties)
        {
            IQueryable<T> query = DbSet;

            query = GenerateQueryablePaginationWhereExpression(query, filter, order, direction, page, count, out int total);
            query = GenerateIncludeProperties(query, includeProperties);

            return await Task.FromResult(await Pagination<T>.CreateAsync(query.AsQueryable().AsNoTracking(), total, page, count));
        }

        public virtual async Task<T> Find(params object[] id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<int> Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            DbContext.Entry(entity).Property(nameof(entity.CreatedAt)).IsModified = false;
            DbContext.Entry(entity).Property(nameof(entity.CreatedBy)).IsModified = false;
            return await CommitAsync();
        }

        public virtual async Task<int> UpdateRange(IEnumerable<T> entities)
        {
            DbSet.UpdateRange(entities);
            return await CommitAsync();
        }

        public virtual async Task<bool> Delete(params object[] id)
        {
            T entity = await Find(id);

            if (entity == null) return false;

            return await Delete(entity) > 0 ? true : false;
        }

        public virtual async Task<int> Delete(T entity)
        {
            DbSet.Remove(entity);
            return await CommitAsync();
        }

        public virtual async Task<int> DeleteRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
            return await CommitAsync();
        }

        private async Task<int> CommitAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        protected override IQueryable<T> GenerateQuery(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params string[] includeProperties)
        {
            IQueryable<T> query = DbSet;
            query = GenerateQueryableWhereExpression(query, filter);
            query = GenerateIncludeProperties(query, includeProperties);

            if (orderBy != null)
                return orderBy(query);

            return query;
        }

        private IQueryable<T> GenerateQueryableWhereExpression(IQueryable<T> query,
            Expression<Func<T, bool>> filter)
        {
            if (filter != null)
                return query.Where(filter);

            return query;
        }

        private IQueryable<T> GenerateQueryablePaginationWhereExpression(IQueryable<T> query,
            Expression<Func<T, bool>> filter,
            Expression<Func<T, object>> order,
            EDirection direction,
            int page,
            int count,
            out int total)
        {

            if (filter != null)
                query = query
                        .Where(filter);

            if (order != null)
                query = query
                        .OrderBy(order, direction);

            if (query != null)
                total = query.Count();
            else
                total = 0;

            return query
                    .AsNoTracking()
                    .Skip((page - 1) * count)
                    .Take(count);
        }

        private IQueryable<T> GenerateIncludeProperties(IQueryable<T> query, params string[] includeProperties)
        {
            foreach (string includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return query;
        }

        protected override IEnumerable<T> GetYieldManipulated(IEnumerable<T> entities, Func<T, T> DoAction)
        {
            foreach (var e in entities)
            {
                yield return DoAction(e);
            }
        }
    }
}
