using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scheduler.ViewModel
{
    public class linePartScheduleViewModel
    {

        public Schedule schedule { get; set; }


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


        [Display(Name = "Line Name")]
        public string lineName { get; set; }
        [Display(Name = "Part Name")]
        public string partName { get; set; }
        [Display(Name = "Order Name")]
        public string orderName { get; set; }

        public string error { get; set; }
    }
}