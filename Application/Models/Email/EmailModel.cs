using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models.Email
{
    public class EmailModel
    {
        public Domain.Entity.Email Email { get; set; }
        public ICollection<Domain.Entity.Email> Emails { get; set; }

        public EmailModel()
        {
            Email = new Domain.Entity.Email();
        }
    }
}
