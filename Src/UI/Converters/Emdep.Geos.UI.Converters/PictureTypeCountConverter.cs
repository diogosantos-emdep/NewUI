using Emdep.Geos.Data.Common.SCM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{
    public class PictureTypeCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<SCMConnectorImage> list && int.TryParse(parameter?.ToString(), out int type))
            {
                int count = list.Count(x => x.IdPictureType == type);

                string typeName = "Unknown";
                if (type == 0) typeName = "Sample Image";
                else if (type == 1) typeName = "WTG";
                else if (type == 2) typeName = "Other";

                return $"{typeName}: {count}";
            }
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
