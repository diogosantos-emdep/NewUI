using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.UI.Converters
{
    public class RangeToBooleanConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType,object parameter, CultureInfo culture)

        {

            return ((int)value) > 99;

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
