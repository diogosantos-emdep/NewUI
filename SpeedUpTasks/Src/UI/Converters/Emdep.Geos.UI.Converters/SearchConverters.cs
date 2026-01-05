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
   public  class SearchConverters : MarkupExtension, IValueConverter
    {
     public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
           var list = value as List<string>;
            if (list != null)
                return list.Count > 0 ? string.Join("/n", list.ToArray()) : "<empty>";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
