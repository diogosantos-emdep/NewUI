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
    public class MonthDaysToBooleanConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)

        {
            if (!string.IsNullOrWhiteSpace(System.Convert.ToString(value)))
            {
                return ((int)value) > 30;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType,

          object parameter, CultureInfo culture)

        {

            return false;

        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

}
