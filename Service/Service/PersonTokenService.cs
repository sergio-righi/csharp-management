using Domain.Entity;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service
{
    public class PersonTokenService : BaseService<PersonToken>, IPersonTokenService
    {
        private readonly IPersonTokenRepository Repository;

        public PersonTokenService(IPersonTokenRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<PersonToken> Find(int person_id, EToken token_id)
        {
            return await Repository.Find(person_id, token_id);
        }

        public async Task<PersonToken> Find(string token, EToken token_id)
        {
            return await Repository.Find(token, token_id);
        }

        public async Task<bool> Manage(Person person, PersonToken personToken)
        {
            return await Repository.Manage(person, personToken);
        }
    }
}
