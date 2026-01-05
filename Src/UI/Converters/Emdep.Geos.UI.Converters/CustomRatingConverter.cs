using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Emdep.Geos.UI.Converters
{
    public class CustomRatingConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        int CalcAbsoluteCurrent(double min, double max, int count, double current)
        {
            var delta = max - min;
            if (double.IsNaN(delta))
            {
                delta = 1.0;
            }
            var stepCount = delta / count;
            return (int)Math.Round(current / stepCount);
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var min = (double)values[0];
            var max = (double)values[1];
            var count = (int)values[2];
            var current = (double)values[3];
            var absCurrent = CalcAbsoluteCurrent(min, max, count, current);
            switch (absCurrent)
            {
                case 1: return Brushes.Red;
                case 2: return Brushes.Orange;
                case 3: return Brushes.Yellow;
                case 4: return Brushes.SkyBlue;
                case 5: return Brushes.Green;
                default: return Binding.DoNothing;
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
