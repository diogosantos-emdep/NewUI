using DevExpress.Office.Utils;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;


namespace Emdep.Geos.UI.Converters
{
    public class GridColorConverter : MarkupExtension, IValueConverter    
    {
      
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal TotalHours = (decimal)value;
            if (TotalHours > 0)
            {
                return true;
            }
            else
                return false;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
