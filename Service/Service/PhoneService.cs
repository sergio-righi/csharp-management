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
    public class PhoneService : BaseService<Phone>, IPhoneService
    {
        private readonly IPhoneRepository Repository;

        public PhoneService(IPhoneRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Phone> Find(int? id, bool? deleted)
        {
            return await Repository.Find(id, deleted);
        }
    }
}
