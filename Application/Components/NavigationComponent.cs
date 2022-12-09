using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Service.Interface;

namespace Application.Components
{
    public class NavigationComponent : ViewComponent
    {
        private readonly IFileService FileService;
        private readonly IPersonService PersonService;

        public NavigationComponent(IFileService fileService,
                                   IPersonService personService)
        {
            FileService = fileService;
            PersonService = personService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.photo = await FileService.FindPhoto(0);

            return View("Navigation");
        }
    }
}