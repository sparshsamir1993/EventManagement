﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EventManagement
{
    class NumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int num ;
            if (value != null && !int.TryParse(value.ToString(), out num))
            {
                return new ValidationResult(false, "A number should be entered.");
            }
            else if(value != null && int.TryParse(value.ToString(), out num) && num > 500)
            {
                return new ValidationResult(false, "People should not be more than 500");
            }
            else if(value != null && int.TryParse(value.ToString(), out num) && num < 1)
            {
                return new ValidationResult(false, "People cant be less than 1");
            }



            return ValidationResult.ValidResult;
        }
    }
}
