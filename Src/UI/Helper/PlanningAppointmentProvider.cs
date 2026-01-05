using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Scheduling;
using DevExpress.Xpf.Scheduling.Visual;
using DevExpress.Xpf.Scheduling.VisualData;
using System;
using System.Linq;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class PlanningAppointmentProvider : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty TotalDurationProperty =
    DependencyProperty.Register("TotalDuration", typeof(double), typeof(PlanningAppointmentProvider), new PropertyMetadata(0d));
        public double TotalDuration
        {
            get { return (double)GetValue(TotalDurationProperty); }
            set { SetValue(TotalDurationProperty, value); }
        }

        public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register("StartDate", typeof(DateTime), typeof(AppointmentDurationProvider));
        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }
       
        //Shubham[skadam] GEOS2-4047 Leave Paternity no se carga correctamente  22 12 2022
        public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register("EndDate", typeof(DateTime), typeof(AppointmentDurationProvider));
        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
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
           // UnInit();
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
           // UnInit();
        }

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Init();

        }
        SchedulerControl scheduler;
        void Init()
        {
           // UnInit();
            if (AssociatedObject == null ||
                AssociatedObject.DataContext == null) return;
            scheduler = SchedulerControl.GetScheduler(AssociatedObject);

            if (scheduler == null) return;
            //scheduler.ItemPropertyChanged += OnSchedulerItemPropertyChanged;
            //scheduler.ItemsCollectionChanged += OnSchedulerItemsCollectionChanged;
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
            //if (Count > 0)
            //{
            //    Updatenew();
            //}
            //else
            //{
            //    Update();
            //}
        }

        }
}
