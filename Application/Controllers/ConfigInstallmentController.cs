using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Extensions;
using Application.Services.Interface;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Service.Service;
using Service.Service.Interface;
using Tool.Configurations;
using Tool.Extensions;
using Tool.Resources;

namespace Application.Controllers
{
    [Route("prestacao/")]
    public class ConfigInstallmentController : BaseController
    {
        private readonly IViewRenderService ViewRenderService;
        private readonly IInstallmentService InstallmentService;
        private readonly IInstallmentInfoService InstallmentInfoService;

        public Installment SessionInstallment
        {
            get { return HttpContext.Session.Get<Installment>(AppConfig.Session.Installment); }
            set { HttpContext.Session.Set(AppConfig.Session.Installment, value); }
        }

        public ConfigInstallmentController(IViewRenderService viewRenderService,
                                           IInstallmentService installmentService,
                                           IInstallmentInfoService installmentInfoService)
        {
            ViewRenderService = viewRenderService;
            InstallmentService = installmentService;
            InstallmentInfoService = installmentInfoService;
        }

        [Route("{installmentId:int?}/{date}/{quantity:int}")]
        public async virtual Task<IActionResult> Config(int installmentId, DateTime date, int quantity)
        {
            if (hasSession())
            {
                SessionInstallment = null;

                if (date != null && quantity.IsPositive())
                {
                    Installment installment = await InstallmentService.Find(installmentId);

                    if (installment == null)
                    {
                        installment = new Installment
                        {
                            Dates = new List<InstallmentInfo>()
                        };

                        for (var i = 1; i <= quantity; i++)
                        {
                            installment.Dates.Add(new InstallmentInfo
                            {
                                Number = i,
                                Date = date
                            });

                            date = date.AddMonths(1).GetNextWorkday();
                        }
                    }
                    else
                    {
                        if (!installment.Dates.IsNullOrEmpty())
                        {
                            if (installment.Dates.Count > quantity)
                            {
                                int difference = installment.Dates.Count - quantity;

                                for (var i = installment.Dates.Count - 1; difference > 0; i--, difference--)
                                {
                                    var item = installment.Dates.ElementAt(i);

                                    if (item.Id.IsPositive())
                                    {
                                        item.NotIsDeleted = true;
                                    }
                                    else
                                    {
                                        installment.Dates.Remove(item);
                                    }
                                }
                            }
                            else if (installment.Dates.Count < quantity)
                            {
                                date = installment.Dates.LastOrDefault().Date;

                                for (var i = installment.Dates.Count + 1; i <= quantity; i++)
                                {
                                    date = date.AddMonths(1).GetNextWorkday();

                                    installment.Dates.Add(new InstallmentInfo
                                    {
                                        Number = i,
                                        Date = date
                                    });
                                }
                            }
                        }
                    }

                    var contentHTML = await ViewRenderService.RenderToString("~/Views/Config/Installment/_Form.cshtml", installment);

                    return Json(new ResponseJSON { IsSuccess = true, Content = contentHTML });
                }
                else
                {
                    return Json(new ResponseJSON { IsSuccess = false, Message = Message.FilterRequired });
                }
            }

            return Json(new ResponseJSON { IsSuccess = false, Message = Message.SessionExpired });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async virtual Task<JsonResult> Save(Installment model)
        {
            if (hasSession())
            {
                if (ModelState.IsValid)
                {
                    if (!model.Dates.IsNullOrEmpty())
                    {
                        model.UpdatedAt = DateTime.Now;
                        model.UpdatedBy = base.GetCurrentUser();

                        if (model.Id.IsZero())
                        {
                            model.CreatedAt = DateTime.Now;
                            model.CreatedBy = base.GetCurrentUser();
                        }

                        foreach (var item in model.Dates)
                        {
                            item.UpdatedAt = DateTime.Now;
                            item.UpdatedBy = base.GetCurrentUser();

                            if (item.Id.IsZero())
                            {
                                item.CreatedAt = DateTime.Now;
                                item.CreatedBy = base.GetCurrentUser();
                            }
                        }

                        SessionInstallment = model;

                        return Json(new ResponseJSON { Id = model.Dates.Count, IsSuccess = true, Message = Message.SuccessOnSave });
                    }
                    else
                    {
                        return Json(new ResponseJSON { IsSuccess = false, Message = Message.NoneSelected });
                    }
                }
                else
                {
                    return Json(new ResponseJSON { IsSuccess = false, Message = Message.FillAllRequiredField });
                }
            }

            return Json(new ResponseJSON { IsSuccess = false, Message = Message.SessionExpired });
        }
    }
}