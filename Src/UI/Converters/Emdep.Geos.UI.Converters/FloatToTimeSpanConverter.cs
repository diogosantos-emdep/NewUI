using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{

    [ValueConversion(typeof(float), typeof(string))]
    public class FloatToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan timeSpan=new TimeSpan();
            if (value != null)
            {
                float d = (float)System.Convert.ToDouble(value.ToString());
                timeSpan = TimeSpan.FromHours(d);
               
            }
            return timeSpan;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
