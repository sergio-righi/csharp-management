using Application.Classes;
using Domain.Entity;
using System.Collections.Generic;
using Tool.Utilities;

namespace Application.Models.Drive
{
    public class DriveFilter
    {
        public string FolderName { get; set; }
        public FilterViewModel<File> Filter { get; set; }
        public Pagination<File> Pagination { get; set; }
        public IEnumerable<Folder> Folders { get; set; }
        
        public DriveFilter()
        {
            Filter = new FilterViewModel<File>();
        }
    }
}