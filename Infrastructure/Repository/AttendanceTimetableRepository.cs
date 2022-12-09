using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class AttendanceTimetableRepository : DomainRepository<AttendanceTimetable>, IAttendanceTimetableRepository
    {
        public AttendanceTimetableRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
