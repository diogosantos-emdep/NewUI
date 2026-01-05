using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Emdep.Geos.Modules.APM.Converters
{
    /// <summary>
    /// Converte uma string de cor HTML (ex: "#FF0000" ou "Red") para SolidColorBrush
    /// </summary>
    public class StringToColorBrushConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return Brushes.Transparent;
            }

            try
            {
                string colorString = value.ToString();
                
                // Handle special color names
                if (colorString.Equals("Grey", StringComparison.OrdinalIgnoreCase))
                {
                    colorString = "Gray";
                }
                else if (colorString.Equals("Metallic", StringComparison.OrdinalIgnoreCase))
                {
                    colorString = "Silver";
                }

                Color color = (Color)ColorConverter.ConvertFromString(colorString);
                return new SolidColorBrush(color);
            }
            catch (Exception)
            {
                return Brushes.Transparent;
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
