using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface IJuridicalPersonService : IService<JuridicalPerson>
    {
        Task<JuridicalPerson> Find(int id);
    }
}
