using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler.Models
{
    public class Schedule
    {
        public int orderId { get; set; }
        public int partId { get; set; }
        public int lineId { get; set; }
        public int backendId { get; set; }
        public DateTime BEDate { get; set; }
        public DateTime earlistStartDate { get; set; }
        public DateTime latestStartDate { get; set; }
        public DateTime smtStart { get; set; }
        public DateTime smtEnd { get; set; }





    }
}