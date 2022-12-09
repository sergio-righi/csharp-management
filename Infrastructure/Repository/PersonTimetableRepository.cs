using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class PersonTimetableRepository : DomainRepository<PersonTimetable>, IPersonTimetableRepository
    {
        public PersonTimetableRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
