﻿using Domain.Entity;
using Domain.Entity.Specific;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Interface
{
    public interface IPaycheckService : IService<Paycheck>
    {
        Task<Paycheck> Find(int id);
        Task<bool> Exist(int id, int payrollId, int personId, int? week);
        Task<Pagination<SPaycheckPerson>> SList(int payrollId, string name, Expression<Func<SPaycheckPerson, object>> order, EDirection direction, int page, int count);
    }
}