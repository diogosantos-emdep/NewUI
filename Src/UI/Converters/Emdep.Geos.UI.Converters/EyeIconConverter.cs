using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class EyeIconConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = value is bool b && b;

            // Adjust paths to your actual resource structure
            return isVisible
                ? "/GeosWorkbench;component/Assets/Images/Eye.png"
                : "/GeosWorkbench;component/Assets/Images/Hide.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
