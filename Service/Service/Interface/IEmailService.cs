using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface IEmailService : IService<Email>
    {
        Task<Email> Find(int? id, bool? deleted);
    }
}
