using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface IInstallmentService : IService<Installment>
    {
        Task<Installment> Find(int id);
        Task<int> Count(int? installmentId);
        Task<bool> Manage(Installment installment);
    }
}
