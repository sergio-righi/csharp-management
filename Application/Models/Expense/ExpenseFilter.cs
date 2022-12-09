using Application.Classes;
using Domain.Entity;
using Tool.Utilities;

namespace Application.Models.Expense
{
    public class ExpenseFilter
    {
        public FilterViewModel<Domain.Entity.Expense> Filter { get; set; }
        public Pagination<Domain.Entity.Expense> Pagination { get; set; }

        public ExpenseFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Expense>();
        }
    }
}