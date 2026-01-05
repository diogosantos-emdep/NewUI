using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Emdep.Geos.UI.Converters
{
    // [nsatpute][21-01-2025][GEOS2-5725]
    public class EmptyToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value as string))
                return new GridLength(0); // Set height to 0
            return GridLength.Auto; // Set height to Auto
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // One-way binding
        }
    }
}
