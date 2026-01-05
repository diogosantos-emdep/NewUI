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
    public class TotalAmountValueConvertor : MarkupExtension, IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = new object();
            try
            {
                if (!(values[1].Equals("REMAINING") && values[2] is bool))
                    return values[0];
                var totalAmount = double.Parse(values[0].ToString());
                var name = (string)values[1];
                var isMyValueNegative = (bool)values[2];
                result = totalAmount.ToString("c");
                if (name == "REMAINING")
                {
                    return (isMyValueNegative ? "-" : "+") + result;
                }
                return result;
            }
            catch
            {

            }
            return result;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
