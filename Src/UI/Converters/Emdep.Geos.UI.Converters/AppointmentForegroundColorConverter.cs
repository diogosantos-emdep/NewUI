using DevExpress.Xpf.Scheduler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Emdep.Geos.UI.Converters
{
    public class AppointmentForegroundColorConverter:MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AppointmentLabel label = value as AppointmentLabel;

            var brightness = 0;
            if (label != null)
                brightness = Brightness(label.Color);
            var foreGround = Brushes.Black;
            return foreGround;
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
