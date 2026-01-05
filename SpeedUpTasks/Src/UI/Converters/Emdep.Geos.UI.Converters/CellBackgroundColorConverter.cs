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
    public class CellBackgroundColorConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                return null;
            }

            Int64 CurrentStock = System.Convert.ToInt64(values[0]); // (Int64)values[0];
            Int64 MinStock = System.Convert.ToInt64(values[1]);     // (Int64)values[1];

            if (CurrentStock == 0)
            {
                return "Red";
            }
            else if (CurrentStock >= MinStock)
            {
                return "Green";
            }
            else if (CurrentStock < MinStock)
            {
                return "Yellow";
            }

            return null;
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
