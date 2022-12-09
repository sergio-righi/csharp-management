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
using Tool.Extensions;
using Application.Models.Phone;

namespace Application.Controllers
{
    [Route("telefone/")]
    public partial class PhoneController : BaseController
    {
        private readonly IPhoneService PhoneService;
        private readonly IPersonService PersonService;

        public PhoneController(IPhoneService phoneService,
                               IPersonService personService)
        {
            PhoneService = phoneService;
            PersonService = personService;
        }

        [Route("{personId:int}/{id:int}")]
        public async virtual Task<IActionResult> Index(int personId, int id)
        {
            if (hasSession())
            {
                var person = await PersonService.Find(personId);

                if (person == null)
                {
                    return NotFound();
                }

                var model = new PhoneModel();

                if (id.IsZero())
                {
                    model.Phone.PersonId = personId;
                }
                else
                {
                    model.Phone = await PhoneService.Find(id, false);

                    if (model.Phone == null)
                    {
                        return RedirectToAction(nameof(Index), new { personId = personId }).WithWarning(Message.NotFound);
                    }
                }

                await SetInformation(model);

                model.Phone.NotPersonTypeId = person.TypeId;

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("{personId:int}/{id:int}")]
        public async virtual Task<IActionResult> Index(PhoneModel model)
        {
            if (hasSession())
            {
                await SetInformation(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        model.Phone.Person = null;

                        if (model.Phone.Id.IsPositive())
                        {
                            model.Phone.UpdatedBy = base.GetCurrentUser();

                            await PhoneService.Update(model.Phone);
                        }
                        else
                        {
                            model.Phone.CreatedBy = base.GetCurrentUser();

                            model.Phone = await PhoneService.Insert(model.Phone);
                        }

                        return RedirectToAction(nameof(Index), new { personId = model.Phone.PersonId }).WithSuccess(Message.SuccessOnSave);
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
                        var model = await PhoneService.Find(id, false);

                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index), new { id = 0, personId = personId }).WithWarning(Message.NotFound);
                        }

                        model.IsDeleted = true;

                        await PhoneService.Update(model);

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

        private async Task SetInformation(PhoneModel model)
        {
            model.Phones = (await PhoneService.List(x => x.PersonId == model.Phone.PersonId && x.IsDeleted == false)).ToList();
        }
    }
}
