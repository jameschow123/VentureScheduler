using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class manufacturingTimeModel
    {
        public int lineId { get; set; }
        public int partId { get; set; }
        public int manufacturingTime { get; set; }


        public string lineName { get; set; }
        public string partName { get; set; }


    }
}
