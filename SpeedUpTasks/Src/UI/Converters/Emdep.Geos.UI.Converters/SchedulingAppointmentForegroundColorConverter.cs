using DevExpress.Xpf.Scheduling;
using System;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Globalization;

namespace Emdep.Geos.UI.Converters
{
   public class SchedulingAppointmentForegroundColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AppointmentLabelItem label = value as AppointmentLabelItem;

            var brightness = 0;
            if (label != null)
                brightness = Brightness(label.Color);
            var foreGround = brightness > 130 ? Brushes.Black : Brushes.White;
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
