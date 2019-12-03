using DataLibrary.BusinessLogic;
using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;


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

        /* schedule jobs
         * 1. Use backendId to compute total time needed
         * 2. check to ensure which lines have "Least amount of work"
         * 3. check to to collect ManufacturingTime.
        */


        // compute time needed

        public void scheduleOrder(Order order)
        {


            DateTime BEdate = calculateBEDate(order.partId, order.quantity, order.shipDate);

            Schedule schedule = new Schedule();
            schedule.BEDate = BEdate;
            schedule.earlistStartDate = order.shipDate.AddDays(1);
            schedule.backendId = getBEId(order.partId);

            // calculate Lasteststartdate. BEdate - time needed for production 
            //check and get the line which is the "most free"
            int lineId = -1;

            Schedule NextfreeLine = new Schedule();
            NextfreeLine = GetNextAvailableLine(); // returns schedule object with next available lineId and plannedStartDate 

            // check result if no result , get line 1 
            if (NextfreeLine == null)
            {
                NextfreeLine.lineId = 1;
            }



            //Calculate BE Time here.
            ManufacturingTime MT = new ManufacturingTime();
            var manufacturingTimeData = manufacturingTimeProcessor.GetManufacturingTime(lineId, order.partId);

            Backend BE = new Backend();

            foreach (var row in manufacturingTimeData)
            {

               // BE.BEID = row.backendId;
              //  BE.partId = row.partId;
              //  BE.processName = row.processName;
              //  BE.duration = row.duration;

            }



        }


        public int getBEId(int partId)
        {
            var backendProcessData = BackendProcessor.getBackendId(partId);
            int BEId = -1;

            foreach (var row in backendProcessData)
            {

                BEId = row.backendId;

            }

            return BEId;
        }


        public DateTime calculateBEDate(int partId, int quantity, DateTime shipDate)
        {
            // depending  on partId selected , get backend time
            var backendProcessData = BackendProcessor.getBackendProcessByID(partId);

            Backend BE = new Backend();

            foreach (var row in backendProcessData)
            {

                BE.BEID = row.backendId;
                BE.partId = row.partId;
                BE.processName = row.processName;
                BE.duration = row.duration;

            }


            // Calculate BE Time here
            // calculate BE time needed (quantity * BE.duration) in sec
            int BackendTotalDuration = quantity * BE.duration;

            // calculate BEdate (order.shipdate - backednTime needed)
            DateTime BEdate = shipDate.AddSeconds(BackendTotalDuration);


            return BEdate;

        }


        // Method retrieves the next available line
        //returns lineId, plannedStartDate
        public Schedule GetNextAvailableLine()
        {

            //SQL query that returns the line which is the next most available

            var data = ScheduleProcessor.LoadNextAvailableLine();

            Schedule schedule = new Schedule();

            if (data == null)
            {
                return null;
            }
            else
            {
                foreach (var row in data)
                {
                    schedule.lineId = row.lineId;
                    schedule.plannedStartDate = row.PlannedStartDate;
                }
                return schedule;
            }
        }


    }
}