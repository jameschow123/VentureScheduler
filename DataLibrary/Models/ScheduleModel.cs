using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class ScheduleModel
    {


        public int orderId { get; set; }
        public int partId { get; set; }
        public int lineId { get; set; }

        public int backendId { get; set; }
        public DateTime BEDate { get; set; }

        public DateTime EarliestStartDate { get; set; }
        public DateTime PlannedStartDate { get; set; }

        public DateTime LatestStartDate { get; set; }
        public DateTime SMTStart { get; set; }
        public DateTime SMTEnd { get; set; }


        public string status { get; set; }



    }
}
