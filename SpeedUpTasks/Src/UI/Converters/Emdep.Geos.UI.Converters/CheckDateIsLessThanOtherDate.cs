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
   public class CheckDateIsLessThanOtherDate : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DateTime date1,date2;
                date1 = System.Convert.ToDateTime(values[0]);
                if (values[1] == null)
                {
                    date2 = GeosApplication.Instance.ServerDateTime.Date;

                    if (date1 < date2)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
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
