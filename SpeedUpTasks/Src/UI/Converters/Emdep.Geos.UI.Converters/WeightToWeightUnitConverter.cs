using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{
    public class WeightToWeightUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is string)
                {
                    double number;
                    string stringValue = System.Convert.ToString(value);

                    if (!Double.TryParse(stringValue, out number))
                        return null;
                }

                double weight = System.Convert.ToDouble(value);

                if (weight < 1)
                {
                    if (Math.Round(weight * 1000, 0) == 1000)
                        return string.Format("{0} Kg", 1);

                    return string.Format("{0} gr", Math.Round(weight * 1000, 0));
                }
                else
                {
                    return string.Format("{0} Kg", Math.Round(weight, 3));
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
