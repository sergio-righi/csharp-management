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

namespace Service.Service
{
    public class PersonDocumentService : BaseService<PersonDocument>, IPersonDocumentService
    {
        private readonly IPersonDocumentRepository Repository;

        public PersonDocumentService(IPersonDocumentRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<ICollection<PersonDocument>> List(int personId)
        {
            return await Repository.List(personId);
        }
    }
}
