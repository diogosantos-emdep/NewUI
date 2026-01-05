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
    public class DayWithSuffixConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Day =System.Convert.ToDateTime(value).Day.ToString();
            string suffix = "th";
          
            switch (Day.ToCharArray()[Day.ToCharArray().Length - 1].ToString())
            {
                case "1":
                    suffix = "st";
                    break;
                case "2":
                    suffix = "nd";
                    break;
                case "3":
                    suffix = "rd";
                    break;
                default:
                    suffix = "th";
                    break;
            }
            return suffix;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
