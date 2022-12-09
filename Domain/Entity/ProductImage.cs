using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class ProductImage : BaseEntity
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual File Image { get; set; }
        public virtual Product Product { get; set; }
    }
}
