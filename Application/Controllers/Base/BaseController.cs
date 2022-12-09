using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Classes;
using Application.Extensions;
using Application.Helpers;
using Domain.Entity.Specific;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tool.Configurations;
using Tool.Utilities;

namespace Application.Controllers.Base
{
    public partial class BaseController : Controller
    {
        private IHttpContextAccessor HttpContextAccessor;

        protected const string RouteDelete = "deletar/";
        protected const string RouteManage = "gerenciar/";
        protected const string RouteDeleteId = RouteDelete + "{id}/";
        protected const string RouteManageId = RouteManage + "{id}/";

        public UserSession SessionUser
        {
            get
            {
                return HttpContext.Session.Get<UserSession>(AppConfig.Session.User);
            }
            set
            {
                HttpContext.Session.Set(AppConfig.Session.User, value);
            }
        }

        public BaseController()
        {
        }

        public BaseController(IHttpContextAccessor httpContextAcessor)
        {
            HttpContextAccessor = httpContextAcessor;
        }

        protected bool hasSession()
        {
            #if DEBUG
            
            SessionUser = new UserSession
            {
                Id = 1,
                Name = "Administrator"
            };

            return true;

            #else

            return SessionUser != null;
            
            #endif
        }

        protected void Log(string location, string message, params int[] parameters)
        {
        }

        protected int GetCurrentUser()
        {
            return UserSession.Get().Id;
        }

        protected string GetIP()
        {
            return Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        protected IActionResult Auth(string login = null)
        {
            return RedirectToAction(nameof(Auth), nameof(EController.Login), new { login = login });
        }

        protected IActionResult Logout(string message)
        {
            string redirect_url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";

            if (ControllerContext.RouteData.Values["controller"].Equals(nameof(EController.Login)))
            {
                redirect_url = null;
            }

            HttpContext.Session.Clear();

            SessionUser = null;

            return RedirectToAction(nameof(Index), nameof(EController.Login), new { redirect_url = redirect_url });
        }

        protected IActionResult RedirectToHome()
        {
            return RedirectToAction(nameof(Index), nameof(EController.Home));
        }

        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction(nameof(Index), nameof(EController.Login));
        }
    }
}