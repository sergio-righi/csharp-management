using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Interface
{
    public interface IPayrollRepository : IDomainRepository<Payroll>
    {
        Task<bool> Exist(int year, EMonth month);
    }
}
