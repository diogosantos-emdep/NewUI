using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{
    public class AppointmentTimeDifferenceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is DateTime start && values[1] is DateTime end)
            {
                if ((end - start).TotalSeconds == 1)
                {
                    return ""; // Show empty string if the difference is 1 second
                }
                return end.ToString("g", culture); // Otherwise, return the formatted End time
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
