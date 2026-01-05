using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Emdep.Geos.UI.Common;

namespace Emdep.Geos.UI.Converters
{
    public class CheckValueGreaterThenZeroConverter:MarkupExtension, IValueConverter
    {

        public object Convert(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return (System.Convert.ToInt32(value) > 0);
            }
            catch
            {
                return false;
            }
        }
        public object ConvertBack(object value, System.Type targetType,
                   object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }

}
