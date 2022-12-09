using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Interface
{
    public interface ISaleService : IService<Sale>
    {
        Task<Pagination<Sale>> List(int? personId, ESale situationId, Expression<Func<Sale, object>> order, EDirection direction, int page, int count);
    }
}
