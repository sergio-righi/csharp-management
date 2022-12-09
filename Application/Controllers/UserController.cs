using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [AllowAnonymous]
    [Route("usuario/")]
    public class UserController : BaseController
    {
        private readonly IPersonService PersonService;
        private readonly IPersonTokenService PersonTokenService;

        public UserController(IPersonService personService,
                              IPersonTokenService personTokenService)
        {
            PersonService = personService;
            PersonTokenService = personTokenService;
        }

        [Route("{token:alpha}")]
        public async virtual Task<IActionResult> Index(string token)
        {
            if (!hasSession())
            {
                var personToken = await PersonTokenService.Find(token, EToken.Rookie);

                if (personToken != null)
                {
                    var model = new UserModel()
                    {
                        Token = token
                    };

                    return View(model);
                }

                return RedirectToLogin().WithWarning(Message.TokenNotFoundOrExpired);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("{token:alpha}")]
        public async virtual Task<IActionResult> Index(UserModel model)
        {
            if (!hasSession())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var personToken = await PersonTokenService.Find(model.Token, EToken.Rookie);

                        if (personToken != null)
                        {
                            var exist = await PersonService.Exist(model.Login);

                            if (!exist)
                            {
                                var person = await PersonService.Find(personToken.PersonId);

                                if (person != null)
                                {
                                    person.IsActivated = true;
                                    person.Login = model.Login;
                                    person.UpdatedBy = person.Id;
                                    person.Password = Cryptography.GetEncrypted(model.Password.Password);

                                    await PersonService.Update(person);

                                    return Auth();
                                }
                            }

                            return View(model).WithWarning(Message.UsernameTaken);
                        }
                    }
                    catch
                    {
                        return View(model).WithError(Message.InternalServerError);
                    }
                }
                else
                {
                    return View(model).WithWarning(Message.FillAllRequiredField);
                }

                return RedirectToLogin();
            }

            return Logout(Message.SessionExpired);
        }
    }
}