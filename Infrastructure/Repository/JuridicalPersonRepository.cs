using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class JuridicalPersonRepository : DomainRepository<JuridicalPerson>, IJuridicalPersonRepository
    {
        public JuridicalPersonRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<JuridicalPerson> Find(int id)
        {
            var query = (from PL in DbContext.JuridicalPeople
                         where PL.Id == id
                         select PL)
                         .Include(x => x.Person).ThenInclude(y => y.Photo)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }
    }
}
