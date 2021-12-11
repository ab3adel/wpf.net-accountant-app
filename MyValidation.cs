using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;
using System.Diagnostics;
namespace accountant
{
   public class MyValidation:ValidationRule
    {
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            
            int result;
            bool done = Int32.TryParse((string)value, out result);
            if (!done)
            {
                return new ValidationResult(false,"please enter number only");
            }
            else
            {
                if (result <0)
                {
                    return new ValidationResult(false, "positive number allowed only");
                }
            }
            return ValidationResult.ValidResult;
      
        }
    }
}
