using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.UI.Converters
{
    //[nsatpute][GEOS2-7215][25.07.2025]
    public class ListToCompanyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value; // No conversion needed for display
        }
        //[nsatpute][GEOS2-7215][25.07.2025]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<object> enumerable)
                return enumerable.ToList();

            if (value != null)
                return new List<object> { value };

            return new List<object>();
        }
    }
}
