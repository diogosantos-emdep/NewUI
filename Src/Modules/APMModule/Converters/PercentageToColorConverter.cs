using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Emdep.Geos.Modules.APM.Converters
{
    /// <summary>
    /// Converter que retorna cor baseada na percentagem:
    /// - Vermelho: 0-29%
    /// - Laranja: 30-69%
    /// - Verde: 70-100%
    /// </summary>
    public class PercentageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Gray);

            double percentage = 0;
            
            if (value is double doubleValue)
                percentage = doubleValue;
            else if (value is int intValue)
                percentage = intValue;
            else if (double.TryParse(value.ToString(), out double parsedValue))
                percentage = parsedValue;
            else
                return new SolidColorBrush(Colors.Gray);

            // Vermelho para percentagens baixas (< 30%)
            if (percentage < 30)
                return new SolidColorBrush(Color.FromRgb(244, 67, 54)); // #F44336 - Red

            // Laranja para percentagens médias (30-69%)
            if (percentage < 70)
                return new SolidColorBrush(Color.FromRgb(255, 152, 0)); // #FF9800 - Orange

            // Verde para percentagens altas (>= 70%)
            return new SolidColorBrush(Color.FromRgb(76, 175, 80)); // #4CAF50 - Green
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("PercentageToColorConverter is a one-way converter.");
        }
    }
}
