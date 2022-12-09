using Domain.Entity;
using Domain.Entity.Generic;
using Domain.Entity.Specific;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service
{
    public class PersonService : BaseService<Person>, IPersonService
    {
        private readonly IPersonRepository Repository;

        public PersonService(IPersonRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public Task<bool> Exist(string login)
        {
            return Repository.Exist(login);
        }

        public Task<GCustomInformation> GFind(int id)
        {
            return Repository.GFind(id);
        }

        public async Task<SUserPerson> SFind(string login, string password)
        {
            return await Repository.SFind(login, Cryptography.GetEncrypted(password));
        }

        public async Task<ICollection<GCustomInformation>> GList(EPerson typeId, string name, string code)
        {
            return await Repository.GList(typeId, name, code);
        }

        public async Task<ICollection<GCustomInformation>> GList(ERole roleId, string name, string code)
        {
            return await Repository.GList(roleId, name, code);
        }
    }
}
