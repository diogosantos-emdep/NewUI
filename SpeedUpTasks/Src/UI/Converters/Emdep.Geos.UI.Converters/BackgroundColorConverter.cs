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
    public class BackgroundColorConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string htmlColor = string.Empty;

            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue)
                return null;


            string ColumnName = (string)values[2];

            if (ColumnName.Equals("Qty"))
            {
                if(values[0] == null)
                    return null;

                long OutQty = (long)values[0];
                long TotalQty = (long)values[1];

                if (TotalQty >= OutQty)
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#B6F5BB")); //green
                else if (TotalQty <= 0)
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#F7926D")); //red
                else
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#F7F4C6")); //yellow
            }
            else if (ColumnName.Equals("ExpectedStock"))
            {
                if(values[1] != null)
                {
                    long ExpectedQty = (long)values[0];
                    long NextOtItemPickingStock = (long)values[1];
                    if(ExpectedQty <= 0)
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#F7926D")); //red
                    else if(ExpectedQty < NextOtItemPickingStock)
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#F7F4C6")); //yellow
                    else
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#B6F5BB")); //green
                }
                else
                {
                    long ExpectedQty = (long)values[0];
                    if(ExpectedQty <= 0)
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#F7926D")); //red
                    else
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#B6F5BB")); //green
                }
                
               
            }
            else
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
