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
using System.Text.RegularExpressions;

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

            if (result != false)
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


        public int newManufacturingTimeCSV(int selectedLine, int selectedPart, int manufacturingTIme)
        {


            int created = 0;

            bool timeValid = manufacturingTimeValid(manufacturingTIme);



            if (timeValid == false)
            {
                return -1;
            }




            try
            {

                created = manufacturingTimeProcessor.CreateMT(
                selectedLine,
                selectedPart,
                manufacturingTIme);

                return created;
            }
            catch (Exception ex)
            {
                return created;
            }




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


        public static bool manufacturingTimeValid(int manufacturingTIme)
        {
            if (manufacturingTIme <= 0 || manufacturingTIme > 10000)
            {
                return false;
            }
            else
                return true;

        }


        public int updateManufacturingTimeCSV(int selectedLine, int selectedPart, int manufacturingTIme)
        {

            int updated = 0;

            bool timeValid = manufacturingTimeValid(manufacturingTIme);



            if (timeValid == false)
            {
                return -1;
            }
            /*

            if (manufacturingTIme <= 0 || manufacturingTIme > 10000)
            {
                ModelState.AddModelError("", "Error");

                ModelState.AddModelError(string.Empty, "Please enter a valid Manufacturing Time from 1-9999");
            }

            */

            try
            {

                updated = manufacturingTimeProcessor.updateMT(
               selectedLine,
               selectedPart,
               manufacturingTIme);




                //TempData["newOrderResult"] = created;

                /*
                    if (updated == 1)
                    {
                        TempData["ManufacturingResult"] = 3;
                    }
                    else if (updated == 0)
                    {

                        TempData["ManufacturingResult"] = 0;
                    }

                */





                return updated;
            }
            catch (Exception ex)
            {
                //  TempData["newOrderResult"] = created;
                //return RedirectToAction("ViewOrders");
                return updated;
            }

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
            bool exist = MT.ToList().Exists(p => p.lineId.Equals(lineId) && p.PartId.Equals(partId));



            return exist;
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



        [HttpPost]
        public JsonResult deleteManufacturingJson(int selectedLine, int selectedPart)
        {
            try
            {

                int deleted = manufacturingTimeProcessor.deleteMT(
                    selectedLine, selectedPart
                    );



                return Json("Delete");

            }
            catch
            {
                return Json("Error");
            }

        }


        public ActionResult importManufacturingCSV()
        {
            TempData["importManufacturingCSV"] = 0;

            List<PartLineManufacturingViewModel> MT = new List<PartLineManufacturingViewModel>();
            return View(MT);
        }


        [HttpPost]
        public ActionResult importManufacturingCSV(HttpPostedFileBase excelfile)
        {

            if (excelfile == null || excelfile.ContentLength == 0)
            {
                //ViewBag.Error = "Please select a excel file";
                TempData["ErrorCSV"] = "Please select a excel file";
                return RedirectToAction("importManufacturingCSV");

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




                    //import current list of ManufacturingTime
                    List<ManufacturingTime> existingMTList = new List<ManufacturingTime>();
                    var data = manufacturingTimeProcessor.LoadManufacturingList();
                    foreach (var row in data)
                    {
                        existingMTList.Add(new ManufacturingTime
                        {
                            lineId = row.lineId,
                            PartId = row.partId,
                            manufacturingTIme = row.manufacturingTime

                        });
                    }

                    //import current list of parts
                    List<Part> existingPartList = new List<Part>();
                    var partData = PartProcessor.LoadPart();
                    foreach (var row in partData)
                    {
                        existingPartList.Add(new Part
                        {
                            partId = row.partId,
                            partName = row.partName,
                            side = row.side

                        });
                    }


                    //import current list of lines
                    List<Line> existingLineList = new List<Line>();
                    var lineData = LineProcessor.LoadLine();
                    foreach (var row in lineData)
                    {
                        existingLineList.Add(new Line
                        {
                            lineId = row.lineId,
                            lineName = row.lineName,
                        });
                    }

                    //create List of of orders that have orderID exist and partId does not exist. Return list of objects to user 
                    List<PartLineManufacturingViewModel> ErrorListMT = new List<PartLineManufacturingViewModel>();


                    // check part exist , if exist skip, else add
                    List<ManufacturingTime> listMT = new List<ManufacturingTime>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        ManufacturingTime MT = new ManufacturingTime();
                        Part part = new Part();
                        Line line = new Line();

                        //check if part exist. if exist skip
                        part.partId = int.Parse(((Excel.Range)range.Cells[row, 1]).Text);
                        part.partName = ((Excel.Range)range.Cells[row, 2]).Text;
                        string side = ((Excel.Range)range.Cells[row, 3]).Text;
                        side = Regex.Replace(side, @"\s", "");
                        line.lineName = ((Excel.Range)range.Cells[row, 6]).Text;
                        line.lineName = Regex.Replace(line.lineName, @"\s", "");
                        MT.manufacturingTIme = int.Parse(((Excel.Range)range.Cells[row, 7]).Text);



                        if (String.Equals(side, "Top", StringComparison.OrdinalIgnoreCase))
                        {
                            part.side = 1;
                        }
                        else if (String.Equals(side, "Bottom", StringComparison.OrdinalIgnoreCase))
                        {
                            part.side = 2;
                        }
                        else
                        {
                            part.side = -1;
                            MT.error = " Error with product Information.Invalid boardside";
                            MT.lineId = 0;
                            ErrorListMT.Add(new PartLineManufacturingViewModel { partName = part.partName, lineName = line.lineName, manufacturingTIme = MT.manufacturingTIme, error = MT.error, selectedLine = MT.lineId });
                            continue;


                        }

                        int addPartResult = 0;

                        /*
                        // check if part exist 
                        bool partExist = PartController.CheckPartExist(part.partName, part.side);
                        if (partExist == false)
                        {
                            //add to part DB
                            addPartResult = PartController.addNewPart(part);
                        }
                        */




                        bool continueCond = false;

                        int partId = existingPartList.Where(r => r.partName == part.partName && r.side == part.side).Select(r => r.partId).SingleOrDefault();

                        if (partId != 0)
                        {
                            part.partId = partId;


                            continueCond = true;
                        }
                        else
                        {

                            addPartResult = PartController.addNewPart(part);


                            if (addPartResult == 1)
                            {
                                //set new partId
                                var partIDData = PartProcessor.getPartIdByName(part.partName, part.side);

                                foreach (var row1 in partIDData)
                                {

                                    part.partId = row1.partId;


                                }

                                existingPartList.Add(part);

                                continueCond = true;
                            }

                        }



                        // Error with part information
                        if (continueCond == false)
                        {
                            //order.quantity = 0;
                            MT.error = " Error with product Information. Check product or add new product";
                            MT.lineId = 0;
                            ErrorListMT.Add(new PartLineManufacturingViewModel { partName = part.partName, lineName = line.lineName, manufacturingTIme = MT.manufacturingTIme, error = MT.error, selectedLine = MT.lineId });
                            continue;
                        }


                        // check SMT line exist

                        //check if part exist, if part does not exist discard list
                        bool continueCond2 = false;


                        int lineId = existingLineList.Where(r => r.lineName == line.lineName).Select(x => x.lineId).SingleOrDefault();
                        if (lineId != 0)
                        {
                            line.lineId = lineId;
                            continueCond2 = true;

                        }

                        if (continueCond2 == false)
                        {
                            MT.error = " Error with line information, please check line Information or add new line ";
                            MT.lineId = 0;
                            ErrorListMT.Add(new PartLineManufacturingViewModel { partName = part.partName, lineName = line.lineName, manufacturingTIme = MT.manufacturingTIme, error = MT.error, selectedLine = MT.lineId });
                            continue;
                        }



                        // check line , part, MT data exist
                        bool manufacturingTimeExist = CheckManufacturingTimeExistBool(line.lineId, part.partId);


                        // if exist update
                        if (manufacturingTimeExist == true)
                        {

                            int updateManufacturingTimeResult = updateManufacturingTimeCSV(line.lineId, part.partId, MT.manufacturingTIme);



                            if (updateManufacturingTimeResult == -1)
                            {
                                MT.error = "Cycle time is not valid. please check cycle time";

                                MT.lineId = 0;
                                ErrorListMT.Add(new PartLineManufacturingViewModel { partName = part.partName, lineName = line.lineName, manufacturingTIme = MT.manufacturingTIme, error = MT.error, selectedLine = MT.lineId });
                            }
                            else if (updateManufacturingTimeResult == 1)
                            {
                                MT.error = "Successfully updated";

                                MT.lineId = 1;
                                ErrorListMT.Add(new PartLineManufacturingViewModel { partName = part.partName, lineName = line.lineName, manufacturingTIme = MT.manufacturingTIme, error = MT.error, selectedLine = MT.lineId });
                            }
                            else
                            {
                                MT.error = "Error updating manufacturing time information";

                                MT.lineId = 0;
                                ErrorListMT.Add(new PartLineManufacturingViewModel { partName = part.partName, lineName = line.lineName, manufacturingTIme = MT.manufacturingTIme, error = MT.error, selectedLine = MT.lineId });
                            }
                            continue;
                        }

                        // Does not exist add new manufacturing Time data here

                        int newMTResult = newManufacturingTimeCSV(line.lineId, part.partId, MT.manufacturingTIme);

                        if (newMTResult == -1)
                        {
                            MT.error = "Cycle time is not valid. please check cycle time";

                            MT.lineId = 0;
                            ErrorListMT.Add(new PartLineManufacturingViewModel { partName = part.partName, lineName = line.lineName, manufacturingTIme = MT.manufacturingTIme, error = MT.error, selectedLine = MT.lineId });
                        }
                        else if (newMTResult == 1)
                        {
                            MT.error = "Successfully Added";

                            MT.lineId = 1;
                            ErrorListMT.Add(new PartLineManufacturingViewModel { partName = part.partName, lineName = line.lineName, manufacturingTIme = MT.manufacturingTIme, error = MT.error, selectedLine = MT.lineId });
                        }
                        else
                        {
                            MT.error = "Error in adding manufacturing time information";

                            MT.lineId = 0;
                            ErrorListMT.Add(new PartLineManufacturingViewModel { partName = part.partName, lineName = line.lineName, manufacturingTIme = MT.manufacturingTIme, error = MT.error, selectedLine = MT.lineId });
                        }



                    }




                    workbook.Save();
                    workbook.Close(true);
                    application.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(application);

                    // application.Workbooks.Close();
                    //  workbook.Close(path);


                    TempData["importManufacturingCSV"] = 1;
                    return View(ErrorListMT);
                }
                else
                {
                    // ViewBag.Error = "File type is incorrect";
                    TempData["ErrorCSV"] = "File type is incorrect";

                    return RedirectToAction("importManufacturingCSV");

                }
            }



        }





    }
}