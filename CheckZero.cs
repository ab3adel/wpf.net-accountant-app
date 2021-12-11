using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using System.Globalization;
using System.Diagnostics;
using System.Windows.Media;
namespace accountant
{
     public class CheckZero:IValueConverter
    {
       
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            if (!string.IsNullOrEmpty(value.ToString())){
                int num = int.Parse(value.ToString());
                return num == 0 ? Brushes.Red : Brushes.Transparent;
            }
            return Binding.DoNothing;
            }
            public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
            {
            if (string.IsNullOrEmpty(value.ToString())) return Binding.DoNothing;
                throw new NotSupportedException();
            }
        

    }

}
