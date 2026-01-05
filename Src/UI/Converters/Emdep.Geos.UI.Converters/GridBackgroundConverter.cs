using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Emdep.Geos.UI.Converters
{
    public class GridBackgroundConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {   
            if(value.ToString().Contains('d'))
            {
                if (value.ToString().Contains('-') || value.ToString().Equals("0d"))
                {
                    SolidColorBrush redBrush = new SolidColorBrush();
                    redBrush.Color = Colors.Red;
                    return redBrush;
                }
                else
                {
                    SolidColorBrush greenBrush = new SolidColorBrush();
                    greenBrush.Color = Colors.Green;
                    return greenBrush;
                }
            }
            if (string.IsNullOrEmpty(value.ToString()))
                return false;
            else
            {
                decimal RemainingHours = (decimal)value;
                if (RemainingHours <= 0)
                    return false;
                else
                    return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
