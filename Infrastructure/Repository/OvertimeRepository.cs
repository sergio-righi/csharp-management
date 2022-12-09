using Domain.Entity;
using Domain.Entity.Specific;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;
using Tool.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class OvertimeRepository : DomainRepository<Overtime>, IOvertimeRepository
    {
        public OvertimeRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Overtime> Find(int id)
        {
            var query = (from O in DbContext.Overtime
                         where O.Id == id
                         select O)
                         .Include(x => x.File)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<Pagination<SOvertime>> SList(int? personId, DateTime? date, Expression<Func<SOvertime, object>> order, EDirection direction, int page, int count)
        {
            var query = (from O in DbContext.Overtime
                         join NP in DbContext.NaturalPeople on O.PersonId equals NP.Id
                         where
                          (personId.HasValue ? O.PersonId == personId : true) && (date.HasValue ? O.Date.Equals(date) : true)
                         select new SOvertime
                         {
                             Id = O.Id,
                             Date = O.Date,
                             Count = O.Count,
                             IsPaid = O.IsPaid,
                             Name = NP.FullName,
                             PersonId = O.PersonId,
                             IsConfirmed = O.IsConfirmed
                         })
                        .OrderBy(order, direction)
                        .Skip((page - 1) * count)
                        .Take(count)
                        .ToList();

            var total = (from O in DbContext.Overtime
                         join NP in DbContext.NaturalPeople on O.PersonId equals NP.Id
                         where
                          (personId.HasValue ? O.PersonId == personId : true) && (date.HasValue ? O.Date.Equals(date) : true)
                         select O.Id)
                         .Count();

            query = await Task.FromResult(query);

            return new Pagination<SOvertime>(query, total, page, count);
        }
    }
}
