using System;
using System.Globalization;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{
    public class DueDatePassedConverter : IValueConverter
    {
        // [nsatpute][11-10-2024][GEOS2-5975]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dueDate)
            {
                return dueDate < DateTime.Today;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
