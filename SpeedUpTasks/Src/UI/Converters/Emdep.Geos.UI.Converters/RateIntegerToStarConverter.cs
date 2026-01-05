using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class RateIntegerToStarConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (int.Parse(value.ToString()) >= 0 && int.Parse(value.ToString()) <= 20)
            {
                return 1;
            }
            if (int.Parse(value.ToString()) > 20 && int.Parse(value.ToString()) <= 40)
            {
                return 2;
            }
            if (int.Parse(value.ToString()) > 40 && int.Parse(value.ToString()) <= 60)
            {
                return 3;
            }
            if (int.Parse(value.ToString()) > 60 && int.Parse(value.ToString()) <= 80)
            {
                return 4;
            }
            if (int.Parse(value.ToString()) > 80 && int.Parse(value.ToString()) <= 100)
            {
                return 5;
            }

            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
