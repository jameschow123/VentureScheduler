using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scheduler.Models
{
    public class ManufacturingTime
    {
        [Required(ErrorMessage = "Line is required!")]
        [Display(Name = "Line")]
        public int lineId { get; set; }
        [Required(ErrorMessage = "Part is required!")]
        [Display(Name = "Parts")]
        public int PartId { get; set; }
        [Display(Name = "SMT Manufacturing Time")]
        [Required(ErrorMessage = "Manufacturing Time is required!")]
        [Range(1, 10000, ErrorMessage = "Please enter a valid quantity!")]
        public int manufacturingTIme { get; set; }


        public string error { get; set; }
    }
}