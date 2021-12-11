using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
namespace accountant
{
    public class PasswordValidator : ValidationRule
    {
        
        public override ValidationResult Validate (Object value ,System.Globalization.CultureInfo culterInfo)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, "you have to enter password");
            }
            if (value.ToString().Length < 8)
            {
                return new ValidationResult(false, "it must be 8 characters and numbers at least");
            }
            if (!value.ToString().Any(Char.IsDigit))
            {
                return new ValidationResult(false, "you have to include numbers and character");
            }
            return ValidationResult.ValidResult;
        }
    }
}
