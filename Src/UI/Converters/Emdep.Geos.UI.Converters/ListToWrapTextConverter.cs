using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using Emdep.Geos.Data.Common.Hrm;

namespace Emdep.Geos.UI.Converters
{
    public class ListToWrapTextConverter : MarkupExtension,IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Parameter = parameter as string;

            string result = string.Empty;
            if (Parameter != null)
            {
                if (Parameter.Equals("Departments"))
                {
                    var DeptList = value as List<Department>;
                    result = string.Join("\n", DeptList.Select(x => x.DepartmentName));
                }
                if(Parameter.Equals("CompanyLeave"))
                {
                    List<EmployeeAnnualLeave> _Leave = value as List<EmployeeAnnualLeave>;
                    foreach (var item in _Leave)
                    {
                        result = result + item.CompanyLeave.Name + "\n";
                    }
                }
                if(Parameter.Equals("RegularHoursCount"))
                {
                    List<EmployeeAnnualLeave> _Leave = value as List<EmployeeAnnualLeave>;
                    foreach (var item in _Leave)
                    {
                        if (item.Employee.CompanyShift != null)
                        {
                            if (item.Employee.CompanyShift.CompanyAnnualSchedule != null)
                            {
                                result = result + TotalDaysAndHours(item.RegularHoursCount, item.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount) + "\n";
                            
                            }
                        }
                    }
                }
                if (Parameter.Equals("AdditionalHoursCount"))
                {
                    List<EmployeeAnnualLeave> _Leave = value as List<EmployeeAnnualLeave>;
                    foreach (var item in _Leave)
                    {
                        if (item.Employee.CompanyShift != null)
                        {
                            if (item.Employee.CompanyShift.CompanyAnnualSchedule != null)
                            {
                                result = result + TotalDaysAndHours(item.RegularHoursCount, item.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount) + "\n";
                               
                            }
                        }
                    }
                }
                if (Parameter.Equals("TotalHoursCount"))
                {
                    List<EmployeeAnnualLeave> _Leave = value as List<EmployeeAnnualLeave>;
                    foreach (var item in _Leave)
                    {
                        if (item.Employee.CompanyShift != null)
                        {
                            if (item.Employee.CompanyShift.CompanyAnnualSchedule != null)
                            {
                                result = result + TotalDaysAndHours(item.TotalHoursCount, item.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount) + "\n";
                            }
                        }
                    }
                }
                if (Parameter.Equals("Enjoyed"))
                {
                    List<EmployeeAnnualLeave> _Leave = value as List<EmployeeAnnualLeave>;
                    foreach (var item in _Leave)
                    {
                        if (item.Employee.CompanyShift != null)
                        {
                            if (item.Employee.CompanyShift.CompanyAnnualSchedule != null)
                            {
                                result = result + TotalDaysAndHours(item.Enjoyed, item.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount) + "\n";  
                            }
                        }
                    }
                }
                if (Parameter.Equals("Remaining"))
                {
                    List<EmployeeAnnualLeave> _Leave = value as List<EmployeeAnnualLeave>;
                    foreach (var item in _Leave)
                    {
                        if (item.Employee.CompanyShift != null)
                        {
                            if (item.Employee.CompanyShift.CompanyAnnualSchedule != null)
                            {
                                result = result + TotalDaysAndHours(item.Remaining, item.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount) + "\n";
                            }
                        }
                    }
                }

            }

            return result;
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
    }
}
