using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class InstallmentInfoRepository : DomainRepository<InstallmentInfo>, IInstallmentInfoRepository
    {
        public InstallmentInfoRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
