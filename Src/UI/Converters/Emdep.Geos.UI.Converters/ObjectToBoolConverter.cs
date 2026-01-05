using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class ObjectToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)  
        {  
            bool isBool = true;  
            if (value != null && !string.IsNullOrEmpty(value.ToString()) && value.ToString().Contains(false.ToString()))  
            {
                isBool = false;
            }  
            return isBool;  
        }  
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)  
        {  
            throw new NotImplementedException();  
        }  
    }
}
