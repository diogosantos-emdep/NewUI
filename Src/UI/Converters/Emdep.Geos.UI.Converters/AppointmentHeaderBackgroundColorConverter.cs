using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Emdep.Geos.UI.Converters
{
    public class AppointmentHeaderBackgroundColorConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double TotalDuration = (double)values[0];
            decimal DailyWorkHours = (decimal)values[1];
            if (values[2] == DependencyProperty.UnsetValue || values[3] == DependencyProperty.UnsetValue)
                return new SolidColorBrush();
            SolidColorBrush green = (SolidColorBrush)values[2];
            SolidColorBrush red = (SolidColorBrush)values[3];
            try
            {
                //Shubham[skadam] GEOS2-4087 HRM software close automatically, when i generate weekly reports. 14 12 2022
                if (values.Count() == 6)
                {

                    if (((DateTime)values[4]).Date == DateTime.Now.Date)//shubham[skadam] GEOS2-3751 08 OCT 2022
                    {
                        return new SolidColorBrush();
                    }
                    #region GEOS2-4047
                    //Shubham[skadam] GEOS2-4047 Leave Paternity no se carga correctamente  22 12 2022
                    if (((DateTime)values[4]).Date != DateTime.MinValue && ((DateTime)values[5]).Date != DateTime.MinValue)
                    {
                        TimeSpan Startts = ((DateTime)values[4]).TimeOfDay;
                        TimeSpan Endts = ((DateTime)values[5]).TimeOfDay;
                        var TotalHours = (Endts.Subtract(Startts).TotalHours);
                        if ((decimal)TotalDuration < DailyWorkHours)
                        {
                            if ((decimal)TotalHours < DailyWorkHours)
                            {
                                return red;
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex) { }

            if ((decimal)TotalDuration < DailyWorkHours)
            {
                return red;
            }
            else
            {
                if ((decimal)TotalDuration == 0)
                    return red;
                else
                    return green;
            }
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
