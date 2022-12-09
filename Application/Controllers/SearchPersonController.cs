using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Domain.Entity.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    public partial class SearchPersonController : BaseController
    {
        private readonly IPersonService PersonService;

        public SearchPersonController(IPersonService personService)
        {
            PersonService = personService;
        }

        [HttpPost]
        public async virtual Task<IActionResult> Search(IFormCollection form)
        {
            string feedbackMessage = string.Empty;

            string name = form.ToString("txt_name", null);

            string document = form.ToString("txt_document", null).RemoveMask();

            EPerson typeId = form.ToEnum("rbt_person", EPerson.Natural);

            if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(document))
            {
                var people = await PersonService.GList(typeId, name, document);

                if (people != null)
                {
                    if (people.Count == 1)
                    {
                        return Json(new ResponseJSON { Id = people.ElementAt(0).Id, Name = people.ElementAt(0).Name });
                    }
                    else if (people.Count > 1)
                    {
                        return PartialView("~/Views/Search/Person/_List.cshtml", people);
                    }
                    else
                    {
                        feedbackMessage = Message.NothingToList;
                    }
                }
            }
            else
            {
                feedbackMessage = Message.FilterRequired;
            }

            return Json(new ResponseJSON { Message = feedbackMessage });
        }

        [HttpPost]
        public async virtual Task<JsonResult> Select(IFormCollection form)
        {
            int? id = form.ToInt("rbt_id", null);

            var customInformation = new GCustomInformation();

            if (id.IsPositive())
            {
                customInformation = await PersonService.GFind(id ?? 0);
            }
            else
            {
                return Json(new ResponseJSON { IsSuccess = false });
            }

            if (customInformation != null)
            {
                return Json(new ResponseJSON { IsSuccess = true, Id = customInformation.Id, Name = customInformation.Name });
            }
            else
            {
                return Json(new ResponseJSON { IsSuccess = false, Message = Message.NotFound });
            }
        }
    }
}