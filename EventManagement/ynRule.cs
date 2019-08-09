using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EventManagement
{
    class ynRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            
            if (value.ToString().ToLower() != "y" && value.ToString().ToLower() != "n")
            {
                return new ValidationResult(false, "Please enter y/n.");
            }



            return ValidationResult.ValidResult;
        }
    }
}
