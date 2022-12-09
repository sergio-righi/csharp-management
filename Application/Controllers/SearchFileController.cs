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
    public partial class SearchFileController : BaseController
    {
        private readonly IFileService FileService;

        public SearchFileController(IFileService fileService)
        {
            FileService = fileService;
        }

        [HttpPost]
        public async virtual Task<IActionResult> Search(IFormCollection form)
        {
            string feedbackMessage = string.Empty;

            string name = form.ToString("txt_name", null);

            int? category = form.ToInt("ddl_category", null);

            if (!string.IsNullOrWhiteSpace(name) || (category.HasValue && category.IsPositive()))
            {
                var files = await FileService.List(name, category, base.GetCurrentUser(), null, true, false, x => x.UpdatedAt, EDirection.Descending, int.MaxValue);

                if (files != null)
                {
                    if (files.Count == 1)
                    {
                        return Json(new ResponseJSON { Id = files.ElementAt(0).Id, Name = files.ElementAt(0).Name });
                    }
                    else if (files.Count > 1)
                    {
                        return PartialView("~/Views/Search/File/_List.cshtml", files);
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
            int? id = form.ToInt("chk_id", null);

            var file = new File();

            if (id.IsPositive())
            {
                file = await FileService.Find(id.Value);
            }
            else
            {
                return Json(new ResponseJSON { IsSuccess = false });
            }

            if (file != null)
            {
                return Json(new ResponseJSON { IsSuccess = true, Id = file.Id, Name = file.Name });
            }
            else
            {
                return Json(new ResponseJSON { IsSuccess = false, Message = Message.NotFound });
            }
        }
    }
}