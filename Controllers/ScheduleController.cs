using DataLibrary.BusinessLogic;
using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Linq;
using Scheduler.ViewModel;

namespace Scheduler.Controllers
{
    public class ScheduleController : Controller
    {
        public JsonResult SavePriority(string[] order)
        {
            //string[] data = orden.Split(',');


            return Json("Saved!");
        }


        public ActionResult ViewSchedules()
        {
            if (TempData["ViewSchedulesStatus"] == null)
            {
                //ViewBag.Message = "Order List";

                var data = ScheduleProcessor.LoadSchedule();

                //  List<Schedule> schedule = new List<Schedule>();

                var statusData = OrderProcessor.LoadOrderStatus();
                List<linePartScheduleViewModel> scheduleViewModel = new List<linePartScheduleViewModel>();




                foreach (var row in data)
                {

                    linePartScheduleViewModel linePartSchedule = new linePartScheduleViewModel();


                    linePartSchedule.schedule = new Schedule();



                    linePartSchedule.schedule.orderId = row.orderId;
                    linePartSchedule.schedule.partId = row.partId;
                    linePartSchedule.schedule.lineId = row.lineId;
                    linePartSchedule.schedule.backendId = row.backendId;
                    linePartSchedule.schedule.BEDate = row.BEDate;
                    linePartSchedule.schedule.earlistStartDate = row.EarliestStartDate;
                    linePartSchedule.schedule.plannedStartDate = row.PlannedStartDate;
                    linePartSchedule.schedule.latestStartDate = row.LatestStartDate;
                    linePartSchedule.schedule.smtStart = row.SMTStart;
                    linePartSchedule.schedule.smtEnd = row.SMTEnd;


                    linePartSchedule.lineName = LineController.getLineName(row.lineId);

                    linePartSchedule.partName = PartController.getPartName(row.partId);

                    string Ordername = OrdersController.getOrderByIdReturnName(linePartSchedule.schedule.orderId);
                    linePartSchedule.orderName = Ordername;


                    linePartSchedule.status = statusData.SingleOrDefault(x => x.orderId == linePartSchedule.schedule.orderId).status;

                    scheduleViewModel.Add(linePartSchedule);


                };
                return View(scheduleViewModel);

            }
            else
            {
                List<linePartScheduleViewModel> scheduleViewModel = new List<linePartScheduleViewModel>();
                scheduleViewModel = (List<linePartScheduleViewModel>)TempData["ViewSchedulesStatus"];
                //return View(scheduleViewModel);
                // return RedirectToAction("ViewSchedules", "Schedule");
                return PartialView("schedulesPartial", scheduleViewModel);



            }



        }


        public ActionResult ViewSchedulesStatus(string status, int lineId)
        {
            //ViewBag.Message = "Order List";
            var data = (dynamic)null;


            if (lineId != 0 && status != "null")
            {
                data = ScheduleProcessor.LoadSchedule(status, lineId);

            }
            else if (lineId == 0 && status == "null")
            {
                data = ScheduleProcessor.LoadSchedule();
            }
            else if (lineId != 0 || status != "null")
            {

                if (status != "null")
                {
                    data = ScheduleProcessor.LoadSchedule(status);
                }
                else if (lineId != 0)
                {
                    data = ScheduleProcessor.LoadSchedule(lineId);
                }
            }
           
            //  List<Schedule> schedule = new List<Schedule>();


            List<linePartScheduleViewModel> scheduleViewModel = new List<linePartScheduleViewModel>();

            var statusData = OrderProcessor.LoadOrderStatus();



            foreach (var row in data)
            {

                linePartScheduleViewModel linePartSchedule = new linePartScheduleViewModel();


                linePartSchedule.schedule = new Schedule();



                linePartSchedule.schedule.orderId = row.orderId;
                linePartSchedule.schedule.partId = row.partId;
                linePartSchedule.schedule.lineId = row.lineId;
                linePartSchedule.schedule.backendId = row.backendId;
                linePartSchedule.schedule.BEDate = row.BEDate;
                linePartSchedule.schedule.earlistStartDate = row.EarliestStartDate;
                linePartSchedule.schedule.plannedStartDate = row.PlannedStartDate;
                linePartSchedule.schedule.latestStartDate = row.LatestStartDate;
                linePartSchedule.schedule.smtStart = row.SMTStart;
                linePartSchedule.schedule.smtEnd = row.SMTEnd;


                linePartSchedule.lineName = LineController.getLineName(row.lineId);

                linePartSchedule.partName = PartController.getPartName(row.partId);

                string Ordername = OrdersController.getOrderByIdReturnName(linePartSchedule.schedule.orderId);
                linePartSchedule.orderName = Ordername;

                linePartSchedule.status = statusData.SingleOrDefault(x => x.orderId == linePartSchedule.schedule.orderId).status;


                scheduleViewModel.Add(linePartSchedule);


            };

            TempData["ViewSchedulesStatus"] = scheduleViewModel.ToList();

            return RedirectToAction("ViewSchedules");


        }






