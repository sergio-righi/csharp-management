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

namespace Service.Service.Interface
{
    public interface IPersonRoleService : IService<PersonRole>
    {
        Task<SRolePerson> SFind(int personId, ERole roleId, bool? activated);
        Task<ICollection<SRolePerson>> SList(ERole roleId, bool? activated);
        Task Manage(ICollection<PersonRole> added, ICollection<PersonRole> modified);
    }
}
