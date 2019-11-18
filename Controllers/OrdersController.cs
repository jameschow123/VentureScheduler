using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scheduler.Models;
using DataLibrary;
using DataLibrary.BusinessLogic;

namespace Scheduler.Controllers
{
    public class OrdersController : Controller
    {


        public ActionResult Random()
        {
            var order = new Order() { projectName = "test order" };


            

            return View(order);

        }


        // GET: Orders
       // [Route("Orders/released/{year}/{month:regex(\\d{4}):range(1,12)}")]
        public ActionResult ByReleaseDate(int year,int month)
        {
            return Content(year + "/" + month);
        }

        public ActionResult newOrder()
        {
            return View();
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
                    orderDate = row.orderDate,
                    shipDate = row.orderDate,
                    quantity = row.quantity
                });
            }

            return View(order);
        }

        // POST: Orders
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult newOrder(Order order)
        {

            int created = 0;
            if (ModelState.IsValid)
            {
                try
                {
                     created = OrderProcessor.CreateOrder(
                        order.orderId,
                        order.partId,
                        order.projectName,
                        order.orderDate,
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

    }
}