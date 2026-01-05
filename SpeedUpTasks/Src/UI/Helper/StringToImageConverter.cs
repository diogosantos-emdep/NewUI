using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.UI.Helper
{
    public class StringToImageConverter : MarkupExtension, IValueConverter
    {
        //public ResourceDictionary Items { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new BitmapImage(new Uri(@"/Emdep.Geos.Modules.Crm;component/Assets/Images/" + value.ToString(), UriKind.Relative));
            //string key = Enum.GetName(value.GetType(), value);
            //return Items[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }


    }
}
