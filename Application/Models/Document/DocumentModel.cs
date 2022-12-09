using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models.Document
{
    public class DocumentModel
    {
        public ICollection<Domain.Entity.Country> Countries { get; set; }
        public ICollection<Domain.Entity.State> States { get; set; }
        public Domain.Entity.PersonDocument Document { get; set; }
        public ICollection<Domain.Entity.PersonDocument> Documents { get; set; }

        public DocumentModel()
        {
            Document = new Domain.Entity.PersonDocument();
        }
    }
}
