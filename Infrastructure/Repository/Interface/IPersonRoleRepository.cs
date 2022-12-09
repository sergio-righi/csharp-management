using Domain.Entity;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Interface
{
    public interface IPersonRoleRepository : IDomainRepository<PersonRole>
    {
        Task<SRolePerson> SFind(int personId, ERole roleId, bool? activated);
        Task<ICollection<SRolePerson>> SList(ERole id, bool? activated);
        Task<ICollection<SRolePerson>> List(int personId, bool? activated);
        Task Manage(ICollection<PersonRole> added, ICollection<PersonRole> modified);
    }
}
