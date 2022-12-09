using Domain.Entity;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Interface
{
    public interface IOvertimeRepository : IDomainRepository<Overtime>
    {
        Task<Overtime> Find(int id);
        Task<Pagination<SOvertime>> SList(int? personId, DateTime? date, Expression<Func<SOvertime, object>> order, EDirection direction, int page, int count);
    }
}
