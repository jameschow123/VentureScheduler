using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scheduler.ViewModel
{
    public class OrderPartViewModel
    {

        public Order order { get; set; }


        [Required]
        [Display(Name = "Parts")]
        public int selectedPart { get; set; }
        public List<Part> parts { get; set; }
    }
}