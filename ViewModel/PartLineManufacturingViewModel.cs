using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scheduler.ViewModel
{
    public class PartLineManufacturingViewModel
    {

        [Required(ErrorMessage = "Line is required!")]
        [Display(Name = "Line")]
        public int selectedLine { get; set; }
        public List<Line> lines { get; set; }
        [Required(ErrorMessage = "Part is required!")]
        [Display(Name = "Parts")]
        public int selectedPart { get; set; }
        public List<Part> parts { get; set; }


        [Display(Name = "Manufacturing Time")]
        [Required(ErrorMessage = "Manufacturing Time is required!")]
        [Range(1, 10000, ErrorMessage = "Please enter a valid quantity!")]
        public int manufacturingTIme { get; set; }



        public string lineName { get; set; }
        public string partName { get; set; }


    }
}