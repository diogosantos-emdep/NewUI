using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Emdep.Geos.UI.Common;
using System.Globalization;

namespace Emdep.Geos.UI.Converters
{
   public class CheckValueLessThenOrEqualToConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int value1 = System.Convert.ToInt32(values[0]);
                int value2 = System.Convert.ToInt32(values[1]);

                if (value1 <= value2)
                    return true;
                else
                    return false;
            }
            catch
            {
                return true;
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
