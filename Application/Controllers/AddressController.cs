using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Service.Interface;
using Tool.Resources;
using Tool.Extensions;
using Application.Models.Address;
using Domain.Entity.Generic;

namespace Application.Controllers
{
    [Route("endereco/")]
    public partial class AddressController : Base.AddressController
    {
        private readonly ICityService CityService;
        private readonly IStateService StateService;
        private readonly IPersonService PersonService;
        private readonly ICountryService CountryService;
        private readonly IAddressService AddressService;

        public AddressController(ICityService cityService,
                                 IStateService stateService,
                                 IPersonService personService,
                                 ICountryService countryService,
                                 IAddressService addressService) : base(cityService, stateService, countryService, addressService)
        {
            CityService = cityService;
            StateService = stateService;
            PersonService = personService;
            CountryService = countryService;
            AddressService = addressService;
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

                var model = new AddressModel();

                if (id.IsZero())
                {
                    /// should be improved (get from client config)
                    model.Address.NotCountryId = 30;
                    model.Address.PersonId = personId;

                    await InsertAddress(model);
                }
                else
                {
                    model.Address = await AddressService.Find(id, false);

                    if (model.Address == null)
                    {
                        return RedirectToAction(nameof(Index), new { personId = personId }).WithWarning(Message.NotFound);
                    }

                    model.Address.NotStateId = model.Address.City.StateId;
                    model.Address.NotCountryId = model.Address.City.CountryId;

                    await UpdateAddress(model);
                }


                model.Address.NotPersonTypeId = person.TypeId;

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("{personId:int}/{id:int}/")]
        public async virtual Task<IActionResult> Index(AddressModel model)
        {
            if (hasSession())
            {
                await UpdateAddress(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        model.Address.Person = null;

                        if (model.Address.Id.IsPositive())
                        {
                            model.Address.UpdatedBy = base.GetCurrentUser();

                            await AddressService.Update(model.Address);
                        }
                        else
                        {
                            model.Address.CreatedBy = base.GetCurrentUser();

                            model.Address = await AddressService.Insert(model.Address);
                        }

                        return RedirectToAction(nameof(Index), new { id = 0, personId = model.Address.PersonId }).WithSuccess(Message.SuccessOnSave);
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
                        var model = await AddressService.Find(id, false);

                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index), new { id = 0, personId = personId }).WithWarning(Message.NotFound);
                        }

                        model.IsDeleted = true;

                        await AddressService.Update(model);

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

        private async Task InsertAddress(AddressModel model)
        {
            model.Countries = await ListCountry();

            model.States = await ListState(model.Address.NotCountryId ?? 0);

            model.Addresses = await ListAddress(model.Address.PersonId);
        }

        private async Task UpdateAddress(AddressModel model)
        {
            model.Countries = await ListCountry();

            model.States = await ListState(model.Address.NotCountryId ?? 0);

            model.Citites = await ListCity(model.Address.NotCountryId, model.Address.NotStateId);

            model.Addresses = await ListAddress(model.Address.PersonId);
        }

        [HttpPost]
        [Route("listar-estado/")]
        public JsonResult ListStateJson()
        {
            return Json(ListState());
        }

        [HttpPost]
        [Route("listar-estado/{countryId:int}")]
        public JsonResult ListStateJson(int countryId)
        {
            return Json(ListState(countryId));
        }

        [HttpPost]
        [Route("listar-cidade-estado/{stateId:int}")]
        public JsonResult ListCityStateJson(int stateId)
        {
            return Json(ListCity(null, stateId));
        }

        [HttpPost]
        [Route("listar-cidade-pais/{countryId:int}")]
        public JsonResult ListCityCountryJson(int countryId)
        {
            return Json(ListCity(countryId, null));
        }

        [HttpPost]
        [Route("listar-endereco/{personId:int}")]
        public JsonResult ListAddressJson(int personId)
        {
            return Json(GListAddress(personId));
        }
    }
}