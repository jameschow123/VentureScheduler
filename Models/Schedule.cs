using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class Schedule : IValidatableObject
    {
        [Display(Name = "Order ID")]
        [Required(ErrorMessage = "Order ID is required!")]
        [Range(00000000, 99999999, ErrorMessage = "Please enter a valid orderID")]
        public int orderId { get; set; }
        [Display(Name = "Part ID")]
        [Required(ErrorMessage = "Part ID is required!")]
        [Range(00000000, 99999999, ErrorMessage = "Please enter a valid partID")]
        public int partId { get; set; }
        [Display(Name = "Line ID")]
        [Required(ErrorMessage = "Line ID is required!")]
        [Range(00000000, 99999999, ErrorMessage = "Please enter a valid LineID")]
        public int lineId { get; set; }
        [Display(Name = "Backend ID")]
        [Required(ErrorMessage = "Backend ID is required!")]
        [Range(00000000, 99999999, ErrorMessage = "Please enter a valid BackendID")]
        public int backendId { get; set; }
        [Display(Name = "Backend Required Date")]
        [DataType(DataType.Date)]
        public DateTime BEDate { get; set; }
        [Display(Name = "earliest start Date")]
        [DataType(DataType.Date)]
        public DateTime earlistStartDate { get; set; }
        [Display(Name = "Planned start Date")]
        [DataType(DataType.Date)]
        public DateTime plannedStartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "latest start date")]
        public DateTime latestStartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "SMT start day")]
        public DateTime smtStart { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "SMT end day")]
        public DateTime smtEnd { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (plannedStartDate > earlistStartDate)
            {
                ValidationResult mss = new ValidationResult("Planned start date must be after earliest Start Date");
                res.Add(mss);

            }
            if (earlistStartDate > latestStartDate)
            {
                ValidationResult mss = new ValidationResult("Earliest Start Date must be before Latest Start Date");
                res.Add(mss);

            }
            if (latestStartDate > BEDate)
            {
                ValidationResult mss = new ValidationResult("Backend Process Date must be later then latest start date");
                res.Add(mss);

            }
            if (smtStart > smtEnd)
            {
                ValidationResult mss = new ValidationResult("SMT start date must be earlier then SMT end date");
                res.Add(mss);

            }


            return res;
        }



    }
}