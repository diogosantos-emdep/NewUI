using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class ForeGroundColourBasedOnValue : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int Value = int.Parse(value.ToString());
                if (Value > 0)
                {
                    return "#008000";
                }
                else
                {
                    return "#FF0000";
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ForeGroundColourBasedOnValue Method Convert()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}