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
    public class PersonAgendaService : BaseService<PersonAgenda>, IPersonAgendaService
    {
        private readonly IPersonAgendaRepository Repository;

        public PersonAgendaService(IPersonAgendaRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }
}