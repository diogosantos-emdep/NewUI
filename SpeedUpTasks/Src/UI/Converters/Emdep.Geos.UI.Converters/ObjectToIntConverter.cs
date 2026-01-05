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
    public class ObjectToIntConverter : IValueConverter
    {
        /// <summary>
        /// 1. Only quoted = 1;
        /// 2. Waiting for quote = 2;
        /// 3. CloseDate = 3;
        /// 4. DeliveryDate = 4;
        /// 5. PODateRed = 5;
        /// 6. PODateGreen = 6;
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)  
        {  
            int isoffer = 0;  
            if (!string.IsNullOrEmpty(value.ToString()) && value.ToString().Contains("(Only quoted)"))  
            {
                isoffer = 1;
            }

            if (!string.IsNullOrEmpty(value.ToString()) && value.ToString().Contains("(Waiting for quote)"))
            {
                isoffer = 2;
            }

            if (!string.IsNullOrEmpty(value.ToString()) && value.ToString().Contains("(CloseDate)"))
            {
                isoffer = 3;
            }

            if (!string.IsNullOrEmpty(value.ToString()) && value.ToString().Contains("(DeliveryDate)"))
            {
                isoffer = 4;
            }
            if (!string.IsNullOrEmpty(value.ToString()) && value.ToString().Contains("(PODateRed)"))
            {
                isoffer = 5;
            }
            if (!string.IsNullOrEmpty(value.ToString()) && value.ToString().Contains("(PODateGreen)"))
            {
                isoffer = 6;
            }

            return isoffer;  
        }  
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)  
        {  
            throw new NotImplementedException();  
        }  
    }
}
