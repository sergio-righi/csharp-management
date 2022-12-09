using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class PersonAgendaRepository : DomainRepository<PersonAgenda>, IPersonAgendaRepository
    {
        public PersonAgendaRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
