using Application.Classes;
using Domain.Entity;
using Tool.Utilities;

namespace Application.Models.Bill
{
    public class BillFilter
    {
        public FilterViewModel<Domain.Entity.Bill> Filter { get; set; }
        public Pagination<Domain.Entity.Bill> Pagination { get; set; }

        public BillFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Bill>()
            {
                Direction = EDirection.Descending
            };
        }
    }
}