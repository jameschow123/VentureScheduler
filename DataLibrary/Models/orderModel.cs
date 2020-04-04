using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.models
{
    public class orderModel
    {

        public int orderId { get; set; }
        public int partId { get; set; }
        public string projectName { get; set; }
        public DateTime lastMaterialDate { get; set; }
        public DateTime shipDate { get; set; }
        public int quantity { get; set; }

        public string status { get; set; }

        public int priority { get; set; }



        List<bool> schedule { get; set; }
    }
}
