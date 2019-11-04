using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler.Models
{
    public class ManufacturingTime
    {
        public int lineId { get; set; }
        public int PartId { get; set; }
        public int manufacturingTIme { get; set; }
    }
}