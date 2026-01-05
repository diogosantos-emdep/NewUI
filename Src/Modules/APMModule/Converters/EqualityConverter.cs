using System;
using System.Globalization;
using System.Windows.Data;

namespace Emdep.Geos.Modules.APM.Converters
{
    /// <summary>
    /// Conversor para verificar igualdade entre dois valores
    /// </summary>
    public class EqualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null && parameter == null)
                return true;

            if (value == null || parameter == null)
                return false;

            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
