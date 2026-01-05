using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Emdep.Geos.UI.Helper
{
   public class GridTimerBehavior : Behavior<GridControlWithoutClickEx>
    {

        #region Task Log
        //[000][skale][11-12-2019][GEOS2-1881] Add new option to Log the worked time in an OT
        #endregion

        DispatcherTimer timer = new DispatcherTimer();

        public GridTimerBehavior()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            AssociatedObject.RefreshData();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.CustomUnboundColumnData += AssociatedObject_CustomUnboundColumnData;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.CustomUnboundColumnData -= AssociatedObject_CustomUnboundColumnData;
        }

        private void AssociatedObject_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.IsGetData && e.Column.FieldName == "Timer")
            {
                OTWorkingTime otWorkingTime = ((OTWorkingTime)AssociatedObject.GetRowByListIndex(e.ListSourceRowIndex));


                if (otWorkingTime.IsTimerStarted)
                {
                    otWorkingTime.WorkLogProcess = OTWorkingTime.Process.Start;

                    if (otWorkingTime.StartTime == null)
                    {
                        otWorkingTime.StartTime = DateTime.Now;
                    }
                    else
                    {
                        TimeSpan totalTime = new TimeSpan();
                        if (otWorkingTime.EndTime == null)
                        {
                            otWorkingTime.EndTime = DateTime.Now;
                            totalTime = otWorkingTime.EndTime.Value - otWorkingTime.StartTime.Value;
                            otWorkingTime.TotalTime = otWorkingTime.TotalTime + totalTime;
                        }
                        else
                        {
                            DateTime? tempstartTime = new DateTime();
                            tempstartTime = otWorkingTime.EndTime;
                            otWorkingTime.EndTime = DateTime.Now;
                            totalTime = otWorkingTime.EndTime.Value - tempstartTime.Value;
                            otWorkingTime.TotalTime = otWorkingTime.TotalTime + totalTime;
                        }
                        otWorkingTime.TotalTimeInString = string.Format("{0:00}:{1:00}", (Int32)otWorkingTime.TotalTime.TotalHours, otWorkingTime.TotalTime.Minutes);
                       // otWorkingTime.TotalTimeInString = otWorkingTime.TotalTime.ToString(@"hh\:mm");
                    }
                }
                else
                {
                    otWorkingTime.WorkLogProcess = OTWorkingTime.Process.Stop;
                }
            }
        }

    }
}
