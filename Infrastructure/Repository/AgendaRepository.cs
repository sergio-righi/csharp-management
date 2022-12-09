using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class AgendaRepository : DomainRepository<Agenda>, IAgendaRepository
    {
        public AgendaRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
