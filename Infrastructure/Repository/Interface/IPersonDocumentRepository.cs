﻿using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface IPersonDocumentRepository : IDomainRepository<PersonDocument>
    {
        Task<ICollection<PersonDocument>> List(int personId);
    }
}
