using Domain.Entity;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ExpenseService : BaseService<Expense>, IExpenseService
    {
        private readonly IExpenseRepository Repository;

        public ExpenseService(IExpenseRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Expense> Find(int id)
        {
            return await Repository.Find(id);
        }
    }
}
