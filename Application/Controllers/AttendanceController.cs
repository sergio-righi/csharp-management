using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Models;
using Application.Models.Attendance;
using Application.Models.Role;
using Application.Services.Interface;
using Domain.Entity;
using Domain.Entity.Generic;
using Domain.Entity.Specific;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    public class AttendanceController : BaseController
    {
        private const string _routePrefix = "frequencia/";

        private readonly IPersonService _personService;
        private readonly IViewRenderService _renderService;
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IPersonService personService,
                                    IViewRenderService renderService,
                                    IAttendanceService attendanceService)
        {
            _personService = personService;
            _renderService = renderService;
            _attendanceService = attendanceService;
        }

        [Route(_routePrefix)]
        public async Task<IActionResult> Index(AttendanceFilter filter)
        {
            if (hasSession())
            {
                AttendanceFilter attendanceFilter = new AttendanceFilter();

                DateTime date = new DateTime(filter.year, filter.month, 1);

                attendanceFilter.list = await _attendanceService.List(date.Month, date.Year);

                attendanceFilter.year = filter.year;

                attendanceFilter.month = filter.month;

                return View(attendanceFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(_routePrefix + _routeManage + "{year}/{month}/{day}")]
        public async Task<IActionResult> Manage(int year, int month, int day)
        {
            if (hasSession())
            {
                var model = new AttendancePeopleModel()
                {
                    date = new DateTime(year, month, day)
                };

                model.list = await _attendanceService.List(model.date);

                if (model.list.IsNullOrEmpty())
                {
                    return NotFound();
                }

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(_routePrefix + _routeManage + "{year}/{month}/{day}")]
        public async Task<IActionResult> Manage(AttendancePeopleModel model)
        {
            if (hasSession())
            {
                if (ModelState.IsValid)
                {
                    if (!model.list.IsNullOrEmpty())
                    {
                        try
                        {
                            ICollection<Attendance> added = new List<Attendance>();
                            ICollection<Attendance> modified = new List<Attendance>();

                            foreach (var item in model.list)
                            {
                                var attendance = SetAttendance(item);

                                if (attendance.id.IsZero())
                                {
                                    attendance.date = model.date.Date;

                                    attendance.created_at = DateTime.Now;
                                    attendance.created_by = base.GetCurrentUser();

                                    added.Add(attendance);
                                }
                                else
                                {
                                    modified.Add(attendance);
                                }
                            }

                            await _attendanceService.Manage(added, modified);

                            return RedirectToAction(nameof(Manage), new { year = model.date.Year, month = model.date.Month, day = model.date.Day }).WithSuccess(Message.SuccessOnSave);
                        }
                        catch
                        {
                            return View(nameof(Manage), model).WithError(Message.ErrorOnSave);
                        }
                    }
                }

                return View(nameof(Manage), model).WithWarning(Message.FillAllRequiredField);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        public async Task<JsonResult> SearchAttendance(int id, DateTime date)
        {
            if (hasSession())
            {
                if (id.IsPositive())
                {
                    try
                    {
                        var attendancePerson = await _attendanceService.Find(id, date);

                        var contentHTML = await _renderService.RenderToString($"~/Views/Attendance/_FormAttendance.cshtml", attendancePerson);

                        return Json(new ResponseJSON() { header = $"{attendancePerson.name} - {date.ToShortDateString()}", success = true, content = contentHTML });
                    }
                    catch
                    {
                        return Json(new ResponseJSON() { success = false, message = Message.InternalServerError });
                    }
                }

                return Json(new ResponseJSON() { success = false, message = Message.NotFound });
            }

            return Json(new ResponseJSON() { success = false, message = Message.SessionExpired });
        }

        [HttpPost]
        public async Task<JsonResult> ManageAttendance(SAttendancePerson model)
        {
            if (hasSession())
            {
                try
                {
                    if (model.id.IsPositive())
                    {
                        var attendance = await _attendanceService.FindByIdAsync(model.id);

                        if (attendance != null)
                        {
                            attendance.updated_by = GetCurrentUser();

                            attendance.observation = model.observation;

                            await _attendanceService.UpdateAsync(attendance);
                        }
                        else
                        {
                            return Json(new ResponseJSON() { success = false, message = Message.NotFound });
                        }
                    }
                    else
                    {
                        var attendance = new Attendance()
                        {
                            date = model.date,
                            person_id = model.person_id,
                            updated_by = GetCurrentUser(),
                            created_by = GetCurrentUser(),
                            observation = model.observation
                        };

                        await _attendanceService.InsertAsync(attendance);
                    }

                    return Json(new ResponseJSON() { success = true, message = Message.SuccessOnSave });
                }
                catch
                {
                    return Json(new ResponseJSON() { success = false, message = Message.InternalServerError });
                }
            }

            return Json(new ResponseJSON() { success = false, message = Message.SessionExpired });
        }

        private static Expression<Func<Attendance, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.created_at;
            }

            return null;
        }

        private Attendance SetAttendance(SAttendancePerson attendancePerson)
        {
            return new Attendance()
            {
                id = attendancePerson.id,
                updated_at = DateTime.Now,
                date = attendancePerson.date,
                updated_by = GetCurrentUser(),
                person_id = attendancePerson.person_id,
                situation_id = attendancePerson.situation_id
            };
        }
    }
}