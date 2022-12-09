using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Interface
{
    public interface IPersonTokenRepository : IDomainRepository<PersonToken>
    {
        Task<PersonToken> Find(int personId, EToken tokenId);
        Task<PersonToken> Find(string token, EToken tokenId);
        Task<bool> Manage(Person person, PersonToken personToken);
    }
}
