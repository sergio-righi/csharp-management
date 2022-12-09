using Domain.Entity;
using Domain.Entity.Generic;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Interface
{
    public interface IPersonRepository : IDomainRepository<Person>
    {
        Task<bool> Exist(string login);
        Task<GCustomInformation> GFind(int id);
        Task<SUserPerson> SFind(string login, string password);
        Task<ICollection<GCustomInformation>> GList(EPerson typeId, string name, string code);
        Task<ICollection<GCustomInformation>> GList(ERole roleId, string name, string code);
    }
}
