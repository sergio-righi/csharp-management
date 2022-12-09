using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity.Generic
{
    public class GFormFile
    {
        public IFormFile HttpFile { get; set; }
        public bool IsSetDefault { get; set; }
    }
}
