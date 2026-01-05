using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
   public class DateCellColorConvertor : MarkupExtension, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            var converter = new System.Windows.Media.BrushConverter();
            if (value != System.DBNull.Value)
            {
                if ((value != null && ((DateTime)(value) < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date)))
                    return true;           
            }
            return false;
        }

        public object ConvertBack(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
