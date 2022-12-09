using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class StateRepository : DomainRepository<State>, IStateRepository
    {
        public StateRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<State>> List(bool? activated)
        {
            var query = (from S in DbContext.States
                         where S.IsActivated == (activated ?? S.IsActivated)
                         orderby S.Name
                         select S)
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }

        public async Task<ICollection<State>> List(int countryId, bool? activated)
        {
            var query = (from S in DbContext.States
                         where S.CountryId == countryId && S.IsActivated == (activated ?? S.IsActivated)
                         orderby S.Name
                         select S)
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }
    }
}
