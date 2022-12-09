using Domain.Entity;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class StateService : BaseService<State>, IStateService
    {
        private readonly IStateRepository Repository;

        public StateService(IStateRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<ICollection<State>> List(bool? activated)
        {
            return await Repository.List(activated);
        }

        public async Task<ICollection<State>> List(int countryId, bool? activated)
        {
            return await Repository.List(countryId, activated);
        }
    }
}
