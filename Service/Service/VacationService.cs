using Domain.Entity;
using Domain.Entity.Specific;
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
    public class VacationService : BaseService<Vacation>, IVacationService
    {
        private readonly IVacationRepository Repository;

        public VacationService(IVacationRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Vacation> Find(int id)
        {
            return await Repository.Find(id);
        }

        public async Task<Pagination<SVacation>> SList(int? personId, int? year, Expression<Func<SVacation, object>> order, EDirection direction, int page, int count)
        {
            return await Repository.SList(personId, year, order, direction, page, count);
        }
    }
}