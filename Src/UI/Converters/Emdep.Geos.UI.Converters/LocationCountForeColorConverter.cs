using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Globalization;

namespace Emdep.Geos.UI.Converters
{
    //[GEOS2-5382][rdixit][19.04.2024]
    public class LocationCountForeColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {

                    int num = (int)value;

                    if (num < 0)
                    {
                        return -1;
                    }
                    else if (num > 0)
                    {
                        return +1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return 0;
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
