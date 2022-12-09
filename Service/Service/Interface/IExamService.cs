using Domain.Entity;
using Domain.Entity.Generic;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Interface
{
    public interface IExamService : IService<Exam>
    {
        Task<Exam> Find(int id);
        Task<Pagination<GCustomInformation>> GList(int? personId, DateTime? date, Expression<Func<GCustomInformation, object>> order, EDirection direction, int page, int count);
    }
}
