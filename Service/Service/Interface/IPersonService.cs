using Domain.Entity;
using Domain.Entity.Generic;
using Domain.Entity.Specific;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Interface
{
    public interface IPersonService : IService<Person>
    {
        Task<bool> Exist(string login);
        Task<GCustomInformation> GFind(int id);
        Task<SUserPerson> SFind(string login, string password);
        Task<ICollection<GCustomInformation>> GList(EPerson typeId, string name, string code);
        Task<ICollection<GCustomInformation>> GList(ERole roleId, string name, string document);
    }
}
