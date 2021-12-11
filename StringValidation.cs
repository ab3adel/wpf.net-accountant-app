using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Diagnostics;
namespace accountant
{
    class StringValidation:ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            Trace.WriteLine((string)value);
           
          if (!string.IsNullOrEmpty((string)value)){
                if (value.ToString().Length >= 30)
                {
                    return new ValidationResult(false, "it must be less than 30 ");
                }
            }
            else
            {
                return new ValidationResult(false, "Enter value please");
            }
            return ValidationResult.ValidResult;

        }
        
    }
}
