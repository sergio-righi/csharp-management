using Application.Classes;
using Application.Controllers.Base;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Resources;
using Tool.Utilities;
using Tool.Extensions;
using Application.Models.Email;

namespace Application.Controllers
{
    [Route("email/")]
    public partial class EmailController : BaseController
    {
        private readonly IEmailService EmailService;
        private readonly IPersonService PersonService;

        public EmailController(IEmailService emailService,
                               IPersonService personService)
        {
            EmailService = emailService;
            PersonService = personService;
        }

        [Route("{personId:int}/{id:int}")]
        public async virtual Task<IActionResult> Index(int id, int personId)
        {
            if (hasSession())
            {
                var person = await PersonService.Find(personId);

                if (person == null)
                {
                    return NotFound();
                }

                var model = new EmailModel();

                if (id.IsZero())
                {
                    model.Email.PersonId = personId;
                }
                else
                {
                    model.Email = await EmailService.Find(id, false);

                    if (model.Email == null)
                    {
                        return RedirectToAction(nameof(Index), new { personId = personId }).WithWarning(Message.NotFound);
                    }
                }

                await SetInformation(model);

                model.Email.NotPersonTypeId = person.TypeId;

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("{personId:int}/{id:int}")]
        public async virtual Task<IActionResult> Index(EmailModel model)
        {
            if (hasSession())
            {
                await SetInformation(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        model.Email.Person = null;

                        if (model.Email.TypeId == EEmail.Personal)
                        {
                            model.Email.ConfirmationToken = Token.Get();
                        }

                        if (model.Email.Id.IsPositive())
                        {
                            model.Email.UpdatedBy = base.GetCurrentUser();
                         
                            await EmailService.Update(model.Email);
                        }
                        else
                        {
                            model.Email.CreatedBy = base.GetCurrentUser();

                            model.Email = await EmailService.Insert(model.Email);
                        }

                        return RedirectToAction(nameof(Index), new { personId = model.Email.PersonId }).WithSuccess(Message.SuccessOnSave);
                    }
                    catch
                    {
                        return View(nameof(Index), model).WithError(Message.ErrorOnSave);
                    }
                }

                return View(nameof(Index), model).WithWarning(Message.FillAllRequiredField);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteDelete + "{personId:int}/{id:int}")]
        public async virtual Task<IActionResult> Delete(int personId, int id)
        {
            if (hasSession())
            {
                if (id.IsPositive())
                {
                    try
                    {
                        var model = await EmailService.Find(id, false);

                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index), new { id = 0, personId = personId }).WithWarning(Message.NotFound);
                        }

                        model.IsDeleted = true;

                        await EmailService.Update(model);

                        return RedirectToAction(nameof(Index), new { id = 0, personId = personId }).WithSuccess(Message.SuccessOnDelete);
                    }
                    catch
                    {
                        return RedirectToAction(nameof(Index), new { id = id, personId = personId }).WithSuccess(Message.ErrorOnDelete);
                    }
                }

                return BadRequest();
            }

            return Logout(Message.SessionExpired);
        }

        private async Task SetInformation(EmailModel model)
        {
            model.Emails = (await EmailService.List(x => x.PersonId == model.Email.PersonId && x.IsDeleted == false)).ToList();
        }
    }
}
