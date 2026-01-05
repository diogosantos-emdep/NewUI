using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class ListItemBackgroundColorConverter : MarkupExtension, IMultiValueConverter
    {
        private static ListItemBackgroundColorConverter _converter;
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is IList)
                return ((IList)values[0]).IndexOf(values[1]) == (((IList)values[0]).Count - 1);
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new ListItemBackgroundColorConverter();
            return _converter;
        }
    }
}
