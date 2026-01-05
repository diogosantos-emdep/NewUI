using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{//[Sudhir.Jangra][GEOS2-4816]
    public class DateTimeConverterForEmployeeTrip : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CultureInfo cn = Thread.CurrentThread.CurrentCulture;

            return ((DateTime)value).ToString(cn.DateTimeFormat.ShortDatePattern);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
