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
    public class CountryService : BaseService<Country>, ICountryService
    {
        private readonly ICountryRepository Repository;

        public CountryService(ICountryRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<ICollection<Country>> List(bool? activated)
        {
            return await Repository.List(activated);
        }
    }
}
