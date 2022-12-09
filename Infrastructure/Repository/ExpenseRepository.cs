using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ExpenseRepository : DomainRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Expense> Find(int id)
        {
            var query = (from E in DbContext.Expenses
                         where E.Id == id
                         select E)
                         .Include(x => x.Receipt)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }
    }
}
