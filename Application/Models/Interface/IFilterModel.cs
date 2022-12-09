using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tool.Utilities;

namespace Application.Models
{
    public interface IFilterModel<T>
    {
        int Sort { get; set; }
        int Page { get; set; }
        EDirection Direction { get; set; }
        int Count { get; set; }
        T Model { get; set; }
        string Name { get; set; }
        string Code { get; set; }
        int? SituationId { get; set; }
        int? ReferenceId { get; set; }
        string Activated { get; set; }
        DateTime? EndDate { get; set; }
        DateTime? StartDate { get; set; }

        string GetName();
        bool? IsActivated();
        EDirection GetDirection(int sort);
        void SetDirection(EDirection order);
    }
}