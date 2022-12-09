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
    public class MailService : BaseService<Email>, IEmailService
    {
        private readonly IEmailRepository Repository;

        public MailService(IEmailRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Email> Find(int? id, bool? deleted)
        {
            return await Repository.Find(id, deleted);
        }
    }
}
