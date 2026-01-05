using DevExpress.Mvvm;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.UI.Helper
{
    public class CustomAttendanceCommand
    {
        public static ICommand Back = new DelegateCommand<SchedulerControlEx>(BackExecute, CanNavigate);
        public static ICommand Forw = new DelegateCommand<SchedulerControlEx>(ForwExecute, CanNavigate);
        static void BackExecute(SchedulerControlEx scheduler)
        {
            Navigate(scheduler, -1);
            // [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
        }
        static void ForwExecute(SchedulerControlEx scheduler)
        {
            Navigate(scheduler, 1);
        }
        static void Navigate(SchedulerControlEx scheduler, int count)
        {
            if (scheduler.ActiveViewIndex == 0)
            {
                DateTime dateTimeNew = scheduler.Month.Value.AddMonths(count);
                GeosApplication.Instance.SelectedHRMAttendanceDate = GeosApplication.Instance.SelectedHRMAttendanceDate.AddMonths(count);
                if (dateTimeNew >= scheduler.LimitInterval.Start &&
                    dateTimeNew <= scheduler.LimitInterval.End)
                {
                    scheduler.Uid = count.ToString();
                    scheduler.Month = dateTimeNew;
                }
            }
            else if (scheduler.ActiveViewIndex == 1)
            {
                GeosApplication.Instance.SelectedHRMAttendanceDate = GeosApplication.Instance.SelectedHRMAttendanceDate.AddDays(7 * count);
                DateTime dateTimeNew = scheduler.Start.AddDays(7 * count);
                if (dateTimeNew >= scheduler.LimitInterval.Start &&
                    dateTimeNew <= scheduler.LimitInterval.End)
                {
                    scheduler.Start = dateTimeNew;
                }
            }
            else if (scheduler.ActiveViewIndex == 2)
            {
                GeosApplication.Instance.SelectedHRMAttendanceDate = GeosApplication.Instance.SelectedHRMAttendanceDate.AddDays(count);
                scheduler.Start = scheduler.Start.AddDays(count);
            }
        }
        static bool CanNavigate(SchedulerControlEx scheduler)
        {
            return scheduler != null && scheduler.Month.HasValue;
        }
    }
}
