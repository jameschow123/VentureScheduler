using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scheduler.Models;

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

    }
}