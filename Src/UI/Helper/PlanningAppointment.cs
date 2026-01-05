using DevExpress.XtraScheduler.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Utils;
using System.Drawing;
using System.Windows;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Scheduling;
using DevExpress.Xpf.Scheduling.VisualData;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Scheduling.Visual;

namespace Emdep.Geos.UI.Helper
{
    public class PlanningAppointment : Behavior<FrameworkElement>
    {

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? QueryStart { get; set; }
        public DateTime? QueryEnd { get; set; }
        public bool? AllDay { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int? Label { get; set; }
        public int? ResourceID { get; set; }
        public string ResourceIDs { get; set; }


        public DateTime? DeliveryDate { get; set; }
        public string OTCode { get; set; }

        public string Template { get; set; }
        public Int64? NumItem { get; set; }
        public int? QTY { get; set; }

        public string Type { get; set; }

        public string OriginPlant { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string ContentSubject { get; set; }
        public bool IsPlannedDeliveryDate { get; set; }

        public LabelHelper LabelHelper { get; set; }

        public int? Types { get; set; }
        public DateTime? PlanningDeliveryDate { get; set; }
        //public string RecurrenceInfo { get; set; }

        public Visibility IsHolidayData { get; set; }

        public Visibility IsHolidayDate { get; set; }

        public UInt32 IdOT { get; set; }
        public UInt32 IdCounterpart { get; set; }
        public Int32 IdSite { get; set; }//[GEOS2-5319][gulab lakade][15 11 2024]

        #region [Rupali Sarode][GEOS2-4347][05-05-2023]
        public string Customer { get; set; }
        public string Project { get; set; }


        #endregion

        public static bool isSaveButtonEnabled;

        public static bool IsSaveButtonEnabled
        {
            get
            {
                return isSaveButtonEnabled;
            }
            set
            {
                isSaveButtonEnabled = value;
            }
        }

        public string CurrentWorkStation { get; set; } //[pallavi jadhav] [GEOS2-4481] [26 05 2023] 



        public string Real { get; set; }

        public string Expected { get; set; }

        #region //[pallavi jadhav] [GEOS2-4519] [06 06 2023] 

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(bool), typeof(PlanningAppointment));
        public bool Duration
        {
            get { return (bool)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        public static readonly DependencyProperty TotalQTYProperty =
            DependencyProperty.Register("TotalQTY", typeof(int), typeof(PlanningAppointment), new PropertyMetadata(0));

        public int TotalQTY
        {
            get { return (int)GetValue(TotalQTYProperty); }
            set { SetValue(TotalQTYProperty, value); }
        }
        public static readonly DependencyProperty TotalExpectedTimeStringProperty =
           DependencyProperty.Register("TotalExpectedTimeString", typeof(string), typeof(PlanningAppointment));

        public string TotalExpectedTimeString { get { return (string)GetValue(TotalExpectedTimeStringProperty); } set { SetValue(TotalExpectedTimeStringProperty, value); } }

        public static readonly DependencyProperty TotalRealTimePropety =
           DependencyProperty.Register("TotalRealTime", typeof(string), typeof(PlanningAppointment));
        //  public static readonly DependencyProperty TotalRealTimePropety = DependencyProperty.Register("TotalRealTime", typeof(decimal), typeof(PlanningAppointment));
        public static readonly DependencyProperty TotalQTYStringProperty =
            DependencyProperty.Register("TotalQTYTimeString", typeof(string), typeof(PlanningAppointment));

        public string TotalQTYTimeString { get { return (string)GetValue(TotalQTYStringProperty); } set { SetValue(TotalQTYStringProperty, value); } }


        public string TotalRealTime
        {
            get { return (string)GetValue(TotalRealTimePropety); }
            set { SetValue(TotalRealTimePropety, value); }
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
                if (item.CustomFields["OTCode"] != null)
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

                // Updatenew();
                Update();
            }

        }

        void OnSchedulerItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            Updatenew();
        }

        void UpdateRefresh()
        {
            try
            {
                Duration = false;

                if (scheduler.ActiveViewIndex == 0)
                {
                    MonthCellViewModel vm = AssociatedObject.DataContext as MonthCellViewModel;
                    int Count = 0;
                    if (vm == null || scheduler == null)
                    {
                        TotalQTY = 00;
                        TotalQTYTimeString = string.Empty;
                        TotalExpectedTimeString = string.Empty;
                        TotalRealTime = string.Empty;
                        return;
                    }

                    var apts = scheduler.GetAppointments(vm.Interval);
                    var aptsList = apts.Where(x => x.CustomFields["OTCode"] != null).ToList();

                    if (!apts.Any())
                    {
                        TotalQTY += 0;
                        TotalQTYTimeString = string.Empty;
                        TotalExpectedTimeString = string.Empty;
                        TotalRealTime = string.Empty;
                        return;
                    }
                    else
                    {
                        // TotalExpectedTimeString = null;

                        if (apts.ToList().Count > 0)
                        {
                            TotalQTY = 00;
                            TotalQTYTimeString = string.Empty;
                            TotalExpectedTimeString = string.Empty;
                            TotalRealTime = string.Empty;
                            List<string> TotalReal = new List<string>();
                            List<string> TotalExpected = new List<string>();
                            foreach (var item in apts)
                            {
                                //  TotalQTY = Convert.ToInt32(((Emdep.Geos.UI.Helper.PlanningAppointment)item.SourceObject).QTY);//[002]
                                if (item.CustomFields["IdOT"] != null)
                                {
                                    TotalQTY += Convert.ToInt32(((Emdep.Geos.UI.Helper.PlanningAppointment)item.SourceObject).QTY.Value);
                                    TotalQTYTimeString = Convert.ToString(TotalQTY);
                                    string TotalExpectedTimeStrings = Convert.ToString(((PlanningAppointment)item.SourceObject).Expected);
                                    if (TotalExpectedTimeStrings != null)
                                        TotalExpected.Add(TotalExpectedTimeStrings);


                                    string TotalRealTimes = ((PlanningAppointment)item.SourceObject).Real;
                                    if (TotalRealTimes != null)
                                        TotalReal.Add(TotalRealTimes);


                                    if (item.CustomFields["IdOT"] == null)
                                    {
                                        Count = Count + 1;
                                    }
                                }
                            }
                            if (TotalExpected != null)
                            {

                                TimeSpan sum = TotalExpected.Select(timeString => TimeSpan.Parse(timeString)).Aggregate(TimeSpan.Zero, (currentSum, timeSpan) => currentSum + timeSpan);
                                string formattedTotal = sum.ToString(@"hh\:mm").TrimEnd('0').TrimEnd('.');
                                TotalExpectedTimeString = sum.ToString();
                            }
                            if (TotalReal != null)
                            {
                                TimeSpan sum = TotalReal.Select(timeString => TimeSpan.Parse(timeString)).Aggregate(TimeSpan.Zero, (currentSum, timeSpan) => currentSum + timeSpan);
                                string formattedTotals = sum.ToString(@"hh\:mm").TrimEnd('0').TrimEnd('.');
                                TotalRealTime = sum.ToString();
                            }

                            if (Count == apts.ToList().Count)
                                Duration = false;
                            else
                                Duration = true;
                        }

                    }




                }
                else
                {
                    DateHeaderViewModel wm = AssociatedObject.DataContext as DateHeaderViewModel;
                    int Count = 0;
                    if (wm == null || scheduler == null)
                    {
                        TotalQTY = 00;
                        return;
                    }

                    var apts = scheduler.GetAppointments(wm.Interval);
                    var aptsList = apts.Where(x => x.CustomFields["IdOT"] != null).ToList();

                    if (!apts.Any())
                    {
                        TotalQTY += 0;
                        //TotalExpectedTimeString = "00:00";
                        //TotalRealTime = "00:00";

                        return;
                    }
                    else
                    {
                        // TotalExpectedTimeString = string.Empty;

                        if (apts.ToList().Count > 0)
                        {
                            TotalQTY = 00;
                            TotalQTYTimeString = string.Empty;
                            TotalExpectedTimeString = string.Empty;
                            TotalRealTime = string.Empty;
                            List<string> TotalReal = new List<string>();
                            List<string> TotalExpected = new List<string>();
                            foreach (var item in apts)
                            {
                                //  TotalQTY = Convert.ToInt32(((Emdep.Geos.UI.Helper.PlanningAppointment)item.SourceObject).QTY);//[002]
                                if (item.CustomFields["IdOT"] != null)
                                {
                                    TotalQTY += Convert.ToInt32(((Emdep.Geos.UI.Helper.PlanningAppointment)item.SourceObject).QTY.Value);
                                    TotalQTYTimeString = Convert.ToString(TotalQTY);
                                    string TotalExpectedTimeStrings = Convert.ToString(((PlanningAppointment)item.SourceObject).Expected);
                                    if (TotalExpectedTimeStrings != null)
                                        TotalExpected.Add(TotalExpectedTimeStrings);


                                    string TotalRealTimes = ((PlanningAppointment)item.SourceObject).Real;
                                    if (TotalRealTimes != null)
                                        TotalReal.Add(TotalRealTimes);


                                    if (item.CustomFields["IdOT"] == null)
                                    {
                                        Count = Count + 1;
                                    }
                                }
                            }
                            if (TotalExpected != null)
                            {

                                TimeSpan sum = TotalExpected.Select(timeString => TimeSpan.Parse(timeString)).Aggregate(TimeSpan.Zero, (currentSum, timeSpan) => currentSum + timeSpan);
                                string formattedTotal = sum.ToString(@"hh\:mm").TrimEnd('0').TrimEnd('.');
                                TotalExpectedTimeString = sum.ToString();
                            }
                            if (TotalReal != null)
                            {
                                TimeSpan sum = TotalReal.Select(timeString => TimeSpan.Parse(timeString)).Aggregate(TimeSpan.Zero, (currentSum, timeSpan) => currentSum + timeSpan);
                                string formattedTotals = sum.ToString(@"hh\:mm").TrimEnd('0').TrimEnd('.');
                                TotalRealTime = sum.ToString();
                            }

                            if (Count == apts.ToList().Count)
                                Duration = false;
                            else
                                Duration = true;
                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
        }

        void Update()
        {
            try
            {
                int Count = 0;

                if (scheduler.ActiveViewIndex == 0)
                {
                    #region
                    MonthCellViewModel vm = AssociatedObject.DataContext as MonthCellViewModel;

                    if (vm == null || scheduler == null)
                    {
                        TotalQTY = 00;
                        TotalQTYTimeString = string.Empty;
                        TotalExpectedTimeString = string.Empty;
                        TotalRealTime = string.Empty;
                        return;
                    }


                    var AppointmentList = (scheduler.AppointmentItems).ToList();
                    var apts = AppointmentList.Where(x => x.Start.Date == vm.Date.Date);
                    //var apts = scheduler.GetAppointments(vm.Interval);
                    var aptsList = apts.Where(x => x.CustomFields["IdOT"] != null).ToList();

                    //[001]Added

                    if (!apts.Any())
                    {

                        TotalQTY = 00;
                        TotalQTYTimeString = string.Empty;
                        TotalExpectedTimeString = string.Empty;
                        TotalRealTime = string.Empty;
                        //TotalExpectedTimeString = string.Empty;
                        //TotalRealTime = string.Empty;

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
                         TotalQTY = 0;
                        TotalExpectedTimeString = string.Empty;
                        TotalQTYTimeString = string.Empty;
                        TotalRealTime = string.Empty;
                        // TotalExpectedTimeString = string.Empty;
                        if (apts.ToList().Count > 0)
                        {

                            List<string> TotalReal = new List<string>();
                            List<string> TotalExpected = new List<string>();
                            foreach (var item in apts)
                            {
                                if (item.CustomFields["IdOT"] != null)
                                {
                                    //TotalQTY += Convert.ToInt32(((Emdep.Geos.UI.Helper.PlanningAppointment)item.SourceObject).QTY.Value);
                                    if (((Emdep.Geos.UI.Helper.PlanningAppointment)item.SourceObject).QTY != null)
                                    {
                                        TotalQTY += Convert.ToInt32(((Emdep.Geos.UI.Helper.PlanningAppointment)item.SourceObject).QTY.Value);
                                        TotalQTYTimeString = Convert.ToString(TotalQTY);
                                    }

                                    //string TotalExpectedTimeStrings = Convert.ToString(((PlanningAppointment)item.SourceObject).Expected);
                                    string TotalExpectedTimeStrings = string.Empty;
                                    if (((PlanningAppointment)item.SourceObject).Expected != null)
                                    {
                                        TotalExpectedTimeStrings = Convert.ToString(((PlanningAppointment)item.SourceObject).Expected);
                                    }

                                    if (TotalExpectedTimeStrings != null)
                                        TotalExpected.Add(TotalExpectedTimeStrings);


                                    //string  TotalRealTimes = ((PlanningAppointment)item.SourceObject).Real;
                                    string TotalRealTimes = string.Empty;
                                    if (((PlanningAppointment)item.SourceObject).Real != null)
                                    {
                                        TotalRealTimes = ((PlanningAppointment)item.SourceObject).Real;
                                    }
                                  
                                    if (TotalRealTimes != null)
                                        TotalReal.Add(TotalRealTimes);


                                    if (item.CustomFields["IdOT"] == null)
                                    {
                                        Count = Count + 1;
                                    }
                                }
                            }
                            if (TotalExpected != null)
                            {

                                TimeSpan sum = TotalExpected.Select(timeString => TimeSpan.Parse(timeString)).Aggregate(TimeSpan.Zero, (currentSum, timeSpan) => currentSum + timeSpan);
                                //string formattedTotal = sum.ToString(@"hh\:mm").TrimEnd('0').TrimEnd('.');
                                string formattedTotal = sum.ToString(@"hh\:mm");
                                TotalExpectedTimeString = formattedTotal;
                            }
                            if (TotalReal != null)
                            {
                                TimeSpan sum = TotalReal.Select(timeString => TimeSpan.Parse(timeString)).Aggregate(TimeSpan.Zero, (currentSum, timeSpan) => currentSum + timeSpan);
                                string formattedTotals = sum.ToString(@"hh\:mm");
                                TotalRealTime = formattedTotals;
                            }


                            if (Count == apts.ToList().Count)
                                Duration = false;
                            else
                                Duration = true;
                        }



                    }



                    #endregion
                }
                else
                {
                    #region
                    DateHeaderViewModel wm = AssociatedObject.DataContext as DateHeaderViewModel;

                    if (wm == null || scheduler == null)
                    {
                        TotalExpectedTimeString = string.Empty;
                        TotalRealTime = string.Empty;
                        return;
                    }

                    var apts = scheduler.GetAppointments(wm.Interval);
                    var aptsList = apts.Where(x => x.CustomFields["IdOT"] != null).ToList();

                    //[001]Added

                    if (!apts.Any())
                    {

                        TotalQTY = 0;


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
                        TotalExpectedTimeString = string.Empty;
                    }



                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
        }


        void Updatenew()
        {
            try
            {
                Duration = true;
                int Count = 0;

                if (scheduler.ActiveViewIndex == 0)
                {
                    MonthCellViewModel vm = AssociatedObject.DataContext as MonthCellViewModel;

                    if (vm == null || scheduler == null)
                    {
                        TotalQTY = 00;
                        TotalQTYTimeString = string.Empty;
                        TotalExpectedTimeString = string.Empty;
                        TotalRealTime = string.Empty;
                        return;
                    }

                    var AppointmentList = (scheduler.AppointmentItems).ToList();


                    //var apts = AppointmentList.Where(x => x.Start.Date == vm.Date.Date);


                    var apts = AppointmentList.Where(x => x.Start.Date == vm.Date.Date || (x.Start.Date <= vm.Date.Date && (x.End.Date > vm.Date.Date || (x.End.Date == vm.Date.Date && x.End > new DateTime(vm.Date.Date.Year, vm.Date.Date.Month, vm.Date.Date.Day, 12, 0, 0)) && x.CustomFields["IdOT"] != null)));

                    var aptsList = apts.Where(x => x.CustomFields["IdOT"] != null).ToList();
                    List<AppointmentItem> leaveList = new List<AppointmentItem>();
                    List<AppointmentItem> appointmentList = apts.Where(x => x.CustomFields["IdOT"] != null).ToList();
                    TimeSpan totalMinutes = TimeSpan.FromTicks(aptsList.Sum(x => x.Duration.Ticks));


                }
            }

            catch (Exception ex)
            {

            }
        }





    }

    public class PlanningdateReviewPresenter : FastTextBlock
    {
        public PlanningdateReviewPresenter()
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
    #endregion
}
