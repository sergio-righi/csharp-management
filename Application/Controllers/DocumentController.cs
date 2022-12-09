using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Models.Address;
using Application.Models.Document;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Resources;

namespace Application.Controllers
{
    [Route("documento/")]
    public partial class DocumentController : Base.AddressController
    {
        private readonly IStateService StateService;
        private readonly IPersonService PersonService;
        private readonly ICountryService CountryService;
        private readonly IPersonDocumentService PersonDocumentService;

        public DocumentController(IStateService stateService,
                                  IPersonService personService,
                                  ICountryService countryService,
                                  IPersonDocumentService personDocumentService) : base(stateService, countryService)
        {
            StateService = stateService;
            PersonService = personService;
            CountryService = countryService;
            PersonDocumentService = personDocumentService;
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

                var model = new DocumentModel();

                if (id.IsZero())
                {
                    model.Document.PersonId = personId;

                    await InsertDocument(model);
                }
                else
                {
                    model.Document = await PersonDocumentService.Find(id);

                    if (model.Document == null)
                    {
                        return RedirectToAction(nameof(Index), new { personId = personId }).WithWarning(Message.NotFound);
                    }

                    await UpdateDocument(model);
                }

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("{personId:int}/{id:int}/")]
        public async virtual Task<IActionResult> Index(DocumentModel model)
        {
            if (hasSession())
            {
                await UpdateDocument(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        model.Document.Person = null;

                        if (model.Document.Id.IsPositive())
                        {
                            model.Document.UpdatedBy = base.GetCurrentUser();

                            await PersonDocumentService.Update(model.Document);
                        }
                        else
                        {
                            model.Document.CreatedBy = base.GetCurrentUser();

                            model.Document = await PersonDocumentService.Insert(model.Document);
                        }

                        return RedirectToAction(nameof(Index), new { id = model.Document.Id, personId = model.Document.PersonId }).WithSuccess(Message.SuccessOnSave);
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
                        var model = await PersonDocumentService.Find(id);

                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index), new { id = 0, personId = personId }).WithWarning(Message.NotFound);
                        }

                        await PersonDocumentService.Delete(model);

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

        private async Task InsertDocument(DocumentModel model)
        {
            model.Countries = await ListCountry();

            model.Documents = await ListDocument(model.Document.PersonId);
        }

        private async Task UpdateDocument(DocumentModel model)
        {
            model.States = await ListState(model.Document.CountryId ?? 0);

            model.Countries = await ListCountry();

            model.Documents = await ListDocument(model.Document.PersonId);
        }

        private async Task<ICollection<PersonDocument>> ListDocument(int personId)
        {
            return await PersonDocumentService.List(personId);
        }
    }
}