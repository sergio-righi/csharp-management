using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models.Phone
{
    public class PhoneModel
    {
        public Domain.Entity.Phone Phone { get; set; }
        public ICollection<Domain.Entity.Phone> Phones { get; set; }

        public PhoneModel()
        {
            Phone = new Domain.Entity.Phone();
        }
    }
}
