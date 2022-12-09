using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class PermissionRepository : DomainRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
