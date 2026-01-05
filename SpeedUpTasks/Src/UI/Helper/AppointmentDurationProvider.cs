using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Scheduling;
using DevExpress.Xpf.Scheduling.Visual;
using DevExpress.Xpf.Scheduling.VisualData;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class AppointmentDurationProvider : Behavior<FrameworkElement>
    {

        #region TaskLog
        // [HRM-M052-12] (#56890) Ignore non-working days from attendance [adadibathina]
        #endregion

        public static readonly DependencyProperty TotalDurationProperty =
            DependencyProperty.Register("TotalDuration", typeof(double), typeof(AppointmentDurationProvider), new PropertyMetadata(0d));
        public double TotalDuration { get { return (double)GetValue(TotalDurationProperty); }
            set { SetValue(TotalDurationProperty, value); } }

        public static readonly DependencyProperty TotalDurationStringProperty =
            DependencyProperty.Register("TotalDurationString", typeof(string), typeof(AppointmentDurationProvider), new PropertyMetadata("00:00"));
        public string TotalDurationString { get { return (string)GetValue(TotalDurationStringProperty); } set { SetValue(TotalDurationStringProperty, value); } }

        public static readonly DependencyProperty DailyWorkHoursPropety = DependencyProperty.Register("DailyWorkHours", typeof(decimal), typeof(AppointmentDurationProvider));
        public decimal DailyWorkHours
        {
            get { return (decimal)GetValue(DailyWorkHoursPropety); }
            set { SetValue(DailyWorkHoursPropety, value); }
        }

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(bool), typeof(AppointmentDurationProvider));
        public bool Duration
        {
            get { return (bool)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
            AssociatedObject.DataContextChanged += OnDataContextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            UnInit();
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;
            AssociatedObject.DataContextChanged -= OnDataContextChanged;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        void OnUnloaded(object sender, RoutedEventArgs e)
        {
            UnInit();
        }

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Init();

        }

        SchedulerControl scheduler;
        void Init()
        {

            UnInit();
            if (AssociatedObject == null ||
                AssociatedObject.DataContext == null) return;
            scheduler = SchedulerControl.GetScheduler(AssociatedObject);

            if (scheduler == null) return;
            scheduler.ItemPropertyChanged += OnSchedulerItemPropertyChanged;
            scheduler.ItemsCollectionChanged += OnSchedulerItemsCollectionChanged;
            var Count = 0;
            var AppointmentList = (scheduler.AppointmentItems).ToList();
            foreach (var item in AppointmentList)
            {
                if (item.CustomFields["IdEmployeeAttendance"] != null || item.CustomFields["IdEmployeeLeave"] != null)
                {
                    Count++;
                    break;
                }
            }
            if (Count > 0)
            {
                Updatenew();
            }
            else
            {
                Update();
            }

        }

        void UnInit()
        {
            if (scheduler == null) return;
            scheduler.ItemPropertyChanged -= OnSchedulerItemPropertyChanged;
            scheduler.ItemsCollectionChanged -= OnSchedulerItemsCollectionChanged;
            scheduler = null;
        }

        void OnSchedulerItemsCollectionChanged(object sender, ItemsCollectionChangedEventArgs e)
        {
            if (((System.Windows.UIElement)sender).Uid == "Refresh")
            {

                UpdateRefresh();
            }
            else
            {

                Updatenew();
            }

        }

        void OnSchedulerItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            Updatenew();
        }

        /// <summary>
        /// [001][skale][07-08-2019][GEOS2-1694]HRM - Attendance green visualization
        /// </summary>
        void Update()
        {

            int Count = 0;

            if (scheduler.ActiveViewIndex == 0)
            {
                MonthCellViewModel vm = AssociatedObject.DataContext as MonthCellViewModel;

                if (vm == null || scheduler == null)
                {
                    TotalDuration = 0d;
                    return;
                }


                var AppointmentList = (scheduler.AppointmentItems).ToList();
                var apts = AppointmentList.Where(x => x.Start.Date == vm.Date.Date);
               //var apts = scheduler.GetAppointments(vm.Interval);
                var aptsList = apts.Where(x => x.CustomFields["IdEmployeeAttendance"] != null || x.CustomFields["IdEmployeeLeave"] != null).ToList();

                //[001]Added
                if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeContractSituationList != null)
                {
                    bool IsContract = ((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeContractSituationList.Any(x => x.ContractSituationStartDate <= vm.Date.Date && (x.ContractSituationEndDate == null ? ((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate.Value : x.ContractSituationEndDate) >= vm.Date.Date && vm.Date.Date <= GeosApplication.Instance.ServerDateTime.Date);

                    if (IsContract)
                    {
                        if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate != null)
                        {
                            if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate.Value >= vm.Date)
                            {
                                if (!(((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeWorkingDays).Any(x => x.Contains(vm.Date.DayOfWeek.ToString().Substring(0, 3))))
                                {
                                    if (IsWorkedOnWeekends(apts, vm.Date))
                                    {
                                        Duration = true;
                                    }
                                    else
                                    {
                                        Duration = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    Duration = true;
                                }

                            }
                            else
                            {
                                Duration = false;
                                return;
                            }

                        }
                        if (apts.ToList().Count > 0)
                        {
                            foreach (var item in apts)
                            {
                                if (item.CustomFields["IdEmployeeAttendance"] != null)
                                {
                                    Count++;
                                    break;
                                }
                            }
                            if (Count > 0)
                            {
                                Duration = true;
                            }
                            else
                            {
                                Duration = false;
                                return;
                            }
                        }
                    }
                    else
                    {
                        Duration = false;
                        return;
                    }
                }
                //End
                if (!apts.Any())
                {

                    TotalDuration = 0d;
                    TotalDurationString = "00:00";

                    if ((string)scheduler.Tag == "OnSelection")
                    {
                        Duration = true;
                    }
                    else
                    {
                        Duration = false;
                    }


                    return;
                }
                else
                {
                    TotalDurationString = string.Empty;

                }

                TotalDuration = TimeSpan.FromTicks(apts.Sum(x => x.Duration.Ticks)).TotalHours;
                string[] totalHoursSplit = TotalDuration.ToString().Split('.');
                string totalHours = string.Empty;
                if (totalHoursSplit[0].Contains(','))
                {
                    totalHours = totalHoursSplit[0].Substring(0, totalHoursSplit[0].IndexOf(','));
                }
                else
                {
                    totalHours = totalHoursSplit[0];
                }
                TotalDurationString = String.Format("{0}:{1}", totalHours, TimeSpan.FromTicks(aptsList.Sum(x => x.Duration.Ticks)).ToString(@"mm"));

            }
            else
            {
                DateHeaderViewModel wm = AssociatedObject.DataContext as DateHeaderViewModel;

                if (wm == null || scheduler == null)
                {
                    TotalDuration = 0d;
                    return;
                }

                var apts = scheduler.GetAppointments(wm.Interval);
                var aptsList = apts.Where(x => x.CustomFields["IdEmployeeAttendance"] != null || x.CustomFields["IdEmployeeLeave"] != null).ToList();

                //[001]Added
                if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeContractSituationList != null)
                {
                    bool IsContract = ((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeContractSituationList.Any(x => x.ContractSituationStartDate <= wm.Interval.Start && (x.ContractSituationEndDate == null ? ((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate.Value : x.ContractSituationEndDate) >= wm.Interval.Start && wm.Interval.Start <= GeosApplication.Instance.ServerDateTime.Date);

                    if (IsContract)
                    {
                        if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate != null)
                        {
                            if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate.Value >= wm.Interval.Start)
                            {
                                if (!(((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeWorkingDays).Any(x => x.Contains(wm.Interval.Start.DayOfWeek.ToString().Substring(0, 3))))
                                {
                                    if (IsWorkedOnWeekends(apts, wm.Interval.Start.Date))
                                    {
                                        Duration = true;
                                    }
                                    else
                                    {
                                        Duration = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    Duration = true;
                                }
                            }
                            else
                            {
                                Duration = false;
                                return;
                            }

                        }
                        if (apts.ToList().Count > 0)
                        {
                            foreach (var item in apts)
                            {
                                if (item.CustomFields["IdEmployeeAttendance"] != null)
                                {
                                    Count++;
                                    break;
                                }
                            }
                            if (Count > 0)
                            {
                                Duration = true;
                            }
                            else
                            {
                                Duration = false;
                                return;
                            }
                        }
                    }
                    else
                    {
                        Duration = false;
                        return;
                    }
                }
                //End
                if (!apts.Any())
                {

                    TotalDuration = 0d;
                    TotalDurationString = "00:00";

                    if ((string)scheduler.Tag == "OnSelection")
                    {
                        Duration = true;
                    }
                    else
                    {
                        Duration = false;
                    }


                    return;
                }
                else
                {
                    TotalDurationString = string.Empty;
                }


                TotalDuration = TimeSpan.FromTicks(apts.Sum(x => x.Duration.Ticks)).TotalHours;
                string[] totalHoursSplit = TotalDuration.ToString().Split('.');
                string totalHours = string.Empty;
                if (totalHoursSplit[0].Contains(','))
                {
                    totalHours = totalHoursSplit[0].Substring(0, totalHoursSplit[0].IndexOf(','));
                }
                else
                {
                    totalHours = totalHoursSplit[0];
                }
                TotalDurationString = String.Format("{0}:{1}", totalHours, TimeSpan.FromTicks(aptsList.Sum(x => x.Duration.Ticks)).ToString(@"mm"));


            }
        }

        /// <summary>
        /// [001][SP-66][skale][25-06-2019][GEOS2-1605]The GHRM close all windows when change the to week view [in attendance menu]
        /// [002][skale][07-08-2019][GEOS2-1694]HRM - Attendance green visualization
        /// [003][skale][26-08-2019][GEOS2-1640]Wrong total time display in week view [in attendance menu]
        /// </summary>
        void Updatenew()
        {
            Duration = true;
            int Count = 0;

            if (scheduler.ActiveViewIndex == 0)
            {
                MonthCellViewModel vm = AssociatedObject.DataContext as MonthCellViewModel;

                if (vm == null || scheduler == null)
                {
                    TotalDuration = 0d;
                    return;
                }

                var AppointmentList = (scheduler.AppointmentItems).ToList();


               // var apts = AppointmentList.Where(x => x.Start.Date == vm.Date.Date);
                
                //cpatil for task [GEOS2-2593]
                var apts = AppointmentList.Where(x => x.Start.Date == vm.Date.Date || (x.Start.Date <= vm.Date.Date && (x.End.Date > vm.Date.Date || (x.End.Date == vm.Date.Date && x.End > new DateTime(vm.Date.Date.Year, vm.Date.Date.Month, vm.Date.Date.Day,12,0,0)) && x.CustomFields["IdEmployeeLeave"] != null)));
                
                var aptsList = apts.Where(x => x.CustomFields["IdEmployeeAttendance"] != null || x.CustomFields["IdEmployeeLeave"] != null).ToList();                
                List<AppointmentItem> leaveList = new List<AppointmentItem>();
                List<AppointmentItem> appointmentList = apts.Where(x => x.CustomFields["IdEmployeeAttendance"] != null || x.CustomFields["IdEmployeeLeave"] != null).ToList();
                TimeSpan totalMinutes = TimeSpan.FromTicks(aptsList.Sum(x => x.Duration.Ticks));
                //[002]Added
                if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeContractSituationList != null)
                {
                    bool IsContract = ((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeContractSituationList.Any(x => x.ContractSituationStartDate <= vm.Date.Date && (x.ContractSituationEndDate == null ? ((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate.Value : x.ContractSituationEndDate) >= vm.Date.Date && vm.Date <= GeosApplication.Instance.ServerDateTime.Date);

                    if (IsContract)
                    {
                        if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate != null)
                        {
                            if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate.Value >= vm.Date)
                            {
                                //  HRM M052-12(#56890) Ignore non-working days from attendance
                                if (!(((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeWorkingDays).Any(x => x.Contains(vm.Date.DayOfWeek.ToString().Substring(0,3))))
                                {
                                    if (IsWorkedOnWeekends(apts, vm.Date))
                                    {
                                        Duration = true;
                                    }
                                    else
                                    {
                                        Duration = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    Duration = true;
                                }

                            }
                            else
                            {
                                Duration = false;
                                return;
                            }

                        }
                        if (apts.ToList().Count > 0)
                        {
                            foreach (var item in apts)
                            {
                                if (item.CustomFields["IdEmployeeAttendance"] != null || item.CustomFields["IdEmployeeLeave"] != null)
                                {
                                    Count++;
                                    break;
                                }
                            }
                            if (Count > 0)
                                Duration = true;
                            else
                            {
                                Duration = false;
                                return;
                            }

                        }
                    }
                    else
                    {
                        Duration = false;
                        return;
                    }
                }
                //End
                if (!apts.Any())
                {

                    TotalDuration = 0d;
                    TotalDurationString = "00:00";

                    return;
                }
                else
                {
                    TotalDurationString = string.Empty;
                    foreach (var item in apts)
                    {

                        if (item.CustomFields["IdEmployeeAttendance"] != null)
                        {
                            var Hours = item.CustomFields["DailyHoursCount"].ToString();
                            DailyWorkHours = Convert.ToDecimal(Hours);

                            break;
                        }
                        else if (item.CustomFields["IdEmployeeLeave"] != null)
                        {
                            var Hours = item.CustomFields["DailyHoursCount"].ToString();
                            DailyWorkHours = Convert.ToDecimal(Hours);

                            break;
                        }
                    }


                }
                TotalDuration = TimeSpan.FromTicks(aptsList.Sum(x => x.Duration.Ticks)).TotalHours;

                foreach (var item in aptsList)
                {
                    if (item.CustomFields["IdEmployeeAttendance"] == null && item.CustomFields["IdEmployeeLeave"] != null)
                    {
                        leaveList.Add(item);
                        var _total = TimeSpan.FromTicks(leaveList.Sum(x => x.Duration.Ticks)).TotalHours;
                        var dailyWorkHoursCount = item.CustomFields["DailyHoursCount"];
                        if (_total > Convert.ToDouble(dailyWorkHoursCount))
                        {
                            TotalDuration = TotalDuration - _total + Convert.ToDouble(dailyWorkHoursCount);
                            TimeSpan interval = TimeSpan.FromHours(TotalDuration);
                            appointmentList.Remove(item);
                            totalMinutes = TimeSpan.FromTicks(appointmentList.Sum(x => x.Duration.Ticks)) + interval;
                        }

                    }

                }

                string[] totalHoursSplit = TotalDuration.ToString().Split('.');
                string totalHours = string.Empty;

                if (totalHoursSplit[0].Contains(','))
                {
                    totalHours = totalHoursSplit[0].Substring(0, totalHoursSplit[0].IndexOf(','));
                }
                else
                {
                    totalHours = totalHoursSplit[0];
                }

                TotalDurationString = String.Format("{0}:{1}", totalHours, totalMinutes.ToString(@"mm"));
            }
            else
            {
                DateHeaderViewModel wm = AssociatedObject.DataContext as DateHeaderViewModel;
                //[001] added
                if (wm == null || scheduler == null)
                {
                    TotalDuration = 0d;
                    return;
                }

                //old code
                //var apts = scheduler.GetAppointments(wm.Interval);
                //var aptsList = apts.Where(x => x.CustomFields["IdEmployeeAttendance"] != null || x.CustomFields["IdEmployeeLeave"] != null).ToList();
                //[003] added 
                var AppointmentList = (scheduler.AppointmentItems).ToList();
                var apts = AppointmentList.Where(x => x.Start.Date == wm.Interval.Start.Date);
                var aptsList = apts.Where(x => x.CustomFields["IdEmployeeAttendance"] != null || x.CustomFields["IdEmployeeLeave"] != null).ToList();

                List<AppointmentItem> leaveList = new List<AppointmentItem>();
                List<AppointmentItem> appointmentList = apts.Where(x => x.CustomFields["IdEmployeeAttendance"] != null || x.CustomFields["IdEmployeeLeave"] != null).ToList();
                TimeSpan totalMinutes = TimeSpan.FromTicks(aptsList.Sum(x => x.Duration.Ticks));
                //[002]Added
                if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeContractSituationList != null)
                {
                    bool IsContract = ((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeContractSituationList.Any(x => x.ContractSituationStartDate <= wm.Interval.Start && (x.ContractSituationEndDate == null ? ((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate.Value : x.ContractSituationEndDate) >= wm.Interval.Start && wm.Interval.Start <= GeosApplication.Instance.ServerDateTime.Date);

                    if (IsContract)
                    {
                        if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate != null)
                        {
                            if (((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeEndDate.Value >= wm.Interval.Start)
                            {
                                //[001] added
                                if (!(((Emdep.Geos.UI.Helper.SchedulerControlEx)scheduler).SelectedEmployeeWorkingDays).Any(x => x.Contains(wm.Interval.Start.DayOfWeek.ToString().Substring(0, 3))))
                                {
                                    if (IsWorkedOnWeekends(apts, wm.Interval.Start.Date))
                                    {
                                        Duration = true;
                                    }
                                    else
                                    {
                                        Duration = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    Duration = true;
                                }
                                //end
                            }
                            else
                            {
                                Duration = false;
                                return;
                            }
                        }

                        if (apts.ToList().Count > 0)
                        {
                            foreach (var item in apts)
                            {
                                if (item.CustomFields["IdEmployeeAttendance"] != null || item.CustomFields["IdEmployeeLeave"] != null)
                                {
                                    Count++;
                                    break;
                                }
                            }
                            if (Count > 0)
                                Duration = true;
                            else
                            {
                                Duration = false;
                                return;
                            }

                        }

                    }
                    else
                    {
                        Duration = false;
                        return;
                    }
                }

              //end

                if (!apts.Any())
                {
                    TotalDuration = 0d;
                    TotalDurationString = "00:00";
                    return;
                }
                else
                {
                    TotalDurationString = string.Empty;
                    foreach (var item in apts)
                    {

                        if (item.CustomFields["IdEmployeeAttendance"] != null)
                        {
                            var Hours = item.CustomFields["DailyHoursCount"].ToString();
                            DailyWorkHours = Convert.ToDecimal(Hours);

                            break;
                        }
                    }
                }
                TotalDuration = TimeSpan.FromTicks(aptsList.Sum(x => x.Duration.Ticks)).TotalHours;
                foreach (var item in aptsList)
                {
                    if (item.CustomFields["IdEmployeeAttendance"] == null && item.CustomFields["IdEmployeeLeave"] != null)
                    {
                        leaveList.Add(item);
                        var _total = TimeSpan.FromTicks(leaveList.Sum(x => x.Duration.Ticks)).TotalHours;
                        var dailyWorkHoursCount = item.CustomFields["DailyHoursCount"];
                        if (_total > Convert.ToDouble(dailyWorkHoursCount))
                        {
                            TotalDuration = TotalDuration - _total + Convert.ToDouble(dailyWorkHoursCount);
                            TimeSpan interval = TimeSpan.FromHours(TotalDuration);
                            appointmentList.Remove(item);
                            totalMinutes = TimeSpan.FromTicks(appointmentList.Sum(x => x.Duration.Ticks)) + interval;
                        }
                    }
                }

                string[] totalHoursSplit = TotalDuration.ToString().Split('.');
                string totalHours = string.Empty;
                if (totalHoursSplit[0].Contains(','))
                {
                    totalHours = totalHoursSplit[0].Substring(0, totalHoursSplit[0].IndexOf(','));
                }
                else
                {
                    totalHours = totalHoursSplit[0];
                }
                TotalDurationString = String.Format("{0}:{1}", totalHours, totalMinutes.ToString(@"mm"));
            }

        }

        /// <summary>
        /// [HRM-M052-12]
        /// to check wether the emp working in sat or sunday
        /// </summary>
        /// <param name="apts"></param>
        /// <param name="WorkedDate"></param>
        /// <returns></returns>

        private bool IsWorkedOnWeekends(IEnumerable<AppointmentItem> apts, DateTime WorkedDate)
        {
            try
            {
                if (!apts.Any())
                {
                    return false;
                }
                else
                {
                    var WorkedDay = apts.FirstOrDefault(x => x.Start.Date == WorkedDate.Date);
                    if (WorkedDay != null)
                    {
                        if (WorkedDay.CustomFields[1] != null)
                        {
                            var i = Convert.ToDecimal(WorkedDay.CustomFields[0].ToString());
                            if (Convert.ToDecimal(WorkedDay.CustomFields[0].ToString()) > 0)
                            {
                                return true;
                            }
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //  GeosApplication.Instance.Logger.Log("Method IsWorkedOnWeekends()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            return false;
        }
        void UpdateRefresh()
        {
            Duration = false;

            if (scheduler.ActiveViewIndex == 0)
            {
                MonthCellViewModel vm = AssociatedObject.DataContext as MonthCellViewModel;
                int Count = 0;
                if (vm == null || scheduler == null)
                {
                    TotalDuration = 0d;
                    return;
                }

                var apts = scheduler.GetAppointments(vm.Interval);
                var aptsList = apts.Where(x => x.CustomFields["IdEmployeeAttendance"] != null || x.CustomFields["IdEmployeeLeave"] != null).ToList();

                if (!apts.Any())
                {
                    TotalDuration += 0d;
                    TotalDurationString = "00:00";

                    return;
                }
                else
                {
                    TotalDurationString = string.Empty;

                    if (apts.ToList().Count > 0)
                    {
                        foreach (var item in apts)
                        {
                            if (item.CustomFields["IdEmployeeAttendance"] == null)
                            {
                                Count = Count + 1;
                            }
                        }
                        if (Count == apts.ToList().Count)
                            Duration = false;
                        else
                            Duration = true;
                    }

                }

                TotalDuration = TimeSpan.FromTicks(apts.Sum(x => x.Duration.Ticks)).TotalHours;
                string[] totalHoursSplit = TotalDuration.ToString().Split('.');
                string totalHours = string.Empty;
                if (totalHoursSplit[0].Contains(','))
                {
                    totalHours = totalHoursSplit[0].Substring(0, totalHoursSplit[0].IndexOf(','));
                }
                else
                {
                    totalHours = totalHoursSplit[0];
                }
                TotalDurationString = String.Format("{0}:{1}", totalHours, TimeSpan.FromTicks(aptsList.Sum(x => x.Duration.Ticks)).ToString(@"mm"));


            }
            else
            {
                DateHeaderViewModel wm = AssociatedObject.DataContext as DateHeaderViewModel;
                int Count = 0;
                if (wm == null || scheduler == null)
                {
                    TotalDuration = 0d;
                    return;
                }

                var apts = scheduler.GetAppointments(wm.Interval);
                var aptsList = apts.Where(x => x.CustomFields["IdEmployeeAttendance"] != null || x.CustomFields["IdEmployeeLeave"] != null).ToList();

                if (!apts.Any())
                {
                    TotalDuration += 0d;
                    TotalDurationString = "00:00";

                    return;
                }
                else
                {
                    TotalDurationString = string.Empty;

                    if (apts.ToList().Count > 0)
                    {
                        foreach (var item in apts)
                        {
                            if (item.CustomFields["IdEmployeeAttendance"] == null)
                            {
                                Count = Count + 1;
                            }
                        }
                        if (Count == apts.ToList().Count)
                            Duration = false;
                        else
                            Duration = true;
                    }

                }

                TotalDuration = TimeSpan.FromTicks(apts.Sum(x => x.Duration.Ticks)).TotalHours;
                string[] totalHoursSplit = TotalDuration.ToString().Split('.');
                string totalHours = string.Empty;
                if (totalHoursSplit[0].Contains(','))
                {
                    totalHours = totalHoursSplit[0].Substring(0, totalHoursSplit[0].IndexOf(','));
                }
                else
                {
                    totalHours = totalHoursSplit[0];
                }
                TotalDurationString = String.Format("{0}:{1}", totalHours, TimeSpan.FromTicks(aptsList.Sum(x => x.Duration.Ticks)).ToString(@"mm"));

            }
        }
    }

    public class DurationPresenter : FastTextBlock
    {
        public DurationPresenter()
        {
            DataContextChanged += OnDataContextChanged;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            Init();
        }
        void OnUnloaded(object sender, RoutedEventArgs e)
        {
            UnInit();
        }
        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Init();
        }
        SchedulerControl scheduler;

        void Init()
        {
            UnInit();
            if (DataContext == null) return;
            scheduler = SchedulerControl.GetScheduler(this);
            if (scheduler == null) return;
            scheduler.ItemPropertyChanged += OnSchedulerItemPropertyChanged;
            scheduler.ItemsCollectionChanged += OnSchedulerItemsCollectionChanged;
            Update();
        }
        void UnInit()
        {
            if (scheduler == null) return;
            scheduler.ItemPropertyChanged -= OnSchedulerItemPropertyChanged;
            scheduler.ItemsCollectionChanged -= OnSchedulerItemsCollectionChanged;
            scheduler = null;
        }
        void OnSchedulerItemsCollectionChanged(object sender, ItemsCollectionChangedEventArgs e)
        {
            Update();
        }
        void OnSchedulerItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            Update();
        }
        void Update()
        {
            MonthCellViewModel vm = DataContext as MonthCellViewModel;
            if (vm == null || scheduler == null)
            {
                Text = null;
                return;
            }
            var apts = scheduler.GetAppointments(vm.Interval);
            if (!apts.Any())
            {
                Text = null;
                return;
            }
            Text = TimeSpan.FromTicks(apts.Sum(x => x.Duration.Ticks)).TotalHours.ToString();
        }
    }
}
