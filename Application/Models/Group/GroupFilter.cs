using Application.Classes;
using Domain.Entity;
using Tool.Utilities;

namespace Application.Models.Group
{
    public class GroupFilter
    {
        public FilterViewModel<Domain.Entity.Group> Filter { get; set; }
        public Pagination<Domain.Entity.Group> Pagination { get; set; }
        
        public GroupFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Group>();
        }
    }
}