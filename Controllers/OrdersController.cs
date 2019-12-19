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
            OrderPartViewModel.selectedPart = order.partId;

            OrderPartViewModel.parts = parts;


            return View(OrderPartViewModel);
        }

        public ActionResult ViewOrders()
        {
            //ViewBag.Message = "Order List";

            var data = OrderProcessor.LoadOrder();

            List<Order> order = new List<Order>();

            foreach (var row in data)
            {
                order.Add(new Order
                {
                    orderId = row.orderId,
                    partId = row.partId,
                    projectName = row.projectName,
                    lastMaterialDate = row.lastMaterialDate,
                    shipDate = row.shipDate,
                    quantity = row.quantity
                });
            }

            return View(order);
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
                        result = ScheduleController.scheduleOrder(order);
                        if (result == 1)
                            TempData["newOrderResult"] = "New order is added and scheduled";
                        else if (result == -1)
                        {
                            //rollback and remove order
                            OrderProcessor.DeleteOrder(order.orderId);
                            TempData["newOrderResult"] = "Manufacturing Time Data does not exist for this part.Enter part / line data in Line page or change part.";
                        }
                        else if (result == 0)
                        {

                            TempData["newOrderResult"] = "unknown error has occurred during scheduling , please contact administrator.";
                        }


                    }



                    return result;
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

                return RedirectToAction("ViewOrders");

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



        public ActionResult importOrderCSV()
        {
            TempData["importOrderCSV"] = 0;

            List<Order> order = new List<Order>();
            return View(order);
        }


        [HttpPost]
        public ActionResult importOrderCSV(HttpPostedFileBase excelfile)
        {

            if (excelfile == null || excelfile.ContentLength == 0)
            {
                //ViewBag.Error = "Please select a excel file";
                TempData["ErrorCSV"] = "Please select a excel file";
                return RedirectToAction("ViewOrders");

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
                            order.quantity = 0;
                            order.projectName = " OrderID already exist in database. Check OrderId or edit existing order in View order page";
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
                            order.quantity = 0;
                            order.projectName = " ProductId does not exist in database. Check partId or add part data to Part Database";
                            ErrorListOrder.Add(order);
                            continue;
                        }


                        order.projectName = ((Excel.Range)range.Cells[row, 3]).Text;
                        order.lastMaterialDate = DateTime.Parse(((Excel.Range)range.Cells[row, 4]).Text);
                        order.shipDate = DateTime.Parse(((Excel.Range)range.Cells[row, 5]).Text);
                        order.quantity = int.Parse(((Excel.Range)range.Cells[row, 6]).Text);





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
                            //rollback and remove order
                            order.quantity = 0;
                            order.projectName = TempData["newOrderResult"].ToString();
                        }




                        ErrorListOrder.Add(order);

                        //order.projectName = "Success!";
                        //check if part exist, if part does not exist discard list

                        // listOrder.Add(order);
                    }


                    // ViewBag.ListProducts = listOrder;
                    //  DataLibrary.Models.orderModel orderModel = new orderModel();


                    application.Workbooks.Close();
                    //  workbook.Close(path);


                    TempData["importOrderCSV"] = 1;
                    return View(ErrorListOrder);
                }
                else
                {
                    // ViewBag.Error = "File type is incorrect";
                    TempData["ErrorCSV"] = "File type is incorrect";

                    return RedirectToAction("ViewOrders");

                }
            }


        }





    }
}