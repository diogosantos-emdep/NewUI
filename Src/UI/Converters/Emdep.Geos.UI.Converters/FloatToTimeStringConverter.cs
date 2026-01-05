using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{
   public class FloatToTimeStringConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            if (value != null)
            {
                TimeSpan timeSpan = new TimeSpan();
                float val = (float)float.Parse(value.ToString());
               
                timeSpan = TimeSpan.FromHours(val);
                string hrs = ((int) timeSpan.TotalHours).ToString();
                string min = ((int)timeSpan.Minutes).ToString();
                string format = hrs.PadLeft(2, '0') + ":" + min.PadLeft(2, '0');
                return format;
            }
            return "00:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
