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
    public class CityService : BaseService<City>, ICityService
    {
        private readonly ICityRepository Repository;

        public CityService(ICityRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<ICollection<City>> List(int? countryId, int? stateId, bool? activated)
        {
            return await Repository.List(countryId, stateId, activated);
        }
    }
}
