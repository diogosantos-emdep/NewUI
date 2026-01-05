using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Emdep.Geos.Data.Common.Hrm;

namespace Emdep.Geos.UI.Converters
{
    public class RemainingLeaveNotificationFlagVisibilityConverter : MarkupExtension, IMultiValueConverter
    {
        //public ResourceDictionary Items { get; set; }
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[0] == null
                || values[1] == DependencyProperty.UnsetValue || values[1] == null)
            {
                return Visibility.Hidden;
            }

            EmployeeAnnualLeave employeeAnnualLeaveData = (EmployeeAnnualLeave)values[0];
            decimal percentage = (decimal) values[1];
            var expectedMaximumHoursInRemaining = employeeAnnualLeaveData.RegularHoursCount / 100 * percentage;
            // IdLeave==171 is Holiday in emdep_geos.lookup_values table
            if (employeeAnnualLeaveData.IdLeave==171 &&
                employeeAnnualLeaveData.Remaining>expectedMaximumHoursInRemaining)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}



