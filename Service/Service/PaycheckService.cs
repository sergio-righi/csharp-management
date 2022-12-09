using Domain.Entity;
using Domain.Entity.Specific;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service
{
    public class PaycheckService : BaseService<Paycheck>, IPaycheckService
    {
        private readonly IPaycheckRepository Repository;

        public PaycheckService(IPaycheckRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<bool> Exist(int id, int payrollId, int personId, int? week)
        {
            return await Repository.Exist(id, payrollId, personId, week);
        }

        public async Task<Paycheck> Find(int id)
        {
            return await Repository.Find(id);
        }

        public async Task<Pagination<SPaycheckPerson>> SList(int payrollId, string name, Expression<Func<SPaycheckPerson, object>> order, EDirection direction, int page, int count)
        {
            return await Repository.SList(payrollId, name, order, direction, page, count);
        }
    }
}
