using Domain.Entity;
using Domain.Entity.Specific;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class PersonRoleRepository : DomainRepository<PersonRole>, IPersonRoleRepository
    {
        public PersonRoleRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<SRolePerson> SFind(int personId, ERole roleId, bool? activated)
        {
            var query = (from PR in DbContext.PersonRole
                         join NP in DbContext.NaturalPeople on PR.PersonId equals NP.Id into NP_Join
                         from NP in NP_Join.DefaultIfEmpty()
                         join JP in DbContext.JuridicalPeople on PR.PersonId equals JP.Id into JP_Join
                         from JP in JP_Join.DefaultIfEmpty()
                         where
                              PR.PersonId == personId && (roleId == ERole.All ? true : PR.RoleId == roleId) && (activated == null ? true : (activated.Value == true ? PR.EndDate == null : PR.EndDate != null))
                         select new SRolePerson
                         {
                             Id = PR.Id,
                             RoleId = PR.RoleId,
                             EndDate = PR.EndDate,
                             PersonId = PR.PersonId,
                             StartDate = PR.StartDate,
                             Name = NP.FullName ?? JP.CompanyName
                         })
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<ICollection<SRolePerson>> SList(ERole roleId, bool? activated)
        {
            var query = (from PR in DbContext.PersonRole
                         join NP in DbContext.NaturalPeople on PR.PersonId equals NP.Id into NP_Join
                         from NP in NP_Join.DefaultIfEmpty()
                         join JP in DbContext.JuridicalPeople on PR.PersonId equals JP.Id into JP_Join
                         from JP in JP_Join.DefaultIfEmpty()
                         where
                              PR.RoleId == roleId && (activated == null ? true : (activated.Value == true ? PR.EndDate == null : PR.EndDate != null))
                         select new SRolePerson
                         {
                             Id = PR.Id,
                             RoleId = PR.RoleId,
                             EndDate = PR.EndDate,
                             PersonId = PR.PersonId,
                             StartDate = PR.StartDate,
                             Name = NP.GivenName ?? JP.CompanyName
                         })
                         .OrderBy(x => x.Name)
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }

        public async Task<ICollection<SRolePerson>> List(int personId, bool? activated)
        {
            var query = (from PR in DbContext.PersonRole
                         join NP in DbContext.NaturalPeople on PR.PersonId equals NP.Id into NP_Join
                         from NP in NP_Join.DefaultIfEmpty()
                         join JP in DbContext.JuridicalPeople on PR.PersonId equals JP.Id into JP_Join
                         from JP in JP_Join.DefaultIfEmpty()
                         where
                              PR.PersonId == personId && (activated == null ? true : (activated.Value == true ? PR.EndDate == null : PR.EndDate != null))
                         select new SRolePerson
                         {
                             Id = PR.Id,
                             RoleId = PR.RoleId,
                             EndDate = PR.EndDate,
                             PersonId = PR.PersonId,
                             StartDate = PR.StartDate,
                             Name = NP.GivenName ?? JP.CompanyName
                         })
                         .OrderBy(x => x.Name)
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }

        public async Task Manage(ICollection<PersonRole> added, ICollection<PersonRole> modified)
        {
            using (IDbContextTransaction transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in added)
                    {
                        DbContext.Entry(item).State = EntityState.Added;
                    }

                    foreach (var item in modified)
                    {
                        DbContext.Entry(item).State = EntityState.Modified;

                        DbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                        DbContext.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                    }

                    await DbContext.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