        [HttpPost]
        public ActionResult ViewSchedules(int[] orderId, string[] status)
        {
            if (TempData["ViewSchedulesStatus"] == null)
            {
                List<Schedule> schedule = new List<Schedule>();

                //Get List of orders
                List<Order> orders = new List<Order>();
                orders = OrdersController.getListOrders();

                List<Order> sortedListOrders = new List<Order>();


                //Iterate through the array from first to last (priority), call scheduleOrder method to handle scheduling
                for (int i = 0; i < orderId.Length; i++)
                {
                    //check status of order, if order status = processing , skip the order
                    if (status[i] == "processing" || status[i] == "completed")
                    {
                        continue;
                    }
                    Order order = orders.Find(x => x.orderId == orderId[i]);

                    //  reScheduleOrder(order);

                    sortedListOrders.Add(order);

                }


                reScheduleOrder(sortedListOrders);


                return RedirectToAction("ViewSchedules");

            }

            else
            {
                List<linePartScheduleViewModel> scheduleViewModel = new List<linePartScheduleViewModel>();
                scheduleViewModel = (List<linePartScheduleViewModel>)TempData["ViewSchedulesStatus"];
                return View(scheduleViewModel);

            }
        }

        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }


        [HttpPost]
        public ActionResult Index(DateTime startDate, DateTime endDate)
        {


            DateTime startDateUTC = TimeZoneInfo.ConvertTimeToUtc(startDate);
            DateTime endDateUTC = TimeZoneInfo.ConvertTimeToUtc(endDate);

            long startDateUTC2 = ToUnixTime(startDate) * 1000;
            long endDateUTC2 = ToUnixTime(endDate) * 1000;




            //return temp var with datetime format in ISO.

            TempData["index"] = "http://127.0.0.1:5002/getSchedule?from=" + startDateUTC2 + "&to=" + endDateUTC2 + "&color=Status&ref=Yes&db=ventureDB&search=";

            return RedirectToAction("Index", "Home");
        }


