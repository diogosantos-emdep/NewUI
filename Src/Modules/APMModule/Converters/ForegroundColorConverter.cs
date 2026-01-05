using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Emdep.Geos.Modules.APM.Converters
{
    public class ForegroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var colorStr = value as string;
                if (string.IsNullOrWhiteSpace(colorStr)) return Brushes.Black;
                var brush = (SolidColorBrush)(new BrushConverter().ConvertFromString(colorStr));
                // Decide based on luminance
                var c = brush.Color;
                var lum = (0.299 * c.R + 0.587 * c.G + 0.114 * c.B) / 255.0;
                return lum > 0.5 ? Brushes.Black : Brushes.White;
            }
            catch { return Brushes.Black; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}