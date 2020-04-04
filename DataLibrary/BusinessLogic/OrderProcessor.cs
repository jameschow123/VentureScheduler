using DataLibrary.DataAccess;
using DataLibrary.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class OrderProcessor
    {
        public static int CreateOrder(int orderId, int partId, string projectName, DateTime lastMaterialDate,
            DateTime shipDate, int quantity)
        {
            orderModel data = new orderModel
            {
                orderId = orderId,
                partId = partId,
                projectName = projectName,
                lastMaterialDate = lastMaterialDate,
                shipDate = shipDate,
                quantity = quantity
            };
            string sql = @"insert into [Order] (orderId,partId,projectName,lastMaterialDate,shipDate,quantity)
                            values (@orderId,@partId,@projectName,CONVERT(datetime,@lastMaterialDate,104),CONVERT(datetime,@shipDate,104),@quantity);";

            return SqlDataAccess.SaveData(sql, data);
        }


        /*  public static int CreateOrders (List<orderModel> orderList)
          {
            for (int i = 0; i <orderList.Count;i++)
                  orderModel data = new orderModel
                  {
                      orderId = orderList[i].orderId,
                      partId = partId,
                      projectName = projectName,
                      lastMaterialDate = lastMaterialDate,
                      shipDate = shipDate,
                      quantity = quantity
                  };
                  string sql = @"insert into [Order] (orderId,partId,projectName,lastMaterialDate,shipDate,quantity)
                              values (@orderId,@partId,@projectName,@lastMaterialDate,@shipDate,@quantity);";
                  SqlDataAccess.SaveData(sql, data);
              }
              return 1;
          }

      */
        public static List<orderModel> LoadOrder()
        {
            string sql = @"select orderId,partId,projectName,lastMaterialDate,shipDate,quantity,status,priority 
                        from [Order];";

            return SqlDataAccess.LoadData<orderModel>(sql);
        }



        public static List<orderModel> LoadOrder(string status)
        {

            orderModel data = new orderModel
            {
                status = status,

            };
            string sql = @"select orderId,partId,projectName,lastMaterialDate,shipDate,quantity,status,priority 
                        from [Order] where status = @status;";

            return SqlDataAccess.LoadData<orderModel>(sql, data);
        }

        public static List<orderModel> LoadOrder(int id)
        {
            orderModel data = new orderModel
            {
                orderId = id,

            };

            string sql = @"select orderId,partId,projectName,lastMaterialDate,shipDate,quantity ,status,priority 
                        from [Order] where orderId = @orderId;";

            return SqlDataAccess.LoadData<orderModel>(sql, data);
        }

        public static List<orderModel> LoadOrderIds()
        {
            string sql = @"select orderId
                        from [Order];";

            return SqlDataAccess.LoadData<orderModel>(sql);
        }

        public static List<orderModel> LoadOrderStatus()
        {
            string sql = @"select orderId,status
                        from [Order];";

            return SqlDataAccess.LoadData<orderModel>(sql);
        }


        public static int updateOrder(int orderId, int partId, string projectName, DateTime lastMaterialDate, DateTime shipDate, int quantity, string status, int priority)
        {
            orderModel data = new orderModel
            {
                orderId = orderId,
                partId = partId,
                projectName = projectName,
                lastMaterialDate = lastMaterialDate,
                shipDate = shipDate,
                quantity = quantity,
                status = status,
                priority = priority
            };
            string sql = @"update [Order] set partId=@partId,projectName=@projectName,lastMaterialDate=@lastMaterialDate,shipDate=@shipDate,quantity=@quantity, status=@status,priority=@priority where orderId=@orderId ;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteOrder(int orderId)
        {
            orderModel data = new orderModel
            {
                orderId = orderId
            };
            string sql = @"delete from [Order] where orderId = @orderId;";

            return SqlDataAccess.SaveData(sql, data);

        }


        public static int setOrderSchedule(int orderId, string status)
        {
            orderModel data = new orderModel
            {
                orderId = orderId,
                status = status

            };
            string sql = @"update [Order] set status=@status where orderId=@orderId;";

            return SqlDataAccess.SaveData(sql, data);

        }



    }
}
