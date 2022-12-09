using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Interface
{
    public interface IPersonTokenService : IService<PersonToken>
    {
        Task<PersonToken> Find(int personId, EToken tokenId);
        Task<PersonToken> Find(string token, EToken tokenId);
        Task<bool> Manage(Person person, PersonToken personToken);
    }
}
