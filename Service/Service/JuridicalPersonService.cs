using Domain.Entity;
using Domain.Interface;
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
    public class JuridicalPersonService : BaseService<JuridicalPerson>, IJuridicalPersonService
    {
        private readonly IJuridicalPersonRepository Repository;

        public JuridicalPersonService(IJuridicalPersonRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<JuridicalPerson> Find(int id)
        {
            return await Repository.Find(id);
        }
    }
}
