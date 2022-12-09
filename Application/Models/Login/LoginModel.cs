using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Application.Models.Login
{
    public class LoginModel
    {
        public bool Found { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string RedirectUrl { get; set; }
        public string Photo { get; set; }
        public string FirstLetter { get; set; }
        public bool RememberMe { get; set; }
    }
}
