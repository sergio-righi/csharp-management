using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Interface
{
    public interface IPayrollService : IService<Payroll>
    {
        Task<bool> Exist(int year, EMonth month);
    }
}
