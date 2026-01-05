using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class AppointmentAllDayDateConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Appointment _appointment = (Appointment)value;
            DateTime AllDayEndDate = new DateTime();
            AllDayEndDate = _appointment.End;

            if (_appointment.CustomFields["IsAllDayEvent"]!=null)
            {
                string a = _appointment.CustomFields["IsAllDayEvent"].ToString();
                if (byte.Parse(_appointment.CustomFields["IsAllDayEvent"].ToString()) == 1)
                {
                    AllDayEndDate = AllDayEndDate.AddDays(1);
                }
            }
            return AllDayEndDate;
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
