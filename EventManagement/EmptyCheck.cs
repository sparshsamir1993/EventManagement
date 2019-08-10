using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EventManagement
{
    class EmptyCheck : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = string.Empty;
            
            if(value == null || value == string.Empty)
            {
                return new ValidationResult(false, "Cannot be Empty");
            }

            return ValidationResult.ValidResult;






        }
    }
}
