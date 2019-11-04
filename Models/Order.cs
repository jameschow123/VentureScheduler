using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler.Models
{
    public class Order
    {
        public int orderId { get; set; }
        public int partId { get; set; }
        public string projectName { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime shipDate { get; set; }
        public int quantity { get; set; }


    }
}