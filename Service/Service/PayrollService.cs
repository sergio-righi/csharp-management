using Domain.Entity;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service
{
    public class PayrollService : BaseService<Payroll>, IPayrollService
    {
        private readonly IPayrollRepository Repository;

        public PayrollService(IPayrollRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<bool> Exist(int year, EMonth month)
        {
            return await Repository.Exist(year, month);
        }
    }
}
