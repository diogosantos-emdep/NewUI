using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.UI.Helper
{
     public static class CustomCommands
    {
        public static ICommand Back = new DelegateCommand<SchedulerControlEx>(BackExecute, CanNavigate);
        public static ICommand Forw = new DelegateCommand<SchedulerControlEx>(ForwExecute, CanNavigate);
        static void BackExecute(SchedulerControlEx scheduler)
        {
            Navigate(scheduler, -1);
        }
        static void ForwExecute(SchedulerControlEx scheduler)
        {
            Navigate(scheduler, 1);
        }
        static void Navigate(SchedulerControlEx scheduler, int count)
        {
            if (scheduler.ActiveViewIndex == 0)
            {
                scheduler.Month = scheduler.Month.Value.AddMonths(count);
            }
            else if (scheduler.ActiveViewIndex == 1)
            {
                scheduler.Start = scheduler.Start.AddDays(7 * count);
            }
            else if (scheduler.ActiveViewIndex == 2)
            {
                scheduler.Start = scheduler.Start.AddDays(count);
            }
        }
        static bool CanNavigate(SchedulerControlEx scheduler)
        {
            return scheduler != null && scheduler.Month.HasValue;
        }
    }
}
