using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class HoursToDayConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                if (values.Length > 2 &&
                    (values[2] == null || values[2] == DependencyProperty.UnsetValue))
                {
                    return string.Empty;
                }
                else
                {
                    return "0d";
                }
            }

            decimal TotalHours = (decimal)values[0];
            decimal DailyHours = (decimal)values[1];

            if (TotalHours == 0)
            {
                return TotalHours.ToString() + "d";
            }

            decimal Days = TotalHours / DailyHours;
            decimal Hours = TotalHours % DailyHours;

            string formattedString = " ";
            formattedString = string.Format("{0}d", (Int32)Days);
            if (Hours != 0)
            {
                formattedString += string.Format(" {0}H", Hours.ToString("0.##"));
            }
            return formattedString;

          //  return string.Format("{0}d {1}H", (Int32)Days, Hours.ToString("0.##"));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
