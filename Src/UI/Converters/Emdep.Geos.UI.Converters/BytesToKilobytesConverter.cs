using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{
    public class BytesToKilobytesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string convertSize = "0KB";
            if (value != null)
            {
                double size = System.Convert.ToDouble(value);
                if (0 < size && size < 1022976)
                {
                    size = Math.Round((size / 1024), 2);
                    if (size < 1)
                    {
                        size = 1;
                    }
                    else
                    {
                        size = Math.Round(size, 0);
                    }

                    convertSize = size + " KB";
                }
                else
                {
                    size = Math.Round((size / (1024 * 1024)), 2);

                    if (size < 1)
                    {
                        size = 1;
                    }
                    else
                    {
                        size = Math.Round(size, 0);
                    }

                    convertSize = size + " MB";
                }
            }
            return convertSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
