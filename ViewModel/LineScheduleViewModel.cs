using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scheduler.Models;

namespace Scheduler.ViewModel
{
    public class LineScheduleViewModel
    {
        public List<Line> lines { get; set; }
        public List<Order> order { get; set; }

    }
}