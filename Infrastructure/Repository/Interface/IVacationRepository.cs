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
    public interface IVacationRepository : IDomainRepository<Vacation>
    {
        Task<Vacation> Find(int id);
        Task<Pagination<SVacation>> SList(int? personId, int? year, Expression<Func<SVacation, object>> order, EDirection direction, int page, int count);
    }
}
