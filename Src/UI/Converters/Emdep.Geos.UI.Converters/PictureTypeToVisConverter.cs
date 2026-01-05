using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{
    public class PictureTypeToVisConverter : IValueConverter
    {
        // value = IdPictureType from binding
        // parameter = expected group type (0, 1, 2)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Visibility.Collapsed;

            int idType;
            int expectedType;

            if (int.TryParse(value.ToString(), out idType) &&
                int.TryParse(parameter.ToString(), out expectedType))
            {
                return idType == expectedType ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
