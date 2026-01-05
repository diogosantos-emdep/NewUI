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
    //class NumberToMonthNameConverter
    //{
    //}


    public class NumberToMonthNameConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                switch (System.Convert.ToInt16(value))
                {
                    case 1: return "January";
                    case 2: return "February";
                    case 3: return "March";
                    case 4: return "April";
                    case 5: return "May";
                    case 6: return "June";
                    case 7: return "July";
                    case 8: return "August";
                    case 9: return "September";
                    case 10: return "October";
                    case 11: return "November";
                    case 12: return "December";
                }
            }
            catch
            {
                return "";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new NumberToMonthNameConverter();
        }

    }
}
