using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface IExpenseRepository : IDomainRepository<Expense>
    {
        Task<Expense> Find(int id);
    }
}
