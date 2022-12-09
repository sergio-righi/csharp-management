using Application.Classes;
using Application.Controllers.Base;
using Application.Extensions;
using Application.Models.Login;
using Domain.Entity;
using Domain.Entity.Specific;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Service.Interface;
using System;
using System.Threading.Tasks;
using Tool.Configurations;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [AllowAnonymous]
    public partial class LoginController : BaseController
    {
        private readonly IPersonService PersonService;
        private readonly IPersonTokenService PersonTokenService;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public LoginController(IPersonService personService,
                               IPersonTokenService personTokenService,
                               IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            PersonService = personService;
            PersonTokenService = personTokenService;
            HttpContextAccessor = httpContextAccessor;
        }

        [Route("entrar/")]
        public async virtual Task<IActionResult> Index(LoginModel model)
        {
            if (hasSession())
            {
                return RedirectToHome();
            }
            else
            {
                var cookie = HttpContextAccessor.HttpContext.GetCookie<UserSession>(AppConfig.Cookie.Login);

                if (cookie != null)
                {
                    SessionUser = cookie;

                    SessionUser.Token = Token.Get();

                    return RedirectToHome();
                }
            }

            return View(model);
        }

        [HttpPost]
        [Route("entrar/")]
        public async virtual Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                model.Found = await PersonService.Exist(model.Login);

                if (!model.Found)
                {
                    model.Login = null;

                    return RedirectToAction(nameof(Index), model).WithWarning(Message.UserNotFound);
                }
                else
                {
                    var userPerson = await PersonService.SFind(model.Login, model.Password);

                    if (string.IsNullOrEmpty(model.Password))
                    {
                        model.Photo = userPerson.FullPhotoPath;
                        model.FirstLetter = userPerson.Name.FirstLetter();
                    }
                    else
                    {
                        if (model.Found)
                        {
                            if (userPerson == null)
                            {
                                model.Password = null;

                                return RedirectToAction(nameof(Index), model).WithWarning(Message.IncorrectPassword);
                            }
                            else
                            {
                                SessionUser = new UserSession(userPerson);

                                if (model.RememberMe)
                                {
                                    HttpContextAccessor.HttpContext.SetCookie(SessionUser, AppConfig.Cookie.Login);
                                }

                                SessionUser.Token = Token.Get();

                                if (!string.IsNullOrEmpty(model.RedirectUrl))
                                {
                                    return Redirect(model.RedirectUrl);
                                }
                            }
                        }
                    }

                    return RedirectToAction(nameof(Index), model);
                }
            }
            catch
            {
                return RedirectToAction(nameof(Index), model).WithError(Message.InternalServerError);
            }
        }

        [Route("limpar/")]
        public async virtual Task<IActionResult> Clean(LoginModel model)
        {
            HttpContextAccessor.HttpContext.RemoveCookie(AppConfig.Cookie.Login);

            return View(nameof(Index), model);
        }

        [HttpPost]
        [Route("esqueci-minha-senha/")]
        public async virtual Task<IActionResult> Forgot(LoginModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Login))
                {
                    var userPerson = await PersonService.SFind(model.Login, null);

                    if (userPerson != null)
                    {
                        var personToken = await PersonTokenService.Find(userPerson.Id, EToken.Password);

                        if (personToken != null)
                        {
                            personToken.ExpiryDate = DateTime.Now;

                            await PersonTokenService.Update(personToken);
                        }

                        personToken = new PersonToken()
                        {
                            Token = Token.Get(128),
                            PersonId = userPerson.Id,
                            IpAddress = base.GetIP(),
                            TokenId = EToken.Password,
                            CreatedBy = userPerson.Id,
                            ExpiryDate = DateTime.Now.AddDays(1)
                        };

                        await PersonTokenService.Insert(personToken);

                        await Mail.Send(null, null, null);

                        return View(nameof(Index), model).WithWarning(Message.EmailRecoverSent);
                    }
                }
            }
            catch
            {
                return View(nameof(Index), model).WithWarning(Message.InternalServerError);
            }

            return View(nameof(Index), model).WithWarning(Message.UserNotFound);
        }

        [Route("sair/")]
        public async virtual Task<IActionResult> Logout()
        {
            HttpContextAccessor.HttpContext.RemoveCookie(AppConfig.Cookie.Login);

            return base.Logout(Message.Logout);
        }

        public async virtual Task<IActionResult> Auth(string login)
        {
            if (!hasSession())
            {
                if (!string.IsNullOrWhiteSpace(login))
                {
                    var userPerson = await PersonService.SFind(login, null);

                    if (userPerson != null)
                    {
                        SessionUser = (new UserSession(userPerson));

                        SessionUser.Token = Token.Get();
                    }
                }
            }

            return Logout(Message.SessionExpired);
        }
    }
}