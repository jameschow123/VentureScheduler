﻿using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scheduler.ViewModel
{
    public class OrderPartViewModel
    {

        public Order order { get; set; }


        [Display(Name = "Parts")]
        public int selectedPart { get; set; }
        [Display(Name = "Part Name")]
        public string partName { get; set; }
        public List<Part> parts { get; set; }


        public List<string> status { get; set; }

        public List<int> priority { get; set; }

    }
}