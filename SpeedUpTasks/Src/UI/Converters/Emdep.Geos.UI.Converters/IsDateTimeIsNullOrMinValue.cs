using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{
 public   class DateTimeIsNullOrMinValueVisibility : IValueConverter
    { 
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

           
            if (value == null)
            {
                return Visibility.Hidden;
            }
            else
            {
                try
                {

                    DateTime dt = (DateTime)value;
                   
                    if(dt== DateTime.MinValue)
                    {
                       return  Visibility.Hidden;
                    } 
                }
                catch (Exception ex)
                {

                    return  Visibility.Visible;
                }
            }
            return   Visibility.Visible; 
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new  NotImplementedException();
        }
    }
}
