using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Domain.Entity
{
    public class PersonToken : BaseEntity
    {
        public int PersonId { get; set; }
        public EToken TokenId { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string Token { get; set; }
        public string IpAddress { get; set; }
        public DateTime ExpiryDate { get; set; }

        public virtual Person Person { get; set; }
    }
}
