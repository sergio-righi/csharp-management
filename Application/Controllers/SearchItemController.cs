using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Domain.Entity;
using Domain.Entity.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    public partial class SearchItemController : BaseController
    {
        private readonly IItemService ItemService;

        public SearchItemController(IItemService itemService)
        {
            ItemService = itemService;
        }

        [HttpPost]
        public async virtual Task<IActionResult> Search(IFormCollection form)
        {
            string feedbackMessage = string.Empty;

            string name = form.ToString("txt_name", null);

            if (!string.IsNullOrWhiteSpace(name))
            {
                var items = await ItemService.List(name, true, false);

                if (items != null)
                {
                    if (items.Count == 1)
                    {
                        return Json(new ResponseJSON { Id = items.ElementAt(0).Id, Name = items.ElementAt(0).Name });
                    }
                    else if (items.Count > 1)
                    {
                        return PartialView("~/Views/Search/Item/_List.cshtml", items);
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

            var item = new Item();

            if (id.IsPositive())
            {
                item = await ItemService.Find(id.Value, false);
            }
            else
            {
                return Json(new ResponseJSON { IsSuccess = false });
            }

            if (item != null)
            {
                return Json(new ResponseJSON { IsSuccess = true, Id = item.Id, Name = item.Name });
            }
            else
            {
                return Json(new ResponseJSON { IsSuccess = false, Message = Message.NotFound });
            }
        }
    }
}