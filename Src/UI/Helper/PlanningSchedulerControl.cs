using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Xpf.Scheduling;
using System.Windows;
using Emdep.Geos.Data.Common.ERM;
using System;

namespace Emdep.Geos.UI.Helper
{
    public class PlanningSchedulerControl:SchedulerControl
    {

        public static readonly DependencyProperty MonthProperty =
            DependencyProperty.Register("Month", typeof(DateTime?), typeof(PlanningSchedulerControl), new PropertyMetadata(OnMonthPropertyChanged));
        public static readonly DependencyProperty DisplayProperty =
       DependencyProperty.Register("DisplayName", typeof(string), typeof(PlanningSchedulerControl), new PropertyMetadata(OnDisplayPropertyChanged));

        public string DisplayName
        {
            get { return (string)GetValue(DisplayProperty); }
            set { SetValue(DisplayProperty, value); }
        }

        public DateTime? Month
        {
            get { return (DateTime?)GetValue(MonthProperty); }
            set { SetValue(MonthProperty, value); }
        }
        private static void OnMonthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlanningSchedulerControl)d).OnMonthChanged();
        }
        private static void OnDisplayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlanningSchedulerControl)d).OnDisplayNameChanged();
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
        }
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            e.Handled = true;
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            switch (e.Key)
            {
                case Key.Tab:
                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                case Key.PageDown:
                case Key.PageUp:
                case Key.Home:
                case Key.End:
                    e.Handled = true;
                    break;
            }
        }
        protected virtual void OnMonthChanged()
        {
            if (Month.HasValue)
                Start = Month.Value;
        }

        private void OnDisplayNameChanged()
        {

        }
    }
}
