using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Domain.Base;
using Tool.Utilities;
using Tool.Resources;

namespace Domain.Entity
{
    public class Email : BaseEntity
    {
        [DisplayNameResource(nameof(Label.Type))]
        public EEmail TypeId { get; set; }
        public int PersonId { get; set; }
        [RequiredResource]
        [StringLength(150)]
        [DisplayNameResource(nameof(Label.EmailAddress))]
        public string Address { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        [StringLength(100)]
        public string ConfirmationToken { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        [CompareResource(nameof(Address))]
        [DisplayNameResource(nameof(Label.EmailConfirm))]
        public string NotAddress { get; set; }
        [NotMapped]
        public bool NotConfirmed
        {
            get
            {
                return ConfirmationDate.HasValue;
            }
        }
        [NotMapped]
        public EPerson NotPersonTypeId { get; set; }

        public virtual Person Person { get; set; }
    }
}
