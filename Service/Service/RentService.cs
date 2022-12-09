using Domain.Entity;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class RentService : BaseService<Rent>, IRentService
    {
        private readonly IRentRepository Repository;

        public RentService(IRentRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }
}
