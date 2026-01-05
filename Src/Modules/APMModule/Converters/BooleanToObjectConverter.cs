using System;
using System.Globalization;
using System.Windows.Data;

namespace Emdep.Geos.Modules.APM.Converters
{
    /// <summary>
    /// Converts a boolean value to one of two objects.
    /// </summary>
    public class BooleanToObjectConverter : IValueConverter
    {
        /// <summary>
        /// The value to return when the boolean is true.
        /// </summary>
        public object TrueValue { get; set; }

        /// <summary>
        /// The value to return when the boolean is false.
        /// </summary>
        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? TrueValue : FalseValue;
            }
            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.Equals(TrueValue))
                return true;
            if (value != null && value.Equals(FalseValue))
                return false;
            return false;
        }
    }
}
