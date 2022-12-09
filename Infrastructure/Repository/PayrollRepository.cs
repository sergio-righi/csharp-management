using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository
{
    public class PayrollRepository : DomainRepository<Payroll>, IPayrollRepository
    {
        public PayrollRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> Exist(int year, EMonth month)
        {
            var query = (from P in DbContext.Payrolls
                         where P.Year == year && P.Month == month
                         select P.Id)
                         .Any();

            return await Task.FromResult(query);
        }
    }
}
