using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class PermissionGroupRepository : DomainRepository<PermissionGroup>, IPermissionGroupRepository
    {
        public PermissionGroupRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
