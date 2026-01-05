
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{
    public class CaptionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue)
                return string.Empty;
            DateTime start = (DateTime)values[0];
            DateTime end = (DateTime)values[1];
            string defaultCaption = values[2].ToString();
            int duration = (end - start).Days;
            CultureInfo newCulture = CultureInfo.CreateSpecificCulture(GeosApplication.Instance.UserSettings["Language"]);
            //override the week scale caption
            if (duration == 7)
                return String.Format("{0} - {1}", start.ToString("dddd, dd MMMM yyyy", newCulture), end.ToString("dddd, dd MMMM yyyy", newCulture));
            //override the day scale caption
            if (duration == 1)
                return start.ToString("dd ddd", newCulture);

            //return default caption
            return defaultCaption;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