        //methods get the 
        public static int reScheduleOrder(List<Order> orders)
        {    int result = -1;

            //1.  get existing scheduling List
            // var ScheduleListData = ScheduleProcessor.LoadSMTStartGroupByLineId();
            var ScheduleListData = ScheduleProcessor.LoadLinesLastJobPending();
            List<Schedule> existingScheduleList = new List<Schedule>();

            foreach (var row in ScheduleListData)
            {

                existingScheduleList.Add(new Schedule
                {

                    lineId = row.lineId,
                    //smtStart = row.SMTStart,
                    smtStart = row.SMTEnd

                });
            }
            // count number of lines , if line is not yet scheduled, set line SmtStart as datetime.now + 1hour
            var lines = LineProcessor.LoadLine();
            int NoOfLine = lines.Count();

            if (NoOfLine != existingScheduleList.Count)
            {

                for (int a = 0; a < NoOfLine; a++)
                {
                    bool lineExistInSchedule = false;
                    Line line = new Line { lineId = lines[a].lineId };


                    // lineExistInSchedule = existingScheduleList.Exists(x => x.lineId == line.lineId);
                    var templine = existingScheduleList.FirstOrDefault(p => p.lineId == line.lineId);

                    if (templine != null)
                    {

                        // if exist , check smt startdate is Not later then current DT, if not set as current DT+1
                        if (templine.smtStart <= DateTime.Now)
                        {
                            //existingScheduleList.FirstOrDefault(p => p.lineId == line.lineId).smtStart = DateTime.Now.AddHours(1);

                            templine.smtStart = DateTime.Now.AddHours(1);
                        }
                        // set the smtEnd date as SMT start date for the next process

                       // templine.smtStart = templine.smtEnd;



                        continue;
                    }
                    else
                        existingScheduleList.Add(new Schedule
                        {

                            lineId = line.lineId,
                            smtStart = DateTime.Now.AddHours(1)


                        });

                }

            }


            // iterate though the new ordered list
            for (int i = 0; i < orders.Count; i++)
            {
             
                Order order = orders[i];

                //2. check which line processes part in order of MT
                // check if partId has any ManufacturingTime info
                var MTData = manufacturingTimeProcessor.GetManufacturingTimeByPart(order.partId);

                // check if there is manufacturing data here, if not continue
                if (MTData.Count == 0 | MTData == null)
                    continue;

                int fastestProcessingLine = -1;
                int totalSMTTime = -1;
                DateTime smtEndDate = DateTime.MaxValue;
                //compute processing time for each line
                for (int j = 0; j < MTData.Count; j++)
                {

                    ManufacturingTime MT = new ManufacturingTime { lineId = MTData[j].lineId, manufacturingTIme = MTData[j].manufacturingTime };

                    int lineSMTTime = calculateTotalSMTTIme(MT.manufacturingTIme, order.quantity);
                    // calcuate fastest line to complete order, compare SMTEnddate
                    //get line SMTStartDate + lineSMTTime

                    Schedule currentLineFirstSchedule = existingScheduleList.Find(x => x.lineId == MT.lineId);

                    if (currentLineFirstSchedule.smtStart <= DateTime.Now)
                    {
                        currentLineFirstSchedule.smtStart = DateTime.Now.AddHours(1);
                    }


                    DateTime linePredictedSMTEnd = currentLineFirstSchedule.smtStart.AddSeconds(lineSMTTime);

                    if (linePredictedSMTEnd < smtEndDate)
                    {
                        smtEndDate = linePredictedSMTEnd;
                        totalSMTTime = lineSMTTime;
                        fastestProcessingLine = MT.lineId;
                    }

                }

                DateTime smtStartDate = smtEndDate.AddSeconds(-totalSMTTime);

                // Reschedule the orderId, update lineId, smtStartDate , EndDate 
                result = ScheduleProcessor.updateSchedule(order.orderId, fastestProcessingLine, smtStartDate, smtEndDate);


                // change existingScheduleList smtStart == Order.SmtEnd
                existingScheduleList.Find(x => x.lineId == fastestProcessingLine).smtStart = smtEndDate;

            }




            return result;


        }


        /* schedule jobs
         * 1. Use backendId to compute total time needed
         * 2. check to ensure which lines have "Least amount of work"
         * 3. check to to collect ManufacturingTime.
        */
        // compute time needed

