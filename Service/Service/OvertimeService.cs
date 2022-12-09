using Domain.Entity;
using Domain.Entity.Specific;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service
{
    public class OvertimeService : BaseService<Overtime>, IOvertimeService
    {
        private readonly IOvertimeRepository Repository;

        public OvertimeService(IOvertimeRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Overtime> Find(int id)
        {
            return await Repository.Find(id);
        }

        public async Task<Pagination<SOvertime>> SList(int? personId, DateTime? date, Expression<Func<SOvertime, object>> order, EDirection direction, int page, int count)
        {
            return await Repository.SList(personId, date, order, direction, page, count);
        }
    }
}
