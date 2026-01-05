using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.XtraScheduler;

namespace Emdep.Geos.UI.Helper
{
   public class DisplayWeekInTimelineSchedulerHelper : TimeScaleWeek
    {
        public override string FormatCaption(DateTime start, DateTime end)
        {
            string dateString1 = "CW0{0}";
            string dateString2 = "CW{0}";
            if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(start) < 10)
            {
                return String.Format(dateString1, DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(start),
                                     DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(end.AddTicks(-1)));
            }
            else
            {
                return String.Format(dateString2, DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(start),
                                    DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(end.AddTicks(-1)));
            }
        }


    }
}
