using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Tool.Utilities;

namespace Infrastructure.Repository
{
    public class PersonTokenRepository : DomainRepository<PersonToken>, IPersonTokenRepository
    {
        public PersonTokenRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<PersonToken> Find(int personId, EToken tokenId)
        {
            var query = (from PT in DbContext.PersonToken
                         where PT.PersonId == personId && PT.TokenId == tokenId && PT.ExpiryDate >= DateTime.Now
                         select PT)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<PersonToken> Find(string token, EToken tokenId)
        {
            var query = (from PT in DbContext.PersonToken
                         where PT.Token.Equals(token) && PT.TokenId == tokenId && PT.ExpiryDate >= DateTime.Now
                         select PT)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<bool> Manage(Person person, PersonToken personToken)
        {
            using (IDbContextTransaction transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    DbContext.Entry(person).State = EntityState.Modified;
                    DbContext.Entry(personToken).State = EntityState.Modified;

                    await DbContext.SaveChangesAsync();

                    transaction.Commit();

                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    return false;
                }
            }
        }
    }
}
