using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface INaturalPersonService : IService<NaturalPerson>
    {
        Task<NaturalPerson> Find(int id);
    }
}
