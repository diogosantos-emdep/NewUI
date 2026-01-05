using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class TimeSpanToStringConverer : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values != null)
            {
                if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue)
                    return null;
                TimeSpan fromTime = (TimeSpan)values[0];
                TimeSpan toTime = (TimeSpan)values[1];
                TimeSpan breakTime = (TimeSpan)values[2];
                return fromTime.ToString(@"hh\:mm") + "/" + toTime.ToString(@"hh\:mm") + "/" + breakTime.ToString(@"hh\:mm");
            }

            return null ;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
