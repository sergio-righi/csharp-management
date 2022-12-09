using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Domain.Entity.Specific
{
    public class SUserPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public EPerson TypeId { get; set; }
        public ERole[] Roles { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string FullPhotoPath
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Photo) ? $"{FileManagement.Avatar.Read}{this.Photo}" : null;
            }
        }
    }
}
