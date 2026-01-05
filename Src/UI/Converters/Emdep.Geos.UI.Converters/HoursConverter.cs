using Emdep.Geos.Data.Common.Hrm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class HoursConverter : MarkupExtension, IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            List<EmpLeaves> LeavesList = new List<EmpLeaves>();
            EmpLeaves Leave = new EmpLeaves();
            var _Leave = value as List<EmployeeAnnualLeave>;
         
            foreach(var item in _Leave)
            {
                Leave = new EmpLeaves();
                if(item.Employee.CompanyShift != null)
                {
                    if(item.Employee.CompanyShift.CompanyAnnualSchedule!=null)
                    {
                        decimal DailyHours = item.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                        decimal RegularHoursCount = item.RegularHoursCount;
                        decimal AdditionalHoursCount = item.AdditionalHoursCount;
                        decimal Total = item.RegularHoursCount + item.AdditionalHoursCount;
                        decimal Enjoyed = item.Enjoyed;
                        decimal Remaining = item.Remaining;
                        Leave.IdLeave = item.IdLeave;
                        Leave.RegularHoursCount = TotalDaysAndHours(RegularHoursCount, DailyHours);
                        Leave.AdditionalHoursCount = TotalDaysAndHours(AdditionalHoursCount, DailyHours);
                        Leave.Total = TotalDaysAndHours(Total, DailyHours);
                        Leave.Enjoyed = TotalDaysAndHours(Enjoyed, DailyHours);
                        Leave.Remaining = TotalDaysAndHours(Remaining, DailyHours);
                        LeavesList.Add(Leave);
                    }
                }
                
            }
            
            return LeavesList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        string TotalDaysAndHours(decimal hours, decimal dailyWorkHours)
        {
            decimal TotalHours = hours;
            decimal DailyHours = dailyWorkHours;
            if (TotalHours == 0)
            {
                return TotalHours.ToString() + "d";
            }

            decimal Days = TotalHours / DailyHours;
            decimal Hours = TotalHours % DailyHours;

            if (Hours == 0)
                return Days.ToString() + "d";

            return string.Format("{0}d {1}H", (Int32)Days, Hours.ToString("0.##"));
        }
        class EmpLeaves
        {
            public long IdLeave
            {
                get;
                set;
            }
            public string AdditionalHoursCount
            {
                get;
                set;
            }
            public string RegularHoursCount
            {
                get;
                set;
            }
            public string Total
            {
                get;
                set;
            }
            public string Enjoyed
            {
                get;
                set;
            }
            public string Remaining
            {
                get;
                set;
            }
        }
    }
}
