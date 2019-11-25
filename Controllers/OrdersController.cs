using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scheduler.Models;
using DataLibrary;
using DataLibrary.BusinessLogic;
using System.IO;
using Scheduler.ViewModel;

namespace Scheduler.Controllers
{
    public class OrdersController : Controller
    {


        // GET: Orders
       // [Route("Orders/released/{year}/{month:regex(\\d{4}):range(1,12)}")]
        public ActionResult ByReleaseDate(int year,int month)
        {
            return Content(year + "/" + month);
        }

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
        public ActionResult newOrder(Order order,int selectedPart)
        {

            int created = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    
                     created = OrderProcessor.CreateOrder(
                 order.orderId,
                  selectedPart,
                  order.projectName,
                   order.lastMaterialDate,
                 order.shipDate,
                  order.quantity);




                    TempData["newOrderResult"] = created;
                    return RedirectToAction("ViewOrders");
                }
                catch (Exception ex)
                {
                    TempData["newOrderResult"] = created;
                    return RedirectToAction("ViewOrders");
                    //return View(ex.Message);
                }

            }

                ModelState.AddModelError("", "Error");
                return View();
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



        [HttpPost]
        public ActionResult importOrderCSV(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".csv")
                    {
                        ViewBag.Message = "Please select the csv file with .csv extension";
                        return View();
                    }


                    var orders = new List<Order>();
                    using (var sreader = new StreamReader(postedFile.InputStream))
                    {
                        //First line is header. If header is not passed in csv then we can neglect the below line.
                        string[] headers = sreader.ReadLine().Split(',');
                        //Loop through the records
                        while (!sreader.EndOfStream)
                        {
                            string[] rows = sreader.ReadLine().Split(',');

                            orders.Add(new Order
                            {
                                
                               orderId = int.Parse(rows[0].ToString()),
                               partId = int.Parse(rows[1].ToString()),
                               projectName = rows[2].ToString(),
                                lastMaterialDate = DateTime.Parse(rows[3].ToString()),
                                shipDate = DateTime.Parse(rows[4].ToString()),
                                quantity = int.Parse(rows[5].ToString())
                            });
                        }
                    }

                    return View("View", orders);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Please select the file first to upload.";
            }
            return View();
        }
    

        
        

    }
}