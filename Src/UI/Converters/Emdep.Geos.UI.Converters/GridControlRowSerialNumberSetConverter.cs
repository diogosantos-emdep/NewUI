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
    public class GridControlRowSerialNumberSetConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int _rowNumber = 0;
            try
            {
                if (value != null)
                    _rowNumber = int.Parse(value.ToString()) + 1;
            }
            catch (Exception)
            {

            }
            return _rowNumber;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new GridControlRowSerialNumberSetConverter();
        }

    }
}

