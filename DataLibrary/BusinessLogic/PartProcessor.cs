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
    public class PartProcessor
    {
        public static int CreatePart(string partName, int side)
        {
            partModel data = new partModel
            {
                partName = partName,
                side = side
            };
            string sql = @"insert into [part] (partName,side) values (@partName,@side);";

            return SqlDataAccess.SaveData(sql, data);
        }




        public static List<partModel> LoadPart()
        {

            string sql = @"select partId, partName,side
                        from [Part];";



            return SqlDataAccess.LoadData<partModel>(sql);

        }


        public static List<partModel> LoadPartId()
        {

            string sql = @"select partId
                        from [Part];";



            return SqlDataAccess.LoadData<partModel>(sql);

        }

        public static List<partModel> getPartIdByName(string partName, int side)
        {

            partModel data = new partModel
            {
                partName = partName,
                side = side
            };

            string sql = @"select partId
                        from [Part] where partName=@partName and side = @side;";



            return SqlDataAccess.LoadData<partModel>(sql, data);

        }

        public static List<partModel> getPartName(int partId)
        {

            partModel data = new partModel
            {
                partId = partId
            };

            string sql = @"select partName
                        from [Part] where partId=@partId;";



            return SqlDataAccess.LoadData<partModel>(sql, data);

        }


        public static int DeletePart(int partId)
        {
            partModel data = new partModel
            {
                partId = partId
            };
            string sql = @"delete from [Part] where partId = @partId;";

            return SqlDataAccess.SaveData(sql, data);

        }


        public static int updatePart(int partId, string partName, int side)
        {
            partModel data = new partModel
            {
                partId = partId,
                partName = partName,
                side = side
            };

            string sql = @"update [Part] set partName=@partName,side=@side where partId=@partId ;";

            return SqlDataAccess.SaveData(sql, data);
        }




    }
}
