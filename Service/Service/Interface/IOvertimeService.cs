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
    public interface IOvertimeService : IService<Overtime>
    {
        Task<Overtime> Find(int id);
        Task<Pagination<SOvertime>> SList(int? personId, DateTime? date, Expression<Func<SOvertime, object>> order, EDirection direction, int page, int count);
    }
}
