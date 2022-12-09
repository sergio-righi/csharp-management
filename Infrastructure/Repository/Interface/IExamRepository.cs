using Domain.Entity;
using Domain.Entity.Generic;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Interface
{
    public interface IExamRepository : IDomainRepository<Exam>
    {
        Task<Exam> Find(int id);
        Task<Pagination<GCustomInformation>> GList(int? personId, DateTime? date, Expression<Func<GCustomInformation, object>> order, EDirection direction, int page, int count);
    }
}
