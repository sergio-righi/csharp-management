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
    public class NaturalPerson : BaseEntity
    {
        [DisplayNameResource(nameof(Label.Gender))]
        public EGender GenderId { get; set; }
        [RequiredResource]
        [StringLength(75)]
        [DisplayNameResource(nameof(Label.GivenName))]
        public string GivenName { get; set; }
        [RequiredResource]
        [StringLength(75)]
        [DisplayNameResource(nameof(Label.Surname))]
        public string Surname { get; set; }
        [StringLength(150)]
        public string FullName { get; set; }
        [StringLength(150)]
        [DisplayNameResource(nameof(Label.SocialName))]
        public string SocialName { get; set; }
        [DisplayNameResource(nameof(Label.Birthday))]
        public DateTime? Birthday { get; set; }

        [NotMapped]
        public string NotName
        {
            get
            {
                return string.IsNullOrWhiteSpace(SocialName) ? this.FullName : this.SocialName;
            }
        }

        public virtual Person Person { get; set; }
    }
}
