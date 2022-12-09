using Domain.Base;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Repository
{
    public abstract class SpecificMethod<T> where T : class, IEntity
    {
        protected abstract IQueryable<T> GenerateQuery(Expression<Func<T, bool>> filter = null,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                params string[] includeProperties);

        protected abstract IEnumerable<T> GetYieldManipulated(IEnumerable<T> entities, Func<T, T> DoAction);
    }
}
