using DataLibrary.DataAccess;
using DataLibrary.models;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class manufacturingTimeProcessor
    {

        public static List<manufacturingTimeModel> GetManufacturingTime(int lineId, int partId)
        {
            manufacturingTimeModel data = new manufacturingTimeModel
            {
                lineId = lineId,
                partId = partId
            };

            string sql = @"select lineID,partId, ManufacturingTime from [ManufacturingTime] where lineId = @lineId and partId = @partId;";



            return SqlDataAccess.LoadData<manufacturingTimeModel>(sql, data);
        }




        public static List<manufacturingTimeModel> GetManufacturingTimeByPart(int partId)
        {
            manufacturingTimeModel data = new manufacturingTimeModel
            {
                partId = partId
            };

            string sql = @"select lineID, partId, ManufacturingTime from[ManufacturingTime] where  partId = @partId order by manufacturingTime asc;";



            return SqlDataAccess.LoadData<manufacturingTimeModel>(sql, data);
        }


        public static int CreateMT(int lineId, int partId, int manufacturingTime)
        {
            manufacturingTimeModel data = new manufacturingTimeModel
            {
                lineId = lineId,
                partId = partId,
                manufacturingTime = manufacturingTime
            };
            string sql = @"insert into [ManufacturingTime] (lineId,partId,manufacturingTime)
                            values (@lineId,@partId,@manufacturingTime);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int updateMT(int lineId, int partId, int manufacturingTime)
        {
            manufacturingTimeModel data = new manufacturingTimeModel
            {
                lineId = lineId,
                partId = partId,
                manufacturingTime = manufacturingTime
            };
            string sql = @"update [ManufacturingTime] set manufacturingTime = @manufacturingTime where lineId = @lineId and partId = @partId;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int deleteMT(int lineId, int partId)
        {
            manufacturingTimeModel data = new manufacturingTimeModel
            {
                lineId = lineId,
                partId = partId,
            };
            string sql = @"delete from [ManufacturingTime] where lineId = @lineId and partId = @partId;";

            return SqlDataAccess.SaveData(sql, data);
        }



        // get all the manufacturing time line and part combo
        public static List<manufacturingTimeModel> LoadManufacturingIds()
        {
            string sql = @"select lineId,partId from [ManufacturingTime];";

            return SqlDataAccess.LoadData<manufacturingTimeModel>(sql);
        }


        public static List<manufacturingTimeModel> LoadManufacturingList()
        {
            string sql = @"select lineId,partId,ManufacturingTime
                        from [ManufacturingTime];";

            return SqlDataAccess.LoadData<manufacturingTimeModel>(sql);
        }


        public static List<manufacturingTimeModel> LoadManufacturingPartLineList()
        {
            string sql = @"select m.lineId, m.partId, m.manufacturingTime, p.partName, l.lineName
                        from ManufacturingTime m join Line l on l.lineId= m.lineId join Part p on p.partId= m.partId;";

            return SqlDataAccess.LoadData<manufacturingTimeModel>(sql);
        }



    }
}
