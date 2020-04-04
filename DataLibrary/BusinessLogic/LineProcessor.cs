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
    public class LineProcessor
    {


        public static List<lineModel> LoadLine()
        {

            string sql = @"select lineId, lineName from [Line];";



            return SqlDataAccess.LoadData<lineModel>(sql);

        }




        public static List<lineModel> getLineName(int lineId)
        {

            lineModel data = new lineModel
            {
                lineId = lineId
            };

            string sql = @"select lineName from [Line] where lineId=@lineId ;";



            return SqlDataAccess.LoadData<lineModel>(sql, data);


        }





    }
}
