using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;
using Tool.Resources;

namespace Application.Models.Password
{
    public class PasswordModel
    {
        public string Token { get; set; }

        [RequiredResource]
        [MinLengthResource(8)]
        [DataTypeResource(DataType.Password)]
        [DisplayNameResource(nameof(Label.Password))]
        public string Password { get; set; }

        [RequiredResource]
        [CompareResource(nameof(Password))]
        [DataTypeResource(DataType.Password)]
        [DisplayNameResource(nameof(Label.PasswordConfirm))]
        public string PasswordConfirm { get; set; }
    }
}
