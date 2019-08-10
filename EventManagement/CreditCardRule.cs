using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EventManagement
{
    class CreditCardRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = string.Empty;
            if (value != null && !CheckCC(value.ToString(), out error))
            {
                return new ValidationResult(false, error);
            }



            return ValidationResult.ValidResult;
        }
        public static bool CheckCC(string cc, out string ccError)
        {
            string[] spaceSplit = cc.Split(' ');
            string ccNoSpace = string.Empty;
            bool isBreak = false;
            ccError = string.Empty;
            //ccvalue = string.Empty;


            foreach (string temp in spaceSplit)
            {
                char[] tempChar = temp.ToCharArray();
                foreach (char t in tempChar)
                {
                    if (!char.IsDigit(t))
                    {
                        Console.WriteLine("You entered a non digit character. Please check.");
                        ccError = "You entered a non digit character. Please check.";
                        isBreak = true;
                        break;
                    }
                }
                if (isBreak)
                {
                    break;
                }
                ccNoSpace += temp;
            }

            if (isBreak)
            {
                return false;
            }
            else if (ccNoSpace.Length != 16)
            {
                Console.WriteLine("The length of Credit card number should be 16");
                ccError = "The length of Credit card number should be 16";
                return false;
            }
            return true;
        }
    }
}

