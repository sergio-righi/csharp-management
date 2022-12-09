using Application.Models.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Application.Models.User
{
    public class UserModel
    {
        public string Token { get; set; }
        [RequiredResource]
        public string Login { get; set; }
        public PasswordModel Password { get; set; }

        public UserModel()
        {
            Password = new PasswordModel();
        }
    }
}
