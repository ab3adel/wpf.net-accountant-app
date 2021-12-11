using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Diagnostics;
namespace accountant
{
   public class Translator:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            string str = value.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                if (str == "expense")
                {
                    return "سحوبات";
                }
               else if (str == "revenue")
                {
                    return "عائدات";
                }
              else  if (str == "gain")
                {
                    return "ربح";
                }
                else if (str == "cost")
                {
                    return "كلفة";
                }
                else if (str == "sellings")
                {
                    return "المبيعات";
                }
                else if (str == "boxes")
                {
                    return "الخزائن";
                }
            }
            Trace.WriteLine(str);
            return str;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            return value.ToString();
        }

    }
}
