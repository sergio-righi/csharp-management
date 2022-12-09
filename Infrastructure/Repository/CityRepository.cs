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
    public class CityRepository : DomainRepository<City>, ICityRepository
    {
        public CityRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<City>> List(int? countryId, int? stateId, bool? activated)
        {
            var query = (from C in DbContext.Cities
                         where C.CountryId == (countryId ?? C.CountryId) && C.StateId == (stateId ?? C.StateId) && C.IsActivated == (activated ?? C.IsActivated)
                         orderby C.Name
                         select C)
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }
    }
}
