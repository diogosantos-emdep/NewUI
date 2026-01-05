using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.UI.Converters
{
    public class CellImageSourceConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return null;
            }
            else
            {
                Uri resUri;
                string Type = (string)value;
                if (Type.Trim() == "PO")
                {
                    resUri = new Uri(string.Format("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/Up.png"), UriKind.Relative);
                    return new BitmapImage(resUri);
                }
                else
                {
                   resUri = new Uri(string.Format("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/Down.png"), UriKind.Relative);
                   return new BitmapImage(resUri);
                }
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
