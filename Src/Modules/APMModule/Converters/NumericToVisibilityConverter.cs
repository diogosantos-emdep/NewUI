using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Emdep.Geos.Modules.APM.Converters
{
    /// <summary>
    /// Converts a numeric value to Visibility.
    /// Returns Visible if the value is greater than 0, otherwise Collapsed.
    /// </summary>
    public class NumericToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            try
            {
                double numValue = System.Convert.ToDouble(value);
                return numValue > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
