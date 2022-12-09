using Domain.Entity;
using Domain.Entity.Generic;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;
using Tool.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ExamRepository : DomainRepository<Exam>, IExamRepository
    {
        public ExamRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Exam> Find(int id)
        {
            var query = (from E in DbContext.Exams
                         where E.Id == id
                         select E)
                         .Include(x => x.File)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<Pagination<GCustomInformation>> GList(int? personId, DateTime? date, Expression<Func<GCustomInformation, object>> order, EDirection direction, int page, int count)
        {
            var query = (from E in DbContext.Exams
                         join NP in DbContext.NaturalPeople on E.PersonId equals NP.Id
                         where
                          (personId.HasValue ? E.PersonId == personId : true) && (date.HasValue ? E.Date.Equals(date) : true)
                         select new GCustomInformation
                         {
                             Id = E.PersonId,
                             Name = NP.FullName,
                             Date = E.Date.Value
                         })
                        .OrderBy(order, direction)
                        .Skip((page - 1) * count)
                        .Take(count)
                        .ToList();

            var total = (from E in DbContext.Exams
                         join NP in DbContext.NaturalPeople on E.PersonId equals NP.Id
                         where
                          (personId.HasValue ? E.PersonId == personId : true) && (date.HasValue ? E.Date.Equals(date) : true)
                         select E.Id)
                         .Count();

            query = await Task.FromResult(query);

            return new Pagination<GCustomInformation>(query, total, page, count);
        }
    }
}
