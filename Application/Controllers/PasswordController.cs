using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Models.Password;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [AllowAnonymous]
    [Route("redefinir-senha/")]
    public class PasswordController : BaseController
    {
        private readonly IPersonService PersonService;
        private readonly IPersonTokenService PersonTokenService;

        public PasswordController(IPersonService personService,
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
                var productPassword = await PersonTokenService.Find(token, EToken.Password);

                if (productPassword != null)
                {
                    var password = new PasswordModel
                    {
                        Token = productPassword.Token
                    };

                    return View(password);
                }

                return RedirectToLogin().WithWarning(Message.TokenNotFoundOrExpired);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateRecaptcha(0.7)]
        //[ValidateAntiForgeryToken]
        [Route("{token:alpha}")]
        public async virtual Task<IActionResult> Index(PasswordModel model)
        {
            if (!hasSession())
            {
                string message = string.Empty;

                if (ModelState.IsValid)
                {
                    var personToken = await PersonTokenService.Find(model.Token, EToken.Password);

                    if (personToken != null)
                    {
                        var person = await PersonService.Find(personToken.PersonId);

                        if (person != null)
                        {
                            if (Util.IsValidPassword(model.Password))
                            {
                                try
                                {
                                    person.UpdatedBy = person.Id;
                                    person.Password = Cryptography.GetEncrypted(model.Password);

                                    personToken.UpdatedBy = person.Id;
                                    personToken.IpAddress = base.GetIP();
                                    personToken.TokenId = EToken.Password;
                                    personToken.ExpiryDate = DateTime.Now;

                                    bool success = await PersonTokenService.Manage(person, personToken);

                                    if (success)
                                    {
                                        Mail.Send(null, null, null);

                                        return Auth(person.Login).WithSuccess(Message.PasswordUpdatedSuccess);
                                    }
                                    else
                                    {
                                        message = Message.SomethingWentWrong;
                                    }
                                }
                                catch (Exception)
                                {
                                    message = Message.InternalServerError;
                                }
                            }
                            else
                            {
                                message = Message.PasswordPolicy;
                            }
                        }
                    }
                }
                else
                {
                    if (ViewData.ModelState["ReCaptcha"].Errors.Count > 0)
                    {
                        message = Message.NotRobotRequired;
                    }
                    else
                    {
                        message = Message.FillAllRequiredField;
                    }
                }

                return RedirectToAction(nameof(Index), model).WithWarning(message);
            }

            return Logout(Message.SessionExpired);
        }
    }
}