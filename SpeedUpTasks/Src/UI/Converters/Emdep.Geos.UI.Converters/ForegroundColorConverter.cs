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
    public class ForegroundColorConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// [001][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
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
               // string colorString = (string)value;
                string colorString = (string)value.ToString();//[01] added
                Color _color = (Color)ColorConverter.ConvertFromString(colorString);
                var brightness = 0;
                brightness = Brightness(_color);
                var foreGround = brightness > 150 ? Brushes.Black : Brushes.White;
                return foreGround;
            }

            return null;
        }

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
    }

}
