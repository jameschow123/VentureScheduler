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
using System.Web.Helpers;

namespace Scheduler.Controllers
{
    public class ManufacturingController : Controller
    {
        // GET: Manufacturing
        public ActionResult Index()
        {
            return View();
        }

        public List<Line> getLines()
        {
            List<Line> lines = new List<Line>();
            var lineData = LineProcessor.LoadLine();

            foreach (var row in lineData)
            {
                lines.Add(new Line
                {
                    lineId = row.lineId,
                    lineName = row.lineName,

                });
            }

            return lines;
        }


        public List<Part> getParts()
        {
            List<Part> parts = new List<Part>();
            var partData = PartProcessor.LoadPart();

            foreach (var row in partData)
            {
                parts.Add(new Part
                {
                    partId = row.partId,
                    partName = row.partName,

                });
            }

            return parts;
        }

        public ActionResult newManufacturingTime()
        {
            // get Line list
            List<Line> lines = getLines();
            List<Part> parts = getParts();


            /*
            var lineData = LineProcessor.LoadLine();

            foreach (var row in lineData)
            {
                lines.Add(new Line
                {
                    lineId = row.lineId,
                    lineName = row.lineName,

                });
            }
            */
            // get part list

            /*
            var partData = PartProcessor.LoadPart();

            foreach (var row in partData)
            {
                parts.Add(new Part
                {
                    partId = row.partId,
                    partName = row.partName,

                });
            }
            */
            PartLineManufacturingViewModel PLViewModel = new PartLineManufacturingViewModel();

            PLViewModel.lines = lines;
            PLViewModel.parts = parts;


            return View(PLViewModel);
        }



        // POST: Orders
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult newManufacturingTime(int selectedLine, int selectedPart, int manufacturingTIme)
        {


            int created = 0;

            if (manufacturingTIme <= 0 || manufacturingTIme > 10000)
            {
                ModelState.AddModelError("", "Error");

                ModelState.AddModelError(string.Empty, "Please enter a valid Manufacturing Time from 1-9999");
            }


            bool result = CheckManufacturingTimeExistBool(selectedLine, selectedPart);

            if (result == false)
            {
                ModelState.AddModelError("", "Error");

                ModelState.AddModelError(string.Empty, "Manufacturing Time already exist");
            }

            if (ModelState.IsValid)
            {

                try
                {

                    created = manufacturingTimeProcessor.CreateMT(
                  selectedLine,
                  selectedPart,
                  manufacturingTIme);




                    //TempData["newOrderResult"] = created;


                    if (created == 1)
                    {
                        TempData["ManufacturingResult"] = 1;
                    }
                    else if (created == 0)
                    {

                        TempData["ManufacturingResult"] = 0;
                    }







                    return RedirectToAction("ViewManufacturing");
                }
                catch (Exception ex)
                {
                    //  TempData["newOrderResult"] = created;
                    //return RedirectToAction("ViewOrders");
                    return View(ex.Message);
                }

            }

            // ModelState.AddModelError("", "Error");

            // get Line list
            List<Line> lines = getLines();


            // get part list
            List<Part> parts = getParts();


            PartLineManufacturingViewModel PLViewModel = new PartLineManufacturingViewModel();

            PLViewModel.lines = lines;
            PLViewModel.parts = parts;


            return View(PLViewModel);
        }

        public ActionResult updateManufacturingTime(int selectedLine, int selectedPart, int manufacturingTIme)
        {

            List<Line> lines = getLines();


            // get part list
            List<Part> parts = getParts();


            PartLineManufacturingViewModel PLViewModel = new PartLineManufacturingViewModel();

            PLViewModel.lines = lines;
            PLViewModel.parts = parts;

            PLViewModel.selectedLine = selectedLine;
            PLViewModel.selectedPart = selectedPart;
            PLViewModel.manufacturingTIme = manufacturingTIme;

            return View(PLViewModel);

        }