        public static int scheduleOrder(Order order)
        {

            // check if order exist
            // check if part exist

            try
            {

                Schedule schedule = new Schedule();
                schedule.orderId = order.orderId;
                schedule.partId = order.partId;

                //BackEnd
                //Get BackendId, Backend date
                DateTime BEdate = calculateBEDate(order.partId, order.quantity, order.shipDate);
                schedule.BEDate = BEdate;
                schedule.backendId = getBEId(order.partId);
                schedule.earlistStartDate = order.lastMaterialDate.AddDays(1);

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



                // loads lineId,smtEnd date of lines that is able to process this partId
                var data = ScheduleProcessor.LoadAvailableLineList(order.partId);
                List<Schedule> ExistingSchedule = new List<Schedule>();
                foreach (var row in data)
                {
                    ExistingSchedule.Add(new Schedule
                    {
                        lineId = row.lineId,
                        smtEnd = row.SMTEnd

                    });
                }


                var getLineData = manufacturingTimeProcessor.GetManufacturingTimeByPart(order.partId);
                List<ManufacturingTime> linesThatProcessPartList = new List<ManufacturingTime>();

                foreach (var row in getLineData)
                {
                    linesThatProcessPartList.Add(new ManufacturingTime
                    {
                        lineId = row.lineId,

                    });
                }

                // check if there is any lines that are scheduled
                if (data == null | data.Count.Equals(0))
                {

                    // check if partId has any ManufacturingTime info
                    //  var getLineData = manufacturingTimeProcessor.GetManufacturingTimeByPart(order.partId);


                    if (getLineData == null | getLineData.Count.Equals(0))
                    {
                        // no line information for that part
                        return -1;
                    }

                    schedule.lineId = getLineData[0].lineId;
                    schedule.plannedStartDate = DateTime.Now.AddHours(1);

                }
                else
                {
                    //  var getLineData = manufacturingTimeProcessor.GetManufacturingTimeByPart(order.partId);


                    // check if there are lines that are avaiable lines that are not in use.
                    if (getLineData.Count != data.Count)
                    {



                        var exist = linesThatProcessPartList.Find(p => !ExistingSchedule.Any(p2 => p2.lineId == p.lineId));

                        schedule.lineId = exist.lineId;

                        // Planned startdate = earliest start date or SMendDate. 
                        schedule.plannedStartDate = DateTime.Now.AddHours(1);


                        //find line that is idle and assign that line.

                        /*
                        for (int i = 0; i < linesThatProcessPartList.Count(); i++)
                        {


                            //check though to see if this Id exist in exsiting schedule if not add it with current date.
                            var exist = ExistingSchedule.Exists(x => x.lineId == linesThatProcessPartList[i].lineId);

                            if (exist == false)
                            {
                                ExistingSchedule.Add(new Schedule() { lineId = linesThatProcessPartList[i].lineId, smtEnd = DateTime.Now });


                            }


                        }

                        ExistingSchedule.OrderByDescending(x => x.smtEnd);

                        schedule.lineId = data[0].lineId;

                        // Planned startdate = earliest start date or SMendDate. 
                        schedule.plannedStartDate = data[0].SMTEnd.AddHours(1);
                        */

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
                if (result == 1)
                {
                    //set order scheduled
                    OrdersController.setOrderscheduled(schedule.orderId);

                }

                return result;

            }

            catch (Exception ex)
            {
                return 0;

            }


        }


        public static int getBEId(int partId)
        {
            var backendProcessData = BackendProcessor.getBackendId(partId);
            int BEId = -1;

            foreach (var row in backendProcessData)
            {

                BEId = row.backendId;

            }


            if (BEId == -1)
            {
                // insert BE for this part default time as 0
                int created = BackendProcessor.CreateBackendProcess(partId);
                var partData = BackendProcessor.getBackendId(partId);

                foreach (var row in partData)
                {

                    BEId = row.backendId;

                }



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


        public static int updateSmtStartDate(int orderId, DateTime smtStart)
        {

            var data = ScheduleProcessor.updateScheduleSmtStartDate(orderId, smtStart);


            return data;
        }

        public static int updateSmtEndDate(int orderId, DateTime smtEnd)
        {
            var data = ScheduleProcessor.updateScheduleSmtEndDate(orderId, smtEnd);


            return data;
        }


        /// <summary>
        /// Show Schedule details
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="orderName"></param>
        /// <param name="partName"></param>
        /// <param name="lineName"></param>
        /// <param name="smtStart"></param>
        /// <param name="smtEnd"></param>
        /// <returns></returns>
        public ActionResult scheduleDetails(int Id, string orderName, string partName, string lineName, int lineId, DateTime smtStart, DateTime smtEnd)
        {


            string status = "";
            status = OrdersController.getOrderByID(Id);


            linePartScheduleViewModel order = new linePartScheduleViewModel() { orderName = orderName, partName = partName, lineName = lineName, selectedLine = lineId, status = status, schedule = new Schedule() { orderId = Id, smtStart = smtStart, smtEnd = smtEnd } };



            return View(order);
        }




        /// <summary>
        /// Start of Smt process, change order.status = processing and schedule.smtStart = Datetime.Now
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult smtStart(int Id, string orderName, string partName, string lineName, int lineId, DateTime smtStart, DateTime smtEnd, string status)
        {

            // check if line is processing any job if there is , return error
            var processingLineData = ScheduleProcessor.getLineProcessing();
            List<Schedule> schedules = new List<Schedule>();


            foreach (var row in processingLineData)
            {
                schedules.Add(new Schedule
                {
                   lineId = row.lineId

                });
            }

            bool exist = schedules.Exists(x => x.lineId == lineId);

            if (exist == true)
            {
                TempData["smtStartResults"] = -1;

                return RedirectToAction("scheduleDetails", new { Id, orderName, partName, lineName, lineId, smtStart, smtEnd, status });
            }


            // set order status processing.
            int result1 = OrdersController.setOrderStatusProcessing(Id);
            // update SMT start date to current DT.
            int result2 = updateSmtStartDate(Id, DateTime.Now);
            smtStart = DateTime.Now;

            //updated estimated SMTEND date
            var data = OrderProcessor.LoadOrder(Id);
            Order orderData = new Order();
            foreach (var row in data)
            {
                orderData.partId = row.partId;

            }



            DateTime estimatedEndDate = updateEstimatedEndDate(Id, smtStart, lineId, orderData.partId);


            //get all subsequent jobs 
            updateSubsequentJobs(Id, estimatedEndDate, lineId);
            //reschedule all subsequent orders based on the new start time

            linePartScheduleViewModel order = new linePartScheduleViewModel() { orderName = orderName, partName = partName, lineName = lineName, selectedLine = lineId, status = status, schedule = new Schedule() { orderId = Id, smtStart = smtStart, smtEnd = smtEnd } };

            TempData["smtStartResults"] = result1 + result2;

            return RedirectToAction("scheduleDetails", new { Id, orderName, partName, lineName, lineId, smtStart, smtEnd, status });
        }

        /// <summary>
        /// Start of Smt process, change order.status = processing and schedule.smtEnd = Datetime.Now
        /// Update subsequent jobs smtStartDate based on current job SMT end
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult smtEnd(int Id, string orderName, string partName, string lineName, int lineId, DateTime smtStart, DateTime smtEnd, string status)
        {



            // set order status processing.
            int result1 = OrdersController.setOrderStatusCompleted(Id);
            // update SMT start date to current DT.
            int result2 = updateSmtEndDate(Id, DateTime.Now);
            if (result1 == 1)
            {
                status = "completed";
            }
            if (result2 == 1)
            {
                smtEnd = DateTime.Now;
            }
            // update subsequent jobs

            updateSubsequentJobs(Id, smtEnd, lineId);

            linePartScheduleViewModel order = new linePartScheduleViewModel() { orderName = orderName, partName = partName, lineName = lineName, selectedLine = lineId, status = status, schedule = new Schedule() { orderId = Id, smtStart = smtStart, smtEnd = smtEnd } };

            TempData["smtStartResults"] = result1 + result2;



            return RedirectToAction("scheduleDetails", new { Id, orderName, partName, lineName, lineId, smtStart, smtEnd, status });
        }


        public static void updateSubsequentJobs(int orderId, DateTime smtEnd, int lineId)
        {

            //select all jobs after current jobs returned in order of smtstartDates
            List<Schedule> scheduleList = getSubsequentJobs(lineId, smtEnd);



            for (int i = 0; i < scheduleList.Count; i++)
            {
                // get scheduled jobs and update SMTstart and smt

                Schedule schedule = scheduleList[i];

                schedule.smtStart = smtEnd.AddMinutes(60);
                var getLineData = manufacturingTimeProcessor.GetManufacturingTime(lineId, schedule.partId);
                ManufacturingTime mt = new ManufacturingTime();

                foreach (var row in getLineData)
                {
                    mt.manufacturingTIme = row.manufacturingTime;

                }

                int quantity = OrdersController.getQuantityByID(orderId);
                int totalTime = calculateTotalSMTTIme(mt.manufacturingTIme, quantity);

                schedule.smtEnd = schedule.smtStart.AddSeconds(totalTime);
                smtEnd = schedule.smtEnd;
                int results = ScheduleProcessor.updateSchedule(schedule.orderId, lineId, schedule.smtStart, schedule.smtEnd);



            }



        }

        public static List<Schedule> getSubsequentJobs(int lineId, DateTime smtEnd)
        {

            //select all jobs after current jobs
            var ScheduleListData = ScheduleProcessor.LoadSubsequentSchedule(lineId, smtEnd);

            List<Schedule> scheduleList = new List<Schedule>();

            foreach (var row in ScheduleListData)
            {

                scheduleList.Add(new Schedule
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
                    smtEnd = row.SMTEnd,


                });
            }

            return scheduleList;


        }



        public static DateTime updateEstimatedEndDate(int orderId, DateTime smtStart, int lineId, int partId)
        {


            var MTTimeByPart = manufacturingTimeProcessor.GetManufacturingTime(lineId, partId);
            ManufacturingTime mt = new ManufacturingTime();
            foreach (var row in MTTimeByPart)
            {
                mt.manufacturingTIme = row.manufacturingTime;

            }

            int quantity = OrdersController.getQuantityByID(orderId);


            int totalTIme = calculateTotalSMTTIme(mt.manufacturingTIme, quantity);


            DateTime smtEnd = smtStart.AddSeconds(totalTIme);

            ScheduleProcessor.updateScheduleSmtEndDate(orderId, smtEnd);


            return smtEnd;



        }







    }


}
