using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Accordion;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Scheduler.Drawing;
using DevExpress.Xpf.Scheduling;
using DevExpress.Xpf.Scheduling.Visual;
using DevExpress.Xpf.Scheduling.VisualData;
using DevExpress.XtraScheduler;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Data.Filtering;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Data;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    class EmployeeAttendanceViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        //[M051-08][Year selection is not saved after change section][adadibathina]
        //[000][SP-65][skale][12-06-2019][GEOS2-1556]Grid data reflection problems
        //[001][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        #endregion

        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration

        private ObservableCollection<Department> department;
        private ObservableCollection<Employee> employees;
        private ObservableCollection<EmployeeAttendance> employeeAttendanceList;
        private EmployeeAttendance selectedAttendanceRecord;
        //[M051-08]
        //private long selectedPeriod;
        private ObservableCollection<UI.Helper.Appointment> appointmentItems;
        private ObservableCollection<CompanyHoliday> companyHolidays;
        private ObservableCollection<LookupValue> holidayList;
        private ObservableCollection<LabelHelper> labelItems;
        private ObservableCollection<CompanyWork> companyWorksList;
        private Visibility isSchedulerViewVisible;
        private Visibility isGridViewVisible;
        private string timeEditMask;
        private bool isBusy;
        private bool IsFromRefresh;
        IWorkbenchStartUp objWorkbenchStartUp;
        public bool IsVisible;
        //Sprint-42
        private DateTime selectedStartDate;
        private DateTime selectedEndDate;
        private bool isSet;

        private List<GeosAppSetting> geosAppSettingList;
        private SolidColorBrush oKColor;
        private SolidColorBrush notOKColor;
        private byte viewType;

        private ObservableCollection<EmployeeLeave> employeeLeaves;
        private List<EmployeeLeave> selectedEmployeeLeavesAsPerDate;
        private Visibility isAccordionControlVisible;
        string myFilterString;//[SP-65-000] added

        private ObservableCollection<StatusHelper> statusItems;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;

        private double monthlyTotalHoursCount;
        private Decimal monthlyExpectedTotalHoursCount;
        List<CompanySchedule> getEmployeeCompanySchedulelist;
        private bool isEmployeewiseRegisterAndExpectDays;

        string stringMonthlyTotalHoursCount;
        string stringMonthlyExpectedTotalHoursCount;
        string stringRegisterHoursColour;
        private TimeSpan hoursCount;
        private decimal MonthlyExpectedDeductAllDays;
        private string stringIsManual;
        private Boolean isActive;
        private List<Int32> BreakWTIdCompanyWork;
        private ObservableCollection<Employee> _employeeListFinal;
        #endregion

        #region public ICommand
        public ICommand DisableAppointmentCommand { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand scheduler_VisibleIntervalsChangedCommand { get; set; }
        public ICommand PlantOwnerEditValueChangedCommand { get; private set; }
        public ICommand ImportButtonCommand { get; set; }
        public ICommand CommandDepartmentSelection { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand SelectedYearChangedCommand { get; private set; }
        public ICommand ShowSchedulerViewCommand { get; private set; }
        public ICommand ShowGridViewCommand { get; private set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ButtonAddNewAttendance { get; set; }
        public ICommand SelectedIntervalCommand { get; set; }
        public ICommand EditAttendanceGridDoubleClickCommand { get; set; }
        public ICommand EditEmployeeDoubleClickCommand { get; set; }
        public ICommand EditOccurrenceWindowShowingCommand { get; set; }
        public ICommand DeleteAppointmentCommand { get; set; }
        public ICommand DefaultLoadCommand { get; set; }
        public ICommand HidePanelCommand { get; set; }
        //[SP-65 001]
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand CustomRowFilterCommand { get; set; }
        #endregion

        #region Properties   

        public SolidColorBrush NotOKColor
        {
            get
            {
                return notOKColor;
            }

            set
            {
                notOKColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotOKColor"));
            }
        }
        public SolidColorBrush OKColor
        {
            get
            {
                return oKColor;
            }

            set
            {
                oKColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OKColor"));
            }
        }
        public bool IsSet
        {
            get
            {
                return isSet;
            }

            set
            {
                isSet = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSet"));
            }
        }
        public List<GeosAppSetting> GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }



        public byte ViewType
        {
            get
            {
                return viewType;
            }

            set
            {
                viewType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ViewType"));
            }
        }
        public GeosProvider CurrentGeosProvider { get; set; }
        public List<GeosProvider> GeosProviderList { get; set; }

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public ObservableCollection<LabelHelper> LabelItems
        {
            get
            {
                return labelItems;
            }

            set
            {
                labelItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LabelItems"));
            }
        }
        public virtual object SelectedItem { get; set; }

        public GeosAppSetting Setting { get; set; }
        public ObservableCollection<UI.Helper.Appointment> AppointmentItems
        {
            get
            {
                return appointmentItems;
            }

            set
            {
                appointmentItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentItems"));
            }
        }
        //[M051-08]
        //public long SelectedPeriod
        //{
        //    get { return selectedPeriod; }
        //    set
        //    {
        //        selectedPeriod = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedPeriod"));
        //    }
        //}


        public ObservableCollection<EmployeeAttendance> EmployeeAttendanceList
        {
            get
            {
                return employeeAttendanceList;
            }

            set
            {
                employeeAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAttendanceList"));

            }
        }

        public ObservableCollection<CompanyHoliday> CompanyHolidays
        {
            get
            {
                return companyHolidays;
            }

            set
            {
                companyHolidays = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("CompanyHolidays"));
            }
        }

        public ObservableCollection<LookupValue> HolidayList
        {
            get
            {
                return holidayList;
            }

            set
            {
                holidayList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HolidayList"));
            }
        }

        public ObservableCollection<CompanyWork> CompanyWorksList
        {
            get
            {
                return companyWorksList;
            }

            set
            {
                companyWorksList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyWorksList"));
            }
        }

        public Visibility IsSchedulerViewVisible
        {
            get
            {
                return isSchedulerViewVisible;
            }

            set
            {
                isSchedulerViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSchedulerViewVisible"));
            }
        }

        public Visibility IsGridViewVisible
        {
            get
            {
                return isGridViewVisible;
            }

            set
            {
                isGridViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridViewVisible"));
            }
        }

        public EmployeeAttendance SelectedAttendanceRecord
        {
            get
            {
                return selectedAttendanceRecord;
            }

            set
            {
                selectedAttendanceRecord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttendanceRecord"));
            }
        }

        public string TimeEditMask
        {
            get
            {
                return timeEditMask;
            }

            set
            {
                timeEditMask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeEditMask"));
            }
        }
        public DateTime SelectedStartDate
        {
            get
            {
                return selectedStartDate;
            }

            set
            {
                selectedStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStartDate"));
            }
        }
        public DateTime SelectedEndDate
        {
            get
            {
                return selectedEndDate;
            }

            set
            {
                selectedEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEndDate"));
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get
            {
                return employees;
            }

            set
            {
                employees = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Employees"));
            }
        }


        public ObservableCollection<EmployeeLeave> EmployeeLeaves
        {
            get
            {
                return employeeLeaves;
            }

            set
            {
                employeeLeaves = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));

            }
        }

        public List<EmployeeLeave> SelectedEmployeeLeavesAsPerDate
        {
            get
            {
                return selectedEmployeeLeavesAsPerDate;
            }

            set
            {
                selectedEmployeeLeavesAsPerDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployeeLeavesAsPerDate"));
            }
        }
        public Visibility IsAccordionControlVisible
        {
            get
            {
                return isAccordionControlVisible;
            }

            set
            {
                isAccordionControlVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccordionControlVisible"));
            }
        }
        //[SP-65-000] added
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }

        public ObservableCollection<StatusHelper> StatusItems
        {
            get { return statusItems; }
            set
            {
                statusItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusItems"));
            }
        }
        public bool IsReadOnlyField
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
            }
        }

        public bool IsAcceptEnabled
        {
            get { return isAcceptEnabled; }
            set
            {
                isAcceptEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnabled"));
            }
        }

        public double MonthlyRegisterTotalHoursCount
        {
            get { return monthlyTotalHoursCount; }
            set
            {
                monthlyTotalHoursCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MonthlyRegisterTotalHoursCount"));
            }
        }
        public Decimal MonthlyExpectedTotalHoursCount
        {
            get { return monthlyExpectedTotalHoursCount; }
            set
            {
                monthlyExpectedTotalHoursCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MonthlyExpectedTotalHoursCount"));
            }
        }
        public List<CompanySchedule> GetEmployeeCompanySchedulelist
        {
            get
            {
                return getEmployeeCompanySchedulelist;
            }

            set
            {
                getEmployeeCompanySchedulelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GetEmployeeCompanySchedulelist"));
            }
        }
        public bool IsEmployeewiseRegisterAndExpectDays
        {
            get { return isEmployeewiseRegisterAndExpectDays; }
            set
            {
                isEmployeewiseRegisterAndExpectDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmployeewiseRegisterAndExpectDays"));
            }
        }
        public string StringMonthlyTotalHoursCount
        {
            get { return stringMonthlyTotalHoursCount; }
            set
            {
                stringMonthlyTotalHoursCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StringMonthlyTotalHoursCount"));
            }

        }
        public string StringMonthlyExpectedTotalHoursCount
        {
            get { return stringMonthlyExpectedTotalHoursCount; }
            set
            {
                stringMonthlyExpectedTotalHoursCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StringMonthlyExpectedTotalHoursCount"));
            }

        }
        public string StringRegisterHoursColour
        {
            get { return stringRegisterHoursColour; }
            set
            {
                stringRegisterHoursColour = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StringRegisterHoursColour"));
            }

        }

        public TimeSpan HoursCount
        {
            get
            {
                return hoursCount;
            }

            set
            {
                hoursCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HoursCount"));
            }
        }

        public Boolean IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActive"));
            }
        }

        public string StringIsManual
        {
            get { return stringIsManual; }
            set { stringIsManual = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StringIsManual"));
            }
        }

       

        #endregion

        #region public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Constructor

        static EmployeeAttendanceViewModel()
        {
            ScrollViewer.VerticalScrollBarVisibilityProperty.OverrideMetadata(typeof(DevExpress.Xpf.Scheduler.Drawing.SchedulerScrollViewer), new FrameworkPropertyMetadata(null, (d, e) => ScrollBarVisibility.Visible));
            //ScrollViewer.VerticalScrollBarVisibilityProperty.OverrideMetadata(typeof(SchedulerScrollViewer), new FrameworkPropertyMetadata(null, (d, value) =>
            //{
            //    var scheduler = SchedulerControl.GetScheduler((DependencyObject)d);
            //    if (scheduler != null && scheduler.ActiveView is MonthView)
            //        return ScrollBarVisibility.Hidden;
            //    return value;
            //}));
        }
        /// <summary>
        ///[001]  Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri  
        ///[002] [SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        ///[003] [cpatil][2020-01-22][GEOS2-2008] We can not see some the Employees in LEAVES and ATTENDANCE with Period = 2019, but the same employees in 2020 we can see.
        ///[004] [avpawar][2020-07-24][GEOS2-2432] changed service method because we got error message when try edit the attendance shift.
        ///[005][cpatil][29-09-2020][GEOS2-2113]HRM - Break time in Attendance (Grid View) (#IES16).
        /// </summary>
        public EmployeeAttendanceViewModel()
        {
            try
            {

                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor EmployeeAttendanceViewModel()...", category: Category.Info, priority: Priority.Low);
                Setting = CrmStartUp.GetGeosAppSettings(11);
                DisableAppointmentCommand = new DelegateCommand<AppointmentWindowShowingEventArgs>(AppointmentWindowShowing);
                PopupMenuShowingCommand = new DelegateCommand<PopupMenuShowingEventArgs>(PopupMenuShowing);
                PlantOwnerEditValueChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerEditValueChangedCommandAction);
                AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                scheduler_VisibleIntervalsChangedCommand = new RelayCommand(new Action<object>(VisibleIntervalsChanged));
                CommandDepartmentSelection = new RelayCommand(new Action<object>(SelectItemForScheduler));
                ImportButtonCommand = new DelegateCommand<object>(OpenAttendanceFile);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshAttendanceView));
                SelectedYearChangedCommand = new DelegateCommand<object>(SelectedYearChangedCommandAction);
                ShowSchedulerViewCommand = new RelayCommand(new Action<object>(ShowAttendanceSchedulerView));
                ShowGridViewCommand = new RelayCommand(new Action<object>(ShowAttendanceGridView));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportAttendancetList));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintAttendanceList));
                ButtonAddNewAttendance = new RelayCommand(new Action<object>(AddAttendance));
                SelectedIntervalCommand = new DelegateCommand<MouseButtonEventArgs>(SelectedIntervalCommandAction);
                //Sprint 42-Command to edit attendance from gris view
                EditAttendanceGridDoubleClickCommand = new DelegateCommand<object>(EditAttendanceInformation);
                EditEmployeeDoubleClickCommand = new RelayCommand(new Action<object>(OpenEmployeeProfile));
                EditOccurrenceWindowShowingCommand = new DelegateCommand<EditOccurrenceWindowShowingEventArgs>(EditOccurrenceWindowShowing);
                DeleteAppointmentCommand = new DelegateCommand<object>(DeleteAppointment);
                DefaultLoadCommand = new DelegateCommand<RoutedEventArgs>(DefaultLoadCommandAction);
                //[005]
                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("7,8,41");
                if (GeosAppSettingList.Any(i => i.IdAppSetting == 41))
                {
                    foreach (string itemSplit in GeosAppSettingList.Where(i => i.IdAppSetting == 41).Select(i => i.DefaultValue).FirstOrDefault().Split(','))
                    {
                        if (BreakWTIdCompanyWork == null)
                            BreakWTIdCompanyWork = new List<int>();

                        BreakWTIdCompanyWork.Add(Convert.ToInt32(itemSplit));
                    }
                 
                }
                OKColor = new BrushConverter().ConvertFromString(GeosAppSettingList[0].DefaultValue) as SolidColorBrush;
                NotOKColor = new BrushConverter().ConvertFromString(GeosAppSettingList[1].DefaultValue) as SolidColorBrush;

                DefaultLoadCommand = new DelegateCommand<RoutedEventArgs>(DefaultLoadCommandAction);
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                //[002] added
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                CustomRowFilterCommand = new DelegateCommand<RowFilterEventArgs>(CustomRowFilter);
                IsSet = false;

                IsVisible = false;
                SelectedEmployeeLeavesAsPerDate = new List<EmployeeLeave>();

                //if (!DXSplashScreen.IsActive)
                //{
                //    DXSplashScreen.Show(x =>
                //    {
                //        Window win = new Window()
                //        {
                //            ShowActivated = false,
                //            WindowStyle = WindowStyle.None,
                //            ResizeMode = ResizeMode.NoResize,
                //            AllowsTransparency = true,
                //            Background = new SolidColorBrush(Colors.Transparent),
                //            ShowInTaskbar = false,
                //            Topmost = true,
                //            SizeToContent = SizeToContent.WidthAndHeight,
                //            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                //        };
                //        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                //        win.Topmost = false;
                //        return win;
                //    }, x =>
                //    {
                //        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                //    }, null, null);
                //}


                IsSchedulerViewVisible = Visibility.Visible;
                IsGridViewVisible = Visibility.Hidden;

                //List<long> FinancialYearLst = GeosApplication.Instance.FillFinancialYear();
                //GeosApplication.Instance.FinancialYearLst
                //[M051-08]
                //SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;

                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor EmployeeAttendanceViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeAttendanceViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init(ObservableCollection<Employee> employeesList,
                ObservableCollection<EmployeeAttendance> employeeAttendanceList,
                ObservableCollection<CompanyHoliday> companyHolidays,
                ObservableCollection<LookupValue> holidayList,
                ObservableCollection<EmployeeLeave> employeeLeavesList,
                ObservableCollection<LabelHelper> labelItems,
                ObservableCollection<StatusHelper> statusItems,
                ObservableCollection<UI.Helper.Appointment> appointmentList, ObservableCollection<Employee> EmployeeListFinal)
        {
            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            {
                IsActive = true;
                ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();
                List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                //List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission);
                // [003] Changed service method GetAllEmployeesForAttendanceByIdCompany_V2037 to GetAllEmployeesForAttendanceByIdCompany_V2039
                Employees = employeesList; // new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                // [003] Changed service method GetSelectedIdCompanyEmployeeAttendance_V2037 to GetSelectedIdCompanyEmployeeAttendance_V2039

                // [004] Changed service method GetSelectedIdCompanyEmployeeAttendance_V2044 to GetSelectedIdCompanyEmployeeAttendance_V2045
                // EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>(HrmService.GetSelectedIdCompanyEmployeeAttendance_V2045(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                EmployeeAttendanceList = employeeAttendanceList; // new ObservableCollection<EmployeeAttendance>(HrmService.GetSelectedIdCompanyEmployeeAttendance_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                SetIsManual(EmployeeAttendanceList);
                CompanyHolidays = companyHolidays; //  new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds));
                HolidayList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(28).AsEnumerable());
                // [003] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2032 to GetEmployeeLeavesBySelectedIdCompany_V2039
                EmployeeLeaves = employeeLeavesList; // new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));

                LabelItems = labelItems ; //  new ObservableCollection<LabelHelper>();
                StatusItems = statusItems ; // new ObservableCollection<StatusHelper>();

                SelectedItem = null;
                IsFromRefresh = true;
                //[001] Code Comment and Fill employee work
                //CompanyWorksList = new ObservableCollection<CompanyWork>(HrmService.GetAllCompanyWorks());
                FillEmployeeWorkType();
                FillEmployeeLeaveType();
                if (EmployeeAttendanceList.Count > 0)
                {
                    SelectedAttendanceRecord = EmployeeAttendanceList[0];
                }

                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                _employeeListFinal = EmployeeListFinal;
                //foreach (CompanyHoliday CompanyHoliday in CompanyHolidays)
                //{

                //    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                //    modelAppointment.Subject = CompanyHoliday.Name;
                //    modelAppointment.StartDate = CompanyHoliday.StartDate;
                //    modelAppointment.EndDate = CompanyHoliday.EndDate;
                //    modelAppointment.Label = CompanyHoliday.IdHoliday;
                //    if (CompanyHoliday.IsRecursive == 1)
                //    {
                //        modelAppointment.Type = (int)DevExpress.XtraScheduler.AppointmentType.Pattern;
                //        modelAppointment.RecurrenceInfo = new RecurrenceInfo
                //        {
                //            Type = RecurrenceType.Yearly,
                //            Periodicity = 1,
                //            Month = CompanyHoliday.StartDate.Value.Month,
                //            WeekOfMonth = WeekOfMonth.None,
                //            DayNumber = CompanyHoliday.StartDate.Value.Day,
                //            Range = RecurrenceRange.NoEndDate
                //        }.ToXml();
                //    }
                //    //modelAppointment.AllDay = true;
                //    appointment.Add(modelAppointment);

                //}

                appointment = appointmentList;
                //var groupedEmployeeAttendance = EmployeeAttendanceList.GroupBy(x => new { x.IdEmployee, x.StartDate.Date });

                //foreach (var item in groupedEmployeeAttendance)
                //{
                //    TimeSpan timeSpan = new TimeSpan();

                //    foreach (var itemEmployeeAttendance in item)
                //    {
                //        TimeSpan timeSpan2 = TimeSpan.Parse(itemEmployeeAttendance.Employee.TotalWorkedHours);
                //        timeSpan = timeSpan.Add(timeSpan2);
                //    }

                //    EmployeeAttendance employeeAttendance = item.First();
                //    EmployeeAttendance employeeAttendanceLast = item.Last();

                //    //LabelHelper labelForEmpAttendance = new LabelHelper();
                //    //labelForEmpAttendance.Id = employeeAttendance.CompanyWork.IdCompanyWork;
                //    //labelForEmpAttendance.Color = new BrushConverter().ConvertFromString(employeeAttendance.CompanyWork.HtmlColor.ToString()) as SolidColorBrush;
                //    //labelForEmpAttendance.Caption = employeeAttendance.CompanyWork.Name;
                //    //LabelItems.Add(labelForEmpAttendance);

                //    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                //    modelAppointment.Subject = string.Format("[{0}]   {1} {2}", timeSpan.ToString(@"hh\:mm"), employeeAttendance.Employee.FirstName, employeeAttendance.Employee.LastName);
                //    modelAppointment.StartDate = employeeAttendance.StartDate;
                //    modelAppointment.EndDate = employeeAttendanceLast.EndDate;
                //    modelAppointment.Label = employeeAttendance.IdCompanyWork;
                //    modelAppointment.IdEmployeeAttendance = employeeAttendance.IdEmployeeAttendance;
                //    appointment.Add(modelAppointment);
                //}

                //foreach (EmployeeAttendance employeeAttendance in EmployeeAttendanceList)
                //{
                //    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                //    modelAppointment.Subject = string.Format("[{0}] {1}", employeeAttendance.Employee.TotalWorkedHours, employeeAttendance.CompanyWork.Name);
                //    modelAppointment.StartDate = employeeAttendance.StartDate;
                //    modelAppointment.EndDate = employeeAttendance.EndDate;
                //    modelAppointment.Label = employeeAttendance.IdCompanyWork;
                //    modelAppointment.IdEmployeeAttendance = employeeAttendance.IdEmployeeAttendance;
                //    appointment.Add(modelAppointment);
                //}

                //[001] Code Comment and Label added as per Lookupvalue

                //foreach (var CompanyWork in CompanyWorksList)
                //{
                //    LabelHelper labelForCompanyWork = new LabelHelper();
                //    labelForCompanyWork.Id = CompanyWork.IdCompanyWork;
                //    labelForCompanyWork.Color = new BrushConverter().ConvertFromString(CompanyWork.HtmlColor.ToString()) as SolidColorBrush;
                //    labelForCompanyWork.Caption = CompanyWork.Name;
                //    LabelItems.Add(labelForCompanyWork);
                //}

                //foreach (var AttendenceType in GeosApplication.Instance.AttendanceTypeList)
                //{
                //    if (AttendenceType.IdLookupValue > 0 && AttendenceType.HtmlColor != null && AttendenceType.HtmlColor != string.Empty)
                //    {
                //        LabelHelper labelForCompanyWork = new LabelHelper();
                //        labelForCompanyWork.Id = AttendenceType.IdLookupValue;
                //        labelForCompanyWork.Color = new BrushConverter().ConvertFromString(AttendenceType.HtmlColor.ToString()) as SolidColorBrush;
                //        labelForCompanyWork.Caption = AttendenceType.Value;
                //        LabelItems.Add(labelForCompanyWork);
                //    }
                //}

                //foreach (var Holiday in HolidayList)
                //{
                //    if (Holiday.HtmlColor != null && Holiday.HtmlColor != string.Empty)
                //    {
                //        LabelHelper labelForCompanyHoliday = new LabelHelper();
                //        labelForCompanyHoliday.Id = Holiday.IdLookupValue;
                //        labelForCompanyHoliday.Color = new BrushConverter().ConvertFromString(Holiday.HtmlColor.ToString()) as SolidColorBrush;
                //        labelForCompanyHoliday.Caption = Holiday.Value;
                //        LabelItems.Add(labelForCompanyHoliday);
                //    }
                //}

                //int count = 2;
                //for (int i = 0; i < count; i++)
                //{
                //    StatusHelper statusItem = new StatusHelper();
                //    statusItem.Id = i;
                //    if (i == 0)
                //        statusItem.Brush = new SolidColorBrush(Colors.Transparent); //BrushConverter().ConvertFromString("#7833FF ") as SolidColorBrush;
                //    else
                //        statusItem.Brush = new SolidColorBrush(Colors.SlateBlue);
                //    statusItem.Caption = "Night Shift";
                //    StatusItems.Add(statusItem);
                //}

                //foreach (EmployeeAttendance EmployeeAttendance in EmployeeAttendanceList)
                //{
                //    Appointment modelAppointment = new Appointment();
                //    //modelAppointment.Subject = string.Format("{0} {1} {2}",( EmployeeAttendance.StartDate.Hour).ToString("00.00"),EmployeeAttendance.Employee.FirstName, EmployeeAttendance.Employee.LastName);

                //    modelAppointment.Subject = string.Format("[{0}]   {1} {2}", (EmployeeAttendance.Employee.TotalWorkedHours).ToString(), EmployeeAttendance.Employee.FirstName, EmployeeAttendance.Employee.LastName);
                //    modelAppointment.StartDate = EmployeeAttendance.StartDate.Date;
                //    modelAppointment.EndDate = EmployeeAttendance.EndDate;
                //    modelAppointment.Label = EmployeeAttendance.IdCompanyWork;
                //    appointment.Add(modelAppointment);
                //}

                AppointmentItems = appointment;
            }

            StringMonthlyExpectedTotalHoursCount = "00:00";
            StringMonthlyTotalHoursCount = "00:00";

        }

        #endregion

        #region Methods

        //public void Init()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

        //        GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void PopupMenuShowing(PopupMenuShowingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PopupMenuShowing()...", category: Category.Info, priority: Priority.Low);

                if (obj.MenuType == ContextMenuType.CellContextMenu)
                {
                    PopupMenu menu = (PopupMenu)obj.Menu;
                    object open = menu.Items.FirstOrDefault(x => x is BarItem && (string)((BarItem)x).Content == "Change View To");

                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Clear();
                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Add((BarItem)open);
                }
                else if (obj.MenuType == ContextMenuType.AppointmentContextMenu)
                {
                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Clear();
                }

                GeosApplication.Instance.Logger.Log("Method PopupMenuShowing()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PopupMenuShowing()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method to Show Appointment Window
        ///[001]  Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri   
        ///[002][cpatil][29-09-2020][GEOS2-2113]HRM - Break time in Attendance (Grid View) (#IES16).
        /// </summary>
        /// <param name="obj"></param>
        private void AppointmentWindowShowing(AppointmentWindowShowingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AppointmentWindowShowing()...", category: Category.Info, priority: Priority.Low);

                obj.Cancel = true;

                if (obj.Appointment.SourceObject != null)
                {
                    if (((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeAttendance != null)
                    {
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        EmployeeAttendance selectedEmpAttendance = new EmployeeAttendance();
                        var EmpAttendance = EmployeeAttendanceList.Where(x => x.IdEmployeeAttendance == ((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeAttendance);
                        selectedEmpAttendance = EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == ((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeAttendance);

                        AttendanceView addAttendanceView = new AttendanceView();
                        AttendanceViewModel addAttendanceViewModel = new AttendanceViewModel();
                        EventHandler handle = delegate { addAttendanceView.Close(); };
                        addAttendanceViewModel.RequestClose += handle;
                        addAttendanceView.DataContext = addAttendanceViewModel;
                        //[001] CompanyWork IdCompany  Code comment and changes as per Job description IdCompany
                        //  addAttendanceViewModel.WorkingPlantId = selectedEmpAttendance.CompanyWork.IdCompany.ToString();
                        //addAttendanceViewModel.WorkingPlantId = selectedEmpAttendance.Employee.EmployeeJobDescription.IdCompany.ToString();

                        string[] idEmployeeCompanyIdsSplit = selectedEmpAttendance.Employee.EmployeeCompanyIds.Split(',');
                        addAttendanceViewModel.WorkingPlantId = idEmployeeCompanyIdsSplit[0];

                        addAttendanceViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                        addAttendanceViewModel.SelectedPlantList = plantOwners;
                        if (HrmCommon.Instance.IsPermissionReadOnly)
                            addAttendanceViewModel.InitReadOnly(selectedEmpAttendance);
                        else
                            addAttendanceViewModel.EditInit(selectedEmpAttendance, EmployeeAttendanceList, _employeeListFinal);

                        addAttendanceViewModel.IsNew = false;
                        addAttendanceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditAttendance").ToString();
                        EmployeeJobDescription empJobDesc = new EmployeeJobDescription();
                        if (selectedEmpAttendance.Employee.EmployeeJobDescription != null)
                        {
                            empJobDesc = selectedEmpAttendance.Employee.EmployeeJobDescription;
                        }
                        addAttendanceViewModel.EmployeeLeaves = EmployeeLeaves;
                        var ownerInfo = (obj.OriginalSource as FrameworkElement);
                        addAttendanceView.Owner = Window.GetWindow(ownerInfo);
                        addAttendanceView.ShowDialog();
                        if (addAttendanceViewModel.Result)
                        {
                            if (addAttendanceViewModel.IsAcceptButton)
                            {
                                IsEmployeewiseRegisterAndExpectDays = true;
                            }
                            if (addAttendanceViewModel.IsSave && !addAttendanceViewModel.IsSplit)
                            {


                                TimeSpan timeSpan = new TimeSpan();
                                timeSpan = addAttendanceViewModel.UpdateEmployeeAttendance.EndDate - addAttendanceViewModel.UpdateEmployeeAttendance.StartDate;
                                AppointmentItems.Remove((UI.Helper.Appointment)obj.Appointment.SourceObject);

                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                modelAppointment.Label = addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork;

                                //[001] Changes in Subject as per Lookup Value
                                //modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList[GeosApplication.Instance.AttendanceTypeList.FindIndex(x => x.IdLookupValue == addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork)].Value);
                                modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork).Value);

                                modelAppointment.StartDate = addAttendanceViewModel.UpdateEmployeeAttendance.StartDate;
                                modelAppointment.EndDate = addAttendanceViewModel.UpdateEmployeeAttendance.EndDate;

                                modelAppointment.IdEmployeeAttendance = addAttendanceViewModel.UpdateEmployeeAttendance.IdEmployeeAttendance;

                                modelAppointment.DailyHoursCount = 0;
                                if (addAttendanceViewModel.CompanyShiftDetails != null)
                                {
                                    if (addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule != null)
                                        modelAppointment.DailyHoursCount = addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule.DailyHoursCount;
                                }
                                else
                                    modelAppointment.DailyHoursCount = 0;

                                var EmployeeLeaveForSelectedAttendance = SelectedEmployeeLeavesAsPerDate.Where(x => x.StartDate == addAttendanceViewModel.UpdateEmployeeAttendance.StartDate.Date);

                                modelAppointment.TotalLeaveDurationInHours = 0;
                                if (EmployeeLeaveForSelectedAttendance.Count() > 0)
                                {
                                    foreach (EmployeeLeave item in EmployeeLeaveForSelectedAttendance)
                                    {
                                        TimeSpan fromH = item.StartTime.Value;
                                        TimeSpan toH = item.EndTime.Value;
                                        TimeSpan hourTotalSpan = toH.Subtract(fromH);
                                        modelAppointment.TotalLeaveDurationInHours = modelAppointment.TotalLeaveDurationInHours + hourTotalSpan.Hours + hourTotalSpan.Minutes;
                                    }

                                }

                                if (addAttendanceViewModel.SelectedEmployeeShift != null)
                                {
                                    if (selectedEmpAttendance.IsManual == 1)
                                    {
                                        modelAppointment.Description = "[Manual] ";
                                    }
                                    if (addAttendanceViewModel.SelectedEmployeeShift.CompanyShift.IsNightShift == 1)
                                    {
                                        modelAppointment.Description = modelAppointment.Description + "[Night Shift]";
                                        modelAppointment.IsNightShift = 1;
                                    }
                                    if (selectedEmpAttendance.IsManual == 1)
                                    {
                                        modelAppointment.AttendanceIsManual = true;
                                    }
                                    else
                                    {
                                        modelAppointment.AttendanceIsManual = false;
                                    }
                                }

                                modelAppointment.AccountingDate = addAttendanceViewModel.UpdateEmployeeAttendance.AccountingDate;

                                AppointmentItems.Add(modelAppointment);

                                addAttendanceViewModel.UpdateEmployeeAttendance.Employee.EmployeeJobDescription = empJobDesc;
                                selectedEmpAttendance.Employee = addAttendanceViewModel.UpdateEmployeeAttendance.Employee;
                                selectedEmpAttendance.IdEmployee = addAttendanceViewModel.UpdateEmployeeAttendance.IdEmployee;
                                selectedEmpAttendance.StartDate = addAttendanceViewModel.UpdateEmployeeAttendance.StartDate;
                                selectedEmpAttendance.EndDate = addAttendanceViewModel.UpdateEmployeeAttendance.EndDate;
                                selectedEmpAttendance.StartTime = addAttendanceViewModel.STime;
                                selectedEmpAttendance.EndTime = addAttendanceViewModel.ETime;
                                selectedEmpAttendance.IdCompanyShift = addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyShift;
                                selectedEmpAttendance.CompanyShift = addAttendanceViewModel.SelectedEmployeeShift.CompanyShift;
                                //[002]
                                if (BreakWTIdCompanyWork.Any(bwc => bwc == addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork))
                                    selectedEmpAttendance.CompanyShift.BreakTime = GetBreakTime((int)selectedEmpAttendance.StartDate.DayOfWeek, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);
                                else
                                    selectedEmpAttendance.CompanyShift.BreakTime = new TimeSpan();
                                //[001] Code Comment and get company Work
                                //    selectedEmpAttendance.CompanyWork = addAttendanceViewModel.UpdateEmployeeAttendance.CompanyWork;

                                    //selectedEmpAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList[GeosApplication.Instance.AttendanceTypeList.FindIndex(x => x.IdLookupValue == addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork)]);
                                selectedEmpAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork));

                                selectedEmpAttendance.IdCompanyWork = addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork;
                                selectedEmpAttendance.Employee.TotalWorkedHours = addAttendanceViewModel.UpdateEmployeeAttendance.Employee.TotalWorkedHours;

                                selectedEmpAttendance.TotalTime = TimeSpan.FromHours(timeSpan.TotalHours);
                                if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(selectedEmpAttendance.StartDate) < 10)
                                {
                                    selectedEmpAttendance.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(selectedEmpAttendance.StartDate);
                                }
                                else
                                {
                                    selectedEmpAttendance.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(selectedEmpAttendance.StartDate);

                                }

                                selectedEmpAttendance.AccountingDate = addAttendanceViewModel.UpdateEmployeeAttendance.AccountingDate;

                                SelectedAttendanceRecord = selectedEmpAttendance;

                            }

                            if (addAttendanceViewModel.IsSplit && addAttendanceViewModel.IsSave)
                            {
                                AppointmentItems.Remove((UI.Helper.Appointment)obj.Appointment.SourceObject);
                                ObservableCollection<UI.Helper.Appointment> appointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                                foreach (var item in addAttendanceViewModel.NewEmployeeAttendanceList)
                                {
                                    TimeSpan timeSpan = new TimeSpan();
                                    timeSpan = item.EndDate - item.StartDate;

                                    if (SelectedItem != null)
                                    {
                                        var SelectedEmployeeLeave = EmployeeLeaves.Where(x => x.IdEmployee == item.IdEmployee).ToList();
                                        List<EmployeeLeave> NewLeaveOfSelectedEmployeeAsPerDate = new List<EmployeeLeave>();
                                        foreach (EmployeeLeave employeeLeave in SelectedEmployeeLeave)
                                        {
                                            for (var day = employeeLeave.StartDate.Value.Date; day.Date <= employeeLeave.EndDate.Value.Date; day = day.AddDays(1))
                                            {
                                                EmployeeLeave e = new EmployeeLeave();
                                                e = (EmployeeLeave)employeeLeave.Clone();
                                                e.StartDate = day;
                                                e.EndDate = day;
                                                NewLeaveOfSelectedEmployeeAsPerDate.Add(e);
                                            }
                                        }

                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment.Label = item.IdCompanyWork;
                                        modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == item.IdCompanyWork).Value);
                                        modelAppointment.StartDate = item.StartDate;
                                        modelAppointment.EndDate = item.EndDate;
                                        modelAppointment.IdEmployeeAttendance = item.IdEmployeeAttendance;
                                        modelAppointment.DailyHoursCount = 0;

                                        if (addAttendanceViewModel.CompanyShiftDetails != null)
                                        {
                                            if (addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule != null)
                                            {
                                                modelAppointment.DailyHoursCount = addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule.DailyHoursCount;
                                            }
                                        }
                                        else
                                            modelAppointment.DailyHoursCount = 0;

                                        var EmployeeLeaveForSelectedAttendance = NewLeaveOfSelectedEmployeeAsPerDate.Where(x => x.StartDate == item.StartDate.Date);
                                        modelAppointment.TotalLeaveDurationInHours = 0;
                                        if (EmployeeLeaveForSelectedAttendance.Count() > 0)
                                        {
                                            foreach (EmployeeLeave Leave in EmployeeLeaveForSelectedAttendance)
                                            {
                                                TimeSpan fromH = Leave.StartTime.Value;
                                                TimeSpan toH = Leave.EndTime.Value;
                                                TimeSpan hourTotalSpan = toH.Subtract(fromH);
                                                modelAppointment.TotalLeaveDurationInHours = modelAppointment.TotalLeaveDurationInHours + hourTotalSpan.Hours + hourTotalSpan.Minutes;
                                            }
                                        }


                                        if (addAttendanceViewModel.SelectedEmployeeShift != null)
                                        {
                                            if (selectedEmpAttendance.IsManual == 1)
                                            {
                                                modelAppointment.Description = "[Manual] ";
                                            }
                                            if (addAttendanceViewModel.SelectedEmployeeShift.CompanyShift.IsNightShift == 1)
                                            {
                                                modelAppointment.Description = modelAppointment.Description + "[Night Shift]";
                                                modelAppointment.IsNightShift = 1;
                                            }

                                            if (selectedEmpAttendance.IsManual == 1)
                                            {
                                                modelAppointment.AttendanceIsManual = true;
                                            }
                                            else
                                            {
                                                modelAppointment.AttendanceIsManual = false;
                                            }
                                        }

                                        modelAppointment.AccountingDate = item.AccountingDate;

                                        appointmentItems.Add(modelAppointment);
                                    }

                                    item.StartTime = item.StartDate.TimeOfDay;
                                    item.EndTime = item.EndDate.TimeOfDay;
                                    item.Employee.TotalWorkedHours = timeSpan.ToString(@"hh\:mm");
                                    item.TotalTime = timeSpan;
                                    item.CompanyShift = item.Employee.CompanyShift;
                                    //[002]
                                    if (BreakWTIdCompanyWork.Any(bwc => bwc == item.IdCompanyWork))
                                        item.CompanyShift.BreakTime = GetBreakTime((int)item.StartDate.Date.DayOfWeek, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);
                                    else
                                        item.CompanyShift.BreakTime = new TimeSpan();

                                    if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate) < 10)
                                    {
                                        item.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate);
                                    }
                                    else
                                    {
                                        item.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate);
                                    }
                                    EmployeeAttendanceList.Add(item);
                                    SelectedAttendanceRecord = item;
                                }
                                AppointmentItems.AddRange(appointmentItems);
                                EmployeeAttendanceList.Remove(selectedEmpAttendance);

                            }
                        }

                    }
                }

                GeosApplication.Instance.Logger.Log("Method AppointmentWindowShowing()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AppointmentWindowShowing()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Sprint-42---HRM-Add and Edit Attendance
        /// Method to get SelectedStartDate and SelectedEndDate-----sdesai
        ///[003][spawar][12-03-2020][GEOS2-36]HRM - Add summatory values of times in attendance.
        ///[004][cpatil][29-09-2020][GEOS2-2221] Identify the transferred employee (Blue Vertical Rectangle) in attendance (#ERF53)
        ///[005][cpatil][29-09-2020][GEOS2-2223] Identify the inactive employees  (Red Vertical Rectangle ) in attendance (#ERF50)
        /// </summary>
        /// <param name="obj"></param>
        private void VisibleIntervalsChanged(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIntervalsChanged()...", category: Category.Info, priority: Priority.Low);
                var values = (object[])obj;
                SchedulerControlEx scheduler = (SchedulerControlEx)values[0];


                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;



                if (scheduler.Month != null)
                {
                    if (scheduler.ActiveViewIndex == 0)
                    {
                        scheduler.DisplayName = String.Format("{0:MMMM yyyy}", scheduler.VisibleIntervals[0].End);
                        SelectedEndDate = SelectedEndDate.AddDays(-1);
                    }
                    else
                    {
                        if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start) < 10)
                        {
                            scheduler.DisplayName = String.Format("Week 0{0} of {1:yyyy}", DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start), scheduler.Start);
                        }
                        else
                        {
                            scheduler.DisplayName = String.Format("Week {0} of {1:yyyy}", DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start), scheduler.Start);
                        }
                    }
                    //[004][005]
                    DateTime dtStartCalenderInterval = new DateTime(scheduler.Month.Value.Year, scheduler.Month.Value.Month, 1);
                    DateTime dtEndCalenderInterval = dtStartCalenderInterval.AddMonths(1).AddDays(-1);

                    foreach (Employee item in Employees)
                    {
                        item.IsEmployeeExitDateInCurrentMonth = false;
                        item.IsEmployeeTransferredCompanyInCurrentMonth = false;
                        if (item.EmployeeExitEvents != null)
                            if (item.EmployeeExitEvents.Any(i => i.ExitDate != null && (i.ExitDate.Value.Month == dtEndCalenderInterval.Date.Month && i.ExitDate.Value.Year == dtEndCalenderInterval.Date.Year)))
                            {
                                item.IsEmployeeExitDateInCurrentMonth = true;
                                item.EmployeeExitDateInCurrentMonth = "Exit Date : " + item.EmployeeExitEvents.Where(i => i.ExitDate.Value.Date <= dtEndCalenderInterval.Date).LastOrDefault().ExitDate.Value.Date.ToShortDateString();
                            }
                            else
                            {
                                if (item.EmployeeContractSituations != null)
                                    if (item.EmployeeContractSituations.Any(ecs => ecs.ContractSituationStartDate.Value.Date <= dtStartCalenderInterval.Date && (ecs.ContractSituationEndDate != null ? ecs.ContractSituationEndDate.Value.Date : DateTime.Now.Date.AddYears(1)) >= dtEndCalenderInterval.Date))
                                    {
                                        item.IsEmployeeExitDateInCurrentMonth = false;
                                        item.EmployeeExitDateInCurrentMonth = "";
                                    }
                                    else if (item.EmployeeContractSituations.Any(ecs => ecs.ContractSituationStartDate.Value.Month == dtEndCalenderInterval.Date.Month && ecs.ContractSituationStartDate.Value.Year == dtEndCalenderInterval.Date.Year))
                                    {
                                        item.IsEmployeeExitDateInCurrentMonth = false;
                                        item.EmployeeExitDateInCurrentMonth = "";
                                    }
                                    else
                                    {
                                        if (item.EmployeeExitEvents != null)
                                            if (item.EmployeeExitEvents.Any(i => i.ExitDate != null && (i.ExitDate.Value.Date <= dtEndCalenderInterval.Date)))
                                            {
                                                item.IsEmployeeExitDateInCurrentMonth = true;
                                                item.EmployeeExitDateInCurrentMonth = "Exit Date : " + item.EmployeeExitEvents.Where(i => i.ExitDate.Value.Date <= dtEndCalenderInterval.Date).LastOrDefault().ExitDate.Value.Date.ToShortDateString();
                                            }
                                    }

                            }

                        if (item.EmployeeContractSituations != null)
                        {
                            List<Int32> idsCompany = new List<int>();
                            List<Int32> idsCompanyDistinct = new List<int>();
                            idsCompany = item.EmployeeContractSituations.Where(i => i.IdCompany != null).Select(i => i.IdCompany.Value).ToList();

                            if (idsCompany != null)
                            {
                                idsCompanyDistinct = idsCompany.Distinct().ToList();
                                if (idsCompanyDistinct.Count() > 1)
                                {
                                    Int32 incre = 1;
                                    foreach (EmployeeContractSituation itemECS in item.EmployeeContractSituations.OrderBy(i => i.ContractSituationStartDate))
                                    {
                                        itemECS.IdSerialNo = incre++;
                                    }
                                    bool isBlueLine = true;
                                    foreach (EmployeeContractSituation itemECSS in item.EmployeeContractSituations.OrderBy(i => i.ContractSituationStartDate))
                                    {
                                        if (itemECSS.ContractSituationStartDate.Value.Month == dtEndCalenderInterval.Month && itemECSS.ContractSituationStartDate.Value.Year == dtEndCalenderInterval.Year)
                                        {
                                            if (itemECSS.IdSerialNo > 1)
                                            {
                                                EmployeeContractSituation employeeCSPrevious = item.EmployeeContractSituations.Where(i => i.IdSerialNo == itemECSS.IdSerialNo - 1).FirstOrDefault();
                                                EmployeeContractSituation employeeCSCurrent = new EmployeeContractSituation();

                                                if (itemECSS.IdSerialNo == item.EmployeeContractSituations.Count())
                                                    employeeCSCurrent = item.EmployeeContractSituations.Where(i => i.IdSerialNo == itemECSS.IdSerialNo).FirstOrDefault();
                                                else
                                                    employeeCSCurrent = item.EmployeeContractSituations.Where(i => i.IdSerialNo == itemECSS.IdSerialNo + 1).FirstOrDefault();

                                                if (employeeCSPrevious != null && employeeCSCurrent != null)
                                                    if (employeeCSPrevious.IdCompany != employeeCSCurrent.IdCompany)
                                                    {

                                                        item.IsEmployeeTransferredCompanyInCurrentMonth = true;
                                                        item.EmployeeTransferredCompanyInCurrentMonth = "New Company : " + employeeCSCurrent.Company.Alias + System.Environment.NewLine + "Leave Date : " + employeeCSPrevious.ContractSituationEndDate.Value.Date.ToShortDateString();
                                                        isBlueLine = true;
                                                        break;
                                                    }
                                            }

                                        }
                                        else if (itemECSS.ContractSituationStartDate.Value.Date <= dtEndCalenderInterval.Date && (itemECSS.ContractSituationEndDate != null ? itemECSS.ContractSituationEndDate.Value.Date : DateTime.Now.AddYears(1).Date) >= dtEndCalenderInterval.Date)
                                        {
                                            if (itemECSS.IdSerialNo > 1)
                                            {
                                                EmployeeContractSituation employeeCSPrevious = item.EmployeeContractSituations.Where(i => i.IdSerialNo == itemECSS.IdSerialNo - 1).FirstOrDefault();
                                                EmployeeContractSituation employeeCSCurrent = new EmployeeContractSituation();
                                                if (itemECSS.IdSerialNo == item.EmployeeContractSituations.Count())
                                                    employeeCSCurrent = item.EmployeeContractSituations.Where(i => i.IdSerialNo == itemECSS.IdSerialNo).FirstOrDefault();
                                                else
                                                    employeeCSCurrent = item.EmployeeContractSituations.Where(i => i.IdSerialNo == itemECSS.IdSerialNo + 1).FirstOrDefault();
                                                if (employeeCSPrevious != null && employeeCSCurrent != null)
                                                    if (employeeCSPrevious.IdCompany != employeeCSCurrent.IdCompany)
                                                    {
                                                        item.IsEmployeeTransferredCompanyInCurrentMonth = true;
                                                        item.EmployeeTransferredCompanyInCurrentMonth = "New Company : " + employeeCSCurrent.Company.Alias + System.Environment.NewLine + "Leave Date : " + employeeCSPrevious.ContractSituationEndDate.Value.Date.ToShortDateString();
                                                        isBlueLine = true;
                                                        break;
                                                    }
                                            }
                                        }


                                    }
                                    if(!isBlueLine)
                                    {
                                        EmployeeContractSituation employeeCSPrevious = item.EmployeeContractSituations.Where(i => i.IdSerialNo == item.EmployeeContractSituations.Count() - 1).FirstOrDefault();
                                        EmployeeContractSituation employeeCSCurrent = item.EmployeeContractSituations.Where(i => i.IdSerialNo == item.EmployeeContractSituations.Count()).FirstOrDefault();
                                        if (employeeCSPrevious != null && employeeCSCurrent != null)
                                            if (employeeCSPrevious.IdCompany != employeeCSCurrent.IdCompany)
                                            {
                                                item.IsEmployeeTransferredCompanyInCurrentMonth = true;
                                                item.EmployeeTransferredCompanyInCurrentMonth = "New Company : " + employeeCSCurrent.Company.Alias + System.Environment.NewLine + "Leave Date : " + employeeCSPrevious.ContractSituationEndDate.Value.Date.ToShortDateString();
                                                isBlueLine = true;
                                                break;
                                            }
                                    }
                                }



                                }
                            }

                        }
                    }
             
                if (IsEmployeewiseRegisterAndExpectDays)
                {
                    MonthlyAllRegisterHours(scheduler);
                }


                GeosApplication.Instance.Logger.Log("Method VisibleIntervalsChanged()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method VisibleIntervalsChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        /// <summary>
        /// [001][SP-65][skale][11-06-2019][GEOS2-1556]Grid data reflection problems
        ///[002][spawar][12-03-2020][GEOS2-36]HRM - Add summatory values of times in attendance.
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerEditValueChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

            var values = (object[])obj;
            SchedulerControlEx schedulerControlEx = (SchedulerControlEx)values[0];
            schedulerControlEx.Uid = "Refresh";
            //[002] Added
            IsEmployeewiseRegisterAndExpectDays = false;
            //
            //[001] added
            if (IsSchedulerViewVisible == Visibility.Visible)
            {
                AccordionControl accordionControl = (AccordionControl)values[1];
                var searchControl = LayoutTreeHelper.GetVisualChildren(accordionControl).OfType<SearchControl>().FirstOrDefault();
                if (searchControl != null)
                    searchControl.SearchText = null;
            }
            else
                MyFilterString = string.Empty;
            //end

            //if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            //{
            //    return;
            //}
            if (!DXSplashScreen.IsActive)
            {
                DXSplashScreen.Show(x =>
                {
                    Window win = new Window()
                    {
                        ShowActivated = false,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = ResizeMode.NoResize,
                        AllowsTransparency = true,
                        Background = new SolidColorBrush(Colors.Transparent),
                        ShowInTaskbar = false,
                        Topmost = true,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    };
                    WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }

            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            {
                FillAttendanceListByPlant();
            }
            else
            {
                Employees = new ObservableCollection<Employee>();
                EmployeeAttendanceList.Clear();
                AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();
            }
            schedulerControlEx.Uid = string.Empty;
            schedulerControlEx.Tag = string.Empty;
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// [001][SP-65][skale][11-06-2019][GEOS2-1556]Grid data reflection problems
        /// [002][spawar][12-03-2020][GEOS2-36]HRM - Add summatory values of times in attendance.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectedYearChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

            var values = (object[])obj;
            SchedulerControlEx scheduler = (SchedulerControlEx)values[0];
            int year = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
            DateTime newDt = new DateTime(year, DateTime.Now.Month, 1);

            scheduler.Month = newDt;
            DateTime start = newDt;
            DateTime end = start.AddDays(1);
            scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);
            //[002] Added
            IsEmployeewiseRegisterAndExpectDays = false;
            //
            scheduler.Uid = "Refresh";
            //[001] added
            if (IsSchedulerViewVisible == Visibility.Visible)
            {
                AccordionControl accordionControl = (AccordionControl)values[1];
                var searchControl = LayoutTreeHelper.GetVisualChildren(accordionControl).OfType<SearchControl>().FirstOrDefault();
                if (searchControl != null)
                    searchControl.SearchText = null;
            }
            else
                MyFilterString = string.Empty;
            //end
            //if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            //{
            //    return;
            //}

            if (!DXSplashScreen.IsActive)
            {
                DXSplashScreen.Show(x =>
                {
                    Window win = new Window()
                    {
                        ShowActivated = false,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = ResizeMode.NoResize,
                        AllowsTransparency = true,
                        Background = new SolidColorBrush(Colors.Transparent),
                        ShowInTaskbar = false,
                        Topmost = true,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    };
                    WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }

            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            {
                FillAttendanceListByPlant();
            }
            else
            {
                Employees = new ObservableCollection<Employee>();
                EmployeeAttendanceList.Clear();
                AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();
            }

            scheduler.Uid = string.Empty;
            scheduler.Tag = string.Empty;
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        ///[001]  Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri   
        /// [002] [cpatil][2020-01-22][GEOS2-2008] We can not see some the Employees in LEAVES and ATTENDANCE with Period = 2019, but the same employees in 2020 we can see.
        /// [003] [avpawar][2020-07-24][GEOS2-2432] changed service method because we got error message when try edit the attendance shift.
        /// </summary>
        private void FillAttendanceListByPlant()

        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContactListByPlant ...", category: Category.Info, priority: Priority.Low);
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    // [002] Changed service method GetAllEmployeesForAttendanceByIdCompany_V2037 to GetAllEmployeesForAttendanceByIdCompany_V2039
                    Employees = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    // [002] Changed service method GetSelectedIdCompanyEmployeeAttendance_V2037 to GetSelectedIdCompanyEmployeeAttendance_V2039

                    // [003] Changed service method GetSelectedIdCompanyEmployeeAttendance_V2044 to GetSelectedIdCompanyEmployeeAttendance_V2045
                    EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>(HrmService.GetSelectedIdCompanyEmployeeAttendance_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    SetIsManual(EmployeeAttendanceList);
                    CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds));
                    // [002] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2032 to GetEmployeeLeavesBySelectedIdCompany_V2039
                    EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2045(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    HolidayList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(28).AsEnumerable());

                    LabelItems = new ObservableCollection<LabelHelper>();
                    StatusItems = new ObservableCollection<StatusHelper>();

                    //[001] Code Comment 
                    // CompanyWorksList = new ObservableCollection<CompanyWork>(HrmService.GetAllCompanyWorks());

                    ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();

                    foreach (CompanyHoliday CompanyHoliday in CompanyHolidays)
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = CompanyHoliday.Name;
                        modelAppointment.StartDate = CompanyHoliday.StartDate;
                        modelAppointment.EndDate = CompanyHoliday.EndDate;
                        modelAppointment.Label = CompanyHoliday.IdHoliday;
                        if (CompanyHoliday.IsRecursive == 1)
                        {
                            modelAppointment.Type = (int)DevExpress.XtraScheduler.AppointmentType.Pattern;
                            modelAppointment.RecurrenceInfo = new RecurrenceInfo
                            {
                                Type = RecurrenceType.Yearly,
                                Periodicity = 1,
                                Month = CompanyHoliday.StartDate.Value.Month,
                                WeekOfMonth = WeekOfMonth.None,
                                DayNumber = CompanyHoliday.StartDate.Value.Day,
                                Range = RecurrenceRange.NoEndDate
                            }.ToXml();
                        }

                        appointment.Add(modelAppointment);
                    }



                    //foreach (EmployeeAttendance employeeAttendance in EmployeeAttendanceList)
                    //{
                    //    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                    //    // modelAppointment.Subject = string.Format("[{0}] {1}", employeeAttendance.Employee.TotalWorkedHours, employeeAttendance.CompanyWork.Name);
                    //    modelAppointment.Subject = string.Format("{0}", employeeAttendance.CompanyWork.Name);
                    //    modelAppointment.StartDate = employeeAttendance.StartDate;
                    //    modelAppointment.EndDate = employeeAttendance.EndDate;
                    //    modelAppointment.Label = employeeAttendance.IdCompanyWork;
                    //    modelAppointment.IdEmployeeAttendance = employeeAttendance.IdEmployeeAttendance;
                    //    appointment.Add(modelAppointment);
                    //}

                    //[001] Code Comment and Label added as per Lookupvalue
                    //foreach (var CompanyWork in CompanyWorksList)
                    //{
                    //    LabelHelper labelForCompanyWork = new LabelHelper();
                    //    labelForCompanyWork.Id = CompanyWork.IdCompanyWork;
                    //    labelForCompanyWork.Color = new BrushConverter().ConvertFromString(CompanyWork.HtmlColor.ToString()) as SolidColorBrush;
                    //    labelForCompanyWork.Caption = CompanyWork.Name;
                    //    LabelItems.Add(labelForCompanyWork);
                    //}


                    foreach (var AttendenceType in GeosApplication.Instance.AttendanceTypeList)
                    {
                        if (AttendenceType.IdLookupValue > 0 && AttendenceType.HtmlColor != null && AttendenceType.HtmlColor != string.Empty)
                        {
                            LabelHelper labelForCompanyWork = new LabelHelper();
                            labelForCompanyWork.Id = AttendenceType.IdLookupValue;
                            labelForCompanyWork.Color = new BrushConverter().ConvertFromString(AttendenceType.HtmlColor.ToString()) as SolidColorBrush;
                            labelForCompanyWork.Caption = AttendenceType.Value;
                            LabelItems.Add(labelForCompanyWork);
                        }
                    }

                    foreach (var Holiday in HolidayList)
                    {
                        if (Holiday.HtmlColor != null && Holiday.HtmlColor != string.Empty)
                        {
                            LabelHelper labelForCompanyHoliday = new LabelHelper();
                            labelForCompanyHoliday.Id = Holiday.IdLookupValue;
                            labelForCompanyHoliday.Color = new BrushConverter().ConvertFromString(Holiday.HtmlColor.ToString()) as SolidColorBrush;
                            labelForCompanyHoliday.Caption = Holiday.Value;
                            LabelItems.Add(labelForCompanyHoliday);
                        }
                    }

                    int count = 2;
                    for (int i = 0; i < count; i++)
                    {
                        StatusHelper statusItem = new StatusHelper();
                        statusItem.Id = i;
                        statusItem.Brush = new SolidColorBrush(Colors.SlateBlue); //BrushConverter().ConvertFromString("#7833FF ") as SolidColorBrush;
                        statusItem.Caption = "Night Shift";
                        StatusItems.Add(statusItem);
                    }

                    AppointmentItems = appointment;
                }

                GeosApplication.Instance.Logger.Log("Method FillContactListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        private void OpenAttendanceFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenAttendanceFile()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)((object[])obj)[0];
                ImportAttendanceFileView importAttendanceFileView = new ImportAttendanceFileView();
                ImportAttendanceFileViewModel importAttendanceFileViewModel = new ImportAttendanceFileViewModel();
                importAttendanceFileViewModel.Init(EmployeeAttendanceList);
                EventHandler handle = delegate { importAttendanceFileView.Close(); };
                importAttendanceFileViewModel.RequestClose += handle;
                importAttendanceFileView.DataContext = importAttendanceFileViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                importAttendanceFileView.Owner = Window.GetWindow(ownerInfo);
                importAttendanceFileView.ShowDialog();

                if (importAttendanceFileViewModel.IsSave)
                {

                    EmployeeAttendanceToAppointments(importAttendanceFileViewModel.EmpAddedAttendanceList);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenAttendanceFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenAttendanceFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>       
        ///[001]Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri    
        ///[002][skale][07-08-2019][GEOS2-1694]HRM - Attendance green visualization
        ///[003][spawar][12-03-2020][GEOS2-36]HRM - Add summatory values of times in attendance.
        ///[004][smazhar][26-08-2020][GEOS2-2553]If we add a leave with an “All day Event” in the Attendance section, it is represented with 2 days.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectItemForScheduler(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectItemForScheduler()...", category: Category.Info, priority: Priority.Low);

                if (AppointmentItems.Count != 0)
                    AppointmentItems.Clear();

                var values = (object[])obj;
                SchedulerControlEx schedulerControlEx = (SchedulerControlEx)values[0];
                schedulerControlEx.SelectedEmployeeHireDate = null;
                ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();
                IsSet = true;

                if (SelectedItem is Employee)
                {

                    schedulerControlEx.Tag = "OnSelection";
                    schedulerControlEx.SelectedEmployeeHireDate = ((Employee)SelectedItem).EmployeeContractSituation.ContractSituationEndDate;
                    // schedulerControlEx.SelectedEmployeeEndDate = ((Employee)SelectedItem).EmployeeContractSituation.ContractSituationEndDate;

                    schedulerControlEx.SelectedEmployeeEndDate = (((Employee)SelectedItem)).EmployeeContractSituations.FirstOrDefault(x => x.ContractSituationEndDate == null) != null ?
                                                             GeosApplication.Instance.ServerDateTime.Date :
                                                             ((Employee)SelectedItem).EmployeeContractSituations.OrderByDescending(x => x.ContractSituationEndDate).ToList()
                                                             [0].ContractSituationEndDate;
                    //[002] Added
                    schedulerControlEx.SelectedEmployeeContractSituationList = new List<EmployeeContractSituation>();
                    if (((Employee)SelectedItem).EmployeeContractSituations.Count > 0)
                    {
                        schedulerControlEx.SelectedEmployeeContractSituationList = ((Employee)SelectedItem).EmployeeContractSituations;
                    }
                    //end
                    if (((Employee)SelectedItem).CompanyShift != null)
                    {
                        if (((Employee)SelectedItem).CompanyShift.CompanyAnnualSchedule != null)
                        {
                            string weekdays = ((Employee)SelectedItem).CompanyShift.CompanyAnnualSchedule.WorkingDays;
                            schedulerControlEx.SelectedEmployeeWorkingDays = weekdays.Split(',');
                        }
                    }
                    // List<EmployeeAttendance> MonthlyAttendanceList = EmployeeAttendanceList.Where(x => x.IdEmployee == ((Employee)SelectedItem).IdEmployee && x.StartDate.Date.ToString("MM") == schedulerControlEx.Month.Value.Date.ToString("MM")).ToList();
                    IsVisible = true;
                    List<EmployeeAttendance> AttendanceList = EmployeeAttendanceList.Where(x => x.IdEmployee == ((Employee)SelectedItem).IdEmployee).ToList();


                    if (EmployeeLeaves != null)
                    {
                        List<EmployeeLeave> SelectedEmployeeLeave = EmployeeLeaves.Where(x => x.IdEmployee == ((Employee)SelectedItem).IdEmployee).ToList();

                        foreach (EmployeeLeave employeeLeave in SelectedEmployeeLeave)
                        {
                            for (var day = employeeLeave.StartDate.Value.Date; day.Date <= employeeLeave.EndDate.Value.Date; day = day.AddDays(1))
                            {
                                EmployeeLeave e = new EmployeeLeave();
                                e = (EmployeeLeave)employeeLeave.Clone();
                                e.StartDate = day;
                                e.EndDate = day;
                                SelectedEmployeeLeavesAsPerDate.Add(e);
                            }
                        }

                    }

                    foreach (EmployeeAttendance EmpAttendance in AttendanceList)
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        //[001] Changes in Subject as per Lookup Value
                        //modelAppointment.Subject = string.Format("[{0}] {1}", (EmpAttendanceList.Employee.TotalWorkedHours), EmpAttendanceList.CompanyWork.Name);
                        // modelAppointment.Subject = string.Format("{0}", EmpAttendanceList.CompanyWork.Name);
                        //modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList[GeosApplication.Instance.AttendanceTypeList.FindIndex(x => x.IdLookupValue == EmpAttendanceList.IdCompanyWork)].Value);

                        if (EmpAttendance.IsManual == 1)
                        {
                            modelAppointment.Description = "[Manual] ";
                        }

                        if (EmpAttendance.CompanyShift.IsNightShift == 1)
                        {
                            modelAppointment.Description = modelAppointment.Description + "[Night shift]";
                            modelAppointment.IsNightShift = 1;
                        }

                        if (EmpAttendance.IsManual == 1)
                        {
                            modelAppointment.AttendanceIsManual = true;
                        }
                        else
                        {
                            modelAppointment.AttendanceIsManual = false;
                        }

                        modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == EmpAttendance.IdCompanyWork).Value);
                        //else
                        //    modelAppointment.Subject = string.Format("{0} [{1}]", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == EmpAttendanceList.IdCompanyWork).Value ,"Night Shift");
                        modelAppointment.StartDate = EmpAttendance.StartDate;
                        modelAppointment.EndDate = EmpAttendance.EndDate;
                        modelAppointment.Label = EmpAttendance.IdCompanyWork;
                        modelAppointment.IdEmployeeAttendance = EmpAttendance.IdEmployeeAttendance;
                        modelAppointment.DailyHoursCount = 0;

                        if (EmpAttendance.CompanyShift != null)
                        {
                            if (EmpAttendance.CompanyShift.CompanyAnnualSchedule != null)
                                modelAppointment.DailyHoursCount = EmpAttendance.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                        }
                        else if (EmpAttendance.Employee.CompanyShift != null)
                        {
                            if (EmpAttendance.Employee.CompanyShift.CompanyAnnualSchedule != null)
                                modelAppointment.DailyHoursCount = EmpAttendance.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                        }
                        else
                            modelAppointment.DailyHoursCount = 0;

                        var EmployeeLeaveForSelectedAttendance = SelectedEmployeeLeavesAsPerDate.Where(x => x.StartDate == EmpAttendance.StartDate.Date);
                        modelAppointment.TotalLeaveDurationInHours = 0;

                        if (EmployeeLeaveForSelectedAttendance.Count() > 0)
                        {
                            foreach (EmployeeLeave item in EmployeeLeaveForSelectedAttendance)
                            {
                                TimeSpan fromH = item.StartTime.Value;
                                TimeSpan toH = item.EndTime.Value;
                                TimeSpan hourTotalSpan = toH.Subtract(fromH);
                                modelAppointment.TotalLeaveDurationInHours = modelAppointment.TotalLeaveDurationInHours + hourTotalSpan.Hours + hourTotalSpan.Minutes;
                            }
                        }
                        modelAppointment.AccountingDate = EmpAttendance.AccountingDate;

                        AppointmentItems.Add(modelAppointment);
                    }

                    string[] employeeWorkingDays = (((Employee)SelectedItem).CompanyShift.CompanyAnnualSchedule.WorkingDays).Split(',');
                    var EmpLeaves = EmployeeLeaves.Where(x => x.IdEmployee == ((Employee)SelectedItem).IdEmployee).ToList();
                    List<EmployeeLeave> tempLeaveList = new List<EmployeeLeave>();
                    if (Setting == null)
                    {
                        foreach (var EmployeeLeave in EmpLeaves)
                        {
                            EmployeeLeave newLeave = new EmployeeLeave();
                            newLeave.StartDate = EmployeeLeave.StartDate;
                            newLeave.EndDate = EmployeeLeave.EndDate;
                            newLeave.CompanyLeave = EmployeeLeave.CompanyLeave;
                            newLeave.Employee = EmployeeLeave.Employee;
                            newLeave.EndTime = EmployeeLeave.EndTime;
                            newLeave.IdEmployee = EmployeeLeave.IdEmployee;
                            newLeave.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                            newLeave.IdLeave = EmployeeLeave.IdLeave;
                            newLeave.IsAllDayEvent = EmployeeLeave.IsAllDayEvent;
                            newLeave.StartTime = EmployeeLeave.StartTime;
                            tempLeaveList.Add(newLeave);
                        }
                    }
                    else if (Setting.DefaultValue.Equals("Natural"))
                    {
                        foreach (var EmployeeLeave in EmpLeaves)
                        {
                            EmployeeLeave newLeave = new EmployeeLeave();
                            newLeave.StartDate = EmployeeLeave.StartDate;
                            newLeave.EndDate = EmployeeLeave.EndDate;
                            newLeave.CompanyLeave = EmployeeLeave.CompanyLeave;
                            newLeave.Employee = EmployeeLeave.Employee;
                            newLeave.EndTime = EmployeeLeave.EndTime;
                            newLeave.IdEmployee = EmployeeLeave.IdEmployee;
                            newLeave.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                            newLeave.IdLeave = EmployeeLeave.IdLeave;
                            newLeave.IsAllDayEvent = EmployeeLeave.IsAllDayEvent;
                            newLeave.StartTime = EmployeeLeave.StartTime;
                            tempLeaveList.Add(newLeave);
                        }

                    }
                    else
                    {
                        List<EmployeeLeave> tempEmployeeLeaveList = new List<EmployeeLeave>();
                        List<EmployeeLeave> leaveList = new List<EmployeeLeave>();
                        foreach (var EmployeeLeave in EmpLeaves)
                        {
                            if (EmployeeLeave.CompanyLeave.Company != null)
                            {
                                if (EmployeeLeave.CompanyLeave.Company.CompanySetting != null)
                                {
                                    if (Setting.DefaultValue.Equals(EmployeeLeave.CompanyLeave.Company.CompanySetting.Value))
                                    {
                                        leaveList = FillEmployeeLeaveList(EmployeeLeave);
                                        foreach (var item in leaveList)
                                        {
                                            tempEmployeeLeaveList.Add(item);
                                        }

                                    }
                                    else
                                    {
                                        tempEmployeeLeaveList.Add(EmployeeLeave);
                                    }
                                }
                                else
                                {
                                    tempEmployeeLeaveList.Add(EmployeeLeave);
                                }
                            }
                            else
                            {
                                tempEmployeeLeaveList.Add(EmployeeLeave);
                            }

                        }
                        tempLeaveList.AddRange(tempEmployeeLeaveList);

                    }
                 

                    foreach (var EmployeeLeave in tempLeaveList)
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = string.Format("{0} {1}-{2}", EmployeeLeave.Employee.FirstName, EmployeeLeave.Employee.LastName, EmployeeLeave.CompanyLeave.Name);
                        modelAppointment.StartDate = EmployeeLeave.StartDate;
                        //[004]
                        if (Convert.ToBoolean(EmployeeLeave.IsAllDayEvent))
                        {
                            modelAppointment.EndDate = EmployeeLeave.EndDate.Value;
                        }
                        else
                        {
                            modelAppointment.EndDate = EmployeeLeave.EndDate;
                        }

                        modelAppointment.Label = EmployeeLeave.IdLeave;
                        modelAppointment.EmployeeCode = EmployeeLeave.Employee.EmployeeCode;
                        modelAppointment.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                        modelAppointment.IsAllDayEvent = EmployeeLeave.IsAllDayEvent;
                        var CompanyShift = HrmService.GetCompanyShiftDetailByIdCompanyShift(EmployeeLeave.Employee.IdCompanyShift);

                        if (CompanyShift != null)
                        {
                            if (CompanyShift.CompanyAnnualSchedule != null)
                            {
                                modelAppointment.DailyHoursCount = CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                            }
                            else
                                modelAppointment.DailyHoursCount = 0;
                        }
                        else
                            modelAppointment.DailyHoursCount = 0;

                        AppointmentItems.Add(modelAppointment);
                       
                    }

                    //[003] Added
                    IsEmployeewiseRegisterAndExpectDays = true;
                    MonthlyAllRegisterHours(schedulerControlEx);
                    //
                }

                foreach (CompanyHoliday CompanyHoliday in CompanyHolidays)
                {
                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                    modelAppointment.Subject = CompanyHoliday.Name;
                    modelAppointment.StartDate = CompanyHoliday.StartDate;
                    modelAppointment.EndDate = CompanyHoliday.EndDate;
                    modelAppointment.Label = CompanyHoliday.IdHoliday;

                    if (CompanyHoliday.IsRecursive == 1)
                    {
                        modelAppointment.Type = (int)DevExpress.XtraScheduler.AppointmentType.Pattern;
                        modelAppointment.RecurrenceInfo = new RecurrenceInfo
                        {
                            Type = RecurrenceType.Yearly,
                            Periodicity = 1,
                            Month = CompanyHoliday.StartDate.Value.Month,
                            WeekOfMonth = WeekOfMonth.None,
                            DayNumber = CompanyHoliday.StartDate.Value.Day,
                            Range = RecurrenceRange.NoEndDate
                        }.ToXml();
                    }

                    AppointmentItems.Add(modelAppointment);
                }

                foreach (var EmployeeLeaveType in GeosApplication.Instance.EmployeeLeaveList)
                {
                    if (EmployeeLeaveType.IdLookupValue > 0 && EmployeeLeaveType.HtmlColor != null && EmployeeLeaveType.HtmlColor != string.Empty)
                    {
                        LabelHelper label = new LabelHelper();
                        label.Id = Convert.ToInt32(EmployeeLeaveType.IdLookupValue);
                        label.Color = new BrushConverter().ConvertFromString(EmployeeLeaveType.HtmlColor.ToString()) as SolidColorBrush;
                        label.Caption = EmployeeLeaveType.Value;
                        LabelItems.Add(label);
                    }
                }
                //StatusItems
                IsSet = false;
                GeosApplication.Instance.Logger.Log("Method SelectItemForScheduler()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectItemForScheduler()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Refresh Attendance View
        /// [001]  Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// [002] [cpatil][2020-01-22][GEOS2-2008] We can not see some the Employees in LEAVES and ATTENDANCE with Period = 2019, but the same employees in 2020 we can see.
        /// ///[003][spawar][12-03-2020][GEOS2-36]HRM - Add summatory values of times in attendance.
        /// [004] [avpawar][2020-07-24][GEOS2-2432] changed service method because we got error message when try edit the attendance shift.
        /// </summary>
        /// <param name="obj"></param>
        public void RefreshAttendanceView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshAttendanceView()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                var values = (object[])obj;
                SchedulerControlEx schedulerControlEx = (SchedulerControlEx)values[0];
                int year = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                DateTime newDt = new DateTime(year, SelectedStartDate.Month, 1);
                schedulerControlEx.Month = newDt;
                DateTime start = newDt;
                DateTime end = start.AddDays(1);
                schedulerControlEx.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);
                schedulerControlEx.Uid = "Refresh";

                AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();

                //DateTime now = DateTime.Now;
                //var startDate = new DateTime(now.Year, now.Month, 1);
                //schedulerControlEx.Month = startDate;
                //schedulerControlEx.Start = startDate;

                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                GeosApplication.Instance.FillFinancialYear();
                // SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;

                AccordionControl accordionControl = (AccordionControl)values[1];
                var searchControl = LayoutTreeHelper.GetVisualChildren(accordionControl).OfType<SearchControl>().FirstOrDefault();
                if (searchControl != null)
                    searchControl.SearchText = null;
                //Sprint 42---To clear Search String of GridView----sdesai
                TableView detailView = (TableView)values[2];
                GridControl gridControl = (detailView).Grid;
                detailView.SearchString = null;

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {


                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    // [002] Changed service method GetSelectedIdCompanyEmployeeAttendance_V2037 to GetSelectedIdCompanyEmployeeAttendance_V2039

                    // [004] Changed service method GetSelectedIdCompanyEmployeeAttendance_V2044 to GetSelectedIdCompanyEmployeeAttendance_V2045
                    EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>(HrmService.GetSelectedIdCompanyEmployeeAttendance_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    SetIsManual(EmployeeAttendanceList);
                    CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds));
                    // [002] Changed service method GetAllEmployeesForAttendanceByIdCompany_V2037 to GetAllEmployeesForAttendanceByIdCompany_V2039
                    Employees = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    // [002] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2032 to GetEmployeeLeavesBySelectedIdCompany_V2039
                    EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    HolidayList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(28).AsEnumerable());
                    LabelItems = new ObservableCollection<LabelHelper>();
                    StatusItems = new ObservableCollection<StatusHelper>();

                    //[001] Code Comment
                    //  CompanyWorksList = new ObservableCollection<CompanyWork>(HrmService.GetAllCompanyWorks());
                    SelectedItem = null;
                    ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();

                    foreach (CompanyHoliday CompanyHoliday in CompanyHolidays)
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = CompanyHoliday.Name;
                        modelAppointment.StartDate = CompanyHoliday.StartDate;
                        modelAppointment.EndDate = CompanyHoliday.EndDate;
                        modelAppointment.Label = CompanyHoliday.IdHoliday;
                        if (CompanyHoliday.IsRecursive == 1)
                        {
                            modelAppointment.Type = (int)DevExpress.XtraScheduler.AppointmentType.Pattern;
                            modelAppointment.RecurrenceInfo = new RecurrenceInfo
                            {
                                Type = RecurrenceType.Yearly,
                                Periodicity = 1,
                                Month = CompanyHoliday.StartDate.Value.Month,
                                WeekOfMonth = WeekOfMonth.None,
                                DayNumber = CompanyHoliday.StartDate.Value.Day,
                                Range = RecurrenceRange.NoEndDate
                            }.ToXml();
                        }

                        appointment.Add(modelAppointment);
                    }

                    //[001] Code Comment
                    //foreach (var CompanyWork in CompanyWorksList)
                    //{
                    //    LabelHelper labelForCompanyWork = new LabelHelper();
                    //    labelForCompanyWork.Id = CompanyWork.IdCompanyWork;
                    //    labelForCompanyWork.Color = new BrushConverter().ConvertFromString(CompanyWork.HtmlColor.ToString()) as SolidColorBrush;
                    //    labelForCompanyWork.Caption = CompanyWork.Name;
                    //    LabelItems.Add(labelForCompanyWork);
                    //}

                    foreach (var AttendenceType in GeosApplication.Instance.AttendanceTypeList)
                    {
                        if (AttendenceType.IdLookupValue > 0 && AttendenceType.HtmlColor != null && AttendenceType.HtmlColor != string.Empty)
                        {
                            LabelHelper labelForCompanyWork = new LabelHelper();
                            labelForCompanyWork.Id = AttendenceType.IdLookupValue;
                            labelForCompanyWork.Color = new BrushConverter().ConvertFromString(AttendenceType.HtmlColor.ToString()) as SolidColorBrush;
                            labelForCompanyWork.Caption = AttendenceType.Value;
                            LabelItems.Add(labelForCompanyWork);
                        }
                    }

                    foreach (var Holiday in HolidayList)
                    {
                        if (Holiday.HtmlColor != null && Holiday.HtmlColor != string.Empty)
                        {
                            LabelHelper labelForCompanyHoliday = new LabelHelper();
                            labelForCompanyHoliday.Id = Holiday.IdLookupValue;
                            labelForCompanyHoliday.Color = new BrushConverter().ConvertFromString(Holiday.HtmlColor.ToString()) as SolidColorBrush;
                            labelForCompanyHoliday.Caption = Holiday.Value;
                            LabelItems.Add(labelForCompanyHoliday);
                        }
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        StatusHelper statusItem = new StatusHelper();
                        statusItem.Id = i;
                        statusItem.Brush = new SolidColorBrush(Colors.SlateBlue); //BrushConverter().ConvertFromString("#7833FF ") as SolidColorBrush;
                        statusItem.Caption = "Night Shift";
                        StatusItems.Add(statusItem);
                    }

                    AppointmentItems = appointment;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                else
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }


                SelectedItem = null;
                schedulerControlEx.Tag = string.Empty;
                schedulerControlEx.Uid = string.Empty;

                //[003] Added.
                IsEmployeewiseRegisterAndExpectDays = false;
                //

                GeosApplication.Instance.Logger.Log("Method RefreshAttendanceView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                // IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshAttendanceView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Show Attendance Scheduler View. 
        /// By Amit For Task  [HRM-M042-08] grid view in attendance
        /// </summary>
        private void ShowAttendanceSchedulerView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAttendanceSchedulerView ...", category: Category.Info, priority: Priority.Low);

                IsSchedulerViewVisible = Visibility.Visible;
                IsGridViewVisible = Visibility.Hidden;

                GeosApplication.Instance.Logger.Log("Method ShowAttendanceSchedulerView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowAttendanceSchedulerView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Show Attendance Grid View. 
        /// </summary>
        private void ShowAttendanceGridView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAttendanceGridView ...", category: Category.Info, priority: Priority.Low);
                SchedulerControlEx scheduler = (SchedulerControlEx)obj;

                //DateTime now = DateTime.Now;
                //var startDate = new DateTime(now.Year, now.Month, 1);
                //scheduler.Month = startDate;
                //scheduler.ActiveViewIndex = 0;
                //scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(DateTime.Today, DateTime.Today.AddDays(1));
                //SelectedStartDate = scheduler.SelectedInterval.Start;
                //SelectedEndDate = scheduler.SelectedInterval.End;
                //SelectedEndDate = SelectedEndDate.AddDays(-1);


                IsGridViewVisible = Visibility.Visible;
                IsSchedulerViewVisible = Visibility.Hidden;

                GeosApplication.Instance.Logger.Log("Method ShowAttendanceGridView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowAttendanceGridView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for Export Attendance Details in Excel Sheet. 
        /// </summary>
        private void ExportAttendancetList(object obj)
        {
            try
            {
               
                GeosApplication.Instance.Logger.Log("Method ExportAttendancetList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Attendance List";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            Window win = new Window()
                            {
                                ShowActivated = false,
                                WindowStyle = WindowStyle.None,
                                ResizeMode = ResizeMode.NoResize,
                                AllowsTransparency = true,
                                Background = new SolidColorBrush(Colors.Transparent),
                                ShowInTaskbar = false,
                                Topmost = true,
                                SizeToContent = SizeToContent.WidthAndHeight,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            };
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    ResultFileName = (saveFile.FileName);
                    TableView departmentTableView = ((TableView)obj);
                    departmentTableView.ShowTotalSummary = false;
                    
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                   
                    options.ExportType = ExportType.WYSIWYG;
                   
                    options.CustomizeCell += Options_CustomizeCell;
                    
                    departmentTableView.ShowFixedTotalSummary = false;
                  
                    departmentTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    departmentTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("Method ExportAttendancetList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttendancetList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [20200812][srowlo][GEOS2-2464]HRM - Attendance: Grid view export excel - Accounting Date
        /// </summary>
        /// <param name="e"></param>
        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            // XlFormattingObject rowFormatting = new XlFormattingObject();
            // rowFormatting.Font = new XlCellFont { Bold = true, Size = 14 ,Name= "Calibri" };
            //// rowFormatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            // // row.Formatting = rowFormatting;
            // e.Formatting = rowFormatting;
            if (e.ColumnFieldName == "AccountingDate")
            {
                if (e.Value != null && e.Value.ToString() != "Accounting Date" && e.ColumnFieldName == "AccountingDate")
                {
                    e.Value = string.Format("{0:dd/MM/yyyy}", (DateTime)e.Value);

                }
            }


            e.Handled = true;
        }
        
        private List<EmployeeLeave> FillEmployeeLeaveList(EmployeeLeave NewEmpLeave)
        {
            List<EmployeeLeave> tempEmployeeLeaveList = new List<EmployeeLeave>();
            string[] employeeWorkingDays;
            if (NewEmpLeave.CompanyLeave.Company.CompanyAnnualSchedule != null)
            {
                employeeWorkingDays = NewEmpLeave.CompanyLeave.Company.CompanyAnnualSchedule.WorkingDays.Split(',');
            }
            else
            {
                employeeWorkingDays = NewEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.WorkingDays.Split(',');
            }

            List<EmployeeLeave> tempLeaveList = new List<EmployeeLeave>();
            bool isLeave = false;
          
            //[001]
            var tempStartDate = NewEmpLeave.StartDate.Value.Date;
            var tempEndDate = NewEmpLeave.EndDate.Value.Date;
            for (var item = tempStartDate; item.Date <= tempEndDate.Date; item = item.AddDays(1))
            {
                EmployeeLeave tempLeave = new EmployeeLeave();
                EmployeeLeave newLeave = new EmployeeLeave();

                if (employeeWorkingDays.Any(x => x.Contains(item.DayOfWeek.ToString().Substring(0, 3))))
                {
                    tempLeave.Employee = NewEmpLeave.Employee;
                    tempLeave.StartDate = item.Date.AddHours(NewEmpLeave.StartDate.Value.TimeOfDay.Hours).AddMinutes(NewEmpLeave.StartDate.Value.TimeOfDay.Minutes);
                    tempLeave.EndDate = item.Date.AddHours(NewEmpLeave.EndDate.Value.TimeOfDay.Hours).AddMinutes(NewEmpLeave.EndDate.Value.TimeOfDay.Minutes);
                    tempLeave.IdEmployee = NewEmpLeave.IdEmployee;
                    tempLeave.IdLeave = NewEmpLeave.IdLeave;
                    tempLeave.IdEmployeeLeave = NewEmpLeave.IdEmployeeLeave;
                    tempLeave.IsAllDayEvent = NewEmpLeave.IsAllDayEvent;
                    tempLeave.CompanyLeave = NewEmpLeave.CompanyLeave;
                    tempLeaveList.Add(tempLeave);
                    isLeave = true;
                }
                else
                {
                    if (tempLeaveList.Count > 0 && isLeave)
                    {
                        EmployeeLeave tempStartLeave = tempLeaveList.FirstOrDefault();
                        EmployeeLeave tempEndLeave = tempLeaveList.LastOrDefault();
                        newLeave.Employee = tempStartLeave.Employee;
                        newLeave.StartDate = tempStartLeave.StartDate;
                        newLeave.EndDate = tempEndLeave.EndDate;
                        newLeave.IdEmployee = tempStartLeave.IdEmployee;
                        newLeave.IdLeave = tempStartLeave.IdLeave;
                        newLeave.IdEmployeeLeave = tempStartLeave.IdEmployeeLeave;
                        newLeave.IsAllDayEvent = tempStartLeave.IsAllDayEvent;
                        newLeave.CompanyLeave = tempStartLeave.CompanyLeave;
                        tempEmployeeLeaveList.Add(newLeave);
                        isLeave = false;
                        tempLeaveList = new List<EmployeeLeave>();
                    }
                }
            }

            if (tempLeaveList.Count > 0 && isLeave)
            {
                EmployeeLeave newLeave = new EmployeeLeave();
                EmployeeLeave tempStartLeave = tempLeaveList.FirstOrDefault();
                EmployeeLeave tempEndLeave = tempLeaveList.LastOrDefault();
                newLeave.Employee = tempStartLeave.Employee;
                newLeave.StartDate = tempStartLeave.StartDate;
                newLeave.EndDate = tempEndLeave.EndDate;
                newLeave.IdEmployee = tempStartLeave.IdEmployee;
                newLeave.IdLeave = tempStartLeave.IdLeave;
                newLeave.IdEmployeeLeave = tempStartLeave.IdEmployeeLeave;
                newLeave.IsAllDayEvent = tempStartLeave.IsAllDayEvent;
                newLeave.CompanyLeave = tempStartLeave.CompanyLeave;
                tempEmployeeLeaveList.Add(newLeave);
                isLeave = false;
                tempLeaveList = new List<EmployeeLeave>();
            }
            return tempEmployeeLeaveList;
        }

        /// <summary>
        /// </summary>
        private void PrintAttendanceList(object obj)
        {
            /// Method for Print Attendance Details List. 
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintAttendanceList()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["AttendanceReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["AttendanceReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintAttendanceList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintAttendanceList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Sprint 42---HRM	M042-06	Add and Edit Attendance----sdesai
        /// Method to add new attendance
        ///[001]print46 HRM Take values from lookup values instead of the existing tables by Mayuri     
        ///[002][SP-65][skale][11-06-2019][GEOS2-1556]Grid data reflection problems
        ///[003][spawar][12-03-2020][GEOS2-36]HRM - Add summatory values of times in attendance.
        ///[004][cpatil][29-09-2020][GEOS2-2113]HRM - Break time in Attendance (Grid View) (#IES16).
        /// </summary>
        /// <param name="obj"></param>

        public void AddAttendance(object obj)
        {
            try
            {
                //[003] Added.
                IsEmployeewiseRegisterAndExpectDays = false;
                TableView detailView = (TableView)((object[])obj)[0];
                GeosApplication.Instance.Logger.Log("Method AddAttendance()...", category: Category.Info, priority: Priority.Low);
                objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
                CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();
                List<Company> PlantOwners = new List<Company>();
                List<Company> PlantOwnersIds = new List<Company>();
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    PlantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    PlantOwnersIds = PlantOwners.Where(i => i.IdCompany == CurrentGeosProvider.IdCompany).ToList();
                }
                AttendanceView addAttendanceView = new AttendanceView();
                AttendanceViewModel addAttendanceViewModel = new AttendanceViewModel();
                EventHandler handle = delegate { addAttendanceView.Close(); };
                addAttendanceViewModel.RequestClose += handle;
                addAttendanceView.DataContext = addAttendanceViewModel;
                addAttendanceViewModel.IsSplitVisible = false;
                addAttendanceViewModel.WorkingPlantId = CurrentGeosProvider.IdCompany.ToString();
                addAttendanceViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                object selectedEmployee = SelectedItem;
                if (IsGridViewVisible == Visibility.Visible)
                    selectedEmployee = null;
                addAttendanceViewModel.Company = CurrentGeosProvider.Company;
                addAttendanceViewModel.SelectedPlantList = PlantOwners;
                addAttendanceViewModel.Init(EmployeeAttendanceList, selectedEmployee, SelectedStartDate, SelectedEndDate, employeeListFinal:_employeeListFinal);
                addAttendanceViewModel.IsNew = true;
                addAttendanceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewAttendance").ToString();
                addAttendanceViewModel.EmployeeLeaves = EmployeeLeaves;
                var ownerInfo = (detailView as FrameworkElement);
                addAttendanceView.Owner = Window.GetWindow(ownerInfo);
                addAttendanceView.ShowDialog();
                if (addAttendanceViewModel.IsSave == true)
                {
                    if (addAttendanceViewModel.IsAcceptButton)
                    {
                        IsEmployeewiseRegisterAndExpectDays = true;
                    }
                    ObservableCollection<UI.Helper.Appointment> appointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                    foreach (var item in addAttendanceViewModel.NewEmployeeAttendanceList)
                    {
                        TimeSpan timeSpan = new TimeSpan();
                        timeSpan = item.EndDate - item.StartDate;

                        if (SelectedItem != null)
                        {
                            var SelectedEmployeeLeave = EmployeeLeaves.Where(x => x.IdEmployee == item.IdEmployee).ToList();
                            List<EmployeeLeave> NewLeaveOfSelectedEmployeeAsPerDate = new List<EmployeeLeave>();
                            foreach (EmployeeLeave employeeLeave in SelectedEmployeeLeave)
                            {
                                for (var day = employeeLeave.StartDate.Value.Date; day.Date <= employeeLeave.EndDate.Value.Date; day = day.AddDays(1))
                                {
                                    EmployeeLeave e = new EmployeeLeave();
                                    e = (EmployeeLeave)employeeLeave.Clone();
                                    e.StartDate = day;
                                    e.EndDate = day;
                                    NewLeaveOfSelectedEmployeeAsPerDate.Add(e);
                                }
                            }

                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                            modelAppointment.Label = item.IdCompanyWork;
                            //[001] Changes in Subject as per Lookup Value
                            //  modelAppointment.Subject = string.Format("{0}", item.CompanyWork.Name);
                            //modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList[GeosApplication.Instance.AttendanceTypeList.FindIndex(x => x.IdLookupValue == item.IdCompanyWork)].Value);
                            modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == item.IdCompanyWork).Value);
                            modelAppointment.StartDate = item.StartDate;
                            modelAppointment.EndDate = item.EndDate;
                            modelAppointment.IdEmployeeAttendance = item.IdEmployeeAttendance;
                            modelAppointment.DailyHoursCount = 0;

                            if (addAttendanceViewModel.CompanyShiftDetails != null)
                            {
                                if (addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule != null)
                                {
                                    modelAppointment.DailyHoursCount = addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule.DailyHoursCount;
                                }
                            }
                            else
                                modelAppointment.DailyHoursCount = 0;

                            var EmployeeLeaveForSelectedAttendance = NewLeaveOfSelectedEmployeeAsPerDate.Where(x => x.StartDate == item.StartDate.Date);
                            modelAppointment.TotalLeaveDurationInHours = 0;
                            if (EmployeeLeaveForSelectedAttendance.Count() > 0)
                            {
                                foreach (EmployeeLeave Leave in EmployeeLeaveForSelectedAttendance)
                                {
                                    TimeSpan fromH = Leave.StartTime.Value;
                                    TimeSpan toH = Leave.EndTime.Value;
                                    TimeSpan hourTotalSpan = toH.Subtract(fromH);
                                    modelAppointment.TotalLeaveDurationInHours = modelAppointment.TotalLeaveDurationInHours + hourTotalSpan.Hours + hourTotalSpan.Minutes;
                                }
                            }

                            if (addAttendanceViewModel.SelectedEmployeeShift != null)
                               
                            if (item.IsManual == 1)
                            {
                                modelAppointment.Description = "[Manual] ";
                            }
                             if (addAttendanceViewModel.SelectedEmployeeShift.CompanyShift.IsNightShift == 1)
                                {
                                    modelAppointment.Description = modelAppointment.Description + "[Night Shift]";
                                    modelAppointment.IsNightShift = 1;
                                }

                            if (item.IsManual == 1)
                            {
                                modelAppointment.AttendanceIsManual = true;
                            }
                            else
                            {
                                modelAppointment.AttendanceIsManual = false;
                            }
                            modelAppointment.IdEmployee = item.IdEmployee;
                            modelAppointment.AccountingDate = item.AccountingDate;
                            appointmentItems.Add(modelAppointment);
                        }

                        item.StartTime = addAttendanceViewModel.STime;
                        item.EndTime = addAttendanceViewModel.ETime;
                        item.Employee.TotalWorkedHours = timeSpan.ToString(@"hh\:mm");
                        item.TotalTime = timeSpan;
                        item.CompanyShift = item.Employee.CompanyShift;
                        //[004]
                        if (BreakWTIdCompanyWork.Any(bwc => bwc == addAttendanceViewModel.NewEmployeeAttendance.IdCompanyWork))
                            item.CompanyShift.BreakTime = GetBreakTime((int)item.StartDate.Date.DayOfWeek, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);
                        else
                            item.CompanyShift.BreakTime = new TimeSpan();
                        item.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == addAttendanceViewModel.NewEmployeeAttendance.IdCompanyWork));
                        item.IdCompanyWork = addAttendanceViewModel.NewEmployeeAttendance.IdCompanyWork;

                        if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate) < 10)
                        {
                            item.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate);
                        }
                        else
                        {
                            item.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate);
                        }

                        EmployeeAttendanceList.Add(item);
                        SelectedAttendanceRecord = item;
                    }

                    AppointmentItems.AddRange(appointmentItems);
                }


                GeosApplication.Instance.Logger.Log("Method AddAttendance()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddAttendance()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// To create Appointment from EmployeeAttendance
        /// </summary>
        /// <param name="EmployeeAttendance"></param>
        public void EmployeeAttendanceToAppointments(List<EmployeeAttendance> EmployeeAttendance)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EmployeeAttendanceToAppointment()...", category: Category.Info, priority: Priority.Low);
                foreach (var item in EmployeeAttendance)
                {
                    TimeSpan timeSpan = new TimeSpan();
                    timeSpan = item.EndDate - item.StartDate;

                    if (SelectedItem != null)
                    {
                        var SelectedEmployeeLeave = EmployeeLeaves.Where(x => x.IdEmployee == item.IdEmployee).ToList();
                        List<EmployeeLeave> NewLeaveOfSelectedEmployeeAsPerDate = new List<EmployeeLeave>();
                        foreach (EmployeeLeave employeeLeave in SelectedEmployeeLeave)
                        {
                            for (var day = employeeLeave.StartDate.Value.Date; day.Date <= employeeLeave.EndDate.Value.Date; day = day.AddDays(1))
                            {
                                EmployeeLeave e = new EmployeeLeave();
                                e = (EmployeeLeave)employeeLeave.Clone();
                                e.StartDate = day;
                                e.EndDate = day;
                                NewLeaveOfSelectedEmployeeAsPerDate.Add(e);
                            }
                        }

                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Label = item.IdCompanyWork;
                        //[001] Changes in Subject as per Lookup Value
                        //  modelAppointment.Subject = string.Format("{0}", item.CompanyWork.Name);
                        //modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList[GeosApplication.Instance.AttendanceTypeList.FindIndex(x => x.IdLookupValue == item.IdCompanyWork)].Value);
                        modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == item.IdCompanyWork).Value);

                        modelAppointment.StartDate = item.StartDate;
                        modelAppointment.EndDate = item.EndDate;
                        modelAppointment.IdEmployeeAttendance = item.IdEmployeeAttendance;

                        modelAppointment.DailyHoursCount = 0;

                        if (item.CompanyShift != null)
                        {
                            if (item.CompanyShift.CompanyAnnualSchedule != null)
                            {
                                modelAppointment.DailyHoursCount = item.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                            }
                            if (item.CompanyShift.IsNightShift == 1)
                            {
                                modelAppointment.Description = "[Night shift]";
                                modelAppointment.IsNightShift = 1;
                            }
                        }
                        else
                            modelAppointment.DailyHoursCount = 0;

                        var EmployeeLeaveForSelectedAttendance = NewLeaveOfSelectedEmployeeAsPerDate.Where(x => x.StartDate == item.StartDate.Date);
                        modelAppointment.TotalLeaveDurationInHours = 0;
                        if (EmployeeLeaveForSelectedAttendance.Count() > 0)
                        {
                            foreach (EmployeeLeave Leave in EmployeeLeaveForSelectedAttendance)
                            {
                                TimeSpan fromH = Leave.StartTime.Value;
                                TimeSpan toH = Leave.EndTime.Value;
                                TimeSpan hourTotalSpan = toH.Subtract(fromH);
                                modelAppointment.TotalLeaveDurationInHours = modelAppointment.TotalLeaveDurationInHours + hourTotalSpan.Hours + hourTotalSpan.Minutes;
                            }
                        }

                        modelAppointment.AccountingDate = item.AccountingDate;
                        AppointmentItems.Add(modelAppointment);
                    }

                    item.StartTime = item.StartDate.TimeOfDay;
                    item.EndTime = item.EndDate.TimeOfDay;
                    item.Employee.TotalWorkedHours = timeSpan.ToString(@"hh\:mm");
                    item.TotalTime = timeSpan;
                    item.CompanyShift = item.Employee.CompanyShift;
                    item.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == item.IdCompanyWork));
                    item.IdCompanyWork = item.IdCompanyWork;

                    if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate) < 10)
                    {
                        item.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate);
                    }
                    else
                    {
                        item.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate);
                    }
                    item.CompanyContract = item.Employee.EmployeeContractSituation.Company;
                    EmployeeAttendanceList.Add(item);
                    SelectedAttendanceRecord = item;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EmployeeAttendanceToAppointment()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method EmployeeAttendanceToAppointment()....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        ///  Method to get selected dates from scheduler
        /// </summary>
        /// <param name="e"></param>
        private void SelectedIntervalCommandAction(MouseButtonEventArgs e)
        {
            try
            {
                SchedulerControl scheduler = e.Source as SchedulerControl;
                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;

                if (scheduler.ActiveView.Caption == "Month View")
                    SelectedEndDate = SelectedEndDate.AddDays(-1);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIntervalCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method to edit attendance from grid view
        ///[001]  Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri      
       ///[002][cpatil][29-09-2020][GEOS2-2113]HRM - Break time in Attendance (Grid View) (#IES16).
        /// </summary>
        /// <param name="obj"></param>
        private void EditAttendanceInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditAttendanceInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                EmployeeAttendance employeeAttendance = (EmployeeAttendance)detailView.DataControl.CurrentItem;

                SelectedAttendanceRecord = employeeAttendance;

                if (employeeAttendance != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    AttendanceView addAttendanceView = new AttendanceView();
                    AttendanceViewModel addAttendanceViewModel = new AttendanceViewModel();
                    EventHandler handle = delegate { addAttendanceView.Close(); };
                    addAttendanceViewModel.RequestClose += handle;
                    addAttendanceView.DataContext = addAttendanceViewModel;

                    //[001] CompanyWork IdCompany  Code comment and changes as per Job description IdCompany
                    // addAttendanceViewModel.WorkingPlantId = employeeAttendance.CompanyWork.IdCompany.ToString();
                    //addAttendanceViewModel.WorkingPlantId = employeeAttendance.Employee.EmployeeJobDescription.IdCompany.ToString();

                    string[] idEmployeeCompanyIdsSplit = employeeAttendance.Employee.EmployeeCompanyIds.Split(',');
                    addAttendanceViewModel.WorkingPlantId = idEmployeeCompanyIdsSplit[0];

                    addAttendanceViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                    addAttendanceViewModel.SelectedPlantList = plantOwners;

                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addAttendanceViewModel.InitReadOnly(employeeAttendance);
                    else
                        addAttendanceViewModel.EditInit(employeeAttendance, EmployeeAttendanceList, _employeeListFinal);

                    addAttendanceViewModel.IsNew = false;
                    addAttendanceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditAttendance").ToString();
                    EmployeeJobDescription empJobDesc = new EmployeeJobDescription();

                    if (employeeAttendance.Employee.EmployeeJobDescription != null)
                    {
                        empJobDesc = employeeAttendance.Employee.EmployeeJobDescription;
                    }

                    addAttendanceViewModel.EmployeeLeaves = EmployeeLeaves;
                    var ownerInfo = (detailView as FrameworkElement);
                    addAttendanceView.Owner = Window.GetWindow(ownerInfo);
                    addAttendanceView.ShowDialog();

                    if (addAttendanceViewModel.Result)
                    {
                        if (addAttendanceViewModel.IsSave && !addAttendanceViewModel.IsSplit)
                        {
                            var SelectedEmployeeLeave = EmployeeLeaves.Where(x => x.IdEmployee == employeeAttendance.IdEmployee).ToList();
                            List<EmployeeLeave> NewLeaveOfSelectedEmployeeAsPerDate = new List<EmployeeLeave>();
                            foreach (EmployeeLeave employeeLeave in SelectedEmployeeLeave)
                            {
                                for (var day = employeeLeave.StartDate.Value.Date; day.Date <= employeeLeave.EndDate.Value.Date; day = day.AddDays(1))
                                {
                                    EmployeeLeave e = new EmployeeLeave();
                                    e = (EmployeeLeave)employeeLeave.Clone();
                                    e.StartDate = day;
                                    e.EndDate = day;
                                    NewLeaveOfSelectedEmployeeAsPerDate.Add(e);
                                }
                            }

                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                            modelAppointment = AppointmentItems.FirstOrDefault(x => x.IdEmployeeAttendance == employeeAttendance.IdEmployeeAttendance);
                            AppointmentItems.Remove(modelAppointment);

                            TimeSpan timeSpan = new TimeSpan();
                            timeSpan = addAttendanceViewModel.UpdateEmployeeAttendance.EndDate - addAttendanceViewModel.UpdateEmployeeAttendance.StartDate;
                            modelAppointment = new UI.Helper.Appointment();
                            modelAppointment.Label = addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork;
                            //[001] Changes in Subject as per Lookup Value
                            // modelAppointment.Subject = string.Format("[{0}]   {1} {2}", timeSpan.ToString(@"hh\:mm"), addAttendanceViewModel.UpdateEmployeeAttendance.Employee.FirstName, addAttendanceViewModel.UpdateEmployeeAttendance.Employee.LastName);

                            //modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList[GeosApplication.Instance.AttendanceTypeList.FindIndex(x => x.IdLookupValue == addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork)].Value);
                            modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork).Value);

                            modelAppointment.StartDate = addAttendanceViewModel.UpdateEmployeeAttendance.StartDate;
                            modelAppointment.EndDate = addAttendanceViewModel.UpdateEmployeeAttendance.EndDate;
                            modelAppointment.IdEmployeeAttendance = addAttendanceViewModel.UpdateEmployeeAttendance.IdEmployeeAttendance;

                            var EmployeeLeaveForSelectedAttendance = NewLeaveOfSelectedEmployeeAsPerDate.Where(x => x.StartDate == addAttendanceViewModel.UpdateEmployeeAttendance.StartDate.Date);
                            modelAppointment.TotalLeaveDurationInHours = 0;

                            if (EmployeeLeaveForSelectedAttendance.Count() > 0)
                            {
                                foreach (EmployeeLeave Leave in EmployeeLeaveForSelectedAttendance)
                                {
                                    TimeSpan fromH = Leave.StartTime.Value;
                                    TimeSpan toH = Leave.EndTime.Value;
                                    TimeSpan hourTotalSpan = toH.Subtract(fromH);
                                    modelAppointment.TotalLeaveDurationInHours = modelAppointment.TotalLeaveDurationInHours + hourTotalSpan.Hours + hourTotalSpan.Minutes;
                                }
                            }

                            if (addAttendanceViewModel.SelectedEmployeeShift != null)
                                if (addAttendanceViewModel.SelectedEmployeeShift.CompanyShift.IsNightShift == 1)
                                {
                                    modelAppointment.Description = "[Night Shift]";
                                    modelAppointment.IsNightShift = 1;
                                }

                            modelAppointment.AccountingDate = addAttendanceViewModel.UpdateEmployeeAttendance.AccountingDate;

                            AppointmentItems.Add(modelAppointment);

                            addAttendanceViewModel.UpdateEmployeeAttendance.Employee.EmployeeJobDescription = empJobDesc;
                            employeeAttendance.Employee = addAttendanceViewModel.UpdateEmployeeAttendance.Employee;
                            employeeAttendance.IdEmployee = addAttendanceViewModel.UpdateEmployeeAttendance.IdEmployee;
                            employeeAttendance.StartDate = addAttendanceViewModel.UpdateEmployeeAttendance.StartDate;
                            employeeAttendance.StartTime = addAttendanceViewModel.StartTime.Value.TimeOfDay;
                            employeeAttendance.EndDate = addAttendanceViewModel.UpdateEmployeeAttendance.EndDate;
                            employeeAttendance.EndTime = addAttendanceViewModel.EndTime.Value.TimeOfDay;
                            //[001] Code Comment and get company work
                            // employeeAttendance.CompanyWork = addAttendanceViewModel.UpdateEmployeeAttendance.CompanyWork;
                            //employeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList[GeosApplication.Instance.AttendanceTypeList.FindIndex(x => x.IdLookupValue == addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork)]);

                            employeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork));
                            employeeAttendance.IdCompanyWork = addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork;
                            employeeAttendance.Employee.TotalWorkedHours = timeSpan.TotalHours.ToString(@"hh\:mm");
                            employeeAttendance.TotalTime = TimeSpan.FromHours(timeSpan.TotalHours);
                            employeeAttendance.IdCompanyShift = addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyShift;
                            employeeAttendance.CompanyShift = addAttendanceViewModel.UpdateEmployeeAttendance.Employee.CompanyShift;
                            //[002]
                            if (BreakWTIdCompanyWork.Any(bwc => bwc == addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyWork))
                                employeeAttendance.CompanyShift.BreakTime = GetBreakTime((int)employeeAttendance.StartDate.DayOfWeek, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);
                            else
                                employeeAttendance.CompanyShift.BreakTime = new TimeSpan();
                            if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(employeeAttendance.StartDate) < 10)
                            {
                                employeeAttendance.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(employeeAttendance.StartDate);
                            }
                            else
                            {
                                employeeAttendance.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(employeeAttendance.StartDate);
                            }


                            employeeAttendance.AccountingDate = addAttendanceViewModel.UpdateEmployeeAttendance.AccountingDate;

                            SelectedAttendanceRecord = employeeAttendance;
                        }

                        if (addAttendanceViewModel.IsSplit && addAttendanceViewModel.IsSave)
                        {
                            UI.Helper.Appointment tempAppointment = new UI.Helper.Appointment();
                            tempAppointment = AppointmentItems.FirstOrDefault(x => x.IdEmployeeAttendance == employeeAttendance.IdEmployeeAttendance);
                            AppointmentItems.Remove(tempAppointment);
                            ObservableCollection<UI.Helper.Appointment> appointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                            foreach (var item in addAttendanceViewModel.NewEmployeeAttendanceList)
                            {
                                TimeSpan timeSpan = new TimeSpan();
                                timeSpan = item.EndDate - item.StartDate;

                                if (SelectedItem != null)
                                {
                                    var SelectedEmployeeLeave = EmployeeLeaves.Where(x => x.IdEmployee == item.IdEmployee).ToList();
                                    List<EmployeeLeave> NewLeaveOfSelectedEmployeeAsPerDate = new List<EmployeeLeave>();
                                    foreach (EmployeeLeave employeeLeave in SelectedEmployeeLeave)
                                    {
                                        for (var day = employeeLeave.StartDate.Value.Date; day.Date <= employeeLeave.EndDate.Value.Date; day = day.AddDays(1))
                                        {
                                            EmployeeLeave e = new EmployeeLeave();
                                            e = (EmployeeLeave)employeeLeave.Clone();
                                            e.StartDate = day;
                                            e.EndDate = day;
                                            NewLeaveOfSelectedEmployeeAsPerDate.Add(e);
                                        }
                                    }

                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                    modelAppointment.Label = item.IdCompanyWork;
                                    modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == item.IdCompanyWork).Value);
                                    modelAppointment.StartDate = item.StartDate;
                                    modelAppointment.EndDate = item.EndDate;
                                    modelAppointment.IdEmployeeAttendance = item.IdEmployeeAttendance;
                                    modelAppointment.DailyHoursCount = 0;

                                    if (addAttendanceViewModel.CompanyShiftDetails != null)
                                    {
                                        if (addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule != null)
                                        {
                                            modelAppointment.DailyHoursCount = addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule.DailyHoursCount;
                                        }
                                    }
                                    else
                                        modelAppointment.DailyHoursCount = 0;

                                    var EmployeeLeaveForSelectedAttendance = NewLeaveOfSelectedEmployeeAsPerDate.Where(x => x.StartDate == item.StartDate.Date);
                                    modelAppointment.TotalLeaveDurationInHours = 0;
                                    if (EmployeeLeaveForSelectedAttendance.Count() > 0)
                                    {
                                        foreach (EmployeeLeave Leave in EmployeeLeaveForSelectedAttendance)
                                        {
                                            TimeSpan fromH = Leave.StartTime.Value;
                                            TimeSpan toH = Leave.EndTime.Value;
                                            TimeSpan hourTotalSpan = toH.Subtract(fromH);
                                            modelAppointment.TotalLeaveDurationInHours = modelAppointment.TotalLeaveDurationInHours + hourTotalSpan.Hours + hourTotalSpan.Minutes;
                                        }
                                    }

                                    if (addAttendanceViewModel.SelectedEmployeeShift != null)
                                        if (addAttendanceViewModel.SelectedEmployeeShift.CompanyShift.IsNightShift == 1)
                                        {
                                            modelAppointment.Description = "[Night Shift]";
                                            modelAppointment.IsNightShift = 1;
                                        }

                                    modelAppointment.AccountingDate = item.AccountingDate;

                                    appointmentItems.Add(modelAppointment);
                                }

                                item.StartTime = item.StartDate.TimeOfDay;
                                item.EndTime = item.EndDate.TimeOfDay;
                                item.Employee.TotalWorkedHours = timeSpan.ToString(@"hh\:mm");
                                item.TotalTime = timeSpan;
                                item.CompanyShift = item.Employee.CompanyShift;
                                //[002]
                                if (BreakWTIdCompanyWork.Any(bwc => bwc == item.IdCompanyWork))
                                    item.CompanyShift.BreakTime = GetBreakTime((int)item.StartDate.DayOfWeek, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);
                                else
                                    item.CompanyShift.BreakTime = new TimeSpan();
                                if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate) < 10)
                                {
                                    item.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate);
                                }
                                else
                                {
                                    item.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(item.StartDate);
                                }
                                EmployeeAttendanceList.Add(item);
                                SelectedAttendanceRecord = item;
                            }
                            AppointmentItems.AddRange(appointmentItems);
                            EmployeeAttendanceList.Remove(employeeAttendance);
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditAttendanceInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditAttendanceInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        ///  [HRM-M042-32] New layout in attendance navigation menu
        /// This method for Open selected Employee Profile on double click
        /// [001][SP-65][skale][13-06-2019][GEOS2-1556]Grid data reflection problems
        /// </summary>
        /// <param name="e"></param>
        private void OpenEmployeeProfile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeProfile()...", category: Category.Info, priority: Priority.Low);

                AccordionControl AccordionObj = (AccordionControl)((object[])obj)[1];
                //CardView detailView = (CardView)((object[])obj)[0];

                if (SelectedItem is Employee)
                {
                    Employee employee = (Employee)SelectedItem;

                    EmployeeProfileDetailView employeeProfileDetailView = new EmployeeProfileDetailView();
                    EmployeeProfileDetailViewModel employeeProfileDetailViewModel = new EmployeeProfileDetailViewModel();
                    EventHandler handle = delegate { employeeProfileDetailView.Close(); };
                    employeeProfileDetailViewModel.RequestClose += handle;
                    employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;

                    IsBusy = true;
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            Window win = new Window()
                            {
                                ShowActivated = false,
                                WindowStyle = WindowStyle.None,
                                ResizeMode = ResizeMode.NoResize,
                                AllowsTransparency = true,
                                Background = new SolidColorBrush(Colors.Transparent),
                                ShowInTaskbar = false,
                                Topmost = true,
                                SizeToContent = SizeToContent.WidthAndHeight,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            };
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                    {
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                        if (HrmCommon.Instance.IsPermissionReadOnly)
                            employeeProfileDetailViewModel.InitReadOnly(employee, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());
                        else
                            employeeProfileDetailViewModel.Init(employee, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());
                    }

                    employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;

                    var ownerInfo = (AccordionObj as FrameworkElement);
                    employeeProfileDetailView.Owner = Window.GetWindow(ownerInfo);
                    employeeProfileDetailView.ShowDialog();

                    if (employeeProfileDetailViewModel.IsSaveChanges == true)
                    {
                        if (employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdEmployeeStatus == 138)
                        {
                            Employees.Remove((Employees.FirstOrDefault(x => x.IdEmployee == employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdEmployee)));
                            List<EmployeeAttendance> tempEmployeeAttendanceList = EmployeeAttendanceList.Where(x => x.IdEmployee == employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdEmployee).ToList();
                            foreach (EmployeeAttendance item in tempEmployeeAttendanceList)
                            {
                                EmployeeAttendanceList.Remove(item);
                            }
                        }
                        else
                        {
                            employee.FirstName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.FirstName;
                            employee.LastName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.LastName;
                            // [001] added
                            List<EmployeeContractSituation> tempEmployeeContractSituation = employeeProfileDetailViewModel.EmployeeContractSituationList.Where(x => x.Company != null && (x.ContractSituationEndDate == null || x.ContractSituationEndDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date)).ToList();
                            if (tempEmployeeContractSituation != null && tempEmployeeContractSituation.Count > 0)
                            {
                                if (tempEmployeeContractSituation.Count > 1)
                                {
                                    EmployeeContractSituation tempContract = tempEmployeeContractSituation.FirstOrDefault(x => x.ContractSituationEndDate != null);
                                    if (tempContract != null)
                                        employee.Company = tempContract.Company.Alias;
                                }
                                else if (tempEmployeeContractSituation.Count == 1)
                                {
                                    employee.Company = tempEmployeeContractSituation[0].Company.Alias;
                                }
                            }
                            else
                                employee.Company = string.Empty;

                            List<Company> SelectedPlant = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                            if (SelectedPlant != null)
                            {
                                List<Company> isExistPlantList = SelectedPlant.Where(x => x.Alias.Contains(employee.Company)).ToList();
                                if (isExistPlantList.Count <= 0)
                                {
                                    Employees.Remove(employee);

                                    var values = (object[])obj;
                                    SchedulerControlEx schedulerControlEx = (SchedulerControlEx)values[0];
                                    schedulerControlEx.Uid = "Refresh";

                                    AccordionControl accordionControl = (AccordionControl)values[1];
                                    var searchControl = LayoutTreeHelper.GetVisualChildren(accordionControl).OfType<SearchControl>().FirstOrDefault();
                                    if (searchControl != null)
                                        searchControl.SearchText = null;

                                    schedulerControlEx.Uid = string.Empty;
                                    schedulerControlEx.Tag = string.Empty;
                                }
                            }
                        }

                    }

                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                }
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeProfile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeProfile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Methos to show Edit Occurrence Window
        /// </summary>
        /// <param name="obj"></param>
        private void EditOccurrenceWindowShowing(EditOccurrenceWindowShowingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditOccurrenceWindowShowing()...", category: Category.Info, priority: Priority.Low);

                obj.Cancel = true;

                GeosApplication.Instance.Logger.Log("Method EditOccurrenceWindowShowing()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditOccurrenceWindowShowing()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// Function Created to fill EmployeeWork from Lookup values
        /// </summary>
        public static void FillEmployeeWorkType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeWorkType()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.AttendanceTypeList == null)
                {
                    ICrmService myCrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                    GeosApplication.Instance.AttendanceTypeList = new ObservableCollection<LookupValue>(myCrmStartUp.GetLookupValues(33));
                    GeosApplication.Instance.AttendanceTypeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeWorkType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// Function Created to Fill CompanyWork from Lookup values
        /// </summary>
        public CompanyWork GetCompanyWork(LookupValue obj)
        {
            CompanyWork companyWork = new CompanyWork
            {
                IdCompanyWork = obj.IdLookupValue,
                Name = obj.Value,
                HtmlColor = obj.HtmlColor
            };

            return companyWork;
        }


        /// <summary>
        ///  [HRM-M048-10] Allow to remove entries in attendance---Method to delete attendance---sdesai
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteAppointment(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteAppointment()...", category: Category.Info, priority: Priority.Low);

                if (e is KeyEventArgs)
                {
                    KeyEventArgs obj = e as KeyEventArgs;

                    if (obj.Source is SchedulerControlEx)
                    {
                        if (obj.Key == Key.Delete)
                        {
                            SchedulerControlEx schedule = (SchedulerControlEx)obj.Source;
                            if (schedule.SelectedAppointments != null)
                            {
                                if (schedule.SelectedAppointments.Count > 0)
                                {
                                    if (schedule.SelectedAppointments[0].CustomFields["IdEmployeeAttendance"] != null)
                                    {
                                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.Resources["DeleteEmployeeAttendanceMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                        if (MessageBoxResult == MessageBoxResult.Yes)
                                        {
                                            bool result = HrmService.DeleteEmployeeAttendance((long)Convert.ToDouble(schedule.SelectedAppointments[0].CustomFields["IdEmployeeAttendance"].ToString()));
                                            EmployeeAttendanceList.Remove(EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(schedule.SelectedAppointments[0].CustomFields["IdEmployeeAttendance"])));
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                        }
                                        else
                                        {
                                            obj.Handled = true;
                                        }
                                    }
                                    else
                                    {
                                        obj.Handled = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (e is AppointmentItemCancelEventArgs)
                {
                    AppointmentItem appointment = ((AppointmentItemCancelEventArgs)e).Appointment;

                    if (appointment.CustomFields["IdEmployeeAttendance"] != null)
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.Resources["DeleteEmployeeAttendanceMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            bool result = HrmService.DeleteEmployeeAttendance((long)Convert.ToDouble(appointment.CustomFields["IdEmployeeAttendance"].ToString()));
                            EmployeeAttendanceList.Remove(EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(appointment.CustomFields["IdEmployeeAttendance"])));
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                        else
                        {
                            ((AppointmentItemCancelEventArgs)e).Cancel = true;
                        }
                    }
                    else
                    {
                        ((AppointmentItemCancelEventArgs)e).Cancel = true;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteAppointment()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteAppointment() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteAppointment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteAppointment()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void DefaultLoadCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DefaultLoadCommandAction()...", category: Category.Info, priority: Priority.Low);
                SchedulerControlEx scheduler = obj.Source as SchedulerControlEx;
                if (scheduler.ActiveView.Caption == "Month View")
                    ViewType = 1;
                else
                    ViewType = 2;

                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;

                if (scheduler.Month != null)
                {
                    if (scheduler.ActiveViewIndex == 0)
                    {
                        scheduler.DisplayName = String.Format("{0:MMMM yyyy}", scheduler.VisibleIntervals[0].End);
                        SelectedEndDate = SelectedEndDate.AddDays(-1);
                    }
                    else
                    {
                        if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start) < 10)
                        {
                            scheduler.DisplayName = String.Format("Week 0{0} of {1:yyyy}", DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start), scheduler.Start);
                        }
                        else
                        {
                            scheduler.DisplayName = String.Format("Week {0} of {1:yyyy}", DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear(scheduler.Start), scheduler.Start);
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DefaultLoadCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DefaultLoadCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public static void FillEmployeeLeaveType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveType()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.EmployeeLeaveList == null)
                {
                    ICrmService myCrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                    GeosApplication.Instance.EmployeeLeaveList = new ObservableCollection<LookupValue>(myCrmStartUp.GetLookupValues(32));
                    GeosApplication.Instance.EmployeeLeaveList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeLeaveType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //method for Hide unhide Pannel
        private void HidePanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HidePanel ...", category: Category.Info, priority: Priority.Low);

                // IsSchedulerViewVisible = Visibility.Visible;
                // IsGridViewVisible = Visibility.Hidden;
                if (IsAccordionControlVisible == Visibility.Collapsed)
                    IsAccordionControlVisible = Visibility.Visible;
                else
                    IsAccordionControlVisible = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "CurrentContractForAttendance" && e.Column.FieldName != "Employee.LstEmployeeDepartments" && e.Column.FieldName != "Employee.EmployeeJobTitles" && e.Column.FieldName != "Employee.EmpJobCodes")
            {
                return;
            }
            try
            {
                List<object> filterItems = new List<object>();
                if (e.Column.FieldName == "CurrentContractForAttendance")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("CurrentContractForAttendance is null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("CurrentContractForAttendance is not null")
                    });

                    foreach (var dataObject in EmployeeAttendanceList)
                    {
                        if (dataObject.CurrentContractForAttendance == null)
                        {
                            continue;
                        }
                        else if (string.IsNullOrEmpty(dataObject.CurrentContractForAttendance))
                        {
                            continue;
                        }
                        else if (!string.IsNullOrEmpty(dataObject.CurrentContractForAttendance))
                        {
                            string[] employeeCompanyAliasList = dataObject.CurrentContractForAttendance.Split(Environment.NewLine.ToCharArray());

                            foreach (string companyAlias in employeeCompanyAliasList)
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == companyAlias))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = companyAlias;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("CurrentContractForAttendance Like '%{0}%'", companyAlias));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Employee.LstEmployeeDepartments")
                {


                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Employee.LstEmployeeDepartments = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Employee.LstEmployeeDepartments <> ''")
                    });

                    foreach (var dataObject in EmployeeAttendanceList)
                    {
                        if (dataObject.Employee.LstEmployeeDepartments == null)
                        {
                            continue;
                        }
                        else if (dataObject.Employee.LstEmployeeDepartments.Count == 0)
                        {
                            continue;
                        }

                        foreach (var department in dataObject.Employee.LstEmployeeDepartments)
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == department.ToString()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = department;
                                customComboBoxItem.EditValue = department;
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Employee.EmployeeJobTitles")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Employee.EmployeeJobTitles is null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Employee.EmployeeJobTitles is not null")
                    });

                    foreach (var dataObject in EmployeeAttendanceList)
                    {
                        if (dataObject.Employee.EmployeeJobTitles == null)
                        {
                            continue;
                        }
                        else if (dataObject.Employee.EmployeeJobTitles != null)
                        {
                            if (dataObject.Employee.EmployeeJobTitles.Contains("\n"))
                            {
                                string tempEmployeeJobTitles = dataObject.Employee.EmployeeJobTitles;
                                for (int index = 0; index < tempEmployeeJobTitles.Length; index++)
                                {
                                    string employeeJobTitles = tempEmployeeJobTitles.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeJobTitles))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeJobTitles;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Employee.EmployeeJobTitles Like '%{0}%'", employeeJobTitles));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeJobTitles.Contains("\n"))
                                        tempEmployeeJobTitles = tempEmployeeJobTitles.Remove(0, employeeJobTitles.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == EmployeeAttendanceList.Where(y => y.Employee.EmployeeJobTitles == dataObject.Employee.EmployeeJobTitles).Select(slt => slt.Employee.EmployeeJobTitles).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = EmployeeAttendanceList.Where(y => y.Employee.EmployeeJobTitles == dataObject.Employee.EmployeeJobTitles).Select(slt => slt.Employee.EmployeeJobTitles).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Employee.EmployeeJobTitles Like '{0}'", EmployeeAttendanceList.Where(y => y.Employee.EmployeeJobTitles == dataObject.Employee.EmployeeJobTitles).Select(slt => slt.Employee.EmployeeJobTitles).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Employee.EmployeeJobTitles")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Employee.EmployeeJobTitles is null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Employee.EmployeeJobTitles is not null")
                    });

                    foreach (var dataObject in EmployeeAttendanceList)
                    {
                        if (dataObject.Employee.EmployeeJobTitles == null)
                        {
                            continue;
                        }
                        else if (string.IsNullOrEmpty(dataObject.Employee.EmployeeJobTitles))
                        {
                            continue;
                        }
                        else if (!string.IsNullOrEmpty(dataObject.Employee.EmployeeJobTitles))
                        {
                            string[] employeeJobTitles = dataObject.Employee.EmployeeJobTitles.Split(Environment.NewLine.ToCharArray());

                            foreach (string jobTitles in employeeJobTitles)
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == jobTitles))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = jobTitles;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Employee.EmployeeJobTitles Like '%{0}%'", jobTitles));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }

                else if (e.Column.FieldName == "Employee.EmpJobCodes")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmpJobCodes = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmpJobCodes <> ''")
                    });

                    foreach (var dataObject in EmployeeAttendanceList)
                    {
                        if (dataObject.Employee.EmpJobCodes == null)
                        {
                            continue;
                        }
                        else if (dataObject.Employee.EmpJobCodes != null)
                        {
                            if (dataObject.Employee.EmpJobCodes.Contains("\n"))
                            {
                                string tempEmpJobCodes = dataObject.Employee.EmpJobCodes;
                                for (int index = 0; index < tempEmpJobCodes.Length; index++)
                                {
                                    string empJobCodes = tempEmpJobCodes.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empJobCodes))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empJobCodes;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Employee.EmpJobCodes Like '%{0}%'", empJobCodes));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmpJobCodes.Contains("\n"))
                                        tempEmpJobCodes = tempEmpJobCodes.Remove(0, empJobCodes.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == EmployeeAttendanceList.Where(y => y.Employee.EmpJobCodes == dataObject.Employee.EmpJobCodes).Select(slt => slt.Employee.EmpJobCodes).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = EmployeeAttendanceList.Where(y => y.Employee.EmpJobCodes == dataObject.Employee.EmpJobCodes).Select(slt => slt.Employee.EmpJobCodes).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Employee.EmpJobCodes Like '%{0}%'", EmployeeAttendanceList.Where(y => y.Employee.EmpJobCodes == dataObject.Employee.EmpJobCodes).Select(slt => slt.Employee.EmpJobCodes).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void CustomRowFilter(RowFilterEventArgs e)
        {
            try
            {
                if (IsGridViewVisible != Visibility.Visible)
                {
                    return;
                }

                var criteria = e.Source.FilterString;

                if (criteria.Contains("[Employee.LstEmployeeDepartments]"))
                {
                    if (criteria.Contains("In"))
                    {
                        var initVals = criteria.Split(new String[] { "[Employee.LstEmployeeDepartments] In", "'", "(", ")", ",", "#", "ToString" }, StringSplitOptions.RemoveEmptyEntries);
                        bool includeEmpty = false;

                        if (initVals.Contains("Empty"))
                        {
                            includeEmpty = true;
                            initVals = initVals.Where(c => c != "Empty").ToArray();
                        }
                        var vals = initVals.Select(x => x).ToList();
                        if (EmployeeAttendanceList[e.ListSourceRowIndex].Employee.LstEmployeeDepartments != null)
                        {
                            e.Visible = vals.Intersect(this.EmployeeAttendanceList[e.ListSourceRowIndex].Employee.LstEmployeeDepartments.Select(x => x.DepartmentName)).Count() > 0;
                            e.Visible |= this.EmployeeAttendanceList[e.ListSourceRowIndex].Employee.LstEmployeeDepartments.Count == 0 && includeEmpty;
                        }
                    }
                    if (criteria.Contains("=")) //Is Null
                    {
                        var initVals = criteria.Split(new String[] { "[Employee.LstEmployeeDepartments] =", "", "'", "'", "," }, StringSplitOptions.RemoveEmptyEntries);
                        bool includeEmpty = false;
                        if (initVals.Contains("Empty") || initVals.Contains(" "))
                        {
                            includeEmpty = true;
                            initVals = initVals.Where(c => c != "Empty").ToArray();
                        }
                        var vals = initVals.Select(x => x).ToList();
                        if (EmployeeAttendanceList[e.ListSourceRowIndex].Employee.LstEmployeeDepartments != null)
                        {
                            e.Visible = vals.Intersect(EmployeeAttendanceList[e.ListSourceRowIndex].Employee.LstEmployeeDepartments.Select(x => x.DepartmentName)).Count() > 0;
                            e.Visible |= EmployeeAttendanceList[e.ListSourceRowIndex].Employee.LstEmployeeDepartments.Count == 0 && includeEmpty;
                        }
                        else
                            e.Visible |= true;
                    }
                    if (criteria.Contains("<>"))
                    {
                        if (EmployeeAttendanceList[e.ListSourceRowIndex].Employee.LstEmployeeDepartments != null)
                            e.Visible |= EmployeeAttendanceList[e.ListSourceRowIndex].Employee.LstEmployeeDepartments.Count != 0;
                        else
                            e.Visible = false;
                    }

                    e.Handled = true;
                }

                //GeosApplication.Instance.Logger.Log("Method CustomRowFilter() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomRowFilter() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private TimeSpan GetBreakTime(int dayOfWeek, CompanyShift selectedCompanyShift)
        {
            try
            {
               
            
                switch (dayOfWeek)
                {
                    case 0:
                        return selectedCompanyShift.SunBreakTime;
                    case 1:
                        return selectedCompanyShift.MonBreakTime;

                    case 2:
                        return selectedCompanyShift.TueBreakTime;

                    case 3:
                        return selectedCompanyShift.WedBreakTime;

                    case 4:
                        return selectedCompanyShift.ThuBreakTime;

                    case 5:
                        return selectedCompanyShift.FriBreakTime;

                    case 6:
                        return selectedCompanyShift.SatBreakTime;

                    default:
                        return selectedCompanyShift.BreakTime;
                }

            }
            catch (Exception ex)
            {
                return new TimeSpan();
            }
        }
        private string setColonBeforeAfterValues(string Numbers)
        {
            string[] Values = Numbers.Split('.');
            string getConvertString;

            if (Values.Count() == 1)
            {
                getConvertString = string.Format("{0}:{1}", Values[0].PadLeft(2, '0'), "00");
            }
            else
            {
                getConvertString = string.Format("{0}:{1}", Values[0].PadLeft(2, '0'), Values[1].Length == 1 ? Values[1].PadRight(2, '0') : Values[1]);
            }
            return getConvertString;
        }

        public void MonthlyAllRegisterHours(SchedulerControlEx schedulerControlEx)
        {
            try
            {
                MonthlyRegisterTotalHoursCount = 0;
                MonthlyExpectedTotalHoursCount = 0;
                StringMonthlyExpectedTotalHoursCount = "00:00";
                StringMonthlyTotalHoursCount = "00:00";
                StringRegisterHoursColour = string.Empty;

                if (IsEmployeewiseRegisterAndExpectDays)
                {
                    //Register Days
                    List<EmployeeAttendance> MonthlyAttendanceList = EmployeeAttendanceList.Where(x => x.IdEmployee == ((Employee)SelectedItem).IdEmployee && x.StartDate.Date.Month == schedulerControlEx.Month.Value.Date.Month).ToList();

                    if (MonthlyAttendanceList.Count() > 0)
                    {
                        HoursCount = new TimeSpan();
                        foreach (EmployeeAttendance EmpAttendance in MonthlyAttendanceList)
                        {
                            HoursCount = HoursCount.Add(TimeSpan.Parse(string.Format("{0:00}:{1:00}", (int)EmpAttendance.TotalTime.Value.Hours, EmpAttendance.TotalTime.Value.Minutes)));
                        }

                        decimal totalMinutes = Convert.ToDecimal(HoursCount.TotalMinutes);
                        decimal CountTotalHours = Convert.ToDecimal(totalMinutes / 60);
                        string stringTotalHours = CountTotalHours.ToString("0.00", CultureInfo.InvariantCulture); // .ToString();
                        string[] splitHoursAndMinutes = stringTotalHours.Split('.');
                        decimal CountPointAfterMinutes = CountTotalHours - Math.Truncate(CountTotalHours);
                        decimal CountPointAfterTotalMinutes = CountPointAfterMinutes * 60;

                        MonthlyRegisterTotalHoursCount = Convert.ToDouble(splitHoursAndMinutes[0].ToString() + '.' + Math.Round(Convert.ToDouble(CountPointAfterTotalMinutes), 0), CultureInfo.InvariantCulture);

                        StringMonthlyTotalHoursCount = setColonBeforeAfterValues(splitHoursAndMinutes[0].ToString() + '.' + Math.Round(CountPointAfterTotalMinutes, 0).ToString());
                    }
                    //
                    //Expected Days
                    GetEmployeeCompanySchedulelist = HrmService.GetEmployeeCompanySchedule(((Employee)SelectedItem).IdEmployee, HrmCommon.Instance.SelectedPeriod).ToList();

                    CompanySchedule companySchedule = GetEmployeeCompanySchedulelist.FirstOrDefault();

                    if (companySchedule != null)
                    {
                        List<int> AllDaysLeaveWeeklyOfDays = new List<int>();
                        Decimal AllDays = DateTime.DaysInMonth(schedulerControlEx.Month.Value.Year, schedulerControlEx.Month.Value.Month);

                        for (int day = 1; day <= AllDays; day++)
                        {
                            DateTime currentDay = new DateTime(schedulerControlEx.Month.Value.Year, Convert.ToInt32(schedulerControlEx.Month.Value.Month), day);
                            if (currentDay.DayOfWeek != DayOfWeek.Sunday && currentDay.DayOfWeek != DayOfWeek.Saturday)
                            {
                                AllDaysLeaveWeeklyOfDays.Add(day);
                            }
                        }

                        List<int> NumberOfAllHolidays = new List<int>();
                        List<EmployeeLeave> EmpLeaves = EmployeeLeaves.Where(x => x.IdEmployee == ((Employee)SelectedItem).IdEmployee && x.StartDate.Value.Date.Month == schedulerControlEx.Month.Value.Date.Month).OrderBy(x => x.StartDate).ToList();

                        if (EmpLeaves.Count() > 0)
                        {
                            foreach (EmployeeLeave empleave in EmpLeaves)
                            {
                                for (var i = empleave.StartDate.Value.Day; i <= empleave.EndDate.Value.Day; i++)
                                {
                                    DateTime testDate = new DateTime(empleave.StartDate.Value.Year, Convert.ToInt32(empleave.StartDate.Value.Month), i);

                                    if (AllDaysLeaveWeeklyOfDays.Contains(i))
                                    {
                                        AllDaysLeaveWeeklyOfDays.Remove(i);
                                    }
                                }
                            }

                            MonthlyExpectedDeductAllDays = AllDaysLeaveWeeklyOfDays.Count();
                            MonthlyExpectedTotalHoursCount = MonthlyExpectedDeductAllDays * companySchedule.CompanyAnnualSchedule.DailyHoursCount;
                        }
                        else
                        {
                            MonthlyExpectedTotalHoursCount = AllDaysLeaveWeeklyOfDays.Count() * companySchedule.CompanyAnnualSchedule.DailyHoursCount;
                        }

                        string[] splitValue = MonthlyExpectedTotalHoursCount.ToString().Split('.');

                        decimal CountHours = Convert.ToDecimal(MonthlyExpectedTotalHoursCount);

                        decimal CountPointAfterMinutes = CountHours - Math.Truncate(CountHours);

                        decimal CountPointAfterTotalMinutes = CountPointAfterMinutes * 60;

                        StringMonthlyExpectedTotalHoursCount = setColonBeforeAfterValues(splitValue[0].ToString() + '.' + Math.Round(CountPointAfterTotalMinutes, 0).ToString());

                    }
                    //Color Validations
                    if (Convert.ToDecimal(MonthlyRegisterTotalHoursCount) > MonthlyExpectedTotalHoursCount)
                    {
                        StringRegisterHoursColour = OKColor.ToString();
                    }
                    else if (Convert.ToDecimal(MonthlyRegisterTotalHoursCount) < MonthlyExpectedTotalHoursCount)
                    {
                        StringRegisterHoursColour = NotOKColor.ToString();
                    }
                    else
                    {
                        StringRegisterHoursColour = string.Empty;
                    }

                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in MonthlyAllRegisterHours() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public static void SetIsManual(ObservableCollection<EmployeeAttendance> EmployeeAttendance)
        {
            ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();
            foreach (EmployeeAttendance employeeAttendance in EmployeeAttendance)
            {
                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                if(employeeAttendance.IsManual==1)
                {
                    modelAppointment.AttendanceIsManual = true;
                }
                else
                {
                    modelAppointment.AttendanceIsManual = false;
                }
                appointment.Add(modelAppointment);
            }
        }

        #endregion

        private void SetUserPermission()
        {
            //HrmCommon.Instance.UserPermission = PermissionManagement.PlantViewer;

            switch (HrmCommon.Instance.UserPermission)
            {
                case PermissionManagement.SuperAdmin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.Admin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.PlantViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                case PermissionManagement.GlobalViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                default:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;
            }
        }

    }
}
