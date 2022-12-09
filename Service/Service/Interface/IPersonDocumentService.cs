using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface IPersonDocumentService : IService<PersonDocument>
    {
        Task<ICollection<PersonDocument>> List(int personId);
    }
}
