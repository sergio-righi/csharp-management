using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface ICountryService : IService<Country>
    {
        Task<ICollection<Country>> List(bool? activated);
    }
}
