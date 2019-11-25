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

            int created = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    created = PartProcessor.CreatePart(
                       part.partName,
                       part.side);


                    
                   TempData["newPartResult"] = created;              
                    return RedirectToAction("ViewParts");
                }
                catch (Exception ex)
                {
                    TempData["newPartResult"] = "An error has occurred!";
                    return RedirectToAction("ViewParts");


                }
            }

            return View();
        }


        public ActionResult deletePart(int id)
        {

            int deleted = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    deleted = PartProcessor.DeletePart(
                    id
                    );

                    TempData["newPartResult"] = 2;
                    return RedirectToAction("ViewParts");

                }
                catch (Exception ex)
                {
                    TempData["partDeleted"] = deleted;
                    return RedirectToAction("ViewParts");


                }
            }

            return View();
        }

        // method checks if partName has been taken
        [HttpPost]
        public JsonResult CheckPartName(string partName,int side)
        {
            
            List <Part> parts = new List<Part>();
            // gets the list of parts
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

            
            // Checks thru the list of parts to see if parts exist in database
            bool isValid = !parts.ToList().Exists(p => p.partName.Equals(partName, StringComparison.CurrentCultureIgnoreCase));
            if (isValid == true)
            isValid = parts.ToList().Exists(p => p.side.Equals(side));


            return Json(isValid);
        }



    }
}