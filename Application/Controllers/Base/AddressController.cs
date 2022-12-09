using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Entity.Generic;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;

namespace Application.Controllers.Base
{
    public class AddressController : BaseController
    {
        private readonly ICityService CityService;
        private readonly IStateService StateService;
        private readonly IAddressService AddressService;
        private readonly ICountryService CountryService;

        public AddressController(ICityService cityService,
                                 IStateService stateService)
        {
            CityService = cityService;
            StateService = stateService;
        }

        public AddressController(IStateService stateService,
                                 ICountryService countryService)
        {
            StateService = stateService;
            CountryService = countryService;
        }

        public AddressController(ICityService cityService,
                                 IStateService stateService,
                                 ICountryService countryService,
                                 IAddressService addressService)
        {
            CityService = cityService;
            StateService = stateService;
            CountryService = countryService;
            AddressService = addressService;
        }

        protected async Task<ICollection<Country>> ListCountry()
        {
            return await CountryService.List(true);
        }

        protected async Task<ICollection<State>> ListState(int countryId)
        {
            return await StateService.List(countryId, true);
        }

        protected async Task<ICollection<State>> ListState()
        {
            return await StateService.List(true);
        }

        protected async Task<ICollection<City>> ListCity(int? countryId, int? stateId)
        {
            return await CityService.List(countryId, stateId, true);
        }

        protected async Task<ICollection<Address>> ListAddress(int personId)
        {
            return await AddressService.List(personId, false);
        }

        protected async Task<ICollection<GCustomInformation>> GListAddress(int personId)
        {
            return await AddressService.GList(personId, false);
        }
    }
}