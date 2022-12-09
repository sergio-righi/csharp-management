using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;
using Domain.Entity.Specific;
using Tool.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class PaycheckRepository : DomainRepository<Paycheck>, IPaycheckRepository
    {
        public PaycheckRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> Exist(int id, int payrollId, int personId, int? week)
        {
            var query = (from P in DbContext.Paychecks
                         where P.Id != id && P.PayrollId == payrollId && P.PersonId == personId && P.Week == week
                         select P.Id)
                         .Any();

            return await Task.FromResult(query);
        }

        public async Task<Paycheck> Find(int id)
        {
            var query = (from P in DbContext.Paychecks
                         where P.Id == id
                         select P)
                         .Include(x => x.File)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<Pagination<SPaycheckPerson>> SList(int payrollId, string name, Expression<Func<SPaycheckPerson, object>> order, EDirection direction, int page, int count)
        {
            var query = (from P in DbContext.Paychecks
                         join NP in DbContext.NaturalPeople on P.PersonId equals NP.Id
                         where P.Id == payrollId && (!string.IsNullOrWhiteSpace(name) ? NP.FullName.Contains(name) : true)
                         select new SPaycheckPerson
                         {
                             Id = P.Id,
                             Week = P.Week,
                             PersonId = NP.Id,
                             Name = NP.FullName,
                             IsExtra = P.IsExtra,
                             VoucherId = P.FileId
                         })
                         .OrderBy(order, direction)
                         .Distinct();

            var total = (from P in DbContext.Paychecks
                         join NP in DbContext.NaturalPeople on P.PersonId equals NP.Id
                         where P.Id == payrollId && (!string.IsNullOrWhiteSpace(name) ? NP.FullName.Contains(name) : true)
                         select P.Id)
                         .Distinct()
                         .Count();

            query = await Task.FromResult(query);

            return new Pagination<SPaycheckPerson>(query, total, page, count);
        }

        //public async Task<Pagination<SPaycheckPerson>> SList(int payrollId, string name, Expression<Func<SPaycheckPerson, object>> order, EDirection direction, int page, int count)
        //{
        //    var query = (from P in DbContext.Payrolls
        //                 from PR in DbContext.PersonRole
        //                 join NP in DbContext.NaturalPeople on PR.PersonId equals NP.Id
        //                 join PP in DbContext.Paychecks on NP.Id equals PP.PersonId into PP_Join
        //                 from PP in PP_Join.DefaultIfEmpty()
        //                 where P.Id == payrollId && (!string.IsNullOrWhiteSpace(name) ? NP.FullName.Contains(name) : true) && ((PR.StartDate.Year == P.Year && PR.StartDate.Month == (int)P.Month) || ((PR.EndDate == null ? P.Year : PR.EndDate.Value.Year) == P.Year && (PR.EndDate == null ? (int)P.Month : PR.EndDate.Value.Month) == (int)P.Month))
        //                 select new SPaycheckPerson
        //                 {
        //                     Id = PP.Id,
        //                     PersonId = NP.Id,
        //                     Name = NP.FullName
        //                 })
        //                 .OrderBy(order, direction)
        //                 .Distinct();

        //    var total = (from P in DbContext.Payrolls
        //                 from PR in DbContext.PersonRole
        //                 join NP in DbContext.NaturalPeople on PR.PersonId equals NP.Id
        //                 join PP in DbContext.Paychecks on NP.Id equals PP.PersonId into PP_Join
        //                 from PP in PP_Join.DefaultIfEmpty()
        //                 where P.Id == payrollId && (!string.IsNullOrWhiteSpace(name) ? NP.FullName.Contains(name) : true) && ((PR.StartDate.Year == P.Year && PR.StartDate.Month == (int)P.Month) || ((PR.EndDate == null ? P.Year : PR.EndDate.Value.Year) == P.Year && (PR.EndDate == null ? (int)P.Month : PR.EndDate.Value.Month) == (int)P.Month))
        //                 select NP.Id)
        //                 .Distinct()
        //                 .Count();

        //    query = await Task.FromResult(query);

        //    return new Pagination<SPaycheckPerson>(query, total, page, count);
        //}
    }
}
