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
    public class PartController : Controller
    {

        public ActionResult ViewParts()
        {
            ViewBag.Message = "Part List";

            var data = PartProcessor.LoadPart();

            List<Part> parts = new List<Part>();

            foreach (var row in data)
            {
                parts.Add(new Part
                {
                    partId = row.partId,
                    partName = row.partName,
                    side = row.side
                });
            }

            return View(parts);
        }


        public ActionResult newPart()
        {
            return View();
        }

        // GET: Part
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult newPart(Part part)
        {
            if (ModelState.IsValid)
            {

                int created = PartProcessor.CreatePart(
                    part.partName,
                    part.side);

                return RedirectToAction("ViewParts");
            }

            return View();
        }


        public ActionResult deletePart(int id)
        {
            if (ModelState.IsValid)
            {

                int deleted = PartProcessor.DeletePart(
                    id
                    );

                return RedirectToAction("ViewParts");

            }

            return View();
        }


    }
}