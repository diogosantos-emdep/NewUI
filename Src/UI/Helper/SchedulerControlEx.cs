using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Xpf.Scheduling;
using System.Windows;
using Emdep.Geos.Data.Common.Hrm;

namespace Emdep.Geos.UI.Helper
{
    public class SchedulerControlEx : SchedulerControl
    {


        #region TaskLog
        // [000][skale][07-08-2019][GEOS2-1694]HRM - Attendance green visualization
        #endregion

        public static readonly DependencyProperty MonthProperty =
            DependencyProperty.Register("Month", typeof(DateTime?), typeof(SchedulerControlEx), new PropertyMetadata(OnMonthPropertyChanged));


        public static readonly DependencyProperty DisplayProperty =
        DependencyProperty.Register("DisplayName", typeof(string), typeof(SchedulerControlEx), new PropertyMetadata(OnDisplayPropertyChanged));

        public static readonly DependencyProperty SelectedEmployeeHireDateProperty =
        DependencyProperty.Register("SelectedEmployeeHireDateProperty", typeof(DateTime?), typeof(SchedulerControlEx), new PropertyMetadata(OnSelectedEmployeeHireDatePropertyChanged));

        public static readonly DependencyProperty SelectedEmployeeEndDateProperty =
        DependencyProperty.Register("SelectedEmployeeEndDateProperty", typeof(DateTime?), typeof(SchedulerControlEx), new PropertyMetadata(OnSelectedEmployeeEndDatePropertyChanged));

        public static readonly DependencyProperty SelectedEmployeeWorkingDaysProperty =
        DependencyProperty.Register("SelectedEmployeeWorkingDays", typeof(string[]), typeof(SchedulerControlEx), new PropertyMetadata(OnSelectedEmployeeWorkingDaysPropertyChanged));
        //[000] Added
        public static readonly DependencyProperty selectedEmployeeContractSituationList =
        DependencyProperty.Register("selectedEmployeeContractSituationList", typeof(List<EmployeeContractSituation>), typeof(SchedulerControlEx), new PropertyMetadata(OnSelectedEmployeeContractSituationListChanged));

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
        public DateTime? SelectedEmployeeHireDate
        {
            get { return (DateTime?)GetValue(SelectedEmployeeHireDateProperty); }
            set { SetValue(SelectedEmployeeHireDateProperty, value); }
        }

        public string[] SelectedEmployeeWorkingDays
        {
            get { return (string[])GetValue(SelectedEmployeeWorkingDaysProperty); }
            set { SetValue(SelectedEmployeeWorkingDaysProperty, value); }
        }
        public DateTime? SelectedEmployeeEndDate
        {
            get { return (DateTime?)GetValue(SelectedEmployeeEndDateProperty); }
            set { SetValue(SelectedEmployeeEndDateProperty, value); }
        }
        private static void OnMonthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SchedulerControlEx)d).OnMonthChanged();
        }
        private static void OnDisplayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SchedulerControlEx)d).OnDisplayNameChanged();
        }
        private static void OnSelectedEmployeeHireDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SchedulerControlEx)d).OnSelectedEmployeeHireDateChanged();
        }
        //[000]added
        public List<EmployeeContractSituation> SelectedEmployeeContractSituationList
        {
            get { return (List<EmployeeContractSituation>)GetValue(selectedEmployeeContractSituationList); }
            set { SetValue(selectedEmployeeContractSituationList, value); }
        }
        private static void OnSelectedEmployeeContractSituationListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SchedulerControlEx)d).OnSelectedEmployeeContractSituationListChanged();
        }
        private void OnSelectedEmployeeContractSituationListChanged()
        {
          
        }
        //end
        private static void OnSelectedEmployeeEndDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SchedulerControlEx)d).OnSelectedEmployeeEndDateChanged();
        }

        private static void OnSelectedEmployeeWorkingDaysPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SchedulerControlEx)d).OnSelectedEmployeeWorkingDaysChanged();
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
            //[rdixit][GEOS2-7018][16.05.2025]
            if (Month.HasValue)
            {
                //Start = Month.Value;
                var newStart = new DateTime(Month.Value.Year, Month.Value.Month, 1);
                Start = newStart;
            }
        }

        private void OnDisplayNameChanged()
        {

        }
        private void OnSelectedEmployeeHireDateChanged()
        {

        }
        private void OnSelectedEmployeeEndDateChanged()
        {

        }
        private void OnSelectedEmployeeWorkingDaysChanged()
        {

        }
    }

}
