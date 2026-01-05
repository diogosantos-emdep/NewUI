using DevExpress.Xpf.Scheduling;
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
    public class AppointmentSubjectAndDurationConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string appointmentSubject = string.Empty;
            if (value != null)
            {
                AppointmentItem _appointment = (AppointmentItem)value;
                if (_appointment.CustomFields["IdEmployeeAttendance"] != null)
                {
                    appointmentSubject = string.Format("[{0:hh\\:mm}] {1}", _appointment.Duration, _appointment.Subject);
                }
                else if (_appointment.CustomFields["IdEmployeeLeave"] != null)
                {
                    decimal Hours;
                    TimeSpan totalDuration;
                    string Minutes = _appointment.Duration.Minutes.ToString("00");
                    if (_appointment.Duration.Days > 0)
                    {
                        Hours = (decimal)_appointment.CustomFields["DailyHoursCount"];
                        totalDuration = TimeSpan.FromHours((double)Hours);
                    }
                    else if (_appointment.Duration.TotalHours > (double)((decimal)_appointment.CustomFields["DailyHoursCount"]))
                    {
                        Hours = (decimal)_appointment.CustomFields["DailyHoursCount"];
                        totalDuration = TimeSpan.FromHours((double)Hours);
                    }
                    else
                    {
                        totalDuration = TimeSpan.FromHours(_appointment.Duration.TotalHours);
                    }

                    appointmentSubject = string.Format("[{0:hh\\:mm}] {1}", totalDuration, _appointment.Subject);
                }
                else
                {
                    appointmentSubject = _appointment.Subject;
                }
            }
            return appointmentSubject;
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