        [HttpPost, ActionName("updateManufacturingTime")]
        [ValidateAntiForgeryToken]
        public ActionResult updateManufacturingTimePost(int selectedLine, int selectedPart, int manufacturingTIme)
        {

            int updated = 0;

            if (manufacturingTIme <= 0 || manufacturingTIme > 10000)
            {
                ModelState.AddModelError("", "Error");

                ModelState.AddModelError(string.Empty, "Please enter a valid Manufacturing Time from 1-9999");
            }




            if (ModelState.IsValid)
            {

                try
                {

                    updated = manufacturingTimeProcessor.updateMT(
                   selectedLine,
                   selectedPart,
                   manufacturingTIme);




                    //TempData["newOrderResult"] = created;


                    if (updated == 1)
                    {
                        TempData["ManufacturingResult"] = 3;
                    }
                    else if (updated == 0)
                    {

                        TempData["ManufacturingResult"] = 0;
                    }







                    return RedirectToAction("ViewManufacturing");
                }
                catch (Exception ex)
                {
                    //  TempData["newOrderResult"] = created;
                    //return RedirectToAction("ViewOrders");
                    return View(ex.Message);
                }

            }








            PartLineManufacturingViewModel PLViewModel = new PartLineManufacturingViewModel();


            List<Line> lines = getLines();
            List<Part> parts = getParts();

            PLViewModel.lines = lines;
            PLViewModel.parts = parts;

            PLViewModel.selectedLine = selectedLine;
            PLViewModel.selectedPart = selectedPart;
            PLViewModel.manufacturingTIme = manufacturingTIme;

            return View(PLViewModel);
        }




        [HttpPost]
        public JsonResult CheckManufacturingTimeExist(int lineId, int partId)
        {

            List<ManufacturingTime> MT = new List<ManufacturingTime>();
            // gets the list of parts
            var data = manufacturingTimeProcessor.LoadManufacturingIds();

            foreach (var row in data)
            {
                MT.Add(new ManufacturingTime
                {
                    lineId = row.lineId,
                    PartId = row.partId

                });
            }


            // Checks thru the list of manufacturing time to see if  manufacturing time exist in database
            bool isValid = !MT.ToList().Exists(p => p.lineId.Equals(lineId) && p.PartId.Equals(partId));



            return Json(isValid);
        }

        public bool CheckManufacturingTimeExistBool(int lineId, int partId)
        {

            List<ManufacturingTime> MT = new List<ManufacturingTime>();
            // gets the list of parts
            var data = manufacturingTimeProcessor.LoadManufacturingIds();

            foreach (var row in data)
            {
                MT.Add(new ManufacturingTime
                {
                    lineId = row.lineId,
                    PartId = row.partId

                });
            }


            // Checks thru the list of manufacturing time to see if  manufacturing time exist in database
            bool isValid = !MT.ToList().Exists(p => p.lineId.Equals(lineId) && p.PartId.Equals(partId));



            return isValid;
        }


        public ActionResult ViewManufacturing()
        {
            /*
                        var data = manufacturingTimeProcessor.LoadManufacturingList();

                        List<ManufacturingTime> MT = new List<ManufacturingTime>();

                        foreach (var row in data)
                        {
                            MT.Add(new ManufacturingTime
                            {
                                lineId = row.lineId,
                                PartId = row.partId,
                                manufacturingTIme = row.manufacturingTime
                            });
                        }
                        */




            // get part list
            List<PartLineManufacturingViewModel> PLViewModel = new List<PartLineManufacturingViewModel>();
            var data = manufacturingTimeProcessor.LoadManufacturingPartLineList();

            foreach (var row in data)
            {
                PLViewModel.Add(new PartLineManufacturingViewModel
                {
                    selectedLine = row.lineId,
                    selectedPart = row.partId,
                    manufacturingTIme = row.manufacturingTime,
                    lineName = row.lineName,
                    partName = row.partName

                });
            }

            //   PartLineManufacturingViewModel PLViewModel = new PartLineManufacturingViewModel();

            // PLViewModel.lines = lines;
            //PLViewModel.parts = parts;


            return View(PLViewModel);


            // return View(MT);
        }


        public ActionResult deleteManufacturingTime(int lineId, int partId)
        {
            int deleted = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    deleted = manufacturingTimeProcessor.deleteMT(
                    lineId, partId
                    );


                    if (deleted == 1)
                    {
                        TempData["ManufacturingResult"] = 2;
                        return RedirectToAction("ViewManufacturing");
                    }
                    else
                        TempData["ManufacturingResult"] = 0;




                    return RedirectToAction("ViewOrders");


                }
                catch (Exception ex)
                {
                    TempData["ManufacturingResult"] = 0;
                    return RedirectToAction("ViewManufacturing");


                }
            }


            return View();

        }


    }
}