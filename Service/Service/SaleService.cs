using Domain.Entity;
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
    public class SaleService : BaseService<Sale>, ISaleService
    {
        private readonly ISaleRepository Repository;

        public SaleService(ISaleRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Pagination<Sale>> List(int? person_id, ESale situation_id, Expression<Func<Sale, object>> order, EDirection direction, int page, int count)
        {
            return await Repository.List(person_id, situation_id, order, direction, page, count);
        }
    }
}
