using DataLibrary.BusinessLogic;
using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            schedule.earlistStartDate = order.lastMaterialDate.AddSeconds(1);

            // calculate Lasteststartdate. BEdate - time needed for production here 
            //1. check and get the line which is the "most free"

            /*
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
           */

            //Calculate BE Time here.


            var data = ScheduleProcessor.LoadAvailableLineList(order.partId);

            if (data == null | data.Count.Equals(0))
            {

                // check if partId has any ManufacturingTime info
                var getLineData = manufacturingTimeProcessor.GetManufacturingTimeByPart(order.partId);


                if (getLineData == null | getLineData.Count.Equals(0))
                {
                    // no line information for that part
                    return -1;
                }

                schedule.lineId = getLineData[0].lineId;
                schedule.plannedStartDate = schedule.earlistStartDate.AddSeconds(1);

            }
            else
            {

                //check if SMTend date is more then current date 
                if (data[0].SMTEnd <= DateTime.Now)
                {
                    // SMTEnd is lesser then today , set planneStartDate as today + 1 hr
                    schedule.lineId = data[0].lineId;
                    schedule.plannedStartDate = DateTime.Now.AddHours(1);
                }



                schedule.lineId = data[0].lineId;

                // Planned startdate = earliest start date or SMendDate. 
                schedule.plannedStartDate = data[0].SMTEnd.AddHours(1);

            }



            var manufacturingTimeData = manufacturingTimeProcessor.GetManufacturingTime(schedule.lineId, order.partId);

            ManufacturingTime partManufacturingTime = new ManufacturingTime();

            foreach (var row in manufacturingTimeData)
            {
                partManufacturingTime.lineId = row.lineId;
                partManufacturingTime.PartId = row.partId;
                partManufacturingTime.manufacturingTIme = row.manufacturingTime;

            }
            // IF manufacturingTimeData cant be found on any line. return error
            if (manufacturingTimeData == null || manufacturingTimeData[0].Equals(null))
            {
                //display error theres not such line/part combo.
                //string errorMsg = "Manufacturing Time Data does not exist for this part. Enter part/line data in Line page or change part.";

                return 0;
            }

            //calcualte time for SMT process here
            int totalRunningTime = calculateTotalSMTTIme(partManufacturingTime.manufacturingTIme, order.quantity);
            schedule.latestStartDate = schedule.BEDate.AddSeconds(-totalRunningTime);





            //SMT StartDate 
            schedule.smtStart = schedule.plannedStartDate;



            schedule.smtEnd = schedule.smtStart.AddSeconds(totalRunningTime);

            //Add to DB here.
            /*
          CultureInfo provider = CultureInfo.InvariantCulture;
          System.Globalization.DateTimeStyles style = DateTimeStyles.None;
          DateTime dt1;
          DateTime.TryParseExact(schedule.BEDate.ToString(), "yyyy-MM-dd HH:mm:ss", provider, style, out dt1);



          //format string to DB format

          schedule.BEDate = DateTime.ParseExact(schedule.BEDate.ToString(), "dd/MM/yy hh:mm tt", null);
          schedule.earlistStartDate = DateTime.ParseExact(schedule.earlistStartDate.ToString(), "dd/MM/yy hh:mm tt", null);
          schedule.plannedStartDate = DateTime.ParseExact(schedule.plannedStartDate.ToString(), "dd/MM/yy hh:mm tt", null);
          schedule.latestStartDate = DateTime.ParseExact(schedule.latestStartDate.ToString(), "dd/MM/yy hh:mm tt", null);
          schedule.smtStart = DateTime.ParseExact(schedule.smtStart.ToString(), "dd/MM/yy hh:mm tt", null);
          schedule.smtEnd = DateTime.ParseExact(schedule.smtEnd.ToString(), "dd/MM/yy hh:mm tt", null);
          */


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
        //returns lineId, SMTend
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




        /// <summary>
        /// calls LoadAvailableLineList to get list of lines thats processes input part in ascending order
        /// </summary>
        /// <param name="partId"></param>
        /// <returns>List<Schedule></returns>
        public static List<Schedule> LoadAvailableLineList(int partId)
        {


            var data = ScheduleProcessor.LoadAvailableLineList(partId);

            List<Schedule> schedule = new List<Schedule>();

            //  if there is no data (no orders schedule), assign line 1

            if (data == null || data.Count.Equals(0))
            {
                //check which line process parts
                var manufacturingData = manufacturingTimeProcessor.GetManufacturingTimeByPart(partId);
                List<ManufacturingTime> MT = new List<ManufacturingTime>();

                if (manufacturingData == null || manufacturingData.Count.Equals(0))
                {
                    // No line process this part, return error to user

                    return schedule;
                }
                else
                {
                    // set first schedule here, datetime as today


                    schedule.Add(new Schedule
                    {
                        lineId = manufacturingData[0].lineId,
                        smtEnd = DateTime.Now
                    }

                    );
                    return schedule;
                }

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

        public static DateTime CompareDatesNReturnLater(DateTime time1, DateTime time2)
        {
            if (time1 > time2)
                return time1;
            else
                return time2;
        }

    }
}