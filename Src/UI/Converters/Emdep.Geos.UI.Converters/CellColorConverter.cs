using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Emdep.Geos.UI.Common;

namespace Emdep.Geos.UI.Converters
{
    public class CellColorConverter : MarkupExtension, IValueConverter
    {


        #region IValueConverter Members

        public object Convert(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            var converter = new System.Windows.Media.BrushConverter();
            if (value != System.DBNull.Value)
            {
                if ((value != null && ((DateTime)(value) < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date)))
                    return Brushes.Red;
                else if ((value != null && ((DateTime)(value) >= Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date)))
                {
                    if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                        return Brushes.Black;
                    //return new BrushConverter().ConvertFrom("#373A3D");
                    else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
                        return Brushes.White;
                }
            }
            return Brushes.Transparent;
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
