using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Domain.Entity.Specific
{
    public class SRolePerson
    {
        public int Id { get; set; }
        public ERole RoleId { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Deleted { get; set; }

        public bool IsDeleted()
        {
            return !string.IsNullOrEmpty(Deleted) ? bool.Parse(Deleted) : false;
        }
    }
}
