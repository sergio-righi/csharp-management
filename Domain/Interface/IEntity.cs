using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface IEntity
    {
        int Id { get; set; }
        int UpdatedBy { get; set; }
        DateTime UpdatedAt { get; set; }
        int CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
