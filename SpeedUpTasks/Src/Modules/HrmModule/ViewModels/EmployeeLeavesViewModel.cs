using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Scheduling;
using DevExpress.Xpf.Scheduling.Visual;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Helper;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Hrm.Views;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.Scheduler.Menu;
using DevExpress.Xpf.Bars;
using DevExpress.Mvvm.POCO;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Accordion;
using DevExpress.Xpf.Editors;
using DevExpress.XtraScheduler;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using System.Globalization;
using DevExpress.Data.Filtering;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeLeavesViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region TaskLog
        /// <summary>
        /// [0001][25/06/2018][lsharma][SPRINT 41][HRM-M041-21]Simplify the way to add leaves]
        /// In this task default values loaded on Add new leave for Employee and StartDate,EndDate & AllDayEvent as per active view selection
        /// [HRM-M049-37][24/10/2018][adadibathina][no changelog generated when removing a leave]
        /// [M051-08][Year selection is not saved after change section][adadibathina]
        /// [000][SP-65][skale][12-06-2019][GEOS2-1556]Grid data reflection problems
        /// [001][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        /// </summary>

        #endregion
        static EmployeeLeavesViewModel()
        {
            //ScrollViewer.VerticalScrollBarVisibilityProperty.OverrideMetadata(typeof(SchedulerScrollViewer), new FrameworkPropertyMetadata(null, (d, e) => ScrollBarVisibility.Visible));
            ScrollViewer.VerticalScrollBarVisibilityProperty.OverrideMetadata(typeof(SchedulerScrollViewer), new FrameworkPropertyMetadata(null, (d, value) =>
            {
                var scheduler = SchedulerControl.GetScheduler((DependencyObject)d);
                if (scheduler != null && scheduler.ActiveView is MonthView)
                    return ScrollBarVisibility.Hidden;
                return value;
            }));
        }

        #region Services  
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService LeaveService { get { return GetService<INavigationService>(); } }

        public INavigationService _Service;
        #endregion // End Services

        #region Public Icommands
        public ICommand DisableAppointmentCommand { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand DepartmentSelectionCommand { get; set; }
        public ICommand EditEmployeeDoubleClickCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand scheduler_VisibleIntervalsChangedCommand { get; set; }
        public ICommand ButtonAddNewLeaveCommand { get; set; }
        public ICommand PlantOwnerEditValueChangedCommand { get; private set; }
        public ICommand EditOccurrenceWindowShowingCommand { get; set; }

        public ICommand DeleteOccurrenceWindowShowingCommand { get; set; }
        public ICommand DeleteAppointmentCommand { get; set; }
        public ICommand ShowSchedulerViewCommand { get; private set; }
        public ICommand ShowGridViewCommand { get; private set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand EditLeaveGridDoubleClickCommand { get; set; }
        //[HRM-M041-21]
        public ICommand DefaultLoadCommand { get; set; }
        public ICommand SelectedIntervalCommand { get; set; }
        public ICommand SelectedYearChangedCommand { get; private set; }

        //Sprint-48----[HRM-M048-06]--Add new column File in Leaves grid--sdesai
        public ICommand DocumentViewCommand { get; set; }
        public ICommand HidePanelCommand { get; set; }
        //[SP-65 002] added
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand CustomRowFilterCommand { get; set; }
        #endregion

        #region Declaration

        private object selectedItem;
        private ObservableCollection<Department> department;
        private ObservableCollection<EmployeeLeave> employeeLeaves;
        private ObservableCollection<EmployeeLeave> employeeLeavesDetails;
        private ObservableCollection<EmployeeLeave> employeeLeaveOrigional;
        private ObservableCollection<CompanyHoliday> companyHolidays;
        private ObservableCollection<ModelAppointment> appointments;
        private ObservableCollection<ModelAppointment> listappointments;
        private ObservableCollection<CompanyLeave> companyLeaves;
        private ObservableCollection<LookupValue> holidayList;
        private AppointmentLabelItemCollection label;
        private ObservableCollection<LabelHelper> labelItems;
        private ObservableCollection<UI.Helper.Appointment> appointmentItems;
        private bool isBusy;
        private bool showColumn = false;
        private bool hideColumn = true;
        IWorkbenchStartUp objWorkbenchStartUp;
        private Visibility isSchedulerViewVisible;
        private Visibility isGridViewVisible;
        private EmployeeLeave selectedLeaveRecord;

        //[HRM-M041-21]
        private DateTime selectedStartDate;
        private DateTime selectedEndDate;
        private byte viewType;
        private string timeEditMask;
        private Visibility isAccordionControlVisible;
        string myFilterString;//[SP-65-000] added
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        ObservableCollection<Employee> _employeeListFinalForLeaves;
        ObservableCollection<EmployeeAttendance> _employeeAttendanceList;
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
        #endregion // Events

        #region Properties

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

        public GeosProvider CurrentGeosProvider { get; set; }
        public List<GeosProvider> GeosProviderList { get; set; }
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
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


        public ObservableCollection<Department> Departments
        {
            get
            {
                return department;
            }

            set
            {
                department = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Departments"));
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

        public ObservableCollection<EmployeeLeave> EmployeeLeaveOrigional
        {
            get
            {
                return employeeLeaveOrigional;
            }

            set
            {
                employeeLeaveOrigional = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaveOrigional"));

            }
        }
        
        public ObservableCollection<EmployeeLeave> EmployeeLeavesDetails
        {
            get
            {
                return employeeLeavesDetails;
            }

            set
            {
                employeeLeavesDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeavesDetails"));

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

        public ObservableCollection<ModelAppointment> Appointments
        {
            get
            {
                return appointments;
            }

            set
            {
                appointments = value;
            }
        }

        public ObservableCollection<ModelAppointment> Listappointments
        {
            get
            {
                return listappointments;
            }

            set
            {
                listappointments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Listappointments"));
            }
        }

        public AppointmentLabelItemCollection Label
        {
            get { return label; }
            set
            {
                label = value;

            }
        }

        public ObservableCollection<CompanyLeave> CompanyLeaves
        {
            get
            {
                return companyLeaves;
            }

            set
            {
                companyLeaves = value;
            }
        }


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

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public bool ShowColumn
        {
            get { return showColumn; }
            set
            {
                showColumn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowColumn"));
            }
        }

        public bool HideColumn
        {
            get { return hideColumn; }
            set
            {
                hideColumn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HideColumn"));
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

        public EmployeeLeave SelectedLeaveRecord
        {
            get
            {
                return selectedLeaveRecord;
            }

            set
            {
                selectedLeaveRecord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLeaveRecord"));
            }
        }

        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
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
        public GeosAppSetting Setting { get; set; }

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

        #endregion

        #region Constructor
        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// [002][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        /// [003] [cpatil][2020-01-22][GEOS2-2008] We can not see some the Employees in LEAVES and ATTENDANCE with Period = 2019, but the same employees in 2020 we can see.
        /// [004] [cpatil][2020-03-17][GEOS2-1876] Draft users in Leaves.
        /// </summary>
        public EmployeeLeavesViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor EmployeeLeavesViewModel()...", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.FillFinancialYear();
                //[M051-08]
                // SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;

                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                DisableAppointmentCommand = new DelegateCommand<AppointmentWindowShowingEventArgs>(AppointmentWindowShowing);
                EditOccurrenceWindowShowingCommand = new DelegateCommand<EditOccurrenceWindowShowingEventArgs>(EditOccurrenceWindowShowing);
                DeleteOccurrenceWindowShowingCommand = new DelegateCommand<DeleteOccurrenceWindowShowingEventArgs>(DeleteOccurrenceWindowShowing);
                DeleteAppointmentCommand = new DelegateCommand<object>(DeleteAppointment);
                PopupMenuShowingCommand = new DelegateCommand<PopupMenuShowingEventArgs>(PopupMenuShowing);
                DepartmentSelectionCommand = new RelayCommand(new Action<object>(SelectItemForScheduler));
                EditEmployeeDoubleClickCommand = new RelayCommand(new Action<object>(OpenEmployeeProfile));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshLeavesView));
                scheduler_VisibleIntervalsChangedCommand = new RelayCommand(new Action<object>(VisibleIntervalsChanged));
                ButtonAddNewLeaveCommand = new DelegateCommand<object>(AddNewLeave);
                PlantOwnerEditValueChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerEditValueChangedCommandAction);
                EditLeaveGridDoubleClickCommand = new DelegateCommand<object>(EditLeaveInformation);
                ShowSchedulerViewCommand = new RelayCommand(new Action<object>(ShowLeavesSchedulerView));
                ShowGridViewCommand = new RelayCommand(new Action<object>(ShowLeavesGridView));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintEmployeeLeaveList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportEmployeeLeaveList));
                SelectedIntervalCommand = new DelegateCommand<MouseButtonEventArgs>(SelectedIntervalCommandAction);
                DefaultLoadCommand = new DelegateCommand<RoutedEventArgs>(DefaultLoadCommandAction);
                SelectedYearChangedCommand = new DelegateCommand<object>(SelectedYearChangedCommandAction);
                
                DocumentViewCommand = new RelayCommand(new Action<object>(OpenEmployeeLeaveDocument));
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                //[002] added
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                CustomRowFilterCommand = new DelegateCommand<RowFilterEventArgs>(CustomRowFilter);

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

                Setting = CrmStartUp.GetGeosAppSettings(11);
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    // [003] Changed service method GetAllEmployeesByDepartmentByIdCompany_V2032 to GetAllEmployeesByDepartmentByIdCompany_V2039
                    // [004] Changed service method GetAllEmployeesByDepartmentByIdCompany_V2039 to GetAllEmployeesByDepartmentByIdCompany_V2041
                    Departments = new ObservableCollection<Department>(HrmService.GetAllEmployeesByDepartmentByIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds));
                    // [003] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2032 to GetEmployeeLeavesBySelectedIdCompany_V2039
                    // [004] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2039 to GetEmployeeLeavesBySelectedIdCompany_V2041
                    // [005] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2041 to GetEmployeeLeavesBySelectedIdCompany_V2045
                   // EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2045(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                  
                    /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
                    //EmployeeLeaveOrigional = EmployeeLeaves;
                    //if (EmployeeLeaves.Count > 0)
                    //{
                    //    SelectedLeaveRecord = EmployeeLeaves[0];
                    //}
                    //[001] code Comment
                    // CompanyLeaves = new ObservableCollection<CompanyLeave>(HrmService.GetSelectedIdCompanyLeaves(plantOwnersIds));
                    //Listappointments = new ObservableCollection<ModelAppointment>();
                    AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                    LabelItems = new ObservableCollection<LabelHelper>();
                    HolidayList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(28).AsEnumerable());
                    //TimeRulers = new ObservableCollection<TimeRulerHelper>();

                    //TimeRulers.Clear();// Remove TimeRuler from WeekView

                    //[001] Fill employee leave as per lookup value
                    FillEmployeeLeaveType();

                    if (Departments.Count > 0)
                    {
                        SelectedItem = Departments[0];
                    }


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
                    //    AppointmentItems.Add(modelAppointment);

                    //}

                    //foreach (var EmployeeLeave in EmployeeLeaves)
                    //{
                    //    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                    //    modelAppointment.Subject = string.Format("{0} {1}_{2}", EmployeeLeave.Employee.FirstName, EmployeeLeave.Employee.LastName, EmployeeLeave.CompanyLeave.Name);
                    //    modelAppointment.StartDate = Convert.ToDateTime(EmployeeLeave.StartDate.ToString());
                    //    if (Convert.ToBoolean(EmployeeLeave.IsAllDayEvent))
                    //    {
                    //        modelAppointment.EndDate = EmployeeLeave.EndDate.Value.AddDays(1);
                    //    }
                    //    else
                    //    {
                    //        modelAppointment.EndDate = EmployeeLeave.EndDate;
                    //    }
                    //    modelAppointment.Label = EmployeeLeave.IdLeave;
                    //    modelAppointment.EmployeeCode = EmployeeLeave.Employee.EmployeeCode;
                    //    modelAppointment.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                    //    modelAppointment.IsAllDayEvent = EmployeeLeave.IsAllDayEvent;
                    //    AppointmentItems.Add(modelAppointment);
                    //}

                    //[001] Code comment and add label as per Lookup value
                    //foreach (var CompanyLeave in CompanyLeaves)
                    //{
                    //    LabelHelper label = new LabelHelper();
                    //    label.Id = Convert.ToInt32(CompanyLeave.IdCompanyLeave);
                    //    label.Color = new BrushConverter().ConvertFromString(CompanyLeave.HtmlColor.ToString()) as SolidColorBrush;
                    //    label.Caption = CompanyLeave.Name;
                    //    LabelItems.Add(label);
                    //}

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
                }
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor EmployeeLeavesViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeLeavesViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeLeavesViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeLeavesViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        
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
        /// <summary>
        /// [HRM-M041-21] Method to get activeview of scheduler
        /// </summary>
        /// <param name="e"></param>
        private void DefaultLoadCommandAction(RoutedEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DefaultLoadCommandAction()...", category: Category.Info, priority: Priority.Low);
                SchedulerControlEx scheduler = e.Source as SchedulerControlEx;
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
                    else if (scheduler.ActiveViewIndex == 1)
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
                    else
                    {
                        if (scheduler.Start.Day < 10)
                        {
                            scheduler.DisplayName = String.Format("0{0} {1:MMMM} {2:yyyy}", scheduler.Start.Day, scheduler.Start, scheduler.Start);
                        }
                        else
                        {
                            scheduler.DisplayName = String.Format("{0} {1:MMMM} {2:yyyy}", scheduler.Start.Day, scheduler.Start, scheduler.Start);

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
        /// <summary>
        /// [HRM-M041-21] Method to get selected dates from scheduler
        /// </summary>
        /// <param name="e"></param>
        private void SelectedIntervalCommandAction(MouseButtonEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIntervalCommandAction()...", category: Category.Info, priority: Priority.Low);
                SchedulerControl scheduler = e.Source as SchedulerControl;
                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;
                if (scheduler.ActiveView.Caption == "Month View")
                    SelectedEndDate = SelectedEndDate.AddDays(-1);
                else
                    ViewType = 2;

                GeosApplication.Instance.Logger.Log("Method SelectedIntervalCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIntervalCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SelectItemForScheduler(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectItemForScheduler()...", category: Category.Info, priority: Priority.Low);
                AppointmentItems.Clear();
                ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();
                if (SelectedItem is Employee)
                {
                    var EmpLeaves = EmployeeLeaves.Where(x => x.IdEmployee == ((Employee)SelectedItem).IdEmployee).ToList();
                    if (Setting == null)
                    {
                        foreach (var EmployeeLeave in EmpLeaves)
                        {

                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                            modelAppointment = FillAppointment(EmployeeLeave);
                            appointment.Add(modelAppointment);
                        }
                    }
                    else if (Setting.DefaultValue.Equals("Natural"))
                    {
                        foreach (var EmployeeLeave in EmpLeaves)
                        {
                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                            modelAppointment = FillAppointment(EmployeeLeave);
                            appointment.Add(modelAppointment);
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
                        foreach (var EmployeeLeave in tempEmployeeLeaveList)
                        {
                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                            modelAppointment = FillAppointment(EmployeeLeave);
                            appointment.Add(modelAppointment);
                        }
                    }
                }
                else if (SelectedItem is Department)
                {
                    if (((Department)SelectedItem).Employees != null)
                    {
                        var Employees = ((Department)SelectedItem).Employees.ToList();
                        if (Setting == null)
                        {
                            foreach (var employee in Employees)
                            {
                                var EmpLeaves = EmployeeLeaves.Where(x => x.IdEmployee == employee.IdEmployee).ToList();
                                foreach (var EmployeeLeave in EmpLeaves)
                                {
                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                    modelAppointment = FillAppointment(EmployeeLeave);
                                    appointment.Add(modelAppointment);
                                }
                            }
                        }
                        if (Setting.DefaultValue.Equals("Natural"))
                        {
                            foreach (var employee in Employees)
                            {
                                var EmpLeaves = EmployeeLeaves.Where(x => x.IdEmployee == employee.IdEmployee).ToList();
                                foreach (var EmployeeLeave in EmpLeaves)
                                {
                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                    modelAppointment = FillAppointment(EmployeeLeave);
                                    appointment.Add(modelAppointment);
                                }
                            }
                        }
                        else
                        {
                            foreach (var employee in Employees)
                            {
                                List<EmployeeLeave> tempEmployeeLeaveList = new List<EmployeeLeave>();
                                var EmpLeaves = EmployeeLeaves.Where(x => x.IdEmployee == employee.IdEmployee).ToList();
                                foreach (var EmployeeLeave in EmpLeaves)
                                {
                                    if (EmployeeLeave.CompanyLeave.Company != null)
                                    {
                                        if (EmployeeLeave.CompanyLeave.Company.CompanySetting != null)
                                        {
                                            if (Setting.DefaultValue.Equals(EmployeeLeave.CompanyLeave.Company.CompanySetting.Value))
                                            {
                                                string[] employeeWorkingDays = EmployeeLeave.CompanyLeave.Company.CompanyAnnualSchedule.WorkingDays.Split(',');
                                                List<EmployeeLeave> tempLeaveList = new List<EmployeeLeave>();
                                                bool isLeave = false;
                                                DateTime tempEndDate = (DateTime)EmployeeLeave.EndDate;
                                                DateTime tempStartDate = (DateTime)EmployeeLeave.StartDate;
                                                for (var item = tempStartDate; item.Date <= tempEndDate.Date; item = item.AddDays(1))
                                                {
                                                    EmployeeLeave tempLeave = new EmployeeLeave();
                                                    EmployeeLeave newLeave = new EmployeeLeave();

                                                    if (employeeWorkingDays.Any(x => x.Contains(item.DayOfWeek.ToString().Substring(0, 3))))
                                                    {
                                                        tempLeave.Employee = EmployeeLeave.Employee;
                                                        tempLeave.StartDate = item;
                                                        tempLeave.EndDate = item.Date.AddHours(EmployeeLeave.EndDate.Value.TimeOfDay.Hours).AddMinutes(EmployeeLeave.EndDate.Value.TimeOfDay.Minutes);
                                                        tempLeave.IdEmployee = EmployeeLeave.IdEmployee;
                                                        tempLeave.IdLeave = EmployeeLeave.IdLeave;
                                                        tempLeave.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                                                        tempLeave.IsAllDayEvent = EmployeeLeave.IsAllDayEvent;
                                                        tempLeave.CompanyLeave = EmployeeLeave.CompanyLeave;
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

                                foreach (var EmployeeLeave in tempEmployeeLeaveList)
                                {
                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                    modelAppointment = FillAppointment(EmployeeLeave);
                                    appointment.Add(modelAppointment);
                                }
                            }
                        }


                    }
                    else if ((Department)SelectedItem == Departments[0])
                    {
                        if (Setting == null)
                        {
                            foreach (var EmployeeLeave in EmployeeLeaves)
                            {
                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                modelAppointment = FillAppointment(EmployeeLeave);
                                appointment.Add(modelAppointment);
                            }
                        }
                        if (Setting.DefaultValue.Equals("Natural"))
                        {
                            foreach (var EmployeeLeave in EmployeeLeaves)
                            {
                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                modelAppointment = FillAppointment(EmployeeLeave);
                                appointment.Add(modelAppointment);
                            }
                        }
                        else
                        {
                            List<EmployeeLeave> tempEmployeeLeaveList = new List<EmployeeLeave>();
                            foreach (var EmployeeLeave in EmployeeLeaves)
                            {
                                if (EmployeeLeave.CompanyLeave.Company != null)
                                {
                                    if (EmployeeLeave.CompanyLeave.Company.CompanySetting != null)
                                    {
                                        if (Setting.DefaultValue.Equals(EmployeeLeave.CompanyLeave.Company.CompanySetting.Value))
                                        {
                                            string[] employeeWorkingDays = EmployeeLeave.CompanyLeave.Company.CompanyAnnualSchedule.WorkingDays.Split(',');
                                            List<EmployeeLeave> tempLeaveList = new List<EmployeeLeave>();
                                            bool isLeave = false;
                                            DateTime tempEndDate = (DateTime)EmployeeLeave.EndDate;
                                            DateTime tempStartDate = (DateTime)EmployeeLeave.StartDate;
                                            for (var item = tempStartDate; item.Date <= tempEndDate.Date; item = item.AddDays(1))
                                            {
                                                EmployeeLeave tempLeave = new EmployeeLeave();
                                                EmployeeLeave newLeave = new EmployeeLeave();

                                                if (employeeWorkingDays.Any(x => x.Contains(item.DayOfWeek.ToString().Substring(0, 3))))
                                                {
                                                    tempLeave.Employee = EmployeeLeave.Employee;
                                                    tempLeave.StartDate = item;
                                                    tempLeave.EndDate = item.Date.AddHours(EmployeeLeave.EndDate.Value.TimeOfDay.Hours).AddMinutes(EmployeeLeave.EndDate.Value.TimeOfDay.Minutes);
                                                    tempLeave.IdEmployee = EmployeeLeave.IdEmployee;
                                                    tempLeave.IdLeave = EmployeeLeave.IdLeave;
                                                    tempLeave.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                                                    tempLeave.IsAllDayEvent = EmployeeLeave.IsAllDayEvent;
                                                    tempLeave.CompanyLeave = EmployeeLeave.CompanyLeave;
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
                            foreach (var EmployeeLeave in tempEmployeeLeaveList)
                            {
                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                modelAppointment = FillAppointment(EmployeeLeave);
                                appointment.Add(modelAppointment);
                            }
                        }

                    }
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
                    //modelAppointment.AllDay = true;
                    appointment.Add(modelAppointment);

                }
                AppointmentItems = appointment;

                GeosApplication.Instance.Logger.Log("Method SelectItemForScheduler()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectItemForScheduler()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        /// <summary>
        /// [HRM-M042-31] Double-click to open employee profile in Leaves
        /// This method for Open selected Employee Profile on double click
        /// </summary>
        /// <param name="e"></param>
        private void OpenEmployeeProfile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeProfile()...", category: Category.Info, priority: Priority.Low);

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
                    var ownerInfo = (obj as FrameworkElement);
                    employeeProfileDetailView.Owner = Window.GetWindow(ownerInfo);
                    employeeProfileDetailView.ShowDialog();

                    if (employeeProfileDetailViewModel.IsSaveChanges == true)
                    {
                        employee.FirstName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.FirstName;
                        employee.LastName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.LastName;

                        if (employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdEmployeeStatus == 138)
                        {
                            if (Departments != null && Departments.Count > 0)
                            {
                                foreach (Department department in Departments)
                                {
                                    if (department.Employees != null && department.Employees.Any(x => x.IdEmployee == employee.IdEmployee))
                                    {
                                        department.Employees.RemoveAll(x => x.IdEmployee == employee.IdEmployee);
                                    }
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
        /// [001][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        /// </summary>
        /// <param name="obj"></param>
        private void AppointmentWindowShowing(AppointmentWindowShowingEventArgs obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method AppointmeentWindowShowing()...", category: Category.Info, priority: Priority.Low);
                obj.Cancel = true;
               // AppointmentWindowShowingEventArgs detailView = (AppointmentWindowShowingEventArgs)(obj);

                if (obj.Appointment.SourceObject != null)
                {
                    if (((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeLeave != null)
                    {
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();

                        EmployeeLeave selectedEmpLeave = new EmployeeLeave();
                        var EmpLeave = EmployeeLeaves.Where(x => x.IdEmployeeLeave == ((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeLeave);
                        selectedEmpLeave = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == ((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeLeave);
                        AddNewLeaveView addNewLeaveView = new AddNewLeaveView();
                        AddNewLeaveViewModel addNewLeaveViewModel = new AddNewLeaveViewModel();
                        EventHandler handle = delegate { addNewLeaveView.Close(); };
                        addNewLeaveViewModel.RequestClose += handle;
                        addNewLeaveView.DataContext = addNewLeaveViewModel;
                        addNewLeaveViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                        addNewLeaveViewModel.WorkingPlantId = selectedEmpLeave.CompanyLeave.IdCompany.ToString();
                        addNewLeaveViewModel.SelectedPlantList = plantOwners;

                        if (HrmCommon.Instance.IsPermissionReadOnly)
                            addNewLeaveViewModel.InitReadOnly(selectedEmpLeave);
                        else
                            addNewLeaveViewModel.EditInit(selectedEmpLeave, EmployeeLeaves, _employeeListFinalForLeaves, _employeeAttendanceList);

                        addNewLeaveViewModel.IsNew = false;
                        addNewLeaveViewModel.LeaveTitle = System.Windows.Application.Current.FindResource("EditLeave").ToString();
                        EmployeeJobDescription empJobDesc = new EmployeeJobDescription();
                        if (selectedEmpLeave.Employee.EmployeeJobDescription != null)
                        {
                            empJobDesc = selectedEmpLeave.Employee.EmployeeJobDescription;
                        }

                        var ownerInfo = (obj.OriginalSource as FrameworkElement);
                        addNewLeaveView.Owner = Window.GetWindow(ownerInfo);
                        addNewLeaveView.ShowDialog();


                        if (addNewLeaveViewModel.IsSave == true)
                        {
                            // Code For Update Record for Scheduler View
                            if (addNewLeaveViewModel.UpdateEmployeeLeave != null)
                            {
                                // AppointmentItems.Remove((UI.Helper.Appointment)obj.Appointment.SourceObject);
                                EmployeeLeave leave = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == addNewLeaveViewModel.UpdateEmployeeLeave.IdEmployeeLeave);
                                if (leave != null)
                                {
                                    var appointmentItemsList = AppointmentItems.Where(x => x.IdEmployeeLeave == leave.IdEmployeeLeave).ToList();
                                    foreach (var item in appointmentItemsList)
                                    {
                                        AppointmentItems.Remove(item);
                                    }
                                }
                                if (Setting == null)
                                {
                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                    modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                    AppointmentItems.Add(modelAppointment);
                                }
                                else if (Setting.DefaultValue.Equals("Natural"))
                                {
                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                    modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                    AppointmentItems.Add(modelAppointment);
                                }
                                else
                                {
                                    List<EmployeeLeave> tempEmployeeLeaveList = new List<EmployeeLeave>();
                                    if (addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift != null)
                                    {
                                        if (addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule != null)
                                        {
                                            if (addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule.CompanySetting != null)
                                            {
                                                if (Setting.DefaultValue.Equals(addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule.CompanySetting.Value))
                                                {
                                                    string[] employeeWorkingDays = addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule.WorkingDays.Split(',');
                                                    List<EmployeeLeave> tempLeaveList = new List<EmployeeLeave>();
                                                    bool isLeave = false;
                                                    DateTime tempEndDate = (DateTime)addNewLeaveViewModel.UpdateEmployeeLeave.EndDate;
                                                    DateTime tempStartDate = (DateTime)addNewLeaveViewModel.UpdateEmployeeLeave.StartDate;
                                                    for (var item = tempStartDate; item.Date <= tempEndDate.Date; item = item.AddDays(1))
                                                    //for (var item = tempStartDate; item.Date <= tempEndDate.Date; item = item.AddDays(1))
                                                    {
                                                        EmployeeLeave tempLeave = new EmployeeLeave();
                                                        EmployeeLeave newLeave = new EmployeeLeave();

                                                        if (employeeWorkingDays.Any(x => x.Contains(item.DayOfWeek.ToString().Substring(0, 3))))
                                                        {
                                                            tempLeave.Employee = addNewLeaveViewModel.UpdateEmployeeLeave.Employee;
                                                            tempLeave.StartDate = item;
                                                            tempLeave.EndDate = item.Date.AddHours(addNewLeaveViewModel.UpdateEmployeeLeave.EndDate.Value.TimeOfDay.Hours).AddMinutes(addNewLeaveViewModel.UpdateEmployeeLeave.EndDate.Value.TimeOfDay.Minutes);
                                                            tempLeave.IdEmployee = addNewLeaveViewModel.UpdateEmployeeLeave.IdEmployee;
                                                            tempLeave.IdLeave = addNewLeaveViewModel.UpdateEmployeeLeave.IdLeave;
                                                            tempLeave.IdEmployeeLeave = addNewLeaveViewModel.UpdateEmployeeLeave.IdEmployeeLeave;
                                                            tempLeave.IsAllDayEvent = addNewLeaveViewModel.UpdateEmployeeLeave.IsAllDayEvent;
                                                            tempLeave.CompanyLeave = addNewLeaveViewModel.UpdateEmployeeLeave.CompanyLeave;
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
                                                        newLeave.CompanyShift = tempStartLeave.CompanyShift;
                                                        newLeave.IdCompanyShift = tempStartLeave.IdCompanyShift;

                                                        tempEmployeeLeaveList.Add(newLeave);
                                                        isLeave = false;
                                                        tempLeaveList = new List<EmployeeLeave>();
                                                    }
                                                }
                                                else
                                                {
                                                    EmployeeLeave tempLeave = new EmployeeLeave();
                                                    tempLeave.Employee = addNewLeaveViewModel.UpdateEmployeeLeave.Employee;
                                                    tempLeave.StartDate = addNewLeaveViewModel.UpdateEmployeeLeave.StartDate;
                                                    tempLeave.EndDate = addNewLeaveViewModel.UpdateEmployeeLeave.EndDate;
                                                    tempLeave.IdEmployee = addNewLeaveViewModel.UpdateEmployeeLeave.IdEmployee;
                                                    tempLeave.IdLeave = addNewLeaveViewModel.UpdateEmployeeLeave.IdLeave;
                                                    tempLeave.IdEmployeeLeave = addNewLeaveViewModel.UpdateEmployeeLeave.IdEmployeeLeave;
                                                    tempLeave.IsAllDayEvent = addNewLeaveViewModel.UpdateEmployeeLeave.IsAllDayEvent;
                                                    tempLeave.CompanyLeave = addNewLeaveViewModel.UpdateEmployeeLeave.CompanyLeave;
                                                    tempLeave.CompanyShift = addNewLeaveViewModel.UpdateEmployeeLeave.CompanyShift;
                                                    tempLeave.IdCompanyShift = addNewLeaveViewModel.UpdateEmployeeLeave.IdCompanyShift;

                                                    tempEmployeeLeaveList.Add(tempLeave);
                                                }

                                                foreach (var EmployeeLeave in tempEmployeeLeaveList)
                                                {
                                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                    modelAppointment = FillAppointment(EmployeeLeave);
                                                    AppointmentItems.Add(modelAppointment);
                                                }
                                            }
                                            else
                                            {
                                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                                AppointmentItems.Add(modelAppointment);
                                            }
                                        }
                                        else
                                        {
                                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                            modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                            AppointmentItems.Add(modelAppointment);
                                        }
                                    }
                                    else
                                    {
                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                        AppointmentItems.Add(modelAppointment);
                                    }

                                }

                                // Code For Update Record For Grid View
                                addNewLeaveViewModel.UpdateEmployeeLeave.Employee.EmployeeJobDescription = empJobDesc;
                                selectedEmpLeave.Employee = addNewLeaveViewModel.UpdateEmployeeLeave.Employee;
                                selectedEmpLeave.CompanyLeave = addNewLeaveViewModel.UpdateEmployeeLeave.CompanyLeave;
                                selectedEmpLeave.IdEmployee = addNewLeaveViewModel.UpdateEmployeeLeave.IdEmployee;
                                selectedEmpLeave.StartDate = addNewLeaveViewModel.UpdateEmployeeLeave.StartDate;
                                selectedEmpLeave.EndDate = addNewLeaveViewModel.UpdateEmployeeLeave.EndDate;
                                selectedEmpLeave.IdLeave = addNewLeaveViewModel.UpdateEmployeeLeave.IdLeave;
                                selectedEmpLeave.FileName = addNewLeaveViewModel.UpdateEmployeeLeave.FileName;
                                selectedEmpLeave.IsAllDayEvent = addNewLeaveViewModel.UpdateEmployeeLeave.IsAllDayEvent;
                                selectedEmpLeave.CompanyLeave.Company = addNewLeaveViewModel.UpdateEmployeeLeave.CompanyLeave.Company;
                                selectedEmpLeave.StartTime = addNewLeaveViewModel.STime;
                                selectedEmpLeave.EndTime = addNewLeaveViewModel.ETime;
                                selectedEmpLeave.Remark = addNewLeaveViewModel.UpdateEmployeeLeave.Remark;
                                selectedEmpLeave.CompanyShift = addNewLeaveViewModel.UpdateEmployeeLeave.CompanyShift;
                                selectedEmpLeave.IdCompanyShift = addNewLeaveViewModel.UpdateEmployeeLeave.IdCompanyShift;


                                if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)selectedEmpLeave.StartDate) < 10)
                                {
                                    selectedEmpLeave.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)selectedEmpLeave.StartDate);
                                }
                                else
                                {
                                    selectedEmpLeave.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)selectedEmpLeave.StartDate);

                                }
                                SelectedLeaveRecord = selectedEmpLeave;

                            }
                            if (addNewLeaveViewModel.NewEmployeeLeaveList != null)
                            {
                                foreach (EmployeeLeave NewEmpLeave in addNewLeaveViewModel.NewEmployeeLeaveList)
                                {
                                    //[001] changes as per add  new leave in selected employee or departmet
                                    if (SelectedItem != null)
                                    {
                                        Employee SelectedEmployee = null;
                                        if (SelectedItem is Employee)
                                        {
                                            SelectedEmployee = ((Employee)SelectedItem);
                                        }
                                        else if (SelectedItem is Department)
                                        {
                                            if (((Department)SelectedItem).Employees != null)
                                            {
                                                SelectedEmployee = ((Department)SelectedItem).Employees.Where(x => x.IdEmployee == NewEmpLeave.Employee.IdEmployee).FirstOrDefault();
                                            }
                                            else if ((Department)SelectedItem == Departments[0] && SelectedEmployee == null)
                                            {
                                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                modelAppointment = FillAppointment(NewEmpLeave);

                                                AppointmentItems.Add(modelAppointment);
                                            }
                                        }

                                        if (SelectedEmployee != null)
                                        {
                                            if (NewEmpLeave.Employee.IdEmployee == SelectedEmployee.IdEmployee)
                                            {
                                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                modelAppointment = FillAppointment(NewEmpLeave);
                                                AppointmentItems.Add(modelAppointment);
                                            }
                                        }
                                    }
                                    NewEmpLeave.StartTime = addNewLeaveViewModel.STime;
                                    NewEmpLeave.EndTime = addNewLeaveViewModel.ETime;
                                    //NewEmpLeave.CompanyLeave = GetCompanyLeave(GeosApplication.Instance.EmployeeLeaveList[GeosApplication.Instance.EmployeeLeaveList.FindIndex(x => x.IdLookupValue == (int)NewEmpLeave.CompanyLeave.IdCompanyLeave)]);
                                    NewEmpLeave.CompanyLeave = GetCompanyLeave(GeosApplication.Instance.EmployeeLeaveList.FirstOrDefault(x => x.IdLookupValue == (int)NewEmpLeave.CompanyLeave.IdCompanyLeave), NewEmpLeave);

                                    if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)NewEmpLeave.StartDate) < 10)
                                    {
                                        NewEmpLeave.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)NewEmpLeave.StartDate);
                                    }
                                    else
                                    {
                                        NewEmpLeave.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)NewEmpLeave.StartDate);
                                    }
                                    //[001]added
                                    NewEmpLeave.CompanyLeave.Company = NewEmpLeave.CompanyLeave.Company;
                                    EmployeeLeaves.Add(NewEmpLeave);
                                    SelectedLeaveRecord = NewEmpLeave;
                                }
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
        /// Method to Edit Leave Information
        /// Done by Amit in Sprint 41 For Task [HRM-M041-19] New grid view in Leaves
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// </summary>

        private void EditLeaveInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditLeaveInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                EmployeeLeave employeeLeave = (EmployeeLeave)detailView.FocusedRow;
                SelectedLeaveRecord = employeeLeave;
                if (employeeLeave != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    AddNewLeaveView addNewLeaveView = new AddNewLeaveView();
                    AddNewLeaveViewModel addNewLeaveViewModel = new AddNewLeaveViewModel();
                    EventHandler handle = delegate { addNewLeaveView.Close(); };
                    addNewLeaveViewModel.RequestClose += handle;
                    addNewLeaveView.DataContext = addNewLeaveViewModel;
                    addNewLeaveViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                    addNewLeaveViewModel.WorkingPlantId = employeeLeave.CompanyLeave.IdCompany.ToString();
                    addNewLeaveViewModel.SelectedPlantList = plantOwners;

                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addNewLeaveViewModel.InitReadOnly(employeeLeave);
                    else
                        addNewLeaveViewModel.EditInit(employeeLeave, EmployeeLeaves);
                    addNewLeaveViewModel.IsNew = false;
                    addNewLeaveViewModel.LeaveTitle = System.Windows.Application.Current.FindResource("EditLeave").ToString();
                    EmployeeJobDescription empJobDesc = new EmployeeJobDescription();
                    if (employeeLeave.Employee.EmployeeJobDescription != null)
                    {
                        empJobDesc = employeeLeave.Employee.EmployeeJobDescription;
                    }
                    var ownerInfo = (detailView as FrameworkElement);
                    addNewLeaveView.Owner = Window.GetWindow(ownerInfo);
                    addNewLeaveView.ShowDialog();

                    if (addNewLeaveViewModel.IsSave == true)
                    {
                        // Code For Update Record For Grid View
                        addNewLeaveViewModel.UpdateEmployeeLeave.Employee.EmployeeJobDescription = empJobDesc;
                        employeeLeave.Employee = addNewLeaveViewModel.UpdateEmployeeLeave.Employee;
                        employeeLeave.CompanyLeave = addNewLeaveViewModel.UpdateEmployeeLeave.CompanyLeave;
						        /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
                        employeeLeave.CompanyShift= addNewLeaveViewModel.UpdateEmployeeLeave.CompanyShift;
                        employeeLeave.IdCompanyShift = addNewLeaveViewModel.UpdateEmployeeLeave.IdCompanyShift;
                        employeeLeave.IdEmployee = addNewLeaveViewModel.UpdateEmployeeLeave.IdEmployee;
                        employeeLeave.StartDate = addNewLeaveViewModel.UpdateEmployeeLeave.StartDate;
                        employeeLeave.StartTime = addNewLeaveViewModel.STime;
                        employeeLeave.EndDate = addNewLeaveViewModel.UpdateEmployeeLeave.EndDate;
                        employeeLeave.EndTime = addNewLeaveViewModel.ETime;
                        employeeLeave.IdLeave = addNewLeaveViewModel.UpdateEmployeeLeave.IdLeave;
                        employeeLeave.FileName = addNewLeaveViewModel.UpdateEmployeeLeave.FileName;
                        employeeLeave.IsAllDayEvent = addNewLeaveViewModel.UpdateEmployeeLeave.IsAllDayEvent;
                        employeeLeave.Remark = addNewLeaveViewModel.UpdateEmployeeLeave.Remark;
                        employeeLeave.CompanyLeave.Company.Alias = addNewLeaveViewModel.UpdateEmployeeLeave.CompanyLeave.Company.Alias;

                        employeeLeave.CompanyLeave.HtmlColor = GetCompanyLeave(GeosApplication.Instance.EmployeeLeaveList.FirstOrDefault(x => x.IdLookupValue == (int)employeeLeave.CompanyLeave.IdCompanyLeave), employeeLeave).HtmlColor;

                        if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)employeeLeave.StartDate) < 10)
                        {
                            employeeLeave.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)employeeLeave.StartDate);
                        }
                        else
                        {
                            employeeLeave.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)employeeLeave.StartDate);

                        }
                        SelectedLeaveRecord = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == employeeLeave.IdEmployeeLeave);
                        SelectedLeaveRecord = employeeLeave;
                        OnPropertyChanged(new PropertyChangedEventArgs("SelectedLeaveRecord"));

                        //[001] Changes as per lookup value
                        if (SelectedItem != null)
                        {
                            Employee SelectedEmployee = null;
                            if (SelectedItem is Employee)
                            {
                                SelectedEmployee = ((Employee)SelectedItem);
                            }
                            else if (SelectedItem is Department)
                            {
                                if (((Department)SelectedItem).Employees != null)
                                {
                                    SelectedEmployee = ((Department)SelectedItem).Employees.Where(x => x.IdEmployee == addNewLeaveViewModel.UpdateEmployeeLeave.Employee.IdEmployee).FirstOrDefault();
                                }
                                else if ((Department)SelectedItem == Departments[0] && SelectedEmployee == null)
                                {
                                    EmployeeLeave leave = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == addNewLeaveViewModel.UpdateEmployeeLeave.IdEmployeeLeave);
                                    if (leave != null)
                                    {
                                        var appointmentItemsList = AppointmentItems.Where(x => x.IdEmployeeLeave == leave.IdEmployeeLeave).ToList();
                                        foreach (var item in appointmentItemsList)
                                        {
                                            AppointmentItems.Remove(item);
                                        }
                                    }
                                    if (Setting == null)
                                    {
                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                        AppointmentItems.Add(modelAppointment);
                                    }
                                    else if (Setting.DefaultValue.Equals("Natural"))
                                    {
                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                        AppointmentItems.Add(modelAppointment);
                                    }
                                    else
                                    {
                                        List<EmployeeLeave> tempEmployeeLeaveList = new List<EmployeeLeave>();
                                        List<EmployeeLeave> leaveList = new List<EmployeeLeave>();
                                        if (addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift != null)
                                        {
                                            if (addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule != null)
                                            {
                                                if (addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule.CompanySetting != null)
                                                {
                                                    if (Setting.DefaultValue.Equals(addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule.CompanySetting.Value))
                                                    {
                                                        leaveList = FillEmployeeLeaveList(addNewLeaveViewModel.UpdateEmployeeLeave);
                                                        foreach (var item in leaveList)
                                                        {
                                                            tempEmployeeLeaveList.Add(item);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tempEmployeeLeaveList.Add(addNewLeaveViewModel.UpdateEmployeeLeave);
                                                    }

                                                    foreach (var EmployeeLeave in tempEmployeeLeaveList)
                                                    {
                                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                        modelAppointment = FillAppointment(EmployeeLeave);
                                                        AppointmentItems.Add(modelAppointment);
                                                    }
                                                }
                                                else
                                                {
                                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                    modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                                    AppointmentItems.Add(modelAppointment);
                                                }
                                            }
                                            else
                                            {
                                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                                AppointmentItems.Add(modelAppointment);
                                            }
                                        }
                                        else
                                        {
                                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                            modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                            AppointmentItems.Add(modelAppointment);
                                        }
                                    }

                                }
                            }

                            // Code For Update Record for Scheduler View

                            if (SelectedEmployee != null)
                            {
                                if (addNewLeaveViewModel.UpdateEmployeeLeave.Employee.IdEmployee == SelectedEmployee.IdEmployee)
                                {
                                    EmployeeLeave leave = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == addNewLeaveViewModel.UpdateEmployeeLeave.IdEmployeeLeave);
                                    if (leave != null)
                                    {
                                        var appointmentItemsList = AppointmentItems.Where(x => x.IdEmployeeLeave == leave.IdEmployeeLeave).ToList();
                                        foreach (var item in appointmentItemsList)
                                        {
                                            AppointmentItems.Remove(item);
                                        }
                                    }
                                    if (Setting == null)
                                    {
                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                        AppointmentItems.Add(modelAppointment);
                                    }
                                    else if (Setting.DefaultValue.Equals("Natural"))
                                    {
                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                        AppointmentItems.Add(modelAppointment);
                                    }
                                    else
                                    {
                                        List<EmployeeLeave> tempEmployeeLeaveList = new List<EmployeeLeave>();
                                        List<EmployeeLeave> leaveList = new List<EmployeeLeave>();
                                        if (addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift != null)
                                        {
                                            if (addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule != null)
                                            {
                                                if (addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule.CompanySetting != null)
                                                {
                                                    if (Setting.DefaultValue.Equals(addNewLeaveViewModel.UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule.CompanySetting.Value))
                                                    {
                                                        leaveList = FillEmployeeLeaveList(addNewLeaveViewModel.UpdateEmployeeLeave);
                                                        foreach (var item in leaveList)
                                                        {
                                                            tempEmployeeLeaveList.Add(item);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tempEmployeeLeaveList.Add(addNewLeaveViewModel.UpdateEmployeeLeave);
                                                    }

                                                    foreach (var EmployeeLeave in tempEmployeeLeaveList)
                                                    {
                                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                        modelAppointment = FillAppointment(EmployeeLeave);
                                                        AppointmentItems.Add(modelAppointment);
                                                    }
                                                }
                                                else
                                                {
                                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                    modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                                    AppointmentItems.Add(modelAppointment);
                                                }
                                            }
                                            else
                                            {
                                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                                AppointmentItems.Add(modelAppointment);
                                            }
                                        }
                                        else
                                        {
                                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                            modelAppointment = FillAppointment(addNewLeaveViewModel.UpdateEmployeeLeave);
                                            AppointmentItems.Add(modelAppointment);
                                        }
                                    }
                                }
                            }
                        }

                        //While update if StartDate and end Date year not same split in two (First one updated and second inserted)
                        foreach (EmployeeLeave NewEmpLeave in addNewLeaveViewModel.NewEmployeeLeaveList)
                        {
                            //[001] changes as per add  new leave in selected employee or departmet
                            if (SelectedItem != null)
                            {
                                Employee SelectedEmployee = null;
                                if (SelectedItem is Employee)
                                {
                                    SelectedEmployee = ((Employee)SelectedItem);
                                }
                                else if (SelectedItem is Department)
                                {
                                    if (((Department)SelectedItem).Employees != null)
                                    {
                                        SelectedEmployee = ((Department)SelectedItem).Employees.Where(x => x.IdEmployee == NewEmpLeave.Employee.IdEmployee).FirstOrDefault();
                                    }
                                    else if ((Department)SelectedItem == Departments[0] && SelectedEmployee == null)
                                    {
                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment.Subject = string.Format("{0} {1}_{2}", NewEmpLeave.Employee.FirstName, NewEmpLeave.Employee.LastName, NewEmpLeave.CompanyLeave.Name);
                                        modelAppointment.StartDate = NewEmpLeave.StartDate;
                                        if (Convert.ToBoolean(NewEmpLeave.IsAllDayEvent))
                                        {
                                            modelAppointment.EndDate = NewEmpLeave.EndDate.Value.AddDays(1);
                                        }
                                        else
                                        {
                                            modelAppointment.EndDate = NewEmpLeave.EndDate;
                                        }

                                        modelAppointment.Label = NewEmpLeave.IdLeave;
                                        modelAppointment.EmployeeCode = NewEmpLeave.Employee.EmployeeCode;
                                        modelAppointment.IdEmployeeLeave = NewEmpLeave.IdEmployeeLeave;
                                        modelAppointment.IsAllDayEvent = NewEmpLeave.IsAllDayEvent;
                                        modelAppointment.IdEmployee = NewEmpLeave.IdEmployee;

                                        AppointmentItems.Add(modelAppointment);
                                    }
                                }

                                if (SelectedEmployee != null)
                                {
                                    if (NewEmpLeave.Employee.IdEmployee == SelectedEmployee.IdEmployee)
                                    {
                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment.Subject = string.Format("{0} {1}_{2}", NewEmpLeave.Employee.FirstName, NewEmpLeave.Employee.LastName, NewEmpLeave.CompanyLeave.Name);
                                        modelAppointment.StartDate = NewEmpLeave.StartDate;
                                        if (Convert.ToBoolean(NewEmpLeave.IsAllDayEvent))
                                        {
                                            modelAppointment.EndDate = NewEmpLeave.EndDate.Value.AddDays(1);
                                        }
                                        else
                                        {
                                            modelAppointment.EndDate = NewEmpLeave.EndDate;
                                        }
                                        modelAppointment.Label = NewEmpLeave.IdLeave;
                                        modelAppointment.EmployeeCode = NewEmpLeave.Employee.EmployeeCode;
                                        modelAppointment.IdEmployeeLeave = NewEmpLeave.IdEmployeeLeave;
                                        modelAppointment.IdEmployee = NewEmpLeave.IdEmployee;
                                        modelAppointment.IsAllDayEvent = NewEmpLeave.IsAllDayEvent;
                                        AppointmentItems.Add(modelAppointment);
                                    }
                                }
                            }
                            NewEmpLeave.StartTime = addNewLeaveViewModel.STime;
                            NewEmpLeave.EndTime = addNewLeaveViewModel.ETime;
                            //NewEmpLeave.CompanyLeave = GetCompanyLeave(GeosApplication.Instance.EmployeeLeaveList[GeosApplication.Instance.EmployeeLeaveList.FindIndex(x => x.IdLookupValue == (int)NewEmpLeave.CompanyLeave.IdCompanyLeave)]);
                            NewEmpLeave.CompanyLeave = GetCompanyLeave(GeosApplication.Instance.EmployeeLeaveList.FirstOrDefault(x => x.IdLookupValue == (int)NewEmpLeave.CompanyLeave.IdCompanyLeave), NewEmpLeave);

                            if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)NewEmpLeave.StartDate) < 10)
                            {
                                NewEmpLeave.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)NewEmpLeave.StartDate);
                            }
                            else
                            {
                                NewEmpLeave.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)NewEmpLeave.StartDate);
                            }
                            NewEmpLeave.CompanyLeave.Company = NewEmpLeave.CompanyLeave.Company;
                            EmployeeLeaves.Add(NewEmpLeave);
                            SelectedLeaveRecord = NewEmpLeave;
                        }

                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditLeaveInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditLeaveInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to handle Recurrence window popup
        /// Done by mayuri sprint 41
        /// </summary>
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

        private void DeleteOccurrenceWindowShowing(DeleteOccurrenceWindowShowingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteOccurrenceWindowShowing()...", category: Category.Info, priority: Priority.Low);

                obj.Cancel = true;

                GeosApplication.Instance.Logger.Log("Method DeleteOccurrenceWindowShowing()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteOccurrenceWindowShowing()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        
        public void Init(ObservableCollection<Employee> employeeListFinalForLeaves, ObservableCollection<EmployeeAttendance> employeeAttendanceList,
                            ObservableCollection<Department> departments, ObservableCollection<EmployeeLeave> employeeLeaves)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                _employeeListFinalForLeaves = employeeListFinalForLeaves;
				EmployeeLeaves = employeeLeaves;
                EmployeeLeaveOrigional = EmployeeLeaves;

                if (EmployeeLeaves.Count > 0)
                {
                    SelectedLeaveRecord = EmployeeLeaves[0];
                }

                _employeeAttendanceList = employeeAttendanceList;
                Departments = departments;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        /// <summary>
        /// [HRM-M049-37]
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteAppointment(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteAppointment()..", category: Category.Info, priority: Priority.Low);

                if (HrmCommon.Instance.IsPermissionReadOnly)
                    return;

                if (e is KeyEventArgs)
                {
                    KeyEventArgs obj = e as KeyEventArgs;

                    if (obj != null && obj.Source is SchedulerControlEx)
                    {
                        if (obj.Key == Key.Delete)
                        {
                            SchedulerControlEx schedule = (SchedulerControlEx)obj.Source;
                            if (schedule.SelectedAppointments != null)
                            {
                                if (schedule.SelectedAppointments.Count > 0)
                                {
                                    if (schedule.SelectedAppointments[0].CustomFields["IdEmployeeLeave"] != null)
                                    {
                                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteEmpLeaveMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                        if (MessageBoxResult == MessageBoxResult.Yes)
                                        {
                                            string LeaveType = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == (ulong)schedule.SelectedAppointments[0].CustomFields["IdEmployeeLeave"]).CompanyLeave.Name;
                                            EmployeeLeave employeeLeave = new EmployeeLeave();
                                            employeeLeave.EmployeeChangelogs = new List<EmployeeChangelog>();
                                            employeeLeave.IdEmployee = (int)schedule.SelectedAppointments[0].CustomFields["IdEmployee"];
                                            employeeLeave.IdEmployeeLeave = (ulong)schedule.SelectedAppointments[0].CustomFields["IdEmployeeLeave"];
                                            employeeLeave.EmployeeChangelogs.Add(new EmployeeChangelog()
                                            {
                                                IdEmployee = (int)schedule.SelectedAppointments[0].CustomFields["IdEmployee"],
                                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveDeleteChangeLog").ToString(), LeaveType, schedule.SelectedAppointments[0].Start.ToShortDateString(), schedule.SelectedAppointments[0].End.ToShortDateString())
                                            });

                                            bool result = HrmService.DeleteEmployeeLeave((schedule.SelectedAppointments[0].CustomFields["EmployeeCode"]).ToString(), Convert.ToUInt32(schedule.SelectedAppointments[0].CustomFields["IdEmployeeLeave"].ToString()), employeeLeave);
                                            EmployeeLeaves.Remove(EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == Convert.ToUInt64(schedule.SelectedAppointments[0].CustomFields["IdEmployeeLeave"])));
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteEmployeeLeaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
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

                    if (appointment.CustomFields["IdEmployeeLeave"] != null)
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteEmpLeaveMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            var selected_leave = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == (ulong)appointment.CustomFields["IdEmployeeLeave"]);
                            if (selected_leave?.FileName != null)
                            {
                                MessageBoxResult ConformMessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteConformEmpLeaveMessage"].ToString(), Application.Current.Resources["PopUpDeleteConformColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo, MessageBoxResult.No);

                                if (ConformMessageBoxResult == MessageBoxResult.Yes)
                                {
                                    deleteEmployeeLeave(e);
                                }
                                else
                                {
                                    ((AppointmentItemCancelEventArgs)e).Cancel = true;
                                }
                            }
                            if (selected_leave?.FileName == null && MessageBoxResult == MessageBoxResult.Yes)
                            {
                                deleteEmployeeLeave(e);
                            }


                        }


                        else
                        {
                            ((AppointmentItemCancelEventArgs)e).Cancel = true;
                        }
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

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// [002] [cpatil][2020-01-22][GEOS2-2008] We can not see some the Employees in LEAVES and ATTENDANCE with Period = 2019, but the same employees in 2020 we can see.
        /// [003] [cpatil][2020-03-17][GEOS2-1876] Draft users in Leaves.
        /// </summary>
        public void RefreshLeavesView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshLeavesView()...", category: Category.Info, priority: Priority.Low);

                // IsBusy = true;
                if (!DXSplashScreen.IsActive)
                {
                    // DXSplashScreen.Show<SplashScreenView>(); 
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


                //AppointmentItems.Clear();
                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

                var values = (object[])obj;
                SchedulerControlEx schedulerControlEx = (SchedulerControlEx)values[0];
                int year = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                DateTime newDt = new DateTime(year, SelectedStartDate.Month, 1);
                schedulerControlEx.Month = newDt;
                DateTime start = newDt;
                DateTime end = start.AddDays(1);
                schedulerControlEx.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);

                SelectedItem = null;

                Setting = CrmStartUp.GetGeosAppSettings(11);

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
                    // [002] Changed service method GetAllEmployeesByDepartmentByIdCompany_V2032 to GetAllEmployeesByDepartmentByIdCompany_V2039
                    // [003] Changed service method GetAllEmployeesByDepartmentByIdCompany_V2039 to GetAllEmployeesByDepartmentByIdCompany_V2041
                    Departments = new ObservableCollection<Department>(HrmService.GetAllEmployeesByDepartmentByIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    // [002] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2032 to GetEmployeeLeavesBySelectedIdCompany_V2039
                    // [003] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2039 to GetEmployeeLeavesBySelectedIdCompany_V2041
                    // [004] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2041 to GetEmployeeLeavesBySelectedIdCompany_V2045
                    EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2045(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    EmployeeLeaveOrigional = EmployeeLeaves;
                    CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds));

                    //[001] Code comment
                    //  CompanyLeaves = new ObservableCollection<CompanyLeave>(HrmService.GetSelectedIdCompanyLeaves(plantOwnersIds));

                    AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                    LabelItems = new ObservableCollection<LabelHelper>();
                    HolidayList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(28).AsEnumerable());
                    ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();
                    //ObservableCollection<LabelHelper> labelItem = new ObservableCollection<LabelHelper>();

                    //if (SelectedItem is Employee)
                    //{
                    //    var EmpLeaves = EmployeeLeaves.Where(x => x.IdEmployee == ((Employee)SelectedItem).IdEmployee).ToList();
                    //    foreach (var EmployeeLeave in EmpLeaves)
                    //    {
                    //        Appointment modelAppointment = new Appointment();
                    //        modelAppointment.Subject = string.Format("{0} {1}-{2}", EmployeeLeave.Employee.FirstName, EmployeeLeave.Employee.LastName, EmployeeLeave.CompanyLeave.Name);
                    //        modelAppointment.StartDate = EmployeeLeave.StartDate;
                    //        modelAppointment.EndDate = EmployeeLeave.EndDate;
                    //        modelAppointment.Label = EmployeeLeave.IdLeave;
                    //        modelAppointment.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                    //        //   modelAppointment.AllDay = true;
                    //        appointment.Add(modelAppointment);
                    //    }
                    //}
                    //else if (SelectedItem is Department)
                    //{
                    //    if (((Department)SelectedItem).Employees != null)
                    //    {
                    //        var Employees = ((Department)SelectedItem).Employees.ToList();
                    //        foreach (var employee in Employees)
                    //        {
                    //            var EmpLeaves = EmployeeLeaves.Where(x => x.IdEmployee == employee.IdEmployee).ToList();
                    //            foreach (var EmployeeLeave in EmpLeaves)
                    //            {
                    //                Appointment modelAppointment = new Appointment();
                    //                modelAppointment.Subject = string.Format("{0} {1}-{2}", EmployeeLeave.Employee.FirstName, EmployeeLeave.Employee.LastName, EmployeeLeave.CompanyLeave.Name);
                    //                modelAppointment.StartDate = EmployeeLeave.StartDate;
                    //                modelAppointment.EndDate = EmployeeLeave.EndDate;
                    //                modelAppointment.Label = EmployeeLeave.IdLeave;
                    //                modelAppointment.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                    //                //  modelAppointment.AllDay = true;
                    //                appointment.Add(modelAppointment);
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    if (Departments.Count > 0)
                    {
                        SelectedItem = Departments[0];
                    }

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

                    //foreach (var EmployeeLeave in EmployeeLeaves)
                    //{
                    //    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                    //    modelAppointment.Subject = string.Format("{0} {1}_{2}", EmployeeLeave.Employee.FirstName, EmployeeLeave.Employee.LastName, EmployeeLeave.CompanyLeave.Name);
                    //    modelAppointment.StartDate = Convert.ToDateTime(EmployeeLeave.StartDate.ToString());
                    //    if (Convert.ToBoolean(EmployeeLeave.IsAllDayEvent))
                    //    {
                    //        modelAppointment.EndDate = EmployeeLeave.EndDate.Value.AddDays(1);
                    //    }
                    //    else
                    //    {
                    //        modelAppointment.EndDate = EmployeeLeave.EndDate;
                    //    }
                    //    modelAppointment.Label = EmployeeLeave.IdLeave;
                    //    modelAppointment.EmployeeCode = EmployeeLeave.Employee.EmployeeCode;
                    //    modelAppointment.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                    //    modelAppointment.IsAllDayEvent = EmployeeLeave.IsAllDayEvent;
                    //    appointment.Add(modelAppointment);
                    //}
                    //AppointmentItems = appointment;


                    //[001] code comment and Fill Employee leave type as per lookupValue
                    //foreach (var CompanyLeave in CompanyLeaves)
                    //{
                    //    LabelHelper label = new LabelHelper();
                    //    label.Id = Convert.ToInt32(CompanyLeave.IdCompanyLeave);
                    //    label.Color = new BrushConverter().ConvertFromString(CompanyLeave.HtmlColor.ToString()) as SolidColorBrush;
                    //    label.Caption = CompanyLeave.Name;
                    //    LabelItems.Add(label);
                    //}

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
                    // LabelItems = labelItem;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                else
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }

                GeosApplication.Instance.Logger.Log("Method RefreshLeavesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshLeavesView() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshLeavesView() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                // IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshLeavesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewLeave(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewLeave()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)((object[])obj)[0];
                objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
                CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();
                List<Company> plantOwners = new List<Company>();
                List<Company> plantOwnersIds = new List<Company>();
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    plantOwnersIds = plantOwners.Where(i => i.IdCompany == CurrentGeosProvider.IdCompany).ToList();
                }
                AddNewLeaveView addNewLeaveView = new AddNewLeaveView();
                AddNewLeaveViewModel addNewLeaveViewModel = new AddNewLeaveViewModel();
                EventHandler handle = delegate { addNewLeaveView.Close(); };
                addNewLeaveViewModel.RequestClose += handle;
                addNewLeaveView.DataContext = addNewLeaveViewModel;

                addNewLeaveViewModel.WorkingPlantId = CurrentGeosProvider.IdCompany.ToString();

                addNewLeaveViewModel.SelectedPlantList = plantOwners;
                addNewLeaveViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                //[HRM-M041-21]
                object selectedEmployee = SelectedItem;
                if (IsGridViewVisible == Visibility.Visible)
                    selectedEmployee = null;

                addNewLeaveViewModel.Init(EmployeeLeaves, selectedEmployee, SelectedStartDate, SelectedEndDate, ViewType, _employeeListFinalForLeaves, _employeeAttendanceList);
                //UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                //modelAppointment.EmployeeCode = (UI.Helper.Appointment)SelectedItem.EmployeeCode;
                addNewLeaveViewModel.IsNew = true;
                addNewLeaveViewModel.LeaveTitle = System.Windows.Application.Current.FindResource("AddNewLeave").ToString();
                var ownerInfo = (detailView as FrameworkElement);
                addNewLeaveView.Owner = Window.GetWindow(ownerInfo);
                addNewLeaveView.ShowDialog();

                if (addNewLeaveViewModel.IsSave == true)
                {

                    foreach (EmployeeLeave NewEmpLeave in addNewLeaveViewModel.NewEmployeeLeaveList)
                    {
                        //[001] changes as per add  new leave in selected employee or departmet
                        if (SelectedItem != null)
                        {
                            Employee SelectedEmployee = null;
                            if (SelectedItem is Employee)
                            {
                                SelectedEmployee = ((Employee)SelectedItem);
                            }
                            else if (SelectedItem is Department)
                            {
                                if (((Department)SelectedItem).Employees != null)
                                {
                                    SelectedEmployee = ((Department)SelectedItem).Employees.Where(x => x.IdEmployee == NewEmpLeave.Employee.IdEmployee).FirstOrDefault();
                                }
                                else if ((Department)SelectedItem == Departments[0] && SelectedEmployee == null)
                                {
                                    if (Setting == null)
                                    {
                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment = FillAppointment(NewEmpLeave);
                                        AppointmentItems.Add(modelAppointment);
                                    }
                                    else
                                    {
                                        if (Setting.DefaultValue.Equals("Natural"))
                                        {
                                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                            modelAppointment = FillAppointment(NewEmpLeave);
                                            AppointmentItems.Add(modelAppointment);
                                        }
                                        else
                                        {
                                            List<EmployeeLeave> tempEmployeeLeaveList = new List<EmployeeLeave>();
                                            if (NewEmpLeave.Employee.CompanyShift != null)
                                            {
                                                if (NewEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule != null)
                                                {
                                                    if (NewEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.CompanySetting != null)
                                                    {
                                                        if ((Setting.DefaultValue.Equals(NewEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.CompanySetting.Value)))
                                                        {
                                                            string[] employeeWorkingDays = NewEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.WorkingDays.Split(',');
                                                            List<EmployeeLeave> tempLeaveList = new List<EmployeeLeave>();
                                                            bool isLeave = false;
                                                            DateTime tempEndDate = (DateTime)NewEmpLeave.EndDate;
                                                            DateTime tempStartDate = (DateTime)NewEmpLeave.StartDate;
                                                            for (var item = tempStartDate; item.Date <= tempEndDate.Date; item = item.AddDays(1))
                                                            {
                                                                EmployeeLeave tempLeave = new EmployeeLeave();
                                                                EmployeeLeave newLeave = new EmployeeLeave();

                                                                if (employeeWorkingDays.Any(x => x.Contains(item.DayOfWeek.ToString().Substring(0, 3))))
                                                                {
                                                                    tempLeave.Employee = NewEmpLeave.Employee;
                                                                    tempLeave.StartDate = item;
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
                                                                        //newLeave.CompanyShift = tempStartLeave.CompanyShift;
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


                                                        }
                                                        else
                                                        {
                                                            EmployeeLeave tempLeave = new EmployeeLeave();
                                                            tempLeave.Employee = NewEmpLeave.Employee;
                                                            tempLeave.StartDate = NewEmpLeave.StartDate;
                                                            tempLeave.EndDate = NewEmpLeave.EndDate;
                                                            tempLeave.IdEmployee = NewEmpLeave.IdEmployee;
                                                            tempLeave.IdLeave = NewEmpLeave.IdLeave;
                                                            tempLeave.IdEmployeeLeave = NewEmpLeave.IdEmployeeLeave;
                                                            tempLeave.IsAllDayEvent = NewEmpLeave.IsAllDayEvent;
                                                            tempLeave.CompanyLeave = NewEmpLeave.CompanyLeave;
                                                            tempEmployeeLeaveList.Add(tempLeave);
                                                        }

                                                        foreach (var item in tempEmployeeLeaveList)
                                                        {
                                                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                            modelAppointment = FillAppointment(item);
                                                            AppointmentItems.Add(modelAppointment);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                        modelAppointment = FillAppointment(NewEmpLeave);
                                                        AppointmentItems.Add(modelAppointment);
                                                    }
                                                }
                                                else
                                                {
                                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                    modelAppointment = FillAppointment(NewEmpLeave);
                                                    AppointmentItems.Add(modelAppointment);
                                                }
                                            }
                                            else
                                            {
                                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                modelAppointment = FillAppointment(NewEmpLeave);
                                                AppointmentItems.Add(modelAppointment);
                                            }
                                        }
                                    }
                                }
                            }

                            if (SelectedEmployee != null)
                            {
                                if (NewEmpLeave.Employee.IdEmployee == SelectedEmployee.IdEmployee)
                                {
                                    if (Setting == null)
                                    {
                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment = FillAppointment(NewEmpLeave);
                                        AppointmentItems.Add(modelAppointment);
                                    }
                                    else if (Setting.DefaultValue.Equals("Natural"))
                                    {
                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                        modelAppointment = FillAppointment(NewEmpLeave);
                                        AppointmentItems.Add(modelAppointment);
                                    }
                                    else
                                    {

                                        List<EmployeeLeave> tempEmployeeLeaveList = new List<EmployeeLeave>();
                                        if (NewEmpLeave.Employee.CompanyShift != null)
                                        {
                                            if (NewEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule != null)
                                            {
                                                if (NewEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.CompanySetting != null)
                                                {

                                                    if ((Setting.DefaultValue.Equals(NewEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.CompanySetting.Value)))
                                                    {
                                                        string[] employeeWorkingDays = NewEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.WorkingDays.Split(',');
                                                        List<EmployeeLeave> tempLeaveList = new List<EmployeeLeave>();
                                                        bool isLeave = false;
                                                        DateTime tempEndDate = (DateTime)NewEmpLeave.EndDate;
                                                        DateTime tempStartDate = (DateTime)NewEmpLeave.StartDate;
                                                        for (var item = tempStartDate; item.Date <= tempEndDate.Date; item = item.AddDays(1))
                                                        {
                                                            EmployeeLeave tempLeave = new EmployeeLeave();
                                                            EmployeeLeave newLeave = new EmployeeLeave();

                                                            if (employeeWorkingDays.Any(x => x.Contains(item.DayOfWeek.ToString().Substring(0, 3))))
                                                            {
                                                                tempLeave.Employee = NewEmpLeave.Employee;
                                                                tempLeave.StartDate = item;
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
                                                            newLeave.IdCompanyShift = tempStartLeave.IdCompanyShift;
                                                            newLeave.CompanyShift= tempStartLeave.CompanyShift;
                                                            tempEmployeeLeaveList.Add(newLeave);
                                                            isLeave = false;
                                                            tempLeaveList = new List<EmployeeLeave>();
                                                        }

                                                    }


                                                    else
                                                    {
                                                        EmployeeLeave tempLeave = new EmployeeLeave();
                                                        tempLeave.Employee = NewEmpLeave.Employee;
                                                        tempLeave.StartDate = NewEmpLeave.StartDate;
                                                        tempLeave.EndDate = NewEmpLeave.EndDate;
                                                        tempLeave.IdEmployee = NewEmpLeave.IdEmployee;
                                                        tempLeave.IdLeave = NewEmpLeave.IdLeave;
                                                        tempLeave.IdEmployeeLeave = NewEmpLeave.IdEmployeeLeave;
                                                        tempLeave.IsAllDayEvent = NewEmpLeave.IsAllDayEvent;
                                                        tempLeave.CompanyLeave = NewEmpLeave.CompanyLeave;
                                                        tempLeave.IdCompanyShift = NewEmpLeave.IdCompanyShift;
                                                        tempLeave.CompanyShift = NewEmpLeave.CompanyShift;
                                                        tempEmployeeLeaveList.Add(tempLeave);
                                                    }

                                                    foreach (EmployeeLeave item in tempEmployeeLeaveList)
                                                    {
                                                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                        modelAppointment = FillAppointment(item);
                                                        AppointmentItems.Add(modelAppointment);
                                                    }
                                                }
                                                else
                                                {
                                                    UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                    modelAppointment = FillAppointment(NewEmpLeave);
                                                    AppointmentItems.Add(modelAppointment);
                                                }
                                            }
                                            else
                                            {
                                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                                modelAppointment = FillAppointment(NewEmpLeave);
                                                AppointmentItems.Add(modelAppointment);
                                            }
                                        }
                                        else
                                        {
                                            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                            modelAppointment = FillAppointment(NewEmpLeave);
                                            AppointmentItems.Add(modelAppointment);
                                        }
                                    }
                                }
                            }
                        }
                        NewEmpLeave.StartTime = addNewLeaveViewModel.STime;
                        NewEmpLeave.EndTime = addNewLeaveViewModel.ETime;
                        NewEmpLeave.CompanyLeave = GetCompanyLeave(GeosApplication.Instance.EmployeeLeaveList.FirstOrDefault(x => x.IdLookupValue == (int)NewEmpLeave.CompanyLeave.IdCompanyLeave), NewEmpLeave);

                        if (DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)NewEmpLeave.StartDate) < 10)
                        {
                            NewEmpLeave.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)NewEmpLeave.StartDate);
                        }
                        else
                        {
                            NewEmpLeave.WeekNumber = DevExpress.XtraScheduler.Native.DateTimeHelper.GetWeekOfYear((DateTime)NewEmpLeave.StartDate);
                        }

                        NewEmpLeave.CompanyLeave.Company = NewEmpLeave.CompanyLeave.Company;
                        EmployeeLeaves.Add(NewEmpLeave);
                        SelectedLeaveRecord = NewEmpLeave;
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddNewLeave()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewLeave()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private UI.Helper.Appointment FillAppointment(EmployeeLeave NewEmpLeave)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAppointment ...", category: Category.Info, priority: Priority.Low);
              
                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                modelAppointment.Subject = string.Format("{0} {1}_{2}", NewEmpLeave.Employee.FirstName, NewEmpLeave.Employee.LastName, NewEmpLeave.CompanyLeave.Name);
                modelAppointment.StartDate = NewEmpLeave.StartDate;
                if (Convert.ToBoolean(NewEmpLeave.IsAllDayEvent) )
                {
                    TimeSpan NewTime = new TimeSpan(0, 0, 0);
                    if(NewEmpLeave.EndDate.Value.TimeOfDay==NewTime)
                        modelAppointment.EndDate = NewEmpLeave.EndDate.Value.AddDays(1);
                    else
                        modelAppointment.EndDate = NewEmpLeave.EndDate;
                }
                else
                {

                    modelAppointment.EndDate = NewEmpLeave.EndDate;
                }
                modelAppointment.Label = NewEmpLeave.IdLeave;
                modelAppointment.EmployeeCode = NewEmpLeave.Employee.EmployeeCode;
                modelAppointment.IdEmployeeLeave = NewEmpLeave.IdEmployeeLeave;
                modelAppointment.IdEmployee = NewEmpLeave.IdEmployee;
                modelAppointment.IsAllDayEvent = NewEmpLeave.IsAllDayEvent;
                return modelAppointment;

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillAppointment()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return null;
            }
        }
        /// <summary>
        /// [001][smazhar][09-02-2020][GESO2-2552][Leaves with a ” Night Shift”, the system does not represent  in the calendar view ]
        /// </summary>
        /// <param name="NewEmpLeave"></param>
        /// <returns></returns>
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
            //DateTime tempEndDate = (DateTime)NewEmpLeave.EndDate;
            //DateTime tempStartDate = (DateTime)NewEmpLeave.StartDate;
            //[001]
            var tempStartDate = NewEmpLeave.StartDate.Value.Date;
            var tempEndDate = NewEmpLeave.EndDate.Value.Date;
            for (var item =tempStartDate; item.Date <= tempEndDate.Date; item = item.AddDays(1))
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
        /// [001][SP-65][skale][13-06-2019][GEOS2-1556]Grid data reflection problems
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerEditValueChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

                //if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                //{
                //    return;
                //}
                // [001] added
                var values = (object[])obj;
                SchedulerControlEx schedulerControlEx = (SchedulerControlEx)values[0];
                int year = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                DateTime newDt = new DateTime(year, SelectedStartDate.Month, 1);
                schedulerControlEx.Month = newDt;
                DateTime start = newDt;
                DateTime end = start.AddDays(1);
                schedulerControlEx.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);
                //SelectedItem = null;

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

                if (!DXSplashScreen.IsActive)
                {
                    // DXSplashScreen.Show<SplashScreenView>(); 
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
                    FillContactListByPlant();
                }
                else
                {
                    Departments = new ObservableCollection<Department>();
                    CompanyHolidays.Clear();
                    EmployeeLeaves.Clear();
                    AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PlantOwnerEditValueChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][SP-65][skale][13-06-2019][GEOS2-1556]Grid data reflection problems
        /// </summary>
        /// <param name="obj"></param>
        private void SelectedYearChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
                var values = (object[])obj;
                SchedulerControlEx scheduler = (SchedulerControlEx)values[0];
                int year = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                DateTime newDt = new DateTime(year, SelectedStartDate.Month, 1);

                scheduler.Month = newDt;
                DateTime start = newDt;
                DateTime end = start.AddDays(1);
                scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);

                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;

                //if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                //{
                //    return;
                //}
                // [001] added 
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

                if (!DXSplashScreen.IsActive)
                {
                    // DXSplashScreen.Show<SplashScreenView>(); 
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
                    FillContactListByPlant();
                }
                else
                {
                    Departments = new ObservableCollection<Department>();
                    CompanyHolidays.Clear();
                    EmployeeLeaves.Clear();
                    AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedYearChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// [002] [cpatil][2020-01-22][GEOS2-2008] We can not see some the Employees in LEAVES and ATTENDANCE with Period = 2019, but the same employees in 2020 we can see.
        /// [003] [cpatil][2020-03-17][GEOS2-1876] Draft users in Leaves.
        /// </summary>
        private void FillContactListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContactListByPlant ...", category: Category.Info, priority: Priority.Low);
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                    // [002] Changed service method GetAllEmployeesByDepartmentByIdCompany_V2032 to GetAllEmployeesByDepartmentByIdCompany_V2039
                    // [003] Changed service method GetAllEmployeesByDepartmentByIdCompany_V2039 to GetAllEmployeesByDepartmentByIdCompany_V2041
                    Departments = new ObservableCollection<Department>(HrmService.GetAllEmployeesByDepartmentByIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds));

                    // [002] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2032 to GetEmployeeLeavesBySelectedIdCompany_V2039
                    // [003] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2039 to GetEmployeeLeavesBySelectedIdCompany_V2041
                    // [004] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2041 to GetEmployeeLeavesBySelectedIdCompany_V2045
                    EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2045(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                   
                    EmployeeLeaveOrigional = EmployeeLeaves;

                    //[001] code comment
                    //  CompanyLeaves = new ObservableCollection<CompanyLeave>(HrmService.GetSelectedIdCompanyLeaves(plantOwnersIds));
                    // AppointmentItems = new ObservableCollection<Appointment>();

                    AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                    LabelItems = new ObservableCollection<LabelHelper>();
                    HolidayList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(28).AsEnumerable());

                    ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();
                    //  ObservableCollection<LabelHelper> labelItem = new ObservableCollection<LabelHelper>();


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
                        //modelAppointment.AllDay = true;
                        appointment.Add(modelAppointment);

                    }

                    foreach (var EmployeeLeave in EmployeeLeaves)
                    {
                       
                        
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = string.Format("{0} {1}_{2}", EmployeeLeave.Employee.FirstName, EmployeeLeave.Employee.LastName, EmployeeLeave.CompanyLeave.Name);
                        modelAppointment.StartDate = Convert.ToDateTime(EmployeeLeave.StartDate.ToString());
                        if (Convert.ToBoolean(EmployeeLeave.IsAllDayEvent))
                        {
                            TimeSpan NewTime = new TimeSpan(0, 0, 0);
                            if (EmployeeLeave.EndDate.Value.TimeOfDay == NewTime)
                                modelAppointment.EndDate = EmployeeLeave.EndDate.Value.AddDays(1);
                            else
                                modelAppointment.EndDate = EmployeeLeave.EndDate;
                        }
                        else
                        {

                            modelAppointment.EndDate = EmployeeLeave.EndDate;
                        }
                        if (Convert.ToBoolean(EmployeeLeave.IsAllDayEvent))
                        {
                            modelAppointment.EndDate = EmployeeLeave.EndDate.Value.AddDays(1);
                        }
                        else
                        {
                            modelAppointment.EndDate = EmployeeLeave.EndDate;
                        }

                        modelAppointment.Label = EmployeeLeave.IdLeave;
                        modelAppointment.EmployeeCode = EmployeeLeave.Employee.EmployeeCode;
                        modelAppointment.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                        modelAppointment.IdEmployee = EmployeeLeave.IdEmployee;
                        modelAppointment.IsAllDayEvent = EmployeeLeave.IsAllDayEvent;
                        appointment.Add(modelAppointment);
                    }
                    AppointmentItems = appointment;

                    //[001] code comment and Add Employee leave type as per lookupvalue
                    //foreach (var CompanyLeave in CompanyLeaves)
                    //{
                    //    LabelHelper label = new LabelHelper();
                    //    label.Id = Convert.ToInt32(CompanyLeave.IdCompanyLeave);
                    //    label.Color = new BrushConverter().ConvertFromString(CompanyLeave.HtmlColor.ToString()) as SolidColorBrush;
                    //    label.Caption = CompanyLeave.Name;
                    //    LabelItems.Add(label);
                    //}


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
                    // LabelItems = labelItem;

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
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillContactListByPlant()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void VisibleIntervalsChanged(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIntervalsChanged()...", category: Category.Info, priority: Priority.Low);

                SchedulerControlEx scheduler = (SchedulerControlEx)obj;

                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;

                if (scheduler.Month != null)
                {
                    if (scheduler.ActiveViewIndex == 0)
                    {
                        scheduler.DisplayName = String.Format("{0:MMMM yyyy}", scheduler.VisibleIntervals[0].End);
                        SelectedEndDate = SelectedEndDate.AddDays(-1);
                    }
                    else if (scheduler.ActiveViewIndex == 1)
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
                    else
                    {
                        if (scheduler.Start.Day < 10)
                        {
                            scheduler.DisplayName = String.Format("0{0} {1:MMMM} {2:yyyy}", scheduler.Start.Day, scheduler.Start, scheduler.Start);
                        }
                        else
                        {
                            scheduler.DisplayName = String.Format("{0} {1:MMMM} {2:yyyy}", scheduler.Start.Day, scheduler.Start, scheduler.Start);

                        }
                    }

                }
                GeosApplication.Instance.Logger.Log("Method VisibleIntervalsChanged()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method VisibleIntervalsChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        /// <summary>
        /// Method for Show Leaves Scheduler View. 
        /// </summary>
        private void ShowLeavesSchedulerView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowLeavesSchedulerView ...", category: Category.Info, priority: Priority.Low);

                IsSchedulerViewVisible = Visibility.Visible;
                IsGridViewVisible = Visibility.Hidden;

                GeosApplication.Instance.Logger.Log("Method ShowLeavesSchedulerView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowLeavesSchedulerView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Show Leaves Grid View. 
        /// </summary>
        private void ShowLeavesGridView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowLeavesGridView ...", category: Category.Info, priority: Priority.Low);
                SchedulerControlEx scheduler = (SchedulerControlEx)obj;

                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                scheduler.Month = startDate;
                scheduler.ActiveViewIndex = 0;
                scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(DateTime.Today, DateTime.Today.AddDays(1));
                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;
                SelectedEndDate = SelectedEndDate.AddDays(-1);


                IsGridViewVisible = Visibility.Visible;
                IsSchedulerViewVisible = Visibility.Hidden;

                GeosApplication.Instance.Logger.Log("Method ShowLeavesGridView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowLeavesGridView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Print Employee Leave Details List. 
        /// Done by Amit in Sprint 41 For Task [HRM-M041-19] New grid view in Leaves
        /// </summary>
        private void PrintEmployeeLeaveList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintEmployeeLeaveList()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["EmployeeLeaveReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["EmployeeLeaveReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintEmployeeLeaveList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintEmployeeLeaveList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
		/// Calculation of TotalTimeInHours
        private Double GetTotalTime(int dayOfWeek, CompanyShift selectedEmployeeShift)
        {
            try
            {
                TimeSpan Span;
                Double TotalTime = new Double();
                switch (dayOfWeek)
                {
                    case 0:
                        if (selectedEmployeeShift.SunStartTime > selectedEmployeeShift.SunEndTime)
                        {
                            Span = new TimeSpan(0, (24 - selectedEmployeeShift.SunStartTime.Hours) + selectedEmployeeShift.SunEndTime.Hours, 0, 0);
                            Span = Span.Subtract(new TimeSpan(0, 0, Math.Abs(selectedEmployeeShift.SunStartTime.Minutes - selectedEmployeeShift.SunEndTime.Minutes), 0));
                            TotalTime = Convert.ToDouble((Span - selectedEmployeeShift.SunBreakTime).TotalHours);
                        }
                        else
                            TotalTime = Convert.ToDouble((selectedEmployeeShift.SunEndTime - selectedEmployeeShift.SunStartTime - selectedEmployeeShift.SunBreakTime).TotalHours);
                        return TotalTime;

                    case 1:
                        if (selectedEmployeeShift.MonStartTime > selectedEmployeeShift.MonEndTime)
                        {
                            Span = new TimeSpan(0, (24 - selectedEmployeeShift.MonStartTime.Hours) + selectedEmployeeShift.MonEndTime.Hours, 0, 0);
                            Span = Span.Subtract(new TimeSpan(0, 0, Math.Abs(selectedEmployeeShift.MonStartTime.Minutes - selectedEmployeeShift.MonEndTime.Minutes), 0));
                            TotalTime = Convert.ToDouble((Span - selectedEmployeeShift.MonBreakTime).TotalHours);
                        }
                        else
                            TotalTime = Convert.ToDouble((selectedEmployeeShift.MonEndTime - selectedEmployeeShift.MonStartTime - selectedEmployeeShift.MonBreakTime).TotalHours);
                        return TotalTime;

                    case 2:
                        if (selectedEmployeeShift.TueStartTime > selectedEmployeeShift.TueEndTime)
                        {
                            Span = new TimeSpan(0, (24 - selectedEmployeeShift.TueStartTime.Hours) + selectedEmployeeShift.TueEndTime.Hours, 0, 0);
                            Span = Span.Subtract(new TimeSpan(0, 0, Math.Abs(selectedEmployeeShift.TueStartTime.Minutes - selectedEmployeeShift.TueEndTime.Minutes), 0));
                            TotalTime = Convert.ToDouble((Span- selectedEmployeeShift.TueBreakTime).TotalHours);
                        }
                        else
                            TotalTime = Convert.ToDouble((selectedEmployeeShift.TueEndTime - selectedEmployeeShift.TueStartTime - selectedEmployeeShift.TueBreakTime).TotalHours);
                        return TotalTime;

                    case 3:
                        if (selectedEmployeeShift.WedStartTime > selectedEmployeeShift.WedEndTime)
                        {
                            Span = new TimeSpan(0, (24 - selectedEmployeeShift.WedStartTime.Hours) + selectedEmployeeShift.WedEndTime.Hours, 0, 0);
                            Span = Span.Subtract(new TimeSpan(0, 0, Math.Abs(selectedEmployeeShift.WedStartTime.Minutes - selectedEmployeeShift.WedEndTime.Minutes), 0));
                            TotalTime = Convert.ToDouble((Span - selectedEmployeeShift.WedBreakTime).TotalHours);
                        }
                        else
                            TotalTime = Convert.ToDouble((selectedEmployeeShift.WedEndTime - selectedEmployeeShift.WedStartTime - selectedEmployeeShift.WedBreakTime).TotalHours);
                        return TotalTime;

                    case 4:
                        if (selectedEmployeeShift.ThuStartTime > selectedEmployeeShift.ThuEndTime)
                        {
                            Span = new TimeSpan(0, (24 - selectedEmployeeShift.ThuStartTime.Hours) + selectedEmployeeShift.ThuEndTime.Hours, 0, 0);
                            Span = Span.Subtract(new TimeSpan(0, 0, Math.Abs(selectedEmployeeShift.ThuStartTime.Minutes - selectedEmployeeShift.ThuEndTime.Minutes), 0));
                            TotalTime = Convert.ToDouble((Span - selectedEmployeeShift.ThuBreakTime).TotalHours);
                        }
                        else
                            TotalTime = Convert.ToDouble((selectedEmployeeShift.ThuEndTime - selectedEmployeeShift.ThuStartTime - selectedEmployeeShift.ThuBreakTime).TotalHours);
                        return TotalTime;

                    case 5:
                        if (selectedEmployeeShift.FriStartTime > selectedEmployeeShift.FriEndTime)
                        { 
                            Span = new TimeSpan(0, (24 - selectedEmployeeShift.FriStartTime.Hours) + selectedEmployeeShift.FriEndTime.Hours, 0, 0);
                            Span = Span.Subtract(new TimeSpan(0, 0, Math.Abs(selectedEmployeeShift.FriStartTime.Minutes - selectedEmployeeShift.FriEndTime.Minutes), 0));
                            TotalTime = Convert.ToDouble((Span - selectedEmployeeShift.FriBreakTime).TotalHours);
                        }
                        else
                            TotalTime = Convert.ToDouble((selectedEmployeeShift.FriEndTime.Subtract(selectedEmployeeShift.FriStartTime).Subtract(selectedEmployeeShift.FriBreakTime)).TotalHours);
                        return TotalTime;

                    case 6:
                        if (selectedEmployeeShift.SatStartTime > selectedEmployeeShift.SatEndTime)
                        {
                            Span = new TimeSpan(0, (24 - selectedEmployeeShift.SatStartTime.Hours) + selectedEmployeeShift.SatEndTime.Hours, 0, 0);
                            Span = Span.Subtract(new TimeSpan(0, 0, Math.Abs(selectedEmployeeShift.SatStartTime.Minutes - selectedEmployeeShift.SatEndTime.Minutes), 0));
                            TotalTime = Convert.ToDouble((Span - selectedEmployeeShift.SatBreakTime).TotalHours);
                        }
                        else
                            TotalTime = Convert.ToDouble((selectedEmployeeShift.SatEndTime - selectedEmployeeShift.SatStartTime - selectedEmployeeShift.SatBreakTime).TotalHours);
                        return TotalTime;

                    default:
                        return TotalTime;
                }
            }
            catch (Exception ex)
            {
                return new Double();
            }
        }


        /// [0001][07/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
        /// Getting Start and End Time
        private TimeSpan GetStartEndTime(int dayOfWeek, CompanyShift selectedEmployeeShift, bool EndTime)
        {
            try
            {
                TimeSpan Time;
                switch (dayOfWeek)
                {                    
                    case 0:
                        Time = selectedEmployeeShift.SunStartTime;
                        if(EndTime)
                            Time = selectedEmployeeShift.SunEndTime;
                        return Time;

                    case 1:                        
                        Time = selectedEmployeeShift.MonStartTime;
                        if (EndTime)
                            Time = selectedEmployeeShift.MonEndTime;
                        return Time;

                    case 2:
                        Time = selectedEmployeeShift.TueStartTime;
                        if (EndTime)
                            Time = selectedEmployeeShift.TueEndTime;
                        return Time;

                    case 3:
                        Time = selectedEmployeeShift.WedStartTime;
                        if (EndTime)
                            Time = selectedEmployeeShift.WedEndTime;
                        return Time;

                    case 4:
                        Time = selectedEmployeeShift.ThuStartTime;
                        if (EndTime)
                            Time = selectedEmployeeShift.ThuEndTime;
                        return Time;

                    case 5:
                        Time = selectedEmployeeShift.FriStartTime;
                        if (EndTime)
                            Time = selectedEmployeeShift.FriEndTime;
                        return Time;

                    case 6:
                        Time = selectedEmployeeShift.SatStartTime;
                        if (EndTime)
                            Time = selectedEmployeeShift.SatEndTime;
                        return Time;

                    default:
                        Time = selectedEmployeeShift.MonStartTime;
                        if (EndTime)
                            Time = selectedEmployeeShift.MonEndTime;
                        return Time;
                        
                }

            }
            catch (Exception ex)
            {
                return new TimeSpan();
            }
        }
        /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
		/// Calculation of Shift Break Time
        private TimeSpan GetShiftBreakTime(int dayOfWeek, CompanyShift selectedEmployeeShift)
        {           
            try
            {                
                switch (dayOfWeek)
                {
                    case 0:
                        TimeSpan SunBreakTime = selectedEmployeeShift.SunBreakTime;
                        return SunBreakTime;

                    case 1:
                        TimeSpan MonBreakTime = selectedEmployeeShift.MonBreakTime;
                        return MonBreakTime;

                    case 2:
                        TimeSpan TueBreakTime = selectedEmployeeShift.TueBreakTime;
                        return TueBreakTime;

                    case 3:
                        TimeSpan WedBreakTime = selectedEmployeeShift.WedBreakTime;
                        return WedBreakTime;

                    case 4:
                        TimeSpan ThuBreakTime = selectedEmployeeShift.ThuBreakTime;
                        return ThuBreakTime;

                    case 5:
                        TimeSpan FriBreakTime = selectedEmployeeShift.FriBreakTime;
                        return FriBreakTime;

                    case 6:
                        TimeSpan SatBreakTime = selectedEmployeeShift.SatBreakTime;
                        return SatBreakTime;

                    default:
                        return selectedEmployeeShift.BreakTime;
                }

            }
            catch (Exception ex)
            {
                return new TimeSpan();
            }
        }

        /// <summary>
        /// Method for Export Employee Leave Details in Excel Sheet. 
        /// Done by Amit in Sprint 41 For Task [HRM-M041-19] New grid view in Leaves
        /// </summary>
        private void ExportEmployeeLeaveList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportEmployeeLeaveList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Employee Leaves List";
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
                    
                    ResultFileName = (saveFile.FileName);
                    TableView leaveTableView = ((TableView)obj);

			        /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
					///Code for Adding Excel Report data
                    EmployeeLeavesDetails = new ObservableCollection<EmployeeLeave>();
                    foreach (EmployeeLeave EmpLeave in leaveTableView.Grid.VisibleItems)
                    {
                        if (EmpLeave.IsAllDayEvent == 1)
                        {
                            DateTime EndDate = Convert.ToDateTime(EmpLeave.EndDate);
                            DateTime StartDate = Convert.ToDateTime(EmpLeave.StartDate);
                            EmpLeave.StartLeaveTimeForExcel = GetStartEndTime((int)StartDate.Date.DayOfWeek, EmpLeave.CompanyShift, false);
                            EmpLeave.EndLeaveTimeForExcel = GetStartEndTime((int)StartDate.Date.DayOfWeek, EmpLeave.CompanyShift, true);
                            EmpLeave.CompanyShift.BreakTime = GetShiftBreakTime((int)StartDate.Date.DayOfWeek, EmpLeave.CompanyShift);
                            EmpLeave.CompanyShift.TotalTimeInHours = (float)(GetTotalTime((int)StartDate.Date.DayOfWeek, EmpLeave.CompanyShift));
                            DateTime StDate = Convert.ToDateTime(EmpLeave.StartDate).AddDays(-1);
                            TimeSpan Diff = EndDate.Date.Subtract(StartDate.Date);
                            if (Diff.TotalDays >= 1)
                            {
                                double loopVal = 0;
                                if (EmpLeave.StartLeaveTimeForExcel > EmpLeave.EndLeaveTimeForExcel)
                                    loopVal = Diff.TotalDays - 1;
                                else
                                    loopVal = Diff.TotalDays;

                                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                                System.Globalization.Calendar cal = dfi.Calendar;
                                Int32 Week = 0;
                                for (double i = 0; i <= loopVal; i++)
                                {
                                    EmployeeLeave NewEmpLeave = new EmployeeLeave()
                                    {
                                        Employee = EmpLeave.Employee,
                                        CompanyLeave = EmpLeave.CompanyLeave,
                                        EndLeaveTimeForExcel = EmpLeave.EndLeaveTimeForExcel,
                                        StartLeaveTimeForExcel = EmpLeave.StartLeaveTimeForExcel,
                                        Remark = EmpLeave.Remark,
                                        FileName = EmpLeave.FileName,
                                        StartDate = EmpLeave.StartDate,
                                        EndDate = EmpLeave.EndDate,
                                        //WeekNumber = Week,
                                        CompanyShift = EmpLeave.CompanyShift,
                                    };
                                    NewEmpLeave.StartDate = Convert.ToDateTime(StDate).AddDays(1);
                                    NewEmpLeave.WeekNumber = cal.GetWeekOfYear(Convert.ToDateTime(NewEmpLeave.StartDate), dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                                    
                                    if (NewEmpLeave.StartLeaveTimeForExcel > NewEmpLeave.EndLeaveTimeForExcel)
                                    {
                                        NewEmpLeave.EndDate = Convert.ToDateTime(StDate).AddDays(2);
                                    }
                                    else
                                        NewEmpLeave.EndDate = NewEmpLeave.StartDate;
                                    EmployeeLeavesDetails.Add(NewEmpLeave);                                    
                                    StDate = Convert.ToDateTime(NewEmpLeave.StartDate);                                    
                                }
                            }
                            else
                            {
                                //EmpLeave.StartDate = EmpLeave.StartDate;
                                //EmpLeave.EndDate = EmpLeave.EndDate;
                                
                                EmployeeLeavesDetails.Add(EmpLeave);
                            }
                        }
                        else
                        {
                            //EmpLeave.StartDate = EmpLeave.StartDate;
                            //EmpLeave.EndDate = EmpLeave.EndDate;
                            DateTime EndDate = Convert.ToDateTime(EmpLeave.EndDate);
                            DateTime StartDate = Convert.ToDateTime(EmpLeave.StartDate);
                            EmpLeave.StartLeaveTimeForExcel = GetStartEndTime((int)StartDate.Date.DayOfWeek, EmpLeave.CompanyShift, false);
                            EmpLeave.EndLeaveTimeForExcel = GetStartEndTime((int)StartDate.Date.DayOfWeek, EmpLeave.CompanyShift, true);

                            EmpLeave.CompanyShift.BreakTime = new TimeSpan(0);
                            
                            EmpLeave.CompanyShift.TotalTimeInHours = (float)(GetTotalTime((int)StartDate.Date.DayOfWeek, EmpLeave.CompanyShift));
                            EmployeeLeavesDetails.Add(EmpLeave);
                        }
                    }
                    ShowColumn = true;
                    HideColumn = false;
                    leaveTableView.Grid.ItemsSource = EmployeeLeavesDetails.ToList();

                    leaveTableView.ShowTotalSummary = false;
                    var test = leaveTableView.Grid.FilterString;
                    leaveTableView.ShowFixedTotalSummary = false;
                    leaveTableView.Grid.FilterString = null;
                    leaveTableView.ExportToXlsx(ResultFileName, new DevExpress.XtraPrinting.XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG });

                    IsBusy = false;
                    leaveTableView.ShowFixedTotalSummary = true;
                    leaveTableView.Grid.FilterString = test;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
                    ShowColumn = false;
                    HideColumn = true;
                    EmployeeLeaves = EmployeeLeaveOrigional;
                    leaveTableView.Grid.ItemsSource = EmployeeLeaveOrigional;
                    GeosApplication.Instance.Logger.Log("Method ExportEmployeeLeaveList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportEmployeeLeaveList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// Function Created to fill EmployeeLeave from Lookup values
        /// </summary>
        public void FillEmployeeLeaveType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveType()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.EmployeeLeaveList == null)
                {
                    GeosApplication.Instance.EmployeeLeaveList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(32));
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

        /// <summary>
        /// Method to Open Employee Leave Document
        /// </summary>
        /// <param name="obj"></param>
        private void OpenEmployeeLeaveDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeLeaveDocument()...", category: Category.Info, priority: Priority.Low);
                EmployeeLeave employeeLeave = null;

                if (obj is TableView)
                {
                    TableView detailView = (TableView)obj;
                    employeeLeave = (EmployeeLeave)detailView.FocusedRow;
                    SelectedLeaveRecord = employeeLeave;
                }
                else if (obj is Employee)
                {
                    employeeLeave = (EmployeeLeave)obj;
                    SelectedLeaveRecord = employeeLeave;
                }

                byte[] EmployeeLeaveAttachmentBytes = HrmService.GetEmployeeLeaveAttachment(SelectedLeaveRecord);
                EmployeeDocumentView employeeDocumentView = new EmployeeDocumentView();
                EmployeeDocumentViewModel employeeDocumentViewModel = new EmployeeDocumentViewModel();
                if (EmployeeLeaveAttachmentBytes != null)
                {
                    employeeDocumentViewModel.OpenPdfFromBytes(EmployeeLeaveAttachmentBytes, SelectedLeaveRecord.FileName);

                    employeeDocumentView.DataContext = employeeDocumentViewModel;
                    employeeDocumentView.ShowDialog();
                }
                else
                {
                    CustomMessageBox.Show(string.Format("Could not find file {0}", SelectedLeaveRecord.FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeLeaveDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeLeaveDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeLeaveDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeEducationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Filling loockup values. 
        /// </summary>
        /// <param name="obj"></param>
        public CompanyLeave GetCompanyLeave(LookupValue objLookupValue, EmployeeLeave EmployeeLeaveCompany)
        {


            CompanyLeave companyLeave = new CompanyLeave
            {
                IdCompanyLeave = (ulong)objLookupValue.IdLookupValue,
                Name = objLookupValue.Value,
                HtmlColor = objLookupValue.HtmlColor
            };

            if (string.IsNullOrEmpty(companyLeave.HtmlColor))
            {
                if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "WhiteAndBlue")
                    companyLeave.HtmlColor = "#FFFFFFFF";
                if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "BlackAndBlue")
                    companyLeave.HtmlColor = "#FF000000";
            }
            companyLeave.Company = EmployeeLeaveCompany.CompanyLeave.Company;
            return companyLeave;
        }

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


        /// <summary>
        /// [000][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "Employee.EmployeeCompanyAlias" && e.Column.FieldName != "Employee.LstEmployeeDepartments" && e.Column.FieldName != "Employee.EmployeeJobTitles" && e.Column.FieldName != "Employee.EmpJobCodes")
            {
                return;
            }
            try
            {
                List<object> filterItems = new List<object>();
                if (e.Column.FieldName == "Employee.EmployeeCompanyAlias")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Employee.EmployeeCompanyAlias is null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Employee.EmployeeCompanyAlias is not null")
                    });

                    foreach (var dataObject in EmployeeLeaves)
                    {
                        if (dataObject.Employee.EmployeeCompanyAlias == null)
                        {
                            continue;
                        }
                        else if (string.IsNullOrEmpty(dataObject.Employee.EmployeeCompanyAlias))
                        {
                            continue;
                        }
                        else if (!string.IsNullOrEmpty(dataObject.Employee.EmployeeCompanyAlias))
                        {
                            string[] employeeCompanyAliasList = dataObject.Employee.EmployeeCompanyAlias.Split(Environment.NewLine.ToCharArray());

                            foreach (string companyAlias in employeeCompanyAliasList)
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == companyAlias))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = companyAlias;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Employee.EmployeeCompanyAlias Like '%{0}%'", companyAlias));
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

                    foreach (var dataObject in EmployeeLeaves)
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

                    foreach (var dataObject in EmployeeLeaves)
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
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == EmployeeLeaves.Where(y => y.Employee.EmployeeJobTitles == dataObject.Employee.EmployeeJobTitles).Select(slt => slt.Employee.EmployeeJobTitles).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = EmployeeLeaves.Where(y => y.Employee.EmployeeJobTitles == dataObject.Employee.EmployeeJobTitles).Select(slt => slt.Employee.EmployeeJobTitles).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Employee.EmployeeJobTitles Like '{0}'", EmployeeLeaves.Where(y => y.Employee.EmployeeJobTitles == dataObject.Employee.EmployeeJobTitles).Select(slt => slt.Employee.EmployeeJobTitles).FirstOrDefault().Trim()));
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

                    foreach (var dataObject in EmployeeLeaves)
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
                        EditValue = CriteriaOperator.Parse("Employee.EmpJobCodes = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Employee.EmpJobCodes <> ''")
                    });

                    foreach (var dataObject in EmployeeLeaves)
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
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == EmployeeLeaves.Where(y => y.Employee.EmpJobCodes == dataObject.Employee.EmpJobCodes).Select(slt => slt.Employee.EmpJobCodes).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = EmployeeLeaves.Where(y => y.Employee.EmpJobCodes == dataObject.Employee.EmpJobCodes).Select(slt => slt.Employee.EmpJobCodes).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Employee.EmpJobCodes Like '%{0}%'", EmployeeLeaves.Where(y => y.Employee.EmpJobCodes == dataObject.Employee.EmpJobCodes).Select(slt => slt.Employee.EmpJobCodes).FirstOrDefault().Trim()));
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
        /// <summary>
        /// [000][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        /// </summary>
        /// <param name="e"></param>
        private void CustomRowFilter(RowFilterEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomRowFilter ...", category: Category.Info, priority: Priority.Low);

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
                        if (EmployeeLeaves[e.ListSourceRowIndex].Employee.LstEmployeeDepartments != null)
                        {
                            e.Visible = vals.Intersect(this.EmployeeLeaves[e.ListSourceRowIndex].Employee.LstEmployeeDepartments.Select(x => x.DepartmentName)).Count() > 0;
                            e.Visible |= this.EmployeeLeaves[e.ListSourceRowIndex].Employee.LstEmployeeDepartments.Count == 0 && includeEmpty;
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
                        if (EmployeeLeaves[e.ListSourceRowIndex].Employee.LstEmployeeDepartments != null)
                        {
                            e.Visible = vals.Intersect(EmployeeLeaves[e.ListSourceRowIndex].Employee.LstEmployeeDepartments.Select(x => x.DepartmentName)).Count() > 0;
                            e.Visible |= EmployeeLeaves[e.ListSourceRowIndex].Employee.LstEmployeeDepartments.Count == 0 && includeEmpty;
                        }
                        else
                            e.Visible |= true;
                    }
                    if (criteria.Contains("<>"))
                    {
                        if (EmployeeLeaves[e.ListSourceRowIndex].Employee.LstEmployeeDepartments != null)
                            e.Visible |= EmployeeLeaves[e.ListSourceRowIndex].Employee.LstEmployeeDepartments.Count != 0;
                        else
                            e.Visible = false;
                    }

                    e.Handled = true;
                }

                GeosApplication.Instance.Logger.Log("Method CustomRowFilter() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomRowFilter() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        #endregion
        private void deleteEmployeeLeave(Object e)
        {

            AppointmentItem appointment = ((AppointmentItemCancelEventArgs)e).Appointment;
            string LeaveType = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == (ulong)appointment.CustomFields["IdEmployeeLeave"]).CompanyLeave.Name;
            EmployeeLeave employeeLeave = new EmployeeLeave();
            employeeLeave.EmployeeChangelogs = new List<EmployeeChangelog>();
            employeeLeave.IdEmployee = (int)appointment.CustomFields["IdEmployee"];
            employeeLeave.IdEmployeeLeave = (ulong)appointment.CustomFields["IdEmployeeLeave"];
            EmployeeLeave leave = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == Convert.ToUInt64(appointment.CustomFields["IdEmployeeLeave"]));
            employeeLeave.EmployeeChangelogs.Add(new EmployeeChangelog()
            {
                IdEmployee = (int)appointment.CustomFields["IdEmployee"],
                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveDeleteChangeLog").ToString(), LeaveType, leave.StartDate.Value.ToShortDateString(), leave.EndDate.Value.ToShortDateString())
            });

            bool result = HrmService.DeleteEmployeeLeave((appointment.CustomFields["EmployeeCode"]).ToString(), Convert.ToUInt32(appointment.CustomFields["IdEmployeeLeave"].ToString()), employeeLeave);
            if (leave != null)
            {
                var appointmentItemsList = AppointmentItems.Where(x => x.IdEmployeeLeave == leave.IdEmployeeLeave).ToList();
                foreach (var item in appointmentItemsList)
                {
                    AppointmentItems.Remove(item);
                }
            }
            EmployeeLeaves.Remove(EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == Convert.ToUInt64(appointment.CustomFields["IdEmployeeLeave"])));

            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteEmployeeLeaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

        }


    }
}
