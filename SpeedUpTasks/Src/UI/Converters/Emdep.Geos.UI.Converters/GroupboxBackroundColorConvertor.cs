using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Emdep.Geos.UI.Converters
{
    public class GroupboxBackroundColorConvertor : MarkupExtension, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value== DependencyProperty.UnsetValue)
                return null;
            else
            {
                if (value != null)
                {
                    ushort PolyvalenceUsage = (ushort)value;
                    if (PolyvalenceUsage >= 0 && PolyvalenceUsage <= 40)
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0000")); //red
                    else if (PolyvalenceUsage > 40 && PolyvalenceUsage <= 70)
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFF00")); //Yellow
                    else if (PolyvalenceUsage > 70 && PolyvalenceUsage <= 100)
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#008000")); //green 
                }
                else
                    return null;

                return null;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
