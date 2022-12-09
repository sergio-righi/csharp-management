using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Interface
{
    public interface ISaleRepository : IDomainRepository<Sale>
    {
        Task<Pagination<Sale>> List(int? personId, ESale situationId, Expression<Func<Sale, object>> order, EDirection direction, int page, int count);
    }
}
