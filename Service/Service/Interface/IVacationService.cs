using Domain.Entity;
using Domain.Entity.Specific;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Interface
{
    public interface IVacationService : IService<Vacation>
    {
        Task<Vacation> Find(int id);
        Task<Pagination<SVacation>> SList(int? personId, int? year, Expression<Func<SVacation, object>> order, EDirection direction, int page, int count);
    }
}
