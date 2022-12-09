using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Application.Models.Search.Product
{
    public class SearchProductConfig
    {
        public int Id { get; set; }
        public string SessionKey { get; set; }
        public EController ControllerId { get; set; }
    }
}
