using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Domain.Entity.Specific;
using Tool.Utilities;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Tool.Extensions;

namespace Infrastructure.Repository
{
    public class AttendanceRepository : DomainRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<SAttendancePerson> Find(int Id, DateTime date)
        {
            var query = await Task.FromResult((from NP in DbContext.NaturalPeople
                                               join PR in DbContext.PersonRole on new { PersonId = NP.Id } equals new { PersonId = PR.PersonId }
                                               //join A in DbContext.Attendances on new { PersonId = NP.Id, date } equals new { PersonId = A.PersonId, A.Date } into A_Join
                                               //from A in A_Join.DefaultIfEmpty()
                                               where NP.Id == Id && Util.EmployeeRoles.Contains(PR.RoleId)
                                               select new SAttendancePerson
                                               {
                                                   //Id = A.Id,
                                                   Date = date,
                                                   PersonId = NP.Id,
                                                   Name = NP.FullName
                                                   //Observation = A.observation,
                                                   //SituationId = A.situation_Id
                                               })
                                               .FirstOrDefault());

            return query;
        }

        public async Task<ICollection<SAttendancePerson>> List(DateTime date)
        {
            var query = (from NP in DbContext.NaturalPeople
                         join PR in DbContext.PersonRole on new { PersonId = NP.Id } equals new { PersonId = PR.PersonId }
                         //join A in DbContext.Attendances on new { PersonId = NP.Id, date } equals new { PersonId = A.PersonId, A.date } into A_Join
                         //from A in A_Join.DefaultIfEmpty()
                         where Util.EmployeeRoles.Contains(PR.RoleId) && ((PR.EndDate != null && PR.EndDate >= date) || (PR.EndDate == null && PR.StartDate.Date <= date))
                         orderby NP.FullName
                         select new SAttendancePerson
                         {
                             //Id = A.Id,
                             Date = date,
                             PersonId = NP.Id,
                             Name = NP.FullName
                             //Observation = A.observation,
                             //SituationId = A.situation_Id
                         })
                         .Distinct()
                         .ToList();

            return await Task.FromResult(query);
        }

        public async Task<ICollection<DateTime>> List(int month, int year)
        {
            //var query = (from A in DbContext.Attendances
            //             where A.date.Month == month && A.date.Year == year
            //             select A.date)
            //             .Distinct()
            //             .OrderByDescending(x => x)
            //             .ToList();

            return await Task.FromResult(new List<DateTime>());
        }

        public async Task Manage(ICollection<Attendance> added, ICollection<Attendance> modified)
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

                        DbContext.Entry(item).Property(x => x.FileId).IsModified = false;
                        DbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                        DbContext.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                        DbContext.Entry(item).Property(x => x.Observation).IsModified = false;
                    }

                    await DbContext.SaveChangesAsync();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
