
using Domain.Entity;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class TimetableService : BaseService<Timetable>, ITimetableService
    {
        private readonly ITimetableRepository Repository;

        public TimetableService(ITimetableRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }
}
