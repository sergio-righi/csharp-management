
using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class CountryRepository : DomainRepository<Country>, ICountryRepository
    {
        public CountryRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<Country>> List(bool? activated)
        {
            var query = (from C in DbContext.Countries
                         where C.IsActivated == (activated ?? C.IsActivated)
                         select C)
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }
    }
}
