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
    public class BackendProcessor
    {

        public static List<backendModel> getBackendProcessByID(int partId)
        {
            backendModel data = new backendModel
            {
                partId = partId
            };

            string sql = @"select backendId,partId, processName,duration
                        from [Backend] where partId = @partId;";



            return SqlDataAccess.LoadData<backendModel>(sql, data);
        }


        public static List<backendModel> getBackendId(int partId)
        {
            backendModel data = new backendModel
            {
                partId = partId
            };

            string sql = @"select backendId
                        from [Backend] where partId = @partId;";



            return SqlDataAccess.LoadData<backendModel>(sql, data);
        }


        public static int CreateBackendProcess(int partId)
        {
            backendModel data = new backendModel
            {
                partId = partId,
                processName = null,
                duration = 0
            };
            string sql = @"insert into [Backend] (partId, processName,duration)
                            values (@partId,@processName,@duration);";

            return SqlDataAccess.SaveData(sql, data);
        }

    }
}
