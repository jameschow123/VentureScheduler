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

        public static int scheduleOrder(Order order)
        {


            Schedule schedule = new Schedule();
            schedule.orderId = order.orderId;
            schedule.partId = order.partId;

            //BackEnd
            //Get BackendId, Backend date
            DateTime BEdate = calculateBEDate(order.partId, order.quantity, order.shipDate);
            schedule.BEDate = BEdate;
            schedule.backendId = getBEId(order.partId);


            // calculate Lasteststartdate. BEdate - time needed for production here 
            //1. check and get the line which is the "most free"
            int lineId = -1;
            int Index = 0;

            List<Schedule> NextfreeLine = new List<Schedule>();
            NextfreeLine = GetAvailableLines(); // returns schedule object with next available lineId and smtEnd in order of most available line first



            //Calculate BE Time here.
            ManufacturingTime partManufacturingTime = new ManufacturingTime();
            for (int i = 0; i < NextfreeLine.Count; i++)
            {
                // get first available lines lineId
                lineId = NextfreeLine[i].lineId;


                var manufacturingTimeData = manufacturingTimeProcessor.GetManufacturingTime(lineId, order.partId);


                // check to ensure lineId and partID combination exist. if it dosent exist, check another line/part combination.
                // if there is result in manufacturingTimeData , (line and part combo exist) break from loop.
                if (manufacturingTimeData != null)
                {

                    foreach (var row in manufacturingTimeData)
                    {
                        partManufacturingTime.lineId = row.lineId;
                        partManufacturingTime.PartId = row.partId;
                        partManufacturingTime.manufacturingTIme = row.manufacturingTime;

                    }
                    schedule.earlistStartDate = order.lastMaterialDate.AddSeconds(1);
                    //Assuming 1 hour changeover Time.
                    schedule.plannedStartDate = NextfreeLine[i].smtEnd.AddHours(1);
                    schedule.lineId = lineId;
                    break;
                }


            }

            // IF manufacturingTimeData cant be found on any line. return error
            if (partManufacturingTime == null || partManufacturingTime.manufacturingTIme.Equals(null))
            {
                //display error theres not such line/part combo.
                string errorMsg = "Manufacturing Time Data does not exist for this part. Enter part/line data in Line page or change part.";
            }


            //calcualte time for SMT process here
            int totalRunningTime = calculateTotalSMTTIme(partManufacturingTime.manufacturingTIme, order.quantity);
            schedule.latestStartDate = schedule.BEDate.AddSeconds(-totalRunningTime);




            schedule.smtStart = schedule.plannedStartDate;
            schedule.smtEnd = schedule.smtStart.AddSeconds(totalRunningTime);

            //Add to DB here.


            int result = ScheduleProcessor.CreateSchedule(schedule.orderId, schedule.partId, schedule.lineId, schedule.backendId, schedule.BEDate, schedule.earlistStartDate, schedule.plannedStartDate, schedule.latestStartDate, schedule.smtStart, schedule.smtEnd);


            return result;


        }


        public static int getBEId(int partId)
        {
            var backendProcessData = BackendProcessor.getBackendId(partId);
            int BEId = -1;

            foreach (var row in backendProcessData)
            {

                BEId = row.backendId;

            }

            return BEId;
        }


        public static DateTime calculateBEDate(int partId, int quantity, DateTime shipDate)
        {

            try
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
                DateTime BEdate = shipDate.AddSeconds(-BackendTotalDuration);


                return BEdate;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return shipDate;
            }



        }


        // Method retrieves the next available line
        //returns lineId, plannedStartDate
        public static List<Schedule> GetAvailableLines()
        {


            var data = ScheduleProcessor.LoadLinesLastJob();//SQL query that returns lineId and plannedStartDate for all lines last job in order of line that is available first 

            List<Schedule> schedule = new List<Schedule>();

            //  if there is no data (no orders schedule), assign line 1

            if (data == null || data.Count.Equals(0))
            {
                schedule.Add(new Schedule { lineId = 1, smtEnd = DateTime.Today });
                return schedule;

            }
            else
            {
                foreach (var row in data)
                {
                    schedule.Add(new Schedule
                    {
                        lineId = row.lineId,
                        smtEnd = row.SMTEnd

                    });
                }
                return schedule;
            }



        }

        public static int calculateTotalSMTTIme(int timePerPart, int quantity)
        {
            int totalProcessingTime = timePerPart * quantity;

            return totalProcessingTime;
        }


    }
}