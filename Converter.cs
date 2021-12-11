using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Diagnostics;
namespace accountant
{
   public class Converter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            string pass = "";
           if (!string.IsNullOrEmpty(value.ToString()))
            {
                for (int i =1;i <= value.ToString().Length; i++)
                {
                    pass += "*";
                }
                
            }
           
            
            return pass;
        }
       public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

          
            authentication.pass = value.ToString();
            return value.ToString();
        }
       
    }
}
