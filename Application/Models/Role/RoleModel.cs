using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Application.Models.Role
{
    public class RoleModel
    {
        public string NotName { get; set; }
        public ERole RoleId { get; set; }
        public int PersonId { get; set; }
    }
}
