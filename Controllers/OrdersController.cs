using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scheduler.Models;
using DataLibrary.Models;
using DataLibrary.BusinessLogic;
using System.IO;
using Scheduler.ViewModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace Scheduler.Controllers
{
    public class OrdersController : Controller
    {




        public ActionResult newOrder()
        {


            List<Part> parts = new List<Part>();
            var data = PartProcessor.LoadPart();

            foreach (var row in data)
            {
                parts.Add(new Part
                {
                    partId = row.partId,
                    partName = row.partName,
                    side = row.side

                });
            }

            OrderPartViewModel OrderPartViewModel = new OrderPartViewModel();

            OrderPartViewModel.parts = parts;


            return View(OrderPartViewModel);
        }

        public ActionResult editOrder(int id)
        {
            var orderData = OrderProcessor.LoadOrder(id);

            Order order = new Order();

            foreach (var row in orderData)
            {

                order.orderId = row.orderId;
                order.partId = row.partId;
                order.projectName = row.projectName;
                order.lastMaterialDate = row.lastMaterialDate;
                order.shipDate = row.shipDate;
                order.quantity = row.quantity;
                order.status = row.status;
                order.priority = row.priority;
            }

            List<Part> parts = new List<Part>();
            var data = PartProcessor.LoadPart();

            foreach (var row in data)
            {
                parts.Add(new Part
                {
                    partId = row.partId,
                    partName = row.partName,
                    side = row.side

                });
            }



            OrderPartViewModel OrderPartViewModel = new OrderPartViewModel();

            OrderPartViewModel.order = order;
            //   OrderPartViewModel.selectedPart = order.partId;
            //   OrderPartViewModel.selectedStatus = order.status;
            //  OrderPartViewModel.selectedPriority = order.priority;

            OrderPartViewModel.parts = parts;




            return View(OrderPartViewModel);
        }


        public ActionResult editOrderCSV(int id)
        {
            var orderData = OrderProcessor.LoadOrder(id);

            Order order = new Order();

            foreach (var row in orderData)
            {

                order.orderId = row.orderId;
                order.partId = row.partId;
                order.projectName = row.projectName;
                order.lastMaterialDate = row.lastMaterialDate;
                order.shipDate = row.shipDate;
                order.quantity = row.quantity;
                order.status = row.status;
                order.priority = row.priority;
            }

            List<Part> parts = new List<Part>();
            var data = PartProcessor.LoadPart();

            foreach (var row in data)
            {
                parts.Add(new Part
                {
                    partId = row.partId,
                    partName = row.partName,
                    side = row.side

                });
            }



            OrderPartViewModel OrderPartViewModel = new OrderPartViewModel();

            OrderPartViewModel.order = order;
            //   OrderPartViewModel.selectedPart = order.partId;
            //   OrderPartViewModel.selectedStatus = order.status;
            //  OrderPartViewModel.selectedPriority = order.priority;

            OrderPartViewModel.parts = parts;




            return View(OrderPartViewModel);
        }



        [HttpPost, ActionName("editOrder")]
        [ValidateAntiForgeryToken]
        public ActionResult editOrder(Order order)
        {


            //  order.partId = selectedPart;
            //  order.status = selectedStatus;
            //   order.priority = selectedPriority;

            int updated = 0;






            if (ModelState.IsValid)
            {

                try
                {

                    updated = OrderProcessor.updateOrder(order.orderId, order.partId, order.projectName, order.lastMaterialDate, order.shipDate, order.quantity, order.status, order.priority);



                    //TempData["newOrderResult"] = created;


                    if (updated == 1)
                    {
                        TempData["newOrderResult"] = 3;
                    }
                    else if (updated == 0)
                    {

                        TempData["newOrderResult"] = 0;
                    }





                    return RedirectToAction("ViewOrders");
                }
                catch (Exception ex)
                {
                    //  TempData["newOrderResult"] = created;
                    //eturn View(ex.Message);
                    TempData["newOrderResult"] = 0;

                    return RedirectToAction("ViewOrders");
                }

            }








            OrderPartViewModel OPViewModel = new OrderPartViewModel();

            List<Part> parts = new List<Part>();
            var data = PartProcessor.LoadPart();

            foreach (var row in data)
            {
                parts.Add(new Part
                {
                    partId = row.partId,
                    partName = row.partName,
                    side = row.side

                });
            }

            OPViewModel.order = order;

            OPViewModel.parts = parts;

            //  OPViewModel.selectedPart = selectedPart;
            // OPViewModel.selectedStatus = selectedStatus;
            // OPViewModel.selectedPriority = selectedPriority;

            return View(OPViewModel);
        }

        [HttpPost, ActionName("editOrderCSV")]
        [ValidateAntiForgeryToken]
        public ActionResult editOrderCSV(Order order)
        {


            //  order.partId = selectedPart;
            //  order.status = selectedStatus;
            //   order.priority = selectedPriority;

            int updated = 0;






            if (ModelState.IsValid)
            {

                try
                {

                    updated = OrderProcessor.updateOrder(order.orderId, order.partId, order.projectName, order.lastMaterialDate, order.shipDate, order.quantity, order.status, order.priority);



                    //TempData["newOrderResult"] = created;


                    if (updated == 1)
                    {
                        TempData["newOrderResult"] = 3;
                    }
                    else if (updated == 0)
                    {

                        TempData["newOrderResult"] = 0;
                    }





                    return RedirectToAction("reviewOrderCSVPost");
                }
                catch (Exception ex)
                {
                    //  TempData["newOrderResult"] = created;
                    //eturn View(ex.Message);
                    TempData["newOrderResult"] = 0;

                    return RedirectToAction("reviewOrderCSVPost");
                }

            }








            OrderPartViewModel OPViewModel = new OrderPartViewModel();

            List<Part> parts = new List<Part>();
            var data = PartProcessor.LoadPart();

            foreach (var row in data)
            {
                parts.Add(new Part
                {
                    partId = row.partId,
                    partName = row.partName,
                    side = row.side

                });
            }

            OPViewModel.order = order;

            OPViewModel.parts = parts;

            //  OPViewModel.selectedPart = selectedPart;
            // OPViewModel.selectedStatus = selectedStatus;
            // OPViewModel.selectedPriority = selectedPriority;

            return View(OPViewModel);
        }




        public ActionResult ViewOrders(string status)
        {

            var data = (dynamic)null;

            //ViewBag.Message = "Order List";
            if (status == null || status == "")
                data = OrderProcessor.LoadOrder();
            else
                data = OrderProcessor.LoadOrder(status);

            List<Order> orders = new List<Order>();

            foreach (var row in data)
            {
                orders.Add(new Order
                {
                    orderId = row.orderId,
                    partId = row.partId,
                    projectName = row.projectName,
                    lastMaterialDate = row.lastMaterialDate,
                    shipDate = row.shipDate,
                    quantity = row.quantity,
                    status = row.status,
                    priority = row.priority
                });
            }

            foreach (var row in data)
            {


            }






            return View(orders);
        }



        public ActionResult ViewOrdersFilter(string status)
        {
            //ViewBag.Message = "Order List";

            var data = OrderProcessor.LoadOrder(status);

            List<Order> orders = new List<Order>();

            foreach (var row in data)
            {
                orders.Add(new Order
                {
                    orderId = row.orderId,
                    partId = row.partId,
                    projectName = row.projectName,
                    lastMaterialDate = row.lastMaterialDate,
                    shipDate = row.shipDate,
                    quantity = row.quantity,
                    status = row.status,
                    priority = row.priority

                });
            }

            return View(orders);
        }


        public static List<Order> getListOrders()
        {

            var data = OrderProcessor.LoadOrder();

            List<Order> orders = new List<Order>();

            foreach (var row in data)
            {
                orders.Add(new Order
                {
                    orderId = row.orderId,
                    partId = row.partId,
                    projectName = row.projectName,
                    lastMaterialDate = row.lastMaterialDate,
                    shipDate = row.shipDate,
                    quantity = row.quantity
                });
            }

            return orders;
        }


        public static Order getOrderById(int orderId)
        {

            var data = OrderProcessor.LoadOrder(orderId);

            Order orders = new Order();

            foreach (var row in data)
            {

                orders.orderId = row.orderId;
                orders.partId = row.partId;
                orders.projectName = row.projectName;
                orders.lastMaterialDate = row.lastMaterialDate;
                orders.shipDate = row.shipDate;
                orders.quantity = row.quantity;
                orders.priority = row.priority;
                orders.status = row.status;
            };


            return orders;
        }

        public static string getOrderByIdReturnName(int orderId)
        {

            var data = OrderProcessor.LoadOrder(orderId);

            Order orders = new Order();

            foreach (var row in data)
            {


                orders.projectName = row.projectName;

            };


            return orders.projectName;
        }


        // POST: Orders
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult newOrder(Order order, int selectedPart)
        {

            int created = 0;




            order.partId = selectedPart;
            if (ModelState.IsValid)
            {

                try
                {


                    created = OrderProcessor.CreateOrder(
                   order.orderId,
                    order.partId,
                    order.projectName,
                     order.lastMaterialDate,
                   order.shipDate,
                    order.quantity);




                    //TempData["newOrderResult"] = created;


                    if (created == 1)
                    {
                        // Schedule object
                        int result = ScheduleController.scheduleOrder(order);
                        if (result == 1)
                            TempData["newOrderResult"] = 1;
                        else if (result == -1)
                        {
                            //rollback and remove order
                            OrderProcessor.DeleteOrder(order.orderId);
                            TempData["newOrderResult"] = -1;
                        }
                        else if (result == 0)
                        {

                            TempData["newOrderResult"] = 0;
                        }


                    }



                    return RedirectToAction("ViewOrders");
                }
                catch (Exception ex)
                {
                    //  TempData["newOrderResult"] = created;
                    //return RedirectToAction("ViewOrders");
                    return View(ex.Message);
                }

            }

            ModelState.AddModelError("", "Error");
            List<Part> parts = new List<Part>();
            var data = PartProcessor.LoadPart();

            foreach (var row in data)
            {
                parts.Add(new Part
                {
                    partId = row.partId,
                    partName = row.partName,
                    side = row.side

                });
            }



            OrderPartViewModel OrderPartViewModel = new OrderPartViewModel();

            OrderPartViewModel.parts = parts;


            return View(OrderPartViewModel);
        }


        public int newOrderCSV(Order order)
        {

            int created = 0;
            int result = 0;
            if (ModelState.IsValid)
            {

                try
                {

                    created = OrderProcessor.CreateOrder(
                   order.orderId,
                    order.partId,
                    order.projectName,
                     order.lastMaterialDate,
                   order.shipDate,
                    order.quantity);




                    //TempData["newOrderResult"] = created;


                    if (created == 1)
                    {
                        // Schedule object
                        // result = ScheduleController.scheduleOrder(order);
                        if (created == 1)

                            TempData["newOrderResult"] = "New order is added";

                        else if (created == -1)
                        {
                            //rollback and remove order
                            OrderProcessor.DeleteOrder(order.orderId);
                            TempData["newOrderResult"] = "Manufacturing Time Data does not exist for this part.Enter part / line data in Line page or change part.";
                        }
                        else if (created == 0)
                        {

                            TempData["newOrderResult"] = "unknown error has occurred during scheduling , please contact administrator.";
                        }


                    }



                    //return result;
                    return created;
                }
                catch (Exception ex)
                {
                    //  TempData["newOrderResult"] = created;
                    //return RedirectToAction("ViewOrders");
                    return 0;
                }

            }



            TempData["newOrderResult"] = ModelState.Values;



            return -1;
        }





        public ActionResult deleteOrder(int id)
        {
            if (ModelState.IsValid)
            {

                int deleted = OrderProcessor.DeleteOrder(
                    id
                    );


                if (deleted == 1)
                    TempData["newOrderResult"] = 2;
                else
                    TempData["newOrderResult"] = 0;
                return RedirectToAction("ViewOrders");

            }

            return View();
        }


        public ActionResult deleteOrderCSV(int id)
        {
            if (ModelState.IsValid)
            {

                int deleted = OrderProcessor.DeleteOrder(
                    id
                    );
                if (deleted == 1)
                    TempData["newOrderResult"] = 2;
                else
                    TempData["newOrderResult"] = 0;

                return RedirectToAction("reviewOrderCSVPost");

            }

            return View();
        }


        [HttpPost]
        public JsonResult CheckOrderId(int orderId)
        {

            List<Order> order = new List<Order>();
            // gets the list of parts
            var data = OrderProcessor.LoadOrderIds();

            foreach (var row in data)
            {
                order.Add(new Order
                {
                    orderId = row.orderId,

                });
            }


            // Checks thru the list of parts to see if parts exist in database
            bool isValid = !order.ToList().Exists(p => p.orderId.Equals(orderId));



            return Json(isValid);
        }

        public bool checkOrderExist(int orderId)
        {
            int result = 0;

            List<Order> existingOrderList = new List<Order>();
            // gets the list of parts
            var data = OrderProcessor.LoadOrderIds();

            foreach (var row in data)
            {
                existingOrderList.Add(new Order
                {
                    orderId = row.orderId,

                });
            }



            bool continueCond = false;
            for (int i = 0; i < existingOrderList.Count; i++)
            {
                if (orderId.Equals(existingOrderList[i].orderId))
                {


                    continueCond = true;
                    break;
                }
            }

            return continueCond;
        }

        public ActionResult importOrderCSV()
        {
            TempData["importOrderCSV"] = 0;

            List<Order> order = new List<Order>();
            return View();
        }


        [HttpPost]
        public ActionResult importOrderCSV(HttpPostedFileBase excelfile)
        {
            //  try
            //  {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                //ViewBag.Error = "Please select a excel file";
                TempData["ErrorCSV"] = "Please select a excel file";
                return RedirectToAction("importOrderCSV");

            }
            else
            {

                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);

                    //Read data from excel
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;


                    //import current list of orders
                    List<Order> existingOrderList = new List<Order>();
                    // gets the list of parts
                    var data = OrderProcessor.LoadOrderIds();

                    foreach (var row in data)
                    {
                        existingOrderList.Add(new Order
                        {
                            orderId = row.orderId,

                        });
                    }

                    //import current list of parts
                    List<Part> existingPartList = new List<Part>();

                    var partData = PartProcessor.LoadPartId();

                    foreach (var row in partData)
                    {
                        existingPartList.Add(new Part
                        {
                            partId = row.partId,

                        });
                    }

                    //create List of of orders that have orderID exist and partId does not exist. Return list of objects to user 
                    List<Order> ErrorListOrder = new List<Order>();


                    List<Order> listOrder = new List<Order>();
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        Order order = new Order();
                        order.orderId = int.Parse(((Excel.Range)range.Cells[row, 1]).Text);
                        order.partId = int.Parse(((Excel.Range)range.Cells[row, 2]).Text);
                        order.projectName = ((Excel.Range)range.Cells[row, 3]).Text;
                        order.lastMaterialDate = DateTime.Parse(((Excel.Range)range.Cells[row, 4]).Text);



                        order.shipDate = DateTime.Parse(((Excel.Range)range.Cells[row, 5]).Text);
                        order.quantity = int.Parse(((Excel.Range)range.Cells[row, 6]).Text);

                        //default values
                        order.status = "unschedued";
                        order.priority = 3;

                        // check if orderID already exist. if exist skip
                        bool continueCond = false;
                        for (int i = 0; i < existingOrderList.Count; i++)
                        {
                            if (order.orderId.Equals(existingOrderList[i].orderId))
                            {


                                continueCond = true;
                                break;
                            }
                        }
                        // OrderID already exist , dont add 
                        if (continueCond == true)
                        {

                            order.intTempResult = 0;
                            order.StringTempResult = " OrderID already exist in database. Check OrderId or edit existing order in View order page";
                            ErrorListOrder.Add(order);
                            continue;
                        }


                        //check if part exist, if part does not exist discard list
                        bool continueCond2 = false;
                        for (int i = 0; i < existingPartList.Count; i++)
                        {
                            if (order.partId.Equals(existingPartList[i].partId))
                            {
                                continueCond2 = true;
                                break;
                            }
                        }
                        // Part is not found , dont add current order
                        if (continueCond2 == false)
                        {
                            order.intTempResult = 0;
                            order.StringTempResult = " ProductId does not exist in database. Check partId or add part data to Part Database";
                            ErrorListOrder.Add(order);
                            continue;
                        }






                        //insert into DB
                        /*
                        int created = OrderProcessor.CreateOrder(
                        order.orderId,
                        order.partId,
                        order.projectName,
                        order.lastMaterialDate,
                        order.shipDate,
                        order.quantity);
                        */


                        // returns 1 for successfully operation, 0 for unknown error, -1 for Error in manufacturingPart does not exist
                        int importResult = newOrderCSV(order);

                        if (importResult == -1 || importResult == 0)
                        {
                            order.intTempResult = 0;
                            order.StringTempResult = TempData["newOrderResult"].ToString();
                        }
                        else
                        {
                            order.intTempResult = 1;
                            order.StringTempResult = TempData["newOrderResult"].ToString();
                        }





                        ErrorListOrder.Add(order);

                        //order.projectName = "Success!";
                        //check if part exist, if part does not exist discard list

                        // listOrder.Add(order);
                    }


                    // ViewBag.ListProducts = listOrder;
                    //  DataLibrary.Models.orderModel orderModel = new orderModel();


                    workbook.Save();
                    workbook.Close(true);
                    application.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
                    //  workbook.Close(path);


                    TempData["importOrderCSV"] = 1;
                    //return View(ErrorListOrder);

                    return View("reviewOrderCSV", ErrorListOrder);
                }
                else
                {
                    // ViewBag.Error = "File type is incorrect";
                    TempData["ErrorCSV"] = "File type is incorrect";

                    return RedirectToAction("ViewOrders");

                }
            }
            // }
            //   catch (Exception ex)
            //  {
            //     TempData["ErrorCSV"] = ex;

            //      return RedirectToAction("importOrderCSV");
            //   }


        }


        public ActionResult reviewOrderCSV(List<Order> orders)
        {
            TempData["importOrderCSV"] = 0;

            List<Order> order = new List<Order>();
            return View(orders);
        }

        [HttpPost, ActionName("reviewOrderCSVPost")]
        public ActionResult reviewOrderCSVPost(List<int> orderId)
        {

            if (orderId == null || orderId.Count == 0)
            {
                List<Order> orders = new List<Order>();
                var data = OrderProcessor.LoadOrder("unscheduled");

                foreach (var row in data)
                {
                    orders.Add(new Order
                    {
                        orderId = row.orderId,
                        partId = row.partId,
                        projectName = row.projectName,
                        lastMaterialDate = row.lastMaterialDate,
                        shipDate = row.shipDate,
                        quantity = row.quantity,
                        status = row.status,
                        priority = row.priority
                    });
                }

                return View(orders);
            }

            // List<int> ordersList = orderId;
            // display vieworder page with status (unscheduled)
            // return RedirectToAction("ViewOrders", new { status = "unscheduled" });


            // var data = OrderProcessor.LoadOrder("unscheduled");
            else
            {
                List<Order> orders = new List<Order>();

                for (int i = 0; i < orderId.Count; i++)
                {
                    var data = OrderProcessor.LoadOrder(orderId[i]);

                    foreach (var row in data)
                    {
                        orders.Add(new Order
                        {
                            orderId = row.orderId,
                            partId = row.partId,
                            projectName = row.projectName,
                            lastMaterialDate = row.lastMaterialDate,
                            shipDate = row.shipDate,
                            quantity = row.quantity,
                            status = row.status,
                            priority = row.priority
                        });
                    }
                }
                //   OrderDetail ObjorderDetail = new OrderDetail();
                //  ObjorderDetail.OrderDetails = orders;

                return View(orders);
            }
        }


        [HttpPost, ActionName("reviewOrderCSVSchedule")]
        public ActionResult reviewOrderCSVSchedule(int[] orderId, string[] status)
        {

            List<Order> listOrders = new List<Order>();
            List<Order> tempListOrders = new List<Order>();

            for (int i = 0; i < status.Length; i++)
            {
                //check status , if status = unschedule , dont schedule 
                Order order = new Order();
                order = getOrderById(orderId[i]);
                if (status[i].Equals("unscheduled"))
                {
                    // dont schedule here

                    order.intTempResult = -1;
                    order.StringTempResult = "Unscheduled";

                    listOrders.Add(order);

                }
                else
                {
                    tempListOrders.Add(order);

                }
            }

            //sort order by shipping date
            //tempListOrders = tempListOrders.OrderByDescending(o => o.shipDate).ToList();

            tempListOrders.Sort((x, y) => x.shipDate.CompareTo(y.shipDate));

            for (int i = 0; i < tempListOrders.Count; i++)
            {
                //check status , if status = unschedule , dont schedule 
                Order order = tempListOrders[i];



                int scheduled = ScheduleController.scheduleOrder(order);
                //int scheduled = 0;
                if (scheduled == 1)
                {
                    order.intTempResult = 1;
                    order.StringTempResult = "Scheduled";
                }
                else
                {
                    order.intTempResult = 0;
                    order.StringTempResult = "An exception has occured, scheduling is aborted.Please try again.";
                }


                listOrders.Add(order);

            }






            TempData["Orders"] = listOrders;

            return RedirectToAction("scheduledResults");
        }



        public ActionResult scheduledResults(List<Order> orders)
        {

            if (orders == null || orders.Count == 0)
                orders = (List<Order>)TempData["Orders"];


            return View(orders);
        }


        public JsonResult InsertCustomers(List<Order> orders)
        {


            JsonResult result = null;
            return result;
        }


        public static void setOrderscheduled(int orderId)
        {

            var data = OrderProcessor.setOrderSchedule(orderId, "scheduled");



        }





    }
}