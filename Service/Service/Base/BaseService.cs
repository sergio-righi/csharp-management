using Domain.Base;
using Domain.Interface;
using FluentValidation;
using Infrastructure.Repository.Base;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Base
{
    public class BaseService<T> : IService<T> where T : class, IEntity
    {
        private readonly IRepository<T> Repository;

        public BaseService(IRepository<T> repository)
        {
            Repository = repository;
        }

        public virtual async Task<T> Insert(T entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = entity.CreatedBy;
            return await Repository.Insert(entity);
        }

        public virtual async Task InsertRange(IEnumerable<T> entities)
        {
            await Repository.InsertRange(entities);
        }

        public virtual async Task<IEnumerable<T>> List(Func<IQueryable<T>, IOrderedQueryable<T>> order = null, params string[] includeProperties)
        {
            return await Repository.List(order, includeProperties);
        }

        public virtual async Task<IEnumerable<T>> List(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, params string[] includeProperties)
        {
            return await Repository.List(filter, order, includeProperties);
        }

        public virtual async Task<IQueryable<T>> GetQueryable(Expression<Func<T, bool>> filter = null, params string[] includeProperties)
        {
            return await Repository.GetQueryable(filter, includeProperties);
        }

        public virtual async Task<Pagination<T>> GetQueryablePagination(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> order = null, EDirection direction = EDirection.Descending, int page = 1, int count = int.MaxValue, params string[] includeProperties)
        {
            return await Repository.GetQueryablePagination(filter, order, direction, page, count, includeProperties);
        }

        public virtual async Task<T> Find(params object[] id)
        {
            return await Repository.Find(id);
        }

        public virtual async Task<bool> Delete(params object[] id)
        {
            return await Repository.Delete(id);
        }

        public virtual async Task Delete(T entity)
        {
            await Repository.Delete(entity);
        }

        public virtual async Task DeleteRange(IEnumerable<T> entities)
        {
            await Repository.DeleteRange(entities);
        }

        public virtual async Task Update(T entity)
        {
            entity.UpdatedAt = DateTime.Now;
            await Repository.Update(entity);
        }

        public virtual async Task UpdateRange(IEnumerable<T> entities)
        {
            await Repository.UpdateRange(entities);
        }

        //public T Put<V>(T entity) where V : AbstractValidator<T>
        //{
        //    Validate(entity, Activator.CreateInstance<V>());

        //    repository.Update(entity);
        //    return entity;
        //}

        //public void Delete(T entity)
        //{
        //    if (entity.id == 0)
        //        throw new ArgumentException("The id can't be zero.");

        //    repository.Delete(entity);
        //}

        private void Validate(T entity, AbstractValidator<T> validator)
        {
            if (entity == null)
                throw new Exception("Registros não detectados!");

            validator.ValidateAndThrow(entity);
        }
    }
}
