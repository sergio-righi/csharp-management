using Domain.Entity;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class InstallmentService : BaseService<Installment>, IInstallmentService
    {
        private readonly IInstallmentRepository Repository;

        public InstallmentService(IInstallmentRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Installment> Find(int id)
        {
            return await Repository.Find(id);
        }

        public async Task<int> Count(int? installmentId)
        {
            if (installmentId.HasValue)
            {
                return await Repository.Count(installmentId.Value);
            }

            return 0;
        }

        public async Task<bool> Manage(Installment installment)
        {
            return await Repository.Manage(installment);
        }
    }
}
