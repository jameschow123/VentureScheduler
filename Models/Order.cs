using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scheduler.Models
{
    public class Order : IValidatableObject
    {


        [Display(Name = "Order ID")]
        [Required(ErrorMessage = "Order ID is required!")]
        [Range(00000000, 99999999, ErrorMessage = "Please enter a valid orderID")]
        public int orderId { get; set; }
        [Required(ErrorMessage = "Part ID is required!")]
        [Display(Name = "Product ID")]
        public int partId { get; set; }
        [Required(ErrorMessage = "Project Name is required!")]
        [Display(Name = "Project Name")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Please enter a project name between 5-100 characters")]
        public string projectName { get; set; }
        [Required(ErrorMessage = "Order date is required!")]
        [Display(Name = "Order Date (MM/DD/YYYY HH:mm)")]
        [DisplayFormat(DataFormatString = "{0:MM-DD-YYYY HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime lastMaterialDate { get; set; }
        [Display(Name = "Last material in Date (MM/DD/YYYY HH:mm)")]
        [Required(ErrorMessage = "Ship Date is required!")]
        [DataType(DataType.DateTime)]
        public DateTime shipDate { get; set; }
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required!")]
        [Range(1, 10000, ErrorMessage = "Please enter a valid quantity!")]
        public int quantity { get; set; }







        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();

            if (lastMaterialDate > shipDate)
            {
                ValidationResult mss = new ValidationResult("last Material Date must be before shipdate");
                res.Add(mss);

            }
            if (shipDate < DateTime.Today)
            {
                ValidationResult mss = new ValidationResult("shipdate must be greater today");
                res.Add(mss);

            }

            return res;
        }


    }
}