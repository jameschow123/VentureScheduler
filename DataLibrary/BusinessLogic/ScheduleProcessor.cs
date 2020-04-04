using System;
using DataLibrary.DataAccess;
using DataLibrary.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Models;

namespace DataLibrary.BusinessLogic
{
    public static class ScheduleProcessor
    {
        public static int DeleteSchedule(int orderId)
        {
            ScheduleModel data = new ScheduleModel
            {
                orderId = orderId
            };
            string sql = @"delete from [Schedule] where orderId = @orderId;";

            return SqlDataAccess.SaveData(sql, data);

        }

        public static List<ScheduleModel> LoadSchedule()
        {
            string sql = @"select orderId,partId,lineId,backendId,BEDate,EarliestStartDate,
                         PlannedStartDate,LatestStartDate,SMTStart,SMTEnd 
                        from [Schedule]  order by SMTStart;";

            return SqlDataAccess.LoadData<ScheduleModel>(sql);
        }



        public static List<ScheduleModel> LoadSchedule(string status)
        {
            ScheduleModel data = new ScheduleModel
            {

                status = status

            };

            string sql = @"select orderId,partId,lineId,backendId,BEDate,EarliestStartDate,
                         PlannedStartDate,LatestStartDate,SMTStart,SMTEnd 
                        from [Schedule] where orderId in (select orderId from [Order] where status=@status)  order by SMTStart;";

            return SqlDataAccess.LoadData<ScheduleModel>(sql, data);
        }

        public static List<ScheduleModel> LoadSchedule(int lineId)
        {
            ScheduleModel data = new ScheduleModel
            {

                lineId = lineId

            };

            string sql = @"select orderId,partId,lineId,backendId,BEDate,EarliestStartDate,
                         PlannedStartDate,LatestStartDate,SMTStart,SMTEnd 
                        from [Schedule] where lineId=@lineId order by SMTStart;";

            return SqlDataAccess.LoadData<ScheduleModel>(sql, data);
        }






        public static List<ScheduleModel> LoadSchedule(string status,int lineId)
        {
            ScheduleModel data = new ScheduleModel
            {

                status = status,
                lineId = lineId

            };

            string sql = @"select orderId,partId,lineId,backendId,BEDate,EarliestStartDate,
                         PlannedStartDate,LatestStartDate,SMTStart,SMTEnd 
                        from [Schedule] where orderId in (select orderId from [Order] where status=@status) and lineId=@lineId  order by SMTStart;";

            return SqlDataAccess.LoadData<ScheduleModel>(sql, data);
        }

        public static List<ScheduleModel> LoadSMTStartGroupByLineId()
        {
            string sql = @"select lineId,Min(SMTStart)as SMTStart from [Schedule] group by lineID;";

            return SqlDataAccess.LoadData<ScheduleModel>(sql);
        }


        /// <summary>
        /// Deprecated code;
        /// Previously used for reschedule order to take everylines last jobs date and schedule jobs from there. Use LoadLinesLastJobPending now.
        /// </summary>
        /// <returns> lines and last smtEnd date for everyline</returns>
        public static List<ScheduleModel> LoadLinesLastJob()
        {
            string sql = @"select lineID,max(SMTEnd) as SMTEnd from [Schedule]  group by lineID;";

            return SqlDataAccess.LoadData<ScheduleModel>(sql);
        }


        /// <summary>
        /// Function gets a list of lines that are currently processing orders
        /// </summary>
        /// <returns> List<scheduleModel> with lineId of lines that are processing</returns>
        public static List<ScheduleModel> getLineProcessing()
        {
            string sql = @"select lineID from schedule S where orderId in (select orderId from[Order] O where status = 'processing');";

            return SqlDataAccess.LoadData<ScheduleModel>(sql);
        }
        /// <summary>
        /// Takes line and SMTEND of every job that is pending.
        /// </summary>
        /// <returns> lines and last smtEnd date for everyline WITH status equals pending.</returns>
        public static List<ScheduleModel> LoadLinesLastJobPending()
        {
            string sql = @"select lineID, max(SMTEnd) as SMTEnd from[Schedule] S where Exists(select orderId, status from [Order] O where status = 'processing' and S.orderId = O.orderId)   group by lineID;";

            return SqlDataAccess.LoadData<ScheduleModel>(sql);
        }

        /// <summary>
        /// Subquery checks all lines if they have partId as information (Possible that some lines does not process certain parts )
        /// main query retrieves line and last SMTend date from schedule based on subquery line information
        /// </summary>
        /// <param name="partId"></param>
        /// <returns>LineID and last SMTend date for each line that can process part  </returns>
        public static List<ScheduleModel> LoadAvailableLineList(int partId)
        {

            ScheduleModel data = new ScheduleModel
            {

                partId = partId,

            };

            string sql = @"select lineID ,max(SMTEnd) as SMTEnd from [Schedule] where lineID IN (select lineId as lineID from ManufacturingTime 
                            where partId = @partId group by lineId)group by lineID order by SMTEnd ASC ;";


            return SqlDataAccess.LoadData<ScheduleModel>(sql, data);
        }






        public static int CreateSchedule(int orderId, int partId, int lineId, int backendId, DateTime BEDate, DateTime EarliestStartDate, DateTime PlannedStartDate, DateTime LatestStartDate, DateTime SMTStart, DateTime SMTEnd)
        {



            ScheduleModel data = new ScheduleModel
            {
                orderId = orderId,
                partId = partId,
                lineId = lineId,
                backendId = backendId,
                BEDate = BEDate,
                EarliestStartDate = EarliestStartDate,
                PlannedStartDate = PlannedStartDate,
                LatestStartDate = LatestStartDate,
                SMTStart = SMTStart,
                SMTEnd = SMTEnd
            };

            string sql = @"insert into [Schedule] (orderId,partId,lineId,backendId,BEDate,EarliestStartDate,PlannedStartDate,LatestStartDate,SMTStart,SMTEnd)
                            values (@orderId,@partId,@lineId,@backendId,CONVERT(datetime,@BEDate,104),CONVERT(datetime,@EarliestStartDate,104),CONVERT(datetime,@PlannedStartDate,104),CONVERT(datetime,@LatestStartDate,104),CONVERT(datetime,@SMTStart,104),CONVERT(datetime,@SMTEnd,104));";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static int updateSchedule(int orderId, int lineId, DateTime smtStart, DateTime smtEnd)
        {
            ScheduleModel data = new ScheduleModel
            {
                orderId = orderId,
                lineId = lineId,
                SMTStart = smtStart,
                SMTEnd = smtEnd
            };
            string sql = @"update [Schedule]  set lineId = @lineId , SMTStart = CONVERT(datetime,@SMTStart,104),SMTEnd=CONVERT(datetime,@SMTEnd,104) where  orderId = @orderId;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static int updateScheduleSmtStartDate(int orderId, DateTime smtStart)
        {
            ScheduleModel data = new ScheduleModel
            {
                orderId = orderId,
                SMTStart = smtStart,

            };
            string sql = @"update [Schedule]  set SMTStart = CONVERT(datetime,@SMTStart,104) where  orderId = @orderId;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int updateScheduleSmtEndDate(int orderId, DateTime SMTEnd)
        {
            ScheduleModel data = new ScheduleModel
            {
                orderId = orderId,
                SMTEnd = SMTEnd,

            };
            string sql = @"update [Schedule]  set SMTEnd = CONVERT(datetime,@SMTEnd,104) where  orderId = @orderId;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<ScheduleModel> LoadSubsequentSchedule(int lineId, DateTime SMTEnd)
        {

            ScheduleModel data = new ScheduleModel
            {
                lineId = lineId,
                SMTEnd = SMTEnd

            };

            string sql = @"select orderId,partId,lineId,backendId,BEDate,EarliestStartDate,
                         PlannedStartDate,LatestStartDate,SMTStart,SMTEnd 
                        from [Schedule]  where lineId = @lineId and SMTStart > CONVERT(datetime,@SMTEnd,104) order by SMTStart asc;";

            return SqlDataAccess.LoadData<ScheduleModel>(sql, data);
        }


    }
}
