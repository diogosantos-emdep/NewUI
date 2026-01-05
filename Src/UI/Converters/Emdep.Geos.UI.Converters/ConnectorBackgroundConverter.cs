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
    public class ConnectorBackgroundConverter : MarkupExtension, IValueConverter
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
            string colorString = (value != null) ? value.ToString() : string.Empty;
            if (value == null || string.IsNullOrEmpty(value.ToString()) || value.Equals("#00FFFFFF"))
            {
                if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "WhiteAndBlue")
                    return Brushes.Black; // "#FFFFFFFF";
                if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "BlackAndBlue")
                    return Brushes.White; // "#FF000000";
            }
            if (value != null)
            {
                try
                {
                    string colors = value.ToString();
                    if(colors == "Grey")
                    {
                        Color ecolor = (Color)ColorConverter.ConvertFromString("Gray");
                        return new SolidColorBrush(ecolor);
                    }
                    if (colors == "Metallic")
                    {
                        Color ecolor = (Color)ColorConverter.ConvertFromString("Silver");
                        return new SolidColorBrush(ecolor);
                    }
                   
                   Color color = (Color)ColorConverter.ConvertFromString(colorString);
                    return new SolidColorBrush(color);
                }
                catch (Exception ex)
                {
                   
                    return Brushes.Transparent;
                }
            }

            return Brushes.Transparent;
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
