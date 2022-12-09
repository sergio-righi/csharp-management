using Domain.Entity;
using Domain.Entity.Generic;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service
{
    public class ExamService : BaseService<Exam>, IExamService
    {
        private readonly IExamRepository Repository;

        public ExamService(IExamRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Exam> Find(int id)
        {
            return await Repository.Find(id);
        }

        public async Task<Pagination<GCustomInformation>> GList(int? personId, DateTime? date, Expression<Func<GCustomInformation, object>> order, EDirection direction, int page, int count)
        {
            return await Repository.GList(personId, date, order, direction, page, count);
        }
    }
}
