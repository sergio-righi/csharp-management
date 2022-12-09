using System;
using System.Linq;
using Tool.Extensions;
using Tool.Utilities;

namespace Application.Models
{
    public class FilterViewModel<T> : IFilterModel<T>
    {
        public int Page { get; set; }
        public T Model { get; set; }
        public int Sort { get; set; }
        public int Count { get; set; }
        public EDirection Direction { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? SituationId { get; set; }
        public int? ReferenceId { get; set; }
        public string Activated { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }

        public FilterViewModel()
        {
            this.Page = 1;
            this.Sort = 1;
            this.Count = 20;
            this.Direction = EDirection.Ascending;
            this.Model = (T)Activator.CreateInstance(typeof(T));
        }

        public string GetName()
        {
            return string.IsNullOrWhiteSpace(this.Name) ? string.Empty : Name;
        }

        public string GetCode()
        {
            return string.IsNullOrWhiteSpace(this.Code) ? string.Empty : Code.RemoveMask();
        }

        public bool? IsActivated()
        {
            if (!string.IsNullOrWhiteSpace(Activated))
            {
                return Convert.ToBoolean(int.Parse(Activated));
            }

            return null;
        }

        public EDirection GetDirection(int sort)
        {
            if (this.Sort == sort)
            {
                return this.Direction == EDirection.Ascending ? EDirection.Descending : EDirection.Ascending;
            }

            return EDirection.Ascending;
        }

        public void SetDirection(EDirection order)
        {
            this.Direction = order;
        }
    }
}
