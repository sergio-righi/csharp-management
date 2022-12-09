using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;
using Tool.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class SaleRepository : DomainRepository<Sale>, ISaleRepository
    {
        public SaleRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Pagination<Sale>> List(int? personId, ESale situationId, Expression<Func<Sale, object>> order, EDirection direction, int page, int count)
        {
            var query = (from S in DbContext.Sales
                         where
                          S.CustomerId == (personId ?? S.CustomerId) && (situationId == ESale.All ? true : S.SituationId == situationId) && !S.IsDeleted
                         select S)
                         .Include(x => x.Customer.NaturalPerson)
                         .Include(x => x.Customer.JuridicalPerson)
                         .OrderBy(order, direction)
                         .Skip((page - 1) * count)
                         .Take(count)
                         .ToList();

            var total = (from S in DbContext.Sales
                         where
                           S.CustomerId == (personId ?? S.CustomerId) && (situationId == ESale.All ? true : S.SituationId == situationId) && !S.IsDeleted
                         select S.Id)
                         .Count();
            
            query = await Task.FromResult(query);

            return new Pagination<Sale>(query, total, page, count);
        }
    }
}
