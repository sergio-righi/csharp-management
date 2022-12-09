using Domain.Entity;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Application.Models.Role
{
    public class RolePeopleModel
    {
        public RoleModel RolePerson { get; set; }
        public ICollection<SRolePerson> People { get; set; }

        public RolePeopleModel()
        {
            RolePerson = new RoleModel();
        }
    }
}
