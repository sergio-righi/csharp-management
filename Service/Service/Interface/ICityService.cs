using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface ICityService : IService<City>
    {
        Task<ICollection<City>> List(int? countryId, int? stateId, bool? activated);
    }
}
