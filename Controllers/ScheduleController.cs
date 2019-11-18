using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scheduler.Models;
using DataLibrary;
using DataLibrary.BusinessLogic;


namespace Scheduler.Controllers
{
    public class ScheduleController : Controller
    {
        public ActionResult ViewSchedules()
        {
            //ViewBag.Message = "Order List";

            var data = ScheduleProcessor.LoadSchedule();

            List<Schedule> schedule = new List<Schedule>();

            foreach (var row in data)
            {
                schedule.Add(new Schedule
                {
                    orderId = row.orderId,
                    partId = row.partId,
                    lineId = row.lineId,
                    backendId = row.backendId,
                    BEDate = row.BEDate,
                    earlistStartDate = row.EarliestStartDate,
                    plannedStartDate = row.PlannedStartDate,
                    latestStartDate = row.LatestStartDate,
                    smtStart = row.SMTStart,
                    smtEnd = row.SMTEnd       
                });
            }

            return View(schedule);
        }
    }
}