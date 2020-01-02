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
    public class LineController : Controller
    {
        // GET: Line
        public static string getLineName(int lineId)
        {
            string lineName = null;

            var lineData = LineProcessor.getLineName(lineId);
            foreach (var row in lineData)
            {
                lineName = row.lineName;

            }


            return lineName;

        }
    }
}