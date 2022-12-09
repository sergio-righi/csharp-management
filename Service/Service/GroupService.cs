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
    public class GroupService : BaseService<Group>, IGroupService
    {
        private readonly IGroupRepository Repository;

        public GroupService(IGroupRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }
}
