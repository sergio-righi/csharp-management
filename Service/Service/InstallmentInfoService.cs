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
    public class InstallmentInfoService : BaseService<InstallmentInfo>, IInstallmentInfoService
    {
        private readonly IInstallmentInfoRepository Repository;

        public InstallmentInfoService(IInstallmentInfoRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }
}
