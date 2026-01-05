using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using DevExpress.Utils;
using System.Windows.Markup;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.UI.Converters
{
  
    public class ImageConverter : IValueConverter
    {
         object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return new BitmapImage(new Uri(@"/Emdep.Geos.Modules.Warehouse;component/Assets/Images/" + value.ToString(), UriKind.Relative));
               
            }
            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    return new BitmapImage(new Uri(value.ToString(), UriKind.Relative));
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    return null;
        //}

        //public override object ProvideValue(IServiceProvider serviceProvider)
        //{
        //    return this;
        //}



    }
}
