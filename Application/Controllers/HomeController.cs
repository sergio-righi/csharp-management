using Microsoft.AspNetCore.Mvc;
using Application.Controllers.Base;
using Tool.Utilities;
using Tool.Resources;
using Microsoft.AspNetCore.Http;

namespace Application.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IHttpContextAccessor httpContextAcessor) : base(httpContextAcessor)
        {

        }

        [Route("")]
        public virtual IActionResult Index()
        {
            if (hasSession())
            {
                //ViewBag.games = Util.GetGame(new int[] { });

                return View();
            }

            return Logout(Message.SessionExpired);
        }
    }
}