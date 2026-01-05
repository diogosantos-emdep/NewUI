using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Emdep.Geos.UI;
using Emdep.Geos.UI.Common;


namespace Emdep.Geos.UI.Converters
{
    public class ConnectorForegroundConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// rajashri GEOS2-5158 Modify columns in search
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            

            if (value == null || string.IsNullOrEmpty(value.ToString()) || value.Equals("#00FFFFFF"))
            {
                if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "WhiteAndBlue")
                    return Brushes.Black; // "#FFFFFFFF";
                if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "BlackAndBlue")
                    return Brushes.White; // "#FF000000";
            }
            if (value != null)
            {
                if (value != null && value.ToString().Trim().Equals("#00FFFFFF"))
                {
                    if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "WhiteAndBlue")
                        return Brushes.Black; // "#FFFFFFFF";
                    if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "BlackAndBlue")
                        return Brushes.White; // "#FF000000";
                }
                else
                {
                    string colorString = value.ToString();
                    Color _color = (Color)ColorConverter.ConvertFromString(colorString);
                    var brightness = Brightness(_color);
                    var foreGround = brightness > 150 ? Brushes.Black : Brushes.White;
                    return foreGround;
                }
            }

            return Brushes.Black;
        }

        // You may want to define or ensure that the Brightness method is implemented correctly.

        private static int Brightness(Color c)
        {
            return (int)Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        private bool IsColorLight(Color color)
        {
            // Your logic to determine whether the color is light or dark
            double luminance = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
            return luminance > 128;
        }
    }
}
