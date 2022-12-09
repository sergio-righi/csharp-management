using Domain.Entity;
using Domain.Entity.Generic;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using Tool.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity.Specific;
using Tool.Utilities;
using System.Security.Cryptography;
using Tool.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Resources;

namespace Infrastructure.Repository
{
    public class PersonRepository : DomainRepository<Person>, IPersonRepository
    {
        public PersonRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> Exist(string login)
        {
            var query = (from P in DbContext.People
                         join E in DbContext.Emails on P.Id equals E.PersonId into E_Join
                         from E in E_Join.DefaultIfEmpty()
                         where
                           login.Equals(P.Login) || (E != null && login.Equals(E.Address))// || (E.Address.StartsWith(login) && E.Address.EndsWith(AppConfig.Client.Domain))))
                         select P.Id)
                         .Any();

            return await Task.FromResult(query);
        }

        public async Task<GCustomInformation> GFind(int id)
        {
            var query = (from P in DbContext.People
                         join NP in DbContext.NaturalPeople on P.Id equals NP.Id into NP_Join
                         from NP in NP_Join.DefaultIfEmpty()
                         join JP in DbContext.JuridicalPeople on P.Id equals JP.Id into JP_Join
                         from JP in JP_Join.DefaultIfEmpty()
                         where
                           P.Id == id
                         select new GCustomInformation
                         {
                             Id = P.Id,
                             Name = NP.FullName ?? JP.CompanyName
                         })
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<SUserPerson> SFind(string login, string password)
        {
            var query = (from P in DbContext.People
                         join NP in DbContext.NaturalPeople on P.Id equals NP.Id into NP_Join
                         from NP in NP_Join.DefaultIfEmpty()
                         join E in DbContext.Emails on P.Id equals E.PersonId into E_Join
                         from E in E_Join.DefaultIfEmpty()
                         join F in DbContext.Files on P.PhotoId equals F.Id into F_Join
                         from F in F_Join.DefaultIfEmpty()
                         where
                           login.Equals(P.Login) || (E != null && E.Address.Equals(login))// || (E.Address.StartsWith(login) && E.Address.EndsWith(AppConfig.Client.Domain))))) && (password == null ? true : P.Password.Equals(password))
                         select new SUserPerson
                         {
                             Id = P.Id,
                             Email = E.Address,
                             TypeId = P.TypeId,
                             Name = NP.FullName,
                             Birthday = NP.Birthday,
                             Photo = $"{F.GeneratedName}{F.Extension}",
                         })
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<ICollection<GCustomInformation>> GList(EPerson typeId, string name, string code)
        {
            var query = new List<GCustomInformation>();

            switch (typeId)
            {
                case EPerson.Natural:
                    query = (from P in DbContext.People
                             join NP in DbContext.NaturalPeople on P.Id equals NP.Id
                             join PD in DbContext.PersonDoument on P.Id equals PD.Id into PD_Join
                             from PD in PD_Join.DefaultIfEmpty()
                             where
                               (string.IsNullOrWhiteSpace(name) ? true : NP.FullName.Contains(name)) && (string.IsNullOrWhiteSpace(code) ? true : code.Equals(PD.Number))
                             select new GCustomInformation
                             {
                                 Id = P.Id,
                                 Name = NP.FullName
                             })
                             .ToList();
                    break;
                case EPerson.Juridical:
                    query = (from P in DbContext.People
                             join JP in DbContext.JuridicalPeople on P.Id equals JP.Id
                             where
                                (string.IsNullOrWhiteSpace(name) ? true : JP.CompanyName.Contains(name)) && (string.IsNullOrWhiteSpace(code) ? true : JP.CNPJ.Equals(code))
                             select new GCustomInformation
                             {
                                 Id = P.Id,
                                 Name = JP.CompanyName
                             })
                             .ToList();
                    break;
            }

            return await Task.FromResult(query);
        }

        public async Task<ICollection<GCustomInformation>> GList(ERole roleId, string name, string code)
        {
            var query = (from P in DbContext.People
                         join PR in DbContext.PersonRole on new { PersonId = P.Id, IsActivated = true } equals new { PR.PersonId, IsActivated = PR.EndDate == null }
                         join NP in DbContext.NaturalPeople on P.Id equals NP.Id
                         join PD in DbContext.PersonDoument on P.Id equals PD.Id into PD_Join
                         from PD in PD_Join.DefaultIfEmpty()
                         where
                           (string.IsNullOrWhiteSpace(name) ? true : NP.FullName.Contains(name)) 
                           && (string.IsNullOrWhiteSpace(code) ? true : code.Equals(PD.Number)) 
                           && (roleId == ERole.All ? true : PR.RoleId == roleId) 
                           && Util.EmployeeRoles.Contains(PR.RoleId)
                         select new GCustomInformation
                         {
                             Id = P.Id,
                             Name = NP.FullName
                         })
                         .ToList();

            return await Task.FromResult(query);
        }
    }
}
