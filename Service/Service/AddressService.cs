using Domain.Entity;
using Domain.Entity.Generic;
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
    public class AddressService : BaseService<Address>, IAddressService
    {
        private readonly IAddressRepository Repository;

        public AddressService(IAddressRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Address> Find(int id, bool? deleted)
        {
            return await Repository.Find(id, deleted);
        }

        public async Task<ICollection<Address>> List(int person_id, bool? deleted)
        {
            return await Repository.List(person_id, deleted);
        }

        public async Task<ICollection<GCustomInformation>> GList(int person_id, bool? deleted)
        {
            return await Repository.GList(person_id, deleted);
        }
    }
}
