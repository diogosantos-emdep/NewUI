using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class MultiTreeListControlConverter : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 5)
                return null; // or handle appropriately

            return new Tuple<object, object, object, object, object>(values[0], values[1], values[2], values[3], values[4]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
