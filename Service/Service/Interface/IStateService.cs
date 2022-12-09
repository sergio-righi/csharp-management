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

namespace Service.Service.Interface
{
    public interface IStateService : IService<State>
    {
        Task<ICollection<State>> List(bool? activated);
        Task<ICollection<State>> List(int countryId, bool? activated);
    }
}
