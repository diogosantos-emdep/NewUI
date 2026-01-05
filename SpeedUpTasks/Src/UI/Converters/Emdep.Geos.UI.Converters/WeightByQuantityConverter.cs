using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
   public class WeightByQuantityConverter : MarkupExtension, IMultiValueConverter
    {
        #region WeightByQuantityConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            long qty = (long)values[0];
            float weight = (float)values[1];
            return qty * weight;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion
    }
}
