using Domain.Entity;
using Domain.Entity.Specific;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service
{
    public class PersonRoleService : BaseService<PersonRole>, IPersonRoleService
    {
        private readonly IPersonRoleRepository Repository;

        public PersonRoleService(IPersonRoleRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<SRolePerson> SFind(int personId, ERole roleId, bool? activated)
        {
            return await Repository.SFind(personId, roleId, activated);
        }

        public async Task<ICollection<SRolePerson>> SList(ERole roleId, bool? activated)
        {
            return await Repository.SList(roleId, activated);
        }

        public async Task Manage(ICollection<PersonRole> added, ICollection<PersonRole> modified)
        {
            await Repository.Manage(added, modified);
        }
    }
}
