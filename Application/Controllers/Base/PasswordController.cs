using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.Base
{
    public partial class PasswordController : Controller
    {
        public virtual IActionResult Index()
        {
            return View();
        }
    }
}