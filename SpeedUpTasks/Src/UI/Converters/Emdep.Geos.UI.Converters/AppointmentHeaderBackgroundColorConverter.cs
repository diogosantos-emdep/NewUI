using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Emdep.Geos.UI.Converters
{
    public class AppointmentHeaderBackgroundColorConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double TotalDuration = (double)values[0];
            decimal DailyWorkHours = (decimal)values[1];
            if (values[2] == DependencyProperty.UnsetValue || values[3] == DependencyProperty.UnsetValue)
                return new SolidColorBrush();
            SolidColorBrush green = (SolidColorBrush)values[2];
            SolidColorBrush red = (SolidColorBrush)values[3];

            if ((decimal)TotalDuration < DailyWorkHours)
            {
                return red;
            }
            else
            {
                if ((decimal)TotalDuration == 0)
                    return red;
                else
                    return green;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
