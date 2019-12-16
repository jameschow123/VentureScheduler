﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Scheduler.Models
{
    public class DateFormatValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date;
            var format = "0:MM/DD/yyyy HH:mm";
            bool parsed = DateTime.TryParseExact((string)value, format, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (!parsed)
                return false;
            return true;
        }
    }
}