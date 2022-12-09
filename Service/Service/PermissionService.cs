using Domain.Entity;
using Domain.Interface;
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
    public class PermissionService : BaseService<Permission>, IPermissionService
    {
        private readonly IPermissionRepository Repository;

        public PermissionService(IPermissionRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }
}
