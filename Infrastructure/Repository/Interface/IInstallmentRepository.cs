using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface IInstallmentRepository : IDomainRepository<Installment>
    {
        Task<Installment> Find(int id);
        Task<int> Count(int installmentId);
        Task<bool> Manage(Installment installment);
    }
}
