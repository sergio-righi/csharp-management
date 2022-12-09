using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class TimetableRepository : DomainRepository<Timetable>, ITimetableRepository
    {
        public TimetableRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
