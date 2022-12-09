using Domain.Entity;
using Domain.Entity.Specific;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;
using System.Linq;
using Tool.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class VacationRepository : DomainRepository<Vacation>, IVacationRepository
    {
        public VacationRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Vacation> Find(int id)
        {
            var query = (from V in DbContext.Vacations
                         where V.Id == id
                         select V)
                         .Include(x => x.File)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<Pagination<SVacation>> SList(int? personId, int? year, Expression<Func<SVacation, object>> order, EDirection direction, int page, int count)
        {
            var query = (from V in DbContext.Vacations
                         join NP in DbContext.NaturalPeople on V.PersonId equals NP.Id
                         where
                          (personId.HasValue ? V.PersonId == personId : true) && (year.HasValue ? V.StartDate.Value.Year == year : true)
                         select new SVacation
                         {
                             Id = V.Id,
                             FileId = V.FileId,
                             Name = NP.FullName,
                             EndDate = V.EndDate,
                             PersonId = V.PersonId,
                             StartDate = V.StartDate,
                             IsConfirmed = V.IsConfirmed
                         })
                        .OrderBy(order, direction)
                        .Skip((page - 1) * count)
                        .Take(count)
                        .ToList();

            var total = (from V in DbContext.Vacations
                         join NP in DbContext.NaturalPeople on V.PersonId equals NP.Id
                         where
                          (personId.HasValue ? V.PersonId == personId : true) && (year.HasValue ? V.StartDate.Value.Year == year : true)
                         select V.Id)
                         .Count();

            query = await Task.FromResult(query);

            return new Pagination<SVacation>(query, total, page, count);
        }
    }
}
