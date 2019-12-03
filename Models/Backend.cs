using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler.Models
{
    public class Backend
    {
        public int BEID { get; set; }
        public int partId { get; set; }
        public string processName { get; set; }
        public int duration{ get; set; }

    }
}