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
        [Display(Name = "Work order no.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Please enter a Work order no. between 5-100 characters")]
        public string projectName { get; set; }
        [Required(ErrorMessage = "Order date is required!")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last Material date")]
        [DataType(DataType.DateTime)]
        public DateTime lastMaterialDate { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Shipment Date")]
        [Required(ErrorMessage = "Ship Date is required!")]
        [DataType(DataType.DateTime)]
        public DateTime shipDate { get; set; }
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required!")]
        [Range(1, 10000, ErrorMessage = "Please enter a valid quantity!")]
        public int quantity { get; set; }

        public string status { get; set; }
        public int priority { get; set; }

        public bool statusBool { get; set; }
        public int intTempResult { get; set; }
        public string StringTempResult { get; set; }

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
                ValidationResult mss = new ValidationResult("shipdate must be greater then today");
                res.Add(mss);

            }

            return res;
        }


    }

    public class OrderDetail
    {
        /// <summary>  
        /// To hold list of orders  
        /// </summary>  
        public List<Order> OrderDetails { get; set; }

    }

}