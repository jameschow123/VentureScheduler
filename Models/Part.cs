using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scheduler.Models
{
    public class Part
    {
        
        public int partId { get; set; }
        [Display(Name = "Part Name")]
        [Required(ErrorMessage = "Part Name is required!")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Please enter a partName between 5-100 characters")]
        public string partName { get; set; }
        [Display(Name = "Side")]
        [Required(ErrorMessage = "Quantity is required!")]
        [Range(1, 2, ErrorMessage = "Please enter a valid side! enter 2 for both sides.")]
        public int side { get; set; }




    }
}