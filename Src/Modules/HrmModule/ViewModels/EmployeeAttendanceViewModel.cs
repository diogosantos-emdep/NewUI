using DevExpress.CodeParser;
using DevExpress.Data;
using DevExpress.Data.Extensions;
using DevExpress.Data.Filtering;
using DevExpress.Export;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Accordion;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Map.Native;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Scheduler.Drawing;
using DevExpress.Xpf.Scheduling;
using DevExpress.Xpf.Scheduling.Visual;
using DevExpress.Xpf.Scheduling.VisualData;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit.Model;
using DevExpress.XtraScheduler;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm;
using Emdep.Geos.Modules.Hrm.CommonClass;
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
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    class EmployeeAttendanceViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        //[M051-08][Year selection is not saved after change section][adadibathina]
        //[000][SP-65][skale][12-06-2019][GEOS2-1556]Grid data reflection problems
        //[001][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        //[002][rdixit][GEOS2-4049][27.01.2023] Add the shift time to the Attendance calendar 
        #endregion

        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        #endregion

        #region Declaration 
        private bool isAttendanceColumnChooserVisible;
        public string HRM_ConfigurationAttendanceGrid_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "HRM_ConfigurationAttendanceGrid_SettingFilePath.Xml";
        private DateTime selectedDate;
        private static bool _hasShownHolidayAppointments = false;
        private static int _year = DateTime.Now.Year;
        ComboBoxEdit combo;
        public bool isSplitButtonEnable = false;
        public bool IsFilterLoaded = false;
        private CustomObservableCollection<Department> departmentsForFilter;//[rdixit][GEOS2-4054][06/01/2023]
        private CustomObservableCollection<Department> department;
        private CustomObservableCollection<Employee> employees;
        private CustomObservableCollection<EmployeeAttendance> employeeAttendanceList;
        #region Rupali Sarode - GEOS2-3751
        private CustomObservableCollection<Employee> employeesForFilter;
        private CustomObservableCollection<EmployeeAttendance> employeeAttendanceListForFilter;
        ObservableCollection<string> filterAttendanceList;
        private object selectedFilterAttendanceList;
        public virtual object SelectedFilterAttendanceList
        {
            get
            {
                return selectedFilterAttendanceList;
            }
            set
            {
                selectedFilterAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFilterAttendanceList"));
            }
        }
        #endregion

        private EmployeeAttendance selectedAttendanceRecord;
        //[M051-08]
        //private long selectedPeriod;
        private CustomObservableCollection<UI.Helper.Appointment> appointmentItems;
        private CustomObservableCollection<CompanyHoliday> companyHolidays;
        private CustomObservableCollection<LookupValue> holidayList;
        private CustomObservableCollection<LabelHelper> labelItems;
        private ObservableCollection<CompanyWork> companyWorksList;
        private Visibility isSchedulerViewVisible;
        private Visibility isGridViewVisible;
        private string timeEditMask;
        private bool isBusy;
        private bool IsFromRefresh;
        IWorkbenchStartUp objWorkbenchStartUp;
        //public bool IsVisible;
        //Sprint-42
        private DateTime selectedStartDate;
        private DateTime selectedEndDate;
        private bool isSet;

        private List<GeosAppSetting> geosAppSettingList;
        private SolidColorBrush oKColor;
        private SolidColorBrush notOKColor;
        private byte viewType;

        private CustomObservableCollection<EmployeeLeave> employeeLeaves;
        private List<EmployeeLeave> selectedEmployeeLeavesAsPerDate;
        private Visibility isAccordionControlVisible;
        string myFilterString;//[SP-65-000] added

        private CustomObservableCollection<StatusHelper> statusItems;
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

        #region Rupali Sarode - GEOS2-3751

        private DateTime fromTimeForShift;
        private DateTime toTimeForShift;
        private TimeSpan breakTimeForShift;
        #endregion

        private ObservableCollection<EmployeeShift> employeeShiftList;
        private DateTime? startTime;
        private DateTime? endTime;
        private int selectedIndexForCompanyShift;
        int _employeeid;
        DateTime? _fromDate;
        DateTime? _toDate;
        DateTime? fromDate;
        DateTime? toDate;
        GridControl GridControl1;
        private object _selectedItem;
        private List<GeosAppSetting> geosAppSettingList1;
        #region Rupali Sarode - GEOS2-3751
        private bool attendanceFilterAttendanceIsEnabled = true;
        public bool AttendanceFilterAttendanceIsEnabled
        {
            get { return attendanceFilterAttendanceIsEnabled; }
            set
            {
                attendanceFilterAttendanceIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendanceFilterAttendanceIsEnabled"));
            }
        }
        private Visibility attendanceFilterAttendanceVisible;
        public Visibility AttendanceFilterAttendanceVisible
        {
            get
            {
                return attendanceFilterAttendanceVisible;
            }

            set
            {
                attendanceFilterAttendanceVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendanceFilterAttendanceVisible"));
            }
        }
        private Thickness _margin = new Thickness(5, 5, 5, 5);

        public Thickness AttendanceMargin
        {
            get { return _margin; }
            set
            {
                _margin = value;
                //Notify the binding that the value has changed.
                OnPropertyChanged(new PropertyChangedEventArgs("AttendanceMargin"));
            }
        }
        int attendanceFilterAttendanceWidth = 50;
        public int AttendanceFilterAttendanceWidth
        {
            get
            {
                return attendanceFilterAttendanceWidth;
            }
            set
            {
                attendanceFilterAttendanceWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendanceFilterAttendanceWidth"));
            }
        }
        #endregion

        private EmployeeAnnualAdditionalLeave existingHours;

        #region chitra.girigosavi GEOS2-7807 28/05/2025
        private EmployeeLeave selectedLeaveRecord;//chitra.girigosavi GEOS2-7807 28/05/2025
        CustomObservableCollection<Employee> _employeeListFinalForLeaves;
        #endregion
        #endregion

        #region public ICommand
        public ICommand FilterOptionLoadedCommand { get; set; }
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
        public ICommand LoadedEmployeeAttendanceViewInstanceCommand { get; set; }
        #region Rupali Sarode - GEOS2-3751
        public ICommand FilterOptionEditValueChangedCommand { get; set; }
        public ICommand ShowFilterCommand { get; set; }
        #endregion

        public ICommand SplitButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5275]
        public ICommand MonthChangedCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        #endregion

        #region Properties 
        public bool IsAttendanceColumnChooserVisible
        {
            get { return isAttendanceColumnChooserVisible; }
            set
            {
                isAttendanceColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAttendanceColumnChooserVisible"));
            }
        }
        public bool IsLoadAttendance
        {
            get;
            set;
        }
        public DateTime SelectedDate
        {
            get
            {
                return selectedDate;
            }

            set
            {
                selectedDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDate"));
            }
        }
        public List<GeosAppSetting> GeosAppSettingList1
        {
            get
            {
                return geosAppSettingList1;
            }

            set
            {
                geosAppSettingList1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList1"));
            }
        }
        public ObservableCollection<string> FilterAttendanceList
        {
            get { return filterAttendanceList; }
            set
            {
                filterAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterAttendanceList"));


            }
        }
        public bool IsSplitButtonEnable
        {
            get
            {
                return isSplitButtonEnable;
            }

            set
            {
                isSplitButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSplitButtonEnable"));
            }
        }
        Visibility isSplitButtonVisible = Visibility.Collapsed;
        public Visibility IsSplitButtonVisible
        {
            get
            {
                return isSplitButtonVisible;
            }

            set
            {
                isSplitButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSplitButtonVisible"));
            }
        }
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
        //public bool IsSet
        //{
        //    get
        //    {
        //        return isSet;
        //    }

        //    set
        //    {
        //        isSet = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsSet"));
        //    }
        //}
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
        public CustomObservableCollection<LabelHelper> LabelItems
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
               
        public virtual object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
              
                _selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));            
            }
        }

        public GeosAppSetting Setting { get; set; }
        public GeosAppSetting CompaniesNotDeductBreakSetting { get; set; }
        public List<Int64> lstCompanies { get; set; }
        public CustomObservableCollection<UI.Helper.Appointment> AppointmentItems
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
        public CustomObservableCollection<EmployeeAttendance> EmployeeAttendanceList
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
        public EmployeeAttendanceView EmployeeAttendanceViewInstance { get; set; }
        private CustomObservableCollection<CompanyShift> companyShiftsList;
        public CustomObservableCollection<CompanyShift> CompanyShiftsList
        {
            get
            {
                return companyShiftsList;
            }
            set
            {
                companyShiftsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyShiftsList"));
            }
        }
        public CustomObservableCollection<CompanyHoliday> CompanyHolidays
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
        public CustomObservableCollection<LookupValue> HolidayList
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
                if (value == Visibility.Visible && IsGridViewVisible != Visibility.Hidden)
                {
                    IsGridViewVisible = Visibility.Hidden;
                }

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
                if (value == Visibility.Visible && IsSchedulerViewVisible != Visibility.Hidden)
                {
                    IsSchedulerViewVisible = Visibility.Hidden;
                }

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
        CustomObservableCollection<Employee> OldEmployeesListForGrid;
        public CustomObservableCollection<Employee> Employees
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


        public CustomObservableCollection<EmployeeLeave> EmployeeLeaves
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

        public CustomObservableCollection<StatusHelper> StatusItems
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
            set
            {
                stringIsManual = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StringIsManual"));
            }
        }

        #region Rupali Sarode - GEOS2-3751

        public DateTime FromTimeForShift
        {
            get { return fromTimeForShift; }
            set
            {
                fromTimeForShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromTimeForShift"));
            }
        }

        public DateTime ToTimeForShift
        {
            get { return toTimeForShift; }
            set
            {
                toTimeForShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToTimeForShift"));
            }
        }

        public TimeSpan BreakTimeForShift
        {
            get { return breakTimeForShift; }
            set
            {
                breakTimeForShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BreakTimeForShift"));
            }
        }
        #endregion

        public ObservableCollection<EmployeeShift> EmployeeShiftList
        {
            get
            {
                return employeeShiftList;
            }

            set
            {
                employeeShiftList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeShiftList"));
            }
        }
        #region Rupali Sarode - GEOS2-3751
        public CustomObservableCollection<EmployeeAttendance> EmployeeAttendanceListForFilter
        {
            get
            {
                return employeeAttendanceListForFilter;
            }

            set
            {
                employeeAttendanceListForFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAttendanceListForFilter"));

            }
        }
        public CustomObservableCollection<Employee> EmployeesForFilter
        {
            get
            {
                return employeesForFilter;
            }

            set
            {
                employeesForFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeesForFilter"));

            }
        }
        #endregion

        public DateTime? StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTime"));
            }
        }

        public DateTime? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTime"));
            }
        }

        public int SelectedIndexForCompanyShift
        {
            get { return selectedIndexForCompanyShift; }
            set
            {
                selectedIndexForCompanyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForCompanyShift"));


            }
        }

        //[Sudhir.Jangra][GEOS2-4055][03/01/2023]
        public CustomObservableCollection<Department> Departments
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
        //[rdixit][GEOS2-4054][06/01/2023]
        public CustomObservableCollection<Department> DepartmentsForFilter
        {
            get { return departmentsForFilter; }
            set
            {
                departmentsForFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentsForFilter"));
            }
        }

        public EmployeeAnnualAdditionalLeave ExistingHours
        {
            get { return existingHours; }
            set
            {
                existingHours = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistingHours"));
            }
        }

        //chitra.girigosavi GEOS2-7807 28/05/2025
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
                // [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
                IsLoadAttendance = true;
                IsSchedulerViewVisible = Visibility.Visible;
                IsGridViewVisible = Visibility.Collapsed;
                GeosApplication.Instance.SelectedHRMAttendanceDate = DateTime.Now;            
                _year = GeosApplication.Instance.SelectedHRMAttendanceDate.Year;
                SetUserPermission();
                FillAttendanceFilter();
                GeosApplication.Instance.Logger.Log("Constructor EmployeeAttendanceViewModel()...", category: Category.Info, priority: Priority.Low);
                Setting = CrmStartUp.GetGeosAppSettings(11);
                //[cpatil][GEOS2-5640][27-05-2024]
                CompaniesNotDeductBreakSetting = CrmStartUp.GetGeosAppSettings(120);
                lstCompanies = new List<Int64>();
                if (CompaniesNotDeductBreakSetting != null)
                    foreach (string itemSplit in CompaniesNotDeductBreakSetting.DefaultValue.Split(';'))
                    {
                        if (lstCompanies == null)
                            lstCompanies = new List<Int64>();
                        if(!string.IsNullOrEmpty(itemSplit))
                        lstCompanies.Add(Convert.ToInt32(itemSplit));
                    }
                DisableAppointmentCommand = new DelegateCommand<AppointmentWindowShowingEventArgs>(AppointmentWindowShowing);
                PopupMenuShowingCommand = new DelegateCommand<PopupMenuShowingEventArgs>(PopupMenuShowing);
                CommandDepartmentSelection = new RelayCommand(new Action<object>(SelectItemForScheduler));
                PlantOwnerEditValueChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerEditValueChangedCommandAction);
                //AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                scheduler_VisibleIntervalsChangedCommand = new RelayCommand(new Action<object>(VisibleIntervalsChanged));
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                ImportButtonCommand = new DelegateCommand<object>(OpenAttendanceFile);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshAttendanceView));
                SelectedYearChangedCommand = new DelegateCommand<object>(SelectedYearChangedCommandAction);
                ShowSchedulerViewCommand = new RelayCommand(new Action<object>(ShowAttendanceSchedulerView));
                ShowGridViewCommand = new RelayCommand(new Action<object>(ShowAttendanceGridView));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportAttendancetList));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintAttendanceList));
                ButtonAddNewAttendance = new RelayCommand(new Action<object>(AddAttendance));
                MonthChangedCommand = new RelayCommand(new Action<object>(MonthChanged));
                #region Rupali Sarode - GEOS2-3751

                FilterOptionEditValueChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(FilterOptionEditValueChangedCommandAction);
                #endregion

                SelectedIntervalCommand = new DelegateCommand<MouseButtonEventArgs>(SelectedIntervalCommandAction);
                //Sprint 42-Command to edit attendance from gris view
                EditAttendanceGridDoubleClickCommand = new DelegateCommand<object>(EditAttendanceInformation);
                EditEmployeeDoubleClickCommand = new RelayCommand(new Action<object>(OpenEmployeeProfile));
                EditOccurrenceWindowShowingCommand = new DelegateCommand<EditOccurrenceWindowShowingEventArgs>(EditOccurrenceWindowShowing);
                DeleteAppointmentCommand = new DelegateCommand<object>(DeleteAppointment);
                DefaultLoadCommand = new DelegateCommand<RoutedEventArgs>(DefaultLoadCommandAction);
                LoadedEmployeeAttendanceViewInstanceCommand = new RelayCommand(new Action<object>(LoadedEmployeeAttendanceViewInstanceAction));
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
                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                //[002] added
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                CustomRowFilterCommand = new DelegateCommand<RowFilterEventArgs>(CustomRowFilter);
                FilterOptionLoadedCommand = new RelayCommand(new Action<object>(FilterOptionLoadedCommandAction));

                SplitButtonCommand = new RelayCommand(new Action<object>(SplitButtonCommandAction));//[Sudhir.JAngra][GEOS2-5275]

                SelectedEmployeeLeavesAsPerDate = new List<EmployeeLeave>();
                #region SplitButton [rdixit][04.04.2024][GEOS2-5278]
                IsSplitButtonVisible = GeosApplication.Instance.IsHRMAttendanceSplitPermission == true ? Visibility.Visible : Visibility.Collapsed;
                GeosAppSettingList1 = WorkbenchService.GetSelectedGeosAppSettings("119");
                List<int> compwise = new List<int>();
                if (GeosAppSettingList1 != null)
                {
                    foreach (var item in GeosAppSettingList1.FirstOrDefault(i => i.IdAppSetting == 119)?.DefaultValue?.Split(';'))
                    {
                        string trimmedItem = item.Trim();
                        string[] parts = trimmedItem.Split(',');
                        compwise.Add(int.Parse(parts[0].Trim('(', ')').Trim()));
                    }
                }
                foreach (var item in HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>())
                {
                    if (compwise.Any(i => i == item.IdCompany))
                        IsSplitButtonEnable = true;
                    else
                        IsSplitButtonEnable = false;
                }
                #endregion
                GeosApplication.Instance.Logger.Log("Constructor EmployeeAttendanceViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeAttendanceViewModel()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private async void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                var gridViewBase = (GridViewBase)obj.OriginalSource;
                var grid = gridViewBase.Grid;
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() 1", category: Category.Info, priority: Priority.Low);

                // Load layout in background
                if (File.Exists(HRM_ConfigurationAttendanceGrid_SettingFilePath))
                {
                    await System.Threading.Tasks.Task.Run(() =>
                    {
                        // UI components must not be accessed directly here
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            grid.RestoreLayoutFromXml(HRM_ConfigurationAttendanceGrid_SettingFilePath);
                        });
                    });

                    // Clear search string on UI thread
                    if (grid.View is TableView tableView && tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() 2", category: Category.Info, priority: Priority.Low);

                // Hook layout event
                grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() 3", category: Category.Info, priority: Priority.Low);
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                // Save layout on background thread
                await System.Threading.Tasks.Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        grid.SaveLayoutToXml(HRM_ConfigurationAttendanceGrid_SettingFilePath);
                    });
                });
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() 4", category: Category.Info, priority: Priority.Low);

                int visibleFalseColumn = 0;
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() 5", category: Category.Info, priority: Priority.Low);

                // Iterate over columns
                foreach (GridColumn column in grid.Columns)
                {
                    var descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    descriptor?.AddValueChanged(column, VisibleChanged);

                    var descriptorIndex = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    descriptorIndex?.AddValueChanged(column, VisibleIndexChanged);

                    if (!column.Visible)
                    {
                        visibleFalseColumn++;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() 6", category: Category.Info, priority: Priority.Low);

                IsAttendanceColumnChooserVisible = visibleFalseColumn > 0;

                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == UI.Helper.TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((FrameworkContentElement)sender).Parent).SaveLayoutToXml(HRM_ConfigurationAttendanceGrid_SettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsAttendanceColumnChooserVisible = true;
                }
                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                ((GridControl)((FrameworkContentElement)sender).Parent).SaveLayoutToXml(HRM_ConfigurationAttendanceGrid_SettingFilePath);
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void MonthChanged(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method MonthChanged()...", category: Category.Info, priority: Priority.Low);
                ShowLoading();
                  IsLoadAttendance = true;
                if (!string.IsNullOrEmpty(EmployeeAttendanceViewInstance.scheduler.DisplayName))
                {
                    string input = EmployeeAttendanceViewInstance.scheduler.DisplayName;
                    int year = Convert.ToInt32(input.Trim().Split(' ')[1]);

                    GeosApplication.Instance.SelectedHRMAttendanceDate = new DateTime(year, GetMonthNumberFromString(input), 1);                                     
                 
                    if (_year != year)
                    {
                        _year = year;
                        FillAttendanceListByPlant();
                    }
                    else
                        SelectItemForScheduler1(obj); //[rdixit][GEOS2-8233][01.08.2025]
                  
                }
                //EmployeeMonthlyData.ClosePleaseWaitScreen();
                if (SelectedItem is Department || SelectedItem == null)
                {
                    HrmCommon.Instance.AttendanceLoadingMessage = Application.Current.Resources["SelectEmployeeError"].ToString();
                }
              
                GeosApplication.Instance.Logger.Log("Method MonthChanged()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method MonthChanged()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        int GetMonthNumberFromString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be empty.");

            // Extract the month part (assumes format "MonthName YYYY")
            string monthPart = input.Trim().Split(' ')[0].ToLower();

            switch (monthPart)
            {
                case "january":
                case "enero":
                    return 1;
                case "february":
                case "febrero":
                    return 2;
                case "march":
                case "marzo":
                    return 3;
                case "april":
                case "abril":
                    return 4;
                case "may":
                case "mayo":
                    return 5;
                case "june":
                case "junio":
                    return 6;
                case "july":
                case "julio":
                    return 7;
                case "august":
                case "agosto":
                    return 8;
                case "september":
                case "septiembre":
                    return 9;
                case "october":
                case "octubre":
                    return 10;
                case "november":
                case "noviembre":
                    return 11;
                case "december":
                case "diciembre":
                    return 12;
                default:
                    throw new ArgumentException("Unrecognized month name.");
            }
        }


        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                IsSchedulerViewVisible = Visibility.Visible;
                FillAttendanceListByPlant();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][29.08.2022][GEOS2-3751]
        public void FillAttendanceFilter()
        {
            FilterAttendanceList = new ObservableCollection<string>();
            FilterAttendanceList.Add("Complete day");
            FilterAttendanceList.Add("Incomplete day");
        }
        private void LoadedEmployeeAttendanceViewInstanceAction(object obj)
        {
            this.EmployeeAttendanceViewInstance = (EmployeeAttendanceView)obj;
            if (EmployeeAttendanceViewInstance != null)
            {
                UpdateschedulerLimitAndSelectedInterval();
                object datacontext = EmployeeAttendanceViewInstance.DataContext;
                EmployeeAttendanceViewInstance.DataContext = null;
                EmployeeAttendanceViewInstance.DataContext = datacontext;
            }
            HrmCommon.Instance.AttendanceLoadingMessage = Application.Current.Resources["SelectEmployeeError"].ToString();
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

        #region Rupali Sarode - GEOS2-3751
        //private double GetShiftStartTimeForShift(int dayOfWeek, CompanyShift selectedEmployeeShift)
        //{
        //    TimeSpan DailyHrsCount = TimeSpan.Zero;
        //    bool isDeduct = true;

        //    try
        //    {
        //        try
        //        {
        //            //[cpatil][GEOS2-5640][27-05-2024]
        //            if (lstCompanies.Any(i => i == selectedEmployeeShift.CompanySchedule.Company.IdCompany))
        //            {
        //                isDeduct = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            isDeduct = true;
        //        }
          


        //            GeosApplication.Instance.Logger.Log("Method GetShiftStartTimeForShift()...", category: Category.Info, priority: Priority.Low);
        //        if (selectedEmployeeShift == null) return 0;
        //        //[cpatil][GEOS2-5640][27-05-2024]
        //        switch (dayOfWeek)
        //        {
        //            case 0:
        //               if(isDeduct)
        //                    DailyHrsCount = (selectedEmployeeShift.SunEndTime - selectedEmployeeShift.SunStartTime) - selectedEmployeeShift.SunBreakTime;
        //               else
        //                    DailyHrsCount = (selectedEmployeeShift.SunEndTime - selectedEmployeeShift.SunStartTime);

        //                return DailyHrsCount.TotalHours;


        //            case 1:
        //                if (isDeduct)
        //                    DailyHrsCount = (selectedEmployeeShift.MonEndTime - selectedEmployeeShift.MonStartTime) - selectedEmployeeShift.MonBreakTime;
        //                else
        //                    DailyHrsCount = (selectedEmployeeShift.MonEndTime - selectedEmployeeShift.MonStartTime);

        //                return DailyHrsCount.TotalHours;

        //            case 2:
        //                if (isDeduct)
        //                    DailyHrsCount = (selectedEmployeeShift.TueEndTime - selectedEmployeeShift.TueStartTime) - selectedEmployeeShift.TueBreakTime;
        //                else
        //                    DailyHrsCount = (selectedEmployeeShift.TueEndTime - selectedEmployeeShift.TueStartTime);

        //                return DailyHrsCount.TotalHours;

        //            case 3:
        //                if (isDeduct)
        //                    DailyHrsCount = (selectedEmployeeShift.WedEndTime - selectedEmployeeShift.WedStartTime) - selectedEmployeeShift.WedBreakTime;
        //                else
        //                    DailyHrsCount = (selectedEmployeeShift.WedEndTime - selectedEmployeeShift.WedStartTime);

        //                return DailyHrsCount.TotalHours;

        //            case 4:
        //                if (isDeduct)
        //                    DailyHrsCount = (selectedEmployeeShift.ThuEndTime - selectedEmployeeShift.ThuStartTime) - selectedEmployeeShift.ThuBreakTime;
        //                else
        //                    DailyHrsCount = (selectedEmployeeShift.ThuEndTime - selectedEmployeeShift.ThuStartTime);

        //                return DailyHrsCount.TotalHours;

        //            case 5:
        //                if (isDeduct)
        //                    DailyHrsCount = (selectedEmployeeShift.FriEndTime - selectedEmployeeShift.FriStartTime) - selectedEmployeeShift.FriBreakTime;
        //                else
        //                    DailyHrsCount = (selectedEmployeeShift.FriEndTime - selectedEmployeeShift.FriStartTime);

        //                return DailyHrsCount.TotalHours;

        //            case 6:
        //                if (isDeduct)
        //                    DailyHrsCount = (selectedEmployeeShift.SatEndTime - selectedEmployeeShift.SatStartTime) - selectedEmployeeShift.SatBreakTime;
        //                else
        //                    DailyHrsCount = (selectedEmployeeShift.SatEndTime - selectedEmployeeShift.SatStartTime);

        //                return DailyHrsCount.TotalHours;

        //            default:
        //                return DailyHrsCount.TotalHours;

        //        }


        //        GeosApplication.Instance.Logger.Log("Method GetShiftStartTimeForShift()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method GetShiftStartTimeForShift()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
        //        return DailyHrsCount.TotalHours;
        //    }
        //}

        private Tuple<double, TimeSpan, bool> GetShiftStartTimeForShift(int dayOfWeek, CompanyShift selectedEmployeeShift)
        {
            TimeSpan DailyHrsCount = TimeSpan.Zero;
            TimeSpan EmployeeBreakTime = TimeSpan.Zero;
            bool isDeduct = false;

            try
            {
                try
                {
                    //[cpatil][GEOS2-5640][27-05-2024]
                    if (lstCompanies.Any(i => i == selectedEmployeeShift.CompanySchedule.Company.IdCompany))
                    {
                        isDeduct = false;
                    }
                    else
                        isDeduct = true;
                }
                catch (Exception ex)
                {
                    isDeduct = true;
                }



                GeosApplication.Instance.Logger.Log("Method GetShiftStartTimeForShift()...", category: Category.Info, priority: Priority.Low);
                if (selectedEmployeeShift == null) return new Tuple<double, TimeSpan, bool>(DailyHrsCount.TotalHours, EmployeeBreakTime, false); 
                //[cpatil][GEOS2-5640][27-05-2024]
                switch (dayOfWeek)
                {
                    case 0:
                        DailyHrsCount = (selectedEmployeeShift.SunEndTime - selectedEmployeeShift.SunStartTime) - selectedEmployeeShift.SunBreakTime;

                        return new Tuple<double, TimeSpan, bool>( DailyHrsCount.TotalHours, selectedEmployeeShift.SunBreakTime, isDeduct) ;


                    case 1:
                        DailyHrsCount = (selectedEmployeeShift.MonEndTime - selectedEmployeeShift.MonStartTime) - selectedEmployeeShift.MonBreakTime;

                        return new Tuple<double, TimeSpan, bool>(DailyHrsCount.TotalHours, selectedEmployeeShift.MonBreakTime, isDeduct); 

                    case 2:
                        DailyHrsCount = (selectedEmployeeShift.TueEndTime - selectedEmployeeShift.TueStartTime) - selectedEmployeeShift.TueBreakTime;

                        return new Tuple<double, TimeSpan, bool>(DailyHrsCount.TotalHours, selectedEmployeeShift.TueBreakTime, isDeduct);

                    case 3:
                        DailyHrsCount = (selectedEmployeeShift.WedEndTime - selectedEmployeeShift.WedStartTime) - selectedEmployeeShift.WedBreakTime;

                        return new Tuple<double, TimeSpan, bool>(DailyHrsCount.TotalHours, selectedEmployeeShift.WedBreakTime, isDeduct);

                    case 4:
                        DailyHrsCount = (selectedEmployeeShift.ThuEndTime - selectedEmployeeShift.ThuStartTime) - selectedEmployeeShift.ThuBreakTime;

                        return new Tuple<double, TimeSpan, bool>(DailyHrsCount.TotalHours, selectedEmployeeShift.ThuBreakTime, isDeduct); 

                    case 5:
                        DailyHrsCount = (selectedEmployeeShift.FriEndTime - selectedEmployeeShift.FriStartTime) - selectedEmployeeShift.FriBreakTime;

                        return new Tuple<double, TimeSpan, bool>(DailyHrsCount.TotalHours, selectedEmployeeShift.FriBreakTime, isDeduct); 

                    case 6:
                        DailyHrsCount = (selectedEmployeeShift.SatEndTime - selectedEmployeeShift.SatStartTime) - selectedEmployeeShift.SatBreakTime;

                        return new Tuple<double, TimeSpan, bool>(DailyHrsCount.TotalHours, selectedEmployeeShift.SatBreakTime, isDeduct);

                    default:
                        return new Tuple<double, TimeSpan, bool>(DailyHrsCount.TotalHours, EmployeeBreakTime, false);

                }


                GeosApplication.Instance.Logger.Log("Method GetShiftStartTimeForShift()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetShiftStartTimeForShift()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                return new Tuple<double, TimeSpan, bool>(DailyHrsCount.TotalHours, EmployeeBreakTime, false);
            }
        }
        #endregion
        public void ShowLoading()
        {
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
                GeosApplication.Instance.Logger.Log("Get an error in Method PopupMenuShowing()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                        //List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        EmployeeAttendance selectedEmpAttendance = new EmployeeAttendance();
                        //var EmpAttendance = EmployeeAttendanceList.Where(x => x.IdEmployeeAttendance == ((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeAttendance);
                        selectedEmpAttendance = EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == ((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeAttendance);

                        AttendanceView addAttendanceView = new AttendanceView();
                        AttendanceViewModel addAttendanceViewModel = new AttendanceViewModel(addAttendanceView);
                        //EventHandler handle = delegate { addAttendanceView.Close(); };
                        //addAttendanceViewModel.RequestClose += handle;
                        //addAttendanceView.DataContext = addAttendanceViewModel;
                        //[001] CompanyWork IdCompany  Code comment and changes as per Job description IdCompany
                        //  addAttendanceViewModel.WorkingPlantId = selectedEmpAttendance.CompanyWork.IdCompany.ToString();
                        //addAttendanceViewModel.WorkingPlantId = selectedEmpAttendance.Employee.EmployeeJobDescription.IdCompany.ToString();

                        //string[] idEmployeeCompanyIdsSplit = selectedEmpAttendance.Employee.EmployeeCompanyIds.Split(',');
                        //addAttendanceViewModel.WorkingPlantId = idEmployeeCompanyIdsSplit[0];
                        var workingPlantId = selectedEmpAttendance.Employee.EmployeeCompanyIds.Split(',')[0];
                        //addAttendanceViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                        //addAttendanceViewModel.SelectedPlantList = plantOwners;
                        if (HrmCommon.Instance.IsPermissionReadOnly)
                            addAttendanceViewModel.InitReadOnly(selectedEmpAttendance, workingPlantId);
                        else
                            addAttendanceViewModel.EditInit(selectedEmpAttendance, EmployeeAttendanceList, Employees, EmployeeLeaves, workingPlantId);

                        EmployeeJobDescription empJobDesc = new EmployeeJobDescription();
                        if (selectedEmpAttendance.Employee.EmployeeJobDescription != null)
                        {
                            empJobDesc = selectedEmpAttendance.Employee.EmployeeJobDescription;
                        }
                        //addAttendanceViewModel.EmployeeLeaves = EmployeeLeaves;
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
                                modelAppointment.AttendanceStatus = addAttendanceViewModel.UpdateEmployeeAttendance.AttendanceStatus;
                                #region Rupali Sarode - GEOS2-3751

                                //if (addAttendanceViewModel.CompanyShiftDetails != null)
                                //{
                                //    if (addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule != null)
                                //        modelAppointment.DailyHoursCount = addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule.DailyHoursCount;
                                //}
                                //else
                                //    modelAppointment.DailyHoursCount = 0;


                                if (addAttendanceViewModel.UpdateEmployeeAttendance != null)
                                {
                                    Tuple<double, TimeSpan, bool> GetDailyHrBreakTimeIsDeductUpdateEmployeeAttendance;
                                    GetDailyHrBreakTimeIsDeductUpdateEmployeeAttendance = GetShiftStartTimeForShift((int)addAttendanceViewModel.UpdateEmployeeAttendance.StartDate.DayOfWeek, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);
                                    modelAppointment.DailyHoursCount = Convert.ToDecimal(GetDailyHrBreakTimeIsDeductUpdateEmployeeAttendance.Item1);
                                    modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeductUpdateEmployeeAttendance.Item3;
                                    modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeductUpdateEmployeeAttendance.Item2;
                                }
                                else
                                {
                                    modelAppointment.DailyHoursCount = 0;
                                    modelAppointment.IsDeductBreakTime = false;
                                    modelAppointment.EmployeeShiftBreakTime = TimeSpan.Zero;
                                }

                                #endregion

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
                                    // [nsatpute][03-10-2024][GEOS2-6451]
                                    if (selectedEmpAttendance.IsMobileApiAttendance)
                                    {
                                        modelAppointment.Description = modelAppointment.Description + " [Mobile]";
                                        modelAppointment.IsMobileApiAttendance = true;
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

                                Tuple<TimeSpan, TimeSpan, bool> GetDailyHrBreakTimeIsDeduct;
                                GetDailyHrBreakTimeIsDeduct = GetShiftTime(modelAppointment, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);//[002]
                                modelAppointment.ShiftWorkingTime = GetDailyHrBreakTimeIsDeduct.Item1;
                                modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeduct.Item3;
                                modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeduct.Item2;
                                AppointmentItems.Add(modelAppointment);

                                addAttendanceViewModel.UpdateEmployeeAttendance.Employee.EmployeeJobDescription = empJobDesc;
                                //selectedEmpAttendance.Status = addAttendanceViewModel.UpdateEmployeeAttendance.Status;
                                selectedEmpAttendance.Employee = addAttendanceViewModel.UpdateEmployeeAttendance.Employee;
                                selectedEmpAttendance.IdEmployee = addAttendanceViewModel.UpdateEmployeeAttendance.IdEmployee;
                                selectedEmpAttendance.StartDate = addAttendanceViewModel.UpdateEmployeeAttendance.StartDate;
                                selectedEmpAttendance.EndDate = addAttendanceViewModel.UpdateEmployeeAttendance.EndDate;
                                selectedEmpAttendance.StartTime = addAttendanceViewModel.STime;
                                selectedEmpAttendance.EndTime = addAttendanceViewModel.ETime;
                                selectedEmpAttendance.IdCompanyShift = addAttendanceViewModel.UpdateEmployeeAttendance.IdCompanyShift;
                                selectedEmpAttendance.CompanyShift = addAttendanceViewModel.SelectedEmployeeShift.CompanyShift;
                                selectedEmpAttendance.FileName = addAttendanceViewModel.UpdateEmployeeAttendance.FileName;
                                selectedEmpAttendance.Remark = addAttendanceViewModel.UpdateEmployeeAttendance.Remark;
                                selectedEmpAttendance.Location = addAttendanceViewModel.LocationValue;
                                selectedEmpAttendance.AttendanceStatus = addAttendanceViewModel.UpdateEmployeeAttendance.AttendanceStatus;
                                selectedEmpAttendance.IdStatus = addAttendanceViewModel.UpdateEmployeeAttendance.IdStatus;
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
                                        List<EmployeeLeave> NewLeaveOfSelectedEmployeeAsPerDate = new List<EmployeeLeave>();
                                        Employee SelectedEmployee = null;//[Sudhir.Jangra][GEOS2-4055][03/01/2023]
                                        if (SelectedItem is Employee)
                                        {
                                            SelectedEmployee = ((Employee)SelectedItem);

                                        }
                                        else if (SelectedItem is Department)
                                        {
                                            if (((Department)SelectedItem).Employees != null)
                                            {
                                                SelectedEmployee = ((Department)SelectedItem).Employees.Where(x => x.IdEmployee == item.Employee.IdEmployee).FirstOrDefault();
                                            }
                                            else if ((Department)SelectedItem == Departments[0] && SelectedEmployee == null)
                                            {
                                                UI.Helper.Appointment modelAppointment1 = new UI.Helper.Appointment();
                                                modelAppointment1 = CreateAppointmentObjUsingInputs(addAttendanceViewModel, item, NewLeaveOfSelectedEmployeeAsPerDate);
                                                Tuple<TimeSpan, TimeSpan, bool> GetDailyHrBreakTimeIsDeductSelectedEmployee;
                                                GetDailyHrBreakTimeIsDeductSelectedEmployee = GetShiftTime(modelAppointment1, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);//[002]
                                                modelAppointment1.ShiftWorkingTime = GetDailyHrBreakTimeIsDeductSelectedEmployee.Item1;
                                                modelAppointment1.IsDeductBreakTime = GetDailyHrBreakTimeIsDeductSelectedEmployee.Item3;
                                                modelAppointment1.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeductSelectedEmployee.Item2;
                                                AppointmentItems.Add(modelAppointment1);
                                            }
                                        }
                                        var SelectedEmployeeLeave = EmployeeLeaves.Where(x => x.IdEmployee == item.IdEmployee).ToList();

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
                                        modelAppointment.AttendanceStatus = item.AttendanceStatus;
                                        #region Rupali Sarode - GEOS2-3751

                                        //if (addAttendanceViewModel.CompanyShiftDetails != null)
                                        //{
                                        //    if (addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule != null)
                                        //    {
                                        //        modelAppointment.DailyHoursCount = addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule.DailyHoursCount;
                                        //    }
                                        //}
                                        //else
                                        //    modelAppointment.DailyHoursCount = 0;


                                        if (addAttendanceViewModel.SelectedEmployeeShift != null)
                                        {
                                            if (addAttendanceViewModel.SelectedEmployeeShift.CompanyShift != null)
                                            {
                                                Tuple<double, TimeSpan, bool> GetDailyHrBreakTimeIsDeductCompanyShift;
                                                GetDailyHrBreakTimeIsDeductCompanyShift = GetShiftStartTimeForShift((int)item.StartDate.DayOfWeek, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);
                                                modelAppointment.DailyHoursCount = Convert.ToDecimal(GetDailyHrBreakTimeIsDeductCompanyShift.Item1);
                                                modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeductCompanyShift.Item3;
                                                modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeductCompanyShift.Item2;
                                            }
                                            else
                                            {
                                                modelAppointment.DailyHoursCount = 0;
                                                modelAppointment.IsDeductBreakTime = false;
                                                modelAppointment.EmployeeShiftBreakTime = TimeSpan.Zero;
                                            }

                                        }
                                        else
                                        {
                                            modelAppointment.DailyHoursCount = 0;
                                            modelAppointment.IsDeductBreakTime = false;
                                            modelAppointment.EmployeeShiftBreakTime = TimeSpan.Zero;
                                        }

                                        #endregion


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
                                            // [nsatpute][03-10-2024][GEOS2-6451]
                                            if (selectedEmpAttendance.IsMobileApiAttendance)
                                            {
                                                modelAppointment.Description = modelAppointment.Description + " [Mobile]";
                                                modelAppointment.IsMobileApiAttendance = true;
                                            }
                                            else
                                            {
                                                modelAppointment.IsMobileApiAttendance = false;
                                            }
                                        }

                                        modelAppointment.AccountingDate = item.AccountingDate;
                                        Tuple<TimeSpan, TimeSpan, bool> GetDailyHrBreakTimeIsDeduct;
                                        GetDailyHrBreakTimeIsDeduct = GetShiftTime(modelAppointment, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);//[002]
                                        modelAppointment.ShiftWorkingTime = GetDailyHrBreakTimeIsDeduct.Item1;
                                        modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeduct.Item3;
                                        modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeduct.Item2;
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
                    //chitra.girigosavi GEOS2-7807 28/05/2025
                    else if (((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeLeave != null)
                    {
                        //List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();

                        EmployeeLeave selectedEmpLeave = new EmployeeLeave();
                        //var EmpLeave = EmployeeLeaves.Where(x => x.IdEmployeeLeave == ((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeLeave);
                        selectedEmpLeave = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == ((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployeeLeave);
                        AddNewLeaveView addNewLeaveView = new AddNewLeaveView();
                        AddNewLeaveViewModel addNewLeaveViewModel = new AddNewLeaveViewModel(addNewLeaveView);
                        //EventHandler handle = delegate { addNewLeaveView.Close(); };
                        //addNewLeaveViewModel.RequestClose += handle;
                        //addNewLeaveView.DataContext = addNewLeaveViewModel;
                        //addNewLeaveViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                        var workingPlantId = selectedEmpLeave.CompanyLeave.IdCompany.ToString();
                        //addNewLeaveViewModel.SelectedPlantList = plantOwners;

                        if (HrmCommon.Instance.IsPermissionReadOnly)
                            addNewLeaveViewModel.InitReadOnly(selectedEmpLeave, workingPlantId);
                        else
                            addNewLeaveViewModel.EditInit(selectedEmpLeave, EmployeeLeaves, workingPlantId, _employeeListFinalForLeaves);

                        //addNewLeaveViewModel.IsNew = false;
                        //addNewLeaveViewModel.LeaveTitle = System.Windows.Application.Current.FindResource("EditLeave").ToString();
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
                GeosApplication.Instance.Logger.Log("Get an error in Method AppointmentWindowShowing()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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

				// [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
                scheduler.Month = GeosApplication.Instance.SelectedHRMAttendanceDate;          
              
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
                    #region Previously added ExitDetails in Employee List
                    foreach (Employee item in Employees)
                    {
                        if (item.IdEmployeeStatus == 138)
                        {
                            item.IsEmployeeStatus = true;
                            item.EmployeeContractStartDate = "Start Date : " + item.EmployeeContractSituationStartDate.Date.ToShortDateString();
                        }
                        else
                        {
                            item.IsEmployeeStatus = false;
                            item.EmployeeContractStartDate = "";
                        }
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
                                    if (!isBlueLine)
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
                    #endregion
                    #region //[rdixit][GEOS2-4055][06.04.2023] To Add Exit Details Values in Department wise Employee List
                    if (DepartmentsForFilter != null)
                    {
                        foreach (var Deptitem in DepartmentsForFilter)
                        {
                            foreach (Employee item in Deptitem.Employees)
                            {
                                if (item.IdEmployeeStatus == 138)
                                {
                                    item.IsEmployeeStatus = true;
                                    item.EmployeeContractStartDate = "Start Date : " + item.EmployeeContractSituationStartDate.Date.ToShortDateString();
                                }
                                else
                                {
                                    item.IsEmployeeStatus = false;
                                    item.EmployeeContractStartDate = "";
                                }
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
                                            if (!isBlueLine)
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
                    }
                    #endregion                    
                }

                if (IsEmployeewiseRegisterAndExpectDays)
                {
                    MonthlyAllRegisterHours(scheduler);
                }
                if (scheduler.Uid == 1.ToString())
                {
                    if (SelectedFilterAttendanceList != null)
                    {
                        if (SelectedFilterAttendanceList.ToString() == "Incomplete day")
                        {
                            FilterOptionEditValueChangedCommandAction();
                        }
                        else if (SelectedFilterAttendanceList.ToString() == "Complete day")
                        {
                            FilterOptionEditValueChangedCommandAction();
                        }
                    }
                    scheduler.Uid = string.Empty;
                }
                if (scheduler.Uid == "-1".ToString())
                {
                    if (SelectedFilterAttendanceList != null)
                    {
                        if (SelectedFilterAttendanceList.ToString() == "Incomplete day")
                        {
                            FilterOptionEditValueChangedCommandAction();
                        }
                        else if (SelectedFilterAttendanceList.ToString() == "Complete day")
                        {
                            FilterOptionEditValueChangedCommandAction();
                        }
                    }

                    scheduler.Uid = string.Empty;
                }
				// [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentItems"));
                Mouse.OverrideCursor = null;
                GeosApplication.Instance.Logger.Log("Method VisibleIntervalsChanged()....executed successfully", category: Category.Info, priority: Priority.Low);
                //[rdixit][GEOS2-8233][01.08.2025]
                if(HrmMainViewModel.IsAttendanceLoaded)
                {                    
                    EmployeeMonthlyData.ClosePleaseWaitScreen();
                }
                HrmMainViewModel.IsAttendanceLoaded = true;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method VisibleIntervalsChanged()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        // [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
        void LoadEmployeeMonthlyData(Employee employee, SchedulerControlEx scheduler)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LoadEmployeeMonthlyData()....", category: Category.Info, priority: Priority.Low);
                if (scheduler != null && employee != null)
                {                    
                    var selectedDate = GeosApplication.Instance.SelectedHRMAttendanceDate;
                    fromDate = scheduler.VisibleIntervals.Min(i=>i.Start);
                    toDate = scheduler.VisibleIntervals.Max(i => i.End);
                    if (_employeeid != employee.IdEmployee || fromDate != _fromDate || toDate != _toDate || IsLoadAttendance)
                    {
                        IsLoadAttendance = false;
                        employeeAttendanceList = new CustomObservableCollection<EmployeeAttendance>();
                        _toDate = toDate; _fromDate = fromDate; _employeeid = employee.IdEmployee;
                        EmployeeMonthlyData.GetEmployeeAttendanceFromService_2640(employee.IdEmployee,ref HrmService,ref fromDate,ref toDate,ref employeeAttendanceList);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method LoadEmployeeMonthlyData()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LoadEmployeeMonthlyData()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FilterOptionLoadedCommandAction(object obj)//[rdixit][29.08.2022][GEOS2-3751]
        {
            IsFilterLoaded = true;
            ComboBoxEdit combo = obj as ComboBoxEdit;
            combo.SelectAllItems();
            IsFilterLoaded = false;
        }
        #region Rupali Sarode - GEOS2-3751
        private void FilterOptionEditValueChangedCommandAction(object obj)//[rdixit][29.08.2022][GEOS2-3751]
        {
            if (IsFilterLoaded)
            {
                return;
            }

            List<Int32> idsEmployeesIncomplete = new List<int>();
            List<Int32> idsEmployees = new List<int>();
            DateTime? fromDateMonth;
            DateTime? toDateMonth;
            var today = Convert.ToDateTime(EmployeeAttendanceViewInstance.scheduler.Month);
            List<int> AllDaysLeaveWeeklyOfDays = new List<int>();
            Decimal AllDays = DateTime.DaysInMonth(EmployeeAttendanceViewInstance.scheduler.Month.Value.Year, EmployeeAttendanceViewInstance.scheduler.Month.Value.Month);

            for (int day = 1; day <= AllDays; day++)
            {
                DateTime currentDay = new DateTime(EmployeeAttendanceViewInstance.scheduler.Month.Value.Year, Convert.ToInt32(EmployeeAttendanceViewInstance.scheduler.Month.Value.Month), day);
                if (currentDay.Date == DateTime.Now.Date)
                {
                    break;
                }
                if (currentDay.DayOfWeek != DayOfWeek.Sunday && currentDay.DayOfWeek != DayOfWeek.Saturday)
                {
                    AllDaysLeaveWeeklyOfDays.Add(day);
                }
            }
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            fromDateMonth = monthStart;
            toDateMonth = monthEnd;
            // shubham[skadam] GEOS2-3991 Attendance - wrong incomplete day   10 11 2022
            DateTime fromMonthStartDate = fromDateMonth.Value.Date.AddMonths(-1);
            DateTime toMonthEndDate = toDateMonth.Value.Date.AddMonths(1);
            string CurrentDay = DateTime.Now.Day.ToString();
            var plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
            var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
            if (CurrentDay.Equals(AllDaysLeaveWeeklyOfDays.First().ToString()))
            {
                return;
            }
            ComboBoxEdit combo = obj as ComboBoxEdit;
            var options = combo.SelectedItems as ObservableCollection<object>;
            //Hrm.CustomObservableCollection<EmployeeAttendance> EmployeeAttendanceListForFilterCompleteHours = new Hrm.CustomObservableCollection<EmployeeAttendance>();
            //EmployeeAttendanceListForFilter = new Hrm.CustomObservableCollection<EmployeeAttendance>();
            EmployeesForFilter = new Hrm.CustomObservableCollection<Employee>();
            DepartmentsForFilter = new CustomObservableCollection<Department>();//[rdixit][GEOS2-4054]
            DepartmentsForFilter.AddRangeWithTemporarySuppressedNotification(Departments.Select(item => (Department)item.Clone()).ToList());
            foreach (var item in DepartmentsForFilter)
            {
                item.Employees = null;
            }
            // if (this.EmployeesForFilter != null) this.EmployeesForFilter.ClearWithTemporarySuppressedNotification();

            //if (this.EmployeeAttendanceListForFilter != null) this.EmployeeAttendanceListForFilter.ClearWithTemporarySuppressedNotification();

            //AllDaysLeaveWeeklyOfDays[0];

            if (options.Count() == 2) // "SelectALL"
            {
                //FillAttendanceListByPlant();
                EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                EmployeesForFilter = new CustomObservableCollection<Employee>();
                EmployeeAttendanceListForFilter = employeeAttendanceList;
                EmployeesForFilter = Employees;
                DepartmentsForFilter = Departments;
                SelectedFilterAttendanceList = null;
            }
            #region Commmented_Code
            //else if (options.Count() == 1 && options[0].ToString() == "Complete day")  // "Completed"
            //{
            //    EmployeeAttendanceListForFilter = new Hrm.CustomObservableCollection<EmployeeAttendance>();
            //    EmployeesForFilter = new Hrm.CustomObservableCollection<Employee>();

            //    if (this.EmployeesForFilter != null) this.EmployeesForFilter.ClearWithTemporarySuppressedNotification();
            //    if (this.EmployeeAttendanceListForFilter != null) this.EmployeeAttendanceListForFilter.ClearWithTemporarySuppressedNotification();

            //    EmployeeAttendanceListForFilter.AddRange(EmployeeAttendanceList.Where(s => s.DailyHours <= s.WorkedHours).ToList());
            //    List<Int32> idsEmployees = new List<int>();
            //    idsEmployees = EmployeeAttendanceListForFilter.Select(i => i.IdEmployee).Distinct().ToList();
            //    EmployeesForFilter.AddRange(Employees.Where(i=> idsEmployees.Contains(i.IdEmployee)).ToList());
            //}
            //else if (options.Count() == 1 && options[0].ToString() == "Incomplete day") // InCompleted
            //{


            //    EmployeeAttendanceListForFilter = new Hrm.CustomObservableCollection<EmployeeAttendance>();
            //    EmployeesForFilter = new Hrm.CustomObservableCollection<Employee>();

            //    if (this.EmployeesForFilter != null) this.EmployeesForFilter.ClearWithTemporarySuppressedNotification();
            //    if (this.EmployeeAttendanceListForFilter != null) this.EmployeeAttendanceListForFilter.ClearWithTemporarySuppressedNotification();

            //    EmployeeAttendanceListForFilter.AddRange(EmployeeAttendanceList.Where(s => s.DailyHours > s.WorkedHours).ToList());
            //    List<Int32> idsEmployees = new List<int>();
            //    idsEmployees = EmployeeAttendanceListForFilter.Select(i => i.IdEmployee).Distinct().ToList();
            //    EmployeesForFilter.AddRange(Employees.Where(i => idsEmployees.Contains(i.IdEmployee)).ToList());

            #endregion
            else if (options.Count() == 1) // For Single Selection //[rdixit][02.09.2022][GEOS2-3751] 
            {
                if (options[0].ToString() == "Incomplete day")
                {
                    SelectedFilterAttendanceList = "Incomplete day";
                    #region Incompleteday
                    #region oldcode
                    //EmployeeAttendanceListForFilter.AddRange(EmployeeAttendanceList.Where(s => s.DailyHours > s.WorkedHours).ToList());
                    //EmployeeAttendanceListForFilterCompleteHours.AddRange(EmployeeAttendanceList.Where(s => s.DailyHours <= s.WorkedHours).ToList());
                    //EmployeesForFilter.AddRange(Employees);
                    //foreach (EmployeeAttendance item in EmployeeAttendanceListForFilterCompleteHours)
                    //{
                    //    EmployeesForFilter.Remove((Employees.Where(i => i.IdEmployee == item.IdEmployee).FirstOrDefault()));
                    //}
                    #endregion
                    #region comment


                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date).ToList();
                    //var groupByandSumEmployeeAttendanceList = EmployeeAttendanceListGroupBy2.GroupBy(g => new { g.StartDate.Date, g.IdEmployee })
                    //.Select(cl => new EmployeeAttendance
                    //{
                    //   StartDate = cl.First().StartDate,
                    //   Employee = cl.First().Employee,
                    //   EndDate = cl.First().EndDate,
                    //   AccountingDate = cl.First().AccountingDate,
                    //   DailyHours = cl.First().DailyHours,
                    //   TotalTime = new TimeSpan(cl.Sum(p => p.TotalTime.Value.Ticks)),
                    //   WorkedHours = new TimeSpan(cl.Sum(p => p.WorkedHours.Value.Ticks)),
                    //   IdEmployeeAttendance = cl.First().IdEmployeeAttendance,
                    //   IdEmployee = cl.First().IdEmployee,

                    //}).OrderBy(o => o.StartDate).ToList();
                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy = groupByandSumEmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date
                    //&& w.DailyHours <= w.WorkedHours).ToList();
                    //idsEmployees = EmployeeAttendanceListGroupBy.Select(i => i.IdEmployee).Distinct().ToList();
                    //EmployeesForFilter.AddRange(Employees.Where(i => !idsEmployees.Contains(i.IdEmployee)).ToList());
                    //if (EmployeeAttendanceListForFilter.Count == 0)
                    //{
                    //    EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                    //    EmployeeAttendanceListForFilter = employeeAttendanceList;
                    //}
                    #endregion

                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date && w.IdEmployee == 2047).ToList();
                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date && w.IdEmployee == 232).ToList();
                    List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date).ToList();
                    var result1 = EmployeeAttendanceListGroupBy2.GroupBy(g => new { g.StartDate.Date, g.IdEmployee })
                   .Select(cl => new EmployeeAttendance
                   {
                       StartDate = cl.First().StartDate,
                       Employee = cl.First().Employee,
                       EndDate = cl.First().EndDate,
                       AccountingDate = cl.First().AccountingDate,
                       DailyHours = cl.First().DailyHours,
                       TotalTime = new TimeSpan(cl.Sum(p => p.TotalTime == null ? 0 : p.TotalTime.Value.Ticks)),
                       WorkedHours = new TimeSpan(cl.Sum(p => p.WorkedHours == null ? 0 : p.WorkedHours.Value.Ticks)),
                       IdEmployeeAttendance = cl.First().IdEmployeeAttendance,
                       IdEmployee = cl.First().IdEmployee,
                       IdCompanyWork = cl.First().IdCompanyWork,
                       CompanyWork = cl.First().CompanyWork,
                       IdCompanyShift = cl.First().IdCompanyShift,
                       CompanyShift = cl.First().CompanyShift,
                       EmployeeCode = cl.First().EmployeeCode,
                       WeekNumber = cl.First().WeekNumber,

                   }).OrderBy(o => o.StartDate).ToList();

                    List<EmployeeAttendance> EmployeeAttendanceListGroupBy = result1.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date && w.DailyHours <= w.WorkedHours).ToList();
                    idsEmployees = EmployeeAttendanceListGroupBy.Select(i => i.IdEmployee).Distinct().ToList();
                    List<Int32> idEmployees = new List<int>();
                    foreach (int employeesId in idsEmployees)
                    {
                        //if (result1.Where(w => w.IdEmployee == employeesId).ToList().Count() == EmployeeAttendanceListGroupBy.Where(w => w.IdEmployee == employeesId).ToList().Count())
                        {
                            bool flag = false;
                            List<EmployeeAttendance> EmployeeAttendanceListDateFilter = EmployeeAttendanceListGroupBy.Where(w => w.IdEmployee == employeesId).ToList();
                            // shubham[skadam] GEOS2-3991 Attendance - wrong incomplete day   10 11 2022
                            List<EmployeeLeave> selectedEmployeeLeavesListByIdEmployee = EmployeeLeaves.Where(w => w.IdEmployee == employeesId &&
                            w.StartDate.Value.Date >= fromMonthStartDate && w.StartDate.Value.Date <= toMonthEndDate).ToList();
                            foreach (var employeeAttendance in AllDaysLeaveWeeklyOfDays)
                            {
                                if (today.Year == DateTime.Now.Year)
                                {
                                    if (today.Month == DateTime.Now.Month)
                                        if (employeeAttendance == DateTime.Now.Day)
                                        {
                                            idEmployees.Add(employeesId);
                                        }
                                }
                                if (EmployeeAttendanceListDateFilter.Any(a => a.StartDate.Day == employeeAttendance) ||
                                    selectedEmployeeLeavesListByIdEmployee.Any(a => a.StartDate.Value.Day == employeeAttendance)
                                    )
                                {
                                    flag = true;
                                }
                                else
                                {
                                    DateTime employeeAttendanceDateTime = new DateTime(today.Year, today.Month, employeeAttendance);
                                    //if (selectedEmployeeLeavesListByIdEmployee.Any(d => employeeAttendance >= d.StartDate.Value.Day && employeeAttendance <= d.EndDate.Value.Day))
                                    if (selectedEmployeeLeavesListByIdEmployee.Any(d => employeeAttendanceDateTime >= d.StartDate && employeeAttendanceDateTime <= d.EndDate))
                                    {
                                        flag = true;
                                    }
                                    else if (CompanyHolidays.Any(d => employeeAttendance >= d.StartDate.Value.Day && employeeAttendance <= d.EndDate.Value.Day && today.Month == d.StartDate.Value.Month))
                                    {
                                        flag = true;
                                    }
                                    else
                                    {
                                        flag = false;
                                        break;
                                    }

                                }
                            }

                            if (flag)
                            {
                                idEmployees.Add(employeesId);
                            }
                        }
                    }
                    idsEmployees.Clear();
                    idsEmployees = idEmployees;


                    EmployeesForFilter.AddRange(Employees.Where(i => !idsEmployees.Contains(i.IdEmployee)).ToList());

                    if (EmployeeAttendanceListForFilter.Count == 0)
                    {
                        EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                        EmployeeAttendanceListForFilter = employeeAttendanceList;
                    }

                    #endregion
                }
                else if (options[0].ToString() == "Complete day")
                {
                    SelectedFilterAttendanceList = "Complete day";
                    #region CompleteDay


                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate >= fromDateMonth && w.StartDate <= toDateMonth && w.IdEmployee == 232).ToList();
                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate >= fromDateMonth && w.StartDate <= toDateMonth && w.IdEmployee == 405).ToList();
                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date && w.IdEmployee ==553).ToList();
                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date && w.IdEmployee == 2727).ToList();
                    // List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date && w.IdEmployee == 2381).ToList();
                    List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date).ToList();
                    var result1 = EmployeeAttendanceListGroupBy2.GroupBy(g => new { g.StartDate.Date, g.IdEmployee })
                    .Select(cl => new EmployeeAttendance
                    {
                        StartDate = cl.First().StartDate,
                        Employee = cl.First().Employee,
                        EndDate = cl.First().EndDate,
                        AccountingDate = cl.First().AccountingDate,
                        DailyHours = cl.First().DailyHours,
                        TotalTime = new TimeSpan(cl.Sum(p => p.TotalTime == null ? 0 : p.TotalTime.Value.Ticks)),
                        WorkedHours = new TimeSpan(cl.Sum(p => p.WorkedHours == null ? 0 : p.WorkedHours.Value.Ticks)),
                        IdEmployeeAttendance = cl.First().IdEmployeeAttendance,
                        IdEmployee = cl.First().IdEmployee,
                        IdCompanyWork = cl.First().IdCompanyWork,
                        CompanyWork = cl.First().CompanyWork,
                        IdCompanyShift = cl.First().IdCompanyShift,
                        CompanyShift = cl.First().CompanyShift,
                        EmployeeCode = cl.First().EmployeeCode,
                        WeekNumber = cl.First().WeekNumber,


                    }).OrderBy(o => o.StartDate).ToList();

                    List<EmployeeAttendance> EmployeeAttendanceListGroupBy = result1.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date && w.DailyHours <= w.WorkedHours).ToList();
                    idsEmployees = EmployeeAttendanceListGroupBy.Select(i => i.IdEmployee).Distinct().ToList();
                    List<Int32> idEmployees = new List<int>();
                    foreach (int employeesId in idsEmployees)
                    {
                        //if (employeesId == 2727  /* 553 2047*/)
                        //{

                        //}
                        //if (result1.Where(w=>w.IdEmployee== employeesId).ToList().Count()== EmployeeAttendanceListGroupBy.Where(w => w.IdEmployee == employeesId).ToList().Count())
                        {
                            bool flag = false;
                            List<EmployeeAttendance> EmployeeAttendanceListDateFilter = EmployeeAttendanceListGroupBy.Where(w => w.IdEmployee == employeesId).ToList();
                            // shubham[skadam] GEOS2-3991 Attendance - wrong incomplete day   10 11 2022
                            List<EmployeeLeave> selectedEmployeeLeavesListByIdEmployee = EmployeeLeaves.Where(w => w.IdEmployee == employeesId
                             && w.StartDate.Value.Date >= fromMonthStartDate && w.StartDate.Value.Date <= toMonthEndDate).ToList();
                            foreach (var employeeAttendance in AllDaysLeaveWeeklyOfDays)
                            {
                                if (today.Year == DateTime.Now.Year)
                                {
                                    if (today.Month == DateTime.Now.Month)
                                        if (employeeAttendance == DateTime.Now.Day)
                                        {
                                            idEmployees.Add(employeesId);
                                        }
                                }
                                if (EmployeeAttendanceListDateFilter.Any(a => a.StartDate.Day == employeeAttendance) ||
                                    selectedEmployeeLeavesListByIdEmployee.Any(a => a.StartDate.Value.Day == employeeAttendance)
                                    //  || selectedEmployeeLeavesListByIdEmployee.Any(w => w.StartDate.Value.Day >= employeeAttendance || w.EndDate.Value.Day <= employeeAttendance)
                                    )
                                {
                                    flag = true;
                                }
                                else
                                {
                                    DateTime employeeAttendanceDateTime = new DateTime(today.Year, today.Month, employeeAttendance);
                                    //if (selectedEmployeeLeavesListByIdEmployee.Any(d => employeeAttendance >= d.StartDate.Value.Day && employeeAttendance <= d.EndDate.Value.Day))
                                    if (selectedEmployeeLeavesListByIdEmployee.Any(d => employeeAttendanceDateTime >= d.StartDate && employeeAttendanceDateTime <= d.EndDate))
                                    {
                                        flag = true;
                                    }
                                    else if (CompanyHolidays.Any(d => employeeAttendance >= d.StartDate.Value.Day && employeeAttendance <= d.EndDate.Value.Day && today.Month == d.StartDate.Value.Month))
                                    {
                                        flag = true;
                                        //List<CompanyHoliday> tempCompanyHolidays = CompanyHolidays.Where(d => employeeAttendance >= d.StartDate.Value.Day && employeeAttendance <= d.EndDate.Value.Day &&
                                        //today.Month == d.StartDate.Value.Month).ToList();
                                        //foreach (var itemCompanyHolidays in plantOwners)
                                        //{
                                        //    if (tempCompanyHolidays.Any(a=>a.IdCompany== itemCompanyHolidays.IdCompany))
                                        //    {
                                        //        flag = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        flag = false;
                                        //        break;
                                        //    }
                                        //}

                                    }
                                    else
                                    {
                                        flag = false;
                                        break;
                                    }

                                }
                            }
                            //foreach (var employeeAttendance in EmployeeAttendanceListDateFilter)
                            //{
                            //    if (AllDaysLeaveWeeklyOfDays.Any(a => a == employeeAttendance.StartDate.Day))
                            //    {
                            //        flag = true;
                            //    }
                            //    else
                            //    {
                            //        flag = false;
                            //        break;
                            //    }
                            //}
                            if (flag)
                            {
                                idEmployees.Add(employeesId);
                            }
                        }
                    }
                    idsEmployees.Clear();
                    idsEmployees = idEmployees;


                    EmployeesForFilter.AddRange(Employees.Where(i => idsEmployees.Contains(i.IdEmployee)).ToList());



                    if (EmployeeAttendanceListForFilter.Count == 0)
                    {
                        EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                        EmployeeAttendanceListForFilter = employeeAttendanceList;
                    }

                    //EmployeeAttendanceListForFilter.AddRange(EmployeeAttendanceList.Where(s => s.DailyHours <= s.WorkedHours).ToList());
                    //idsEmployees = EmployeeAttendanceListForFilter.Select(i => i.IdEmployee).Distinct().ToList();
                    //EmployeesForFilter.AddRange(Employees.Where(i => idsEmployees.Contains(i.IdEmployee)).ToList());
                    #endregion
                }
            }

            #region [rdixit][06.01.2022][GEOS2-4055]
            //foreach (Department item in Departments)
            //{
            //    foreach (Employee item1 in item.Employees)
            //    {
            //        if (EmployeesForFilter.Where(i => i.EmployeeCode == item1.EmployeeCode).FirstOrDefault() != null)
            //        {
            //            EmployeesForFilter.Where(i => i.EmployeeCode == item1.EmployeeCode).FirstOrDefault().EmployeeJobDescription = new EmployeeJobDescription();
            //            EmployeesForFilter.Where(i => i.EmployeeCode == item1.EmployeeCode).FirstOrDefault().EmployeeJobDescription = item1.EmployeeJobDescription;
            //            EmployeesForFilter.Where(i => i.EmployeeCode == item1.EmployeeCode).FirstOrDefault().EmployeeMainJDAbbreviation = item1.EmployeeMainJDAbbreviation;
            //        }
            //    }
            //}
            //[GEOS2-5071][rdixit][17.01.2024]
            List<string> DepartmentList = EmployeesForFilter.Where(i => i.LstEmployeeDepartments != null).Select(j => j.LstEmployeeDepartments[0].DepartmentName).Distinct().ToList();
            foreach (var item in DepartmentList)
            {
                Department dept = DepartmentsForFilter.FirstOrDefault(x => x.DepartmentName == item);
                dept.Employees = new List<Employee>();
                dept.Employees.AddRange(EmployeesForFilter.Where(i => i.LstEmployeeDepartments != null && i.LstEmployeeDepartments[0].DepartmentName == item).OrderBy(e => e.FullName));
            }

            #endregion

            MyFilterString = string.Empty;

            if (EmployeeAttendanceViewInstance != null)
            {
                object datacontext = EmployeeAttendanceViewInstance.DataContext;
                if (datacontext != null)
                {
                    EmployeeAttendanceViewInstance.DataContext = null;
                    EmployeeAttendanceViewInstance.DataContext = datacontext;
                }
            }
        }
        private void FilterOptionEditValueChangedCommandAction()
        {
            try
            {
                List<Int32> idsEmployeesIncomplete = new List<int>();
                List<Int32> idsEmployees = new List<int>();
                DateTime? fromDateMonth;
                DateTime? toDateMonth;
                var today = Convert.ToDateTime(EmployeeAttendanceViewInstance.scheduler.Month);
                List<int> AllDaysLeaveWeeklyOfDays = new List<int>();
                Decimal AllDays = DateTime.DaysInMonth(EmployeeAttendanceViewInstance.scheduler.Month.Value.Year, EmployeeAttendanceViewInstance.scheduler.Month.Value.Month);

                for (int day = 1; day <= AllDays; day++)
                {
                    DateTime currentDay = new DateTime(EmployeeAttendanceViewInstance.scheduler.Month.Value.Year, Convert.ToInt32(EmployeeAttendanceViewInstance.scheduler.Month.Value.Month), day);
                    if (currentDay.DayOfWeek != DayOfWeek.Sunday && currentDay.DayOfWeek != DayOfWeek.Saturday)
                    {
                        AllDaysLeaveWeeklyOfDays.Add(day);
                    }
                }
                var monthStart = new DateTime(today.Year, today.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                fromDateMonth = monthStart;
                toDateMonth = monthEnd;
                // shubham[skadam] GEOS2-3991 Attendance - wrong incomplete day   10 11 2022
                DateTime fromMonthStartDate = fromDateMonth.Value.Date.AddMonths(-1);
                DateTime toMonthEndDate = toDateMonth.Value.Date.AddMonths(1);
                string CurrentDay = DateTime.Now.Day.ToString();
                var plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                if (CurrentDay.Equals(AllDaysLeaveWeeklyOfDays.First().ToString()))
                {
                    return;
                }
                EmployeesForFilter = new Hrm.CustomObservableCollection<Employee>();
                DepartmentsForFilter = new CustomObservableCollection<Department>();//[rdixit][GEOS2-4054]
                DepartmentsForFilter.AddRangeWithTemporarySuppressedNotification(Departments.Select(item => (Department)item.Clone()).ToList());
                foreach (var item in DepartmentsForFilter)
                {
                    item.Employees = null;
                }
                if (SelectedFilterAttendanceList.ToString() == "Incomplete day")
                {
                    #region Incompleteday
                    List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date).ToList();
                    var result1 = EmployeeAttendanceListGroupBy2.GroupBy(g => new { g.StartDate.Date, g.IdEmployee })
                   .Select(cl => new EmployeeAttendance
                   {
                       StartDate = cl.First().StartDate,
                       Employee = cl.First().Employee,
                       EndDate = cl.First().EndDate,
                       AccountingDate = cl.First().AccountingDate,
                       DailyHours = cl.First().DailyHours,
                       TotalTime = new TimeSpan(cl.Sum(p => p.TotalTime == null ? 0 : p.TotalTime.Value.Ticks)),
                       WorkedHours = new TimeSpan(cl.Sum(p => p.WorkedHours == null ? 0 : p.WorkedHours.Value.Ticks)),
                       IdEmployeeAttendance = cl.First().IdEmployeeAttendance,
                       IdEmployee = cl.First().IdEmployee,
                       IdCompanyWork = cl.First().IdCompanyWork,
                       CompanyWork = cl.First().CompanyWork,
                       IdCompanyShift = cl.First().IdCompanyShift,
                       CompanyShift = cl.First().CompanyShift,
                       EmployeeCode = cl.First().EmployeeCode,
                       WeekNumber = cl.First().WeekNumber,

                   }).OrderBy(o => o.StartDate).ToList();

                    List<EmployeeAttendance> EmployeeAttendanceListGroupBy = result1.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date && w.DailyHours <= w.WorkedHours).ToList();
                    idsEmployees = EmployeeAttendanceListGroupBy.Select(i => i.IdEmployee).Distinct().ToList();
                    List<Int32> idEmployees = new List<int>();
                    foreach (int employeesId in idsEmployees)
                    {
                        //if (result1.Where(w => w.IdEmployee == employeesId).ToList().Count() == EmployeeAttendanceListGroupBy.Where(w => w.IdEmployee == employeesId).ToList().Count())
                        {
                            bool flag = false;
                            List<EmployeeAttendance> EmployeeAttendanceListDateFilter = EmployeeAttendanceListGroupBy.Where(w => w.IdEmployee == employeesId).ToList();
                            // shubham[skadam] GEOS2-3991 Attendance - wrong incomplete day   10 11 2022
                            List<EmployeeLeave> selectedEmployeeLeavesListByIdEmployee = EmployeeLeaves.Where(w => w.IdEmployee == employeesId &&
                            w.StartDate.Value.Date >= fromMonthStartDate && w.StartDate.Value.Date <= toMonthEndDate).ToList();
                            foreach (var employeeAttendance in AllDaysLeaveWeeklyOfDays)
                            {
                                if (today.Year == DateTime.Now.Year)
                                {
                                    if (today.Month == DateTime.Now.Month)
                                        if (employeeAttendance == DateTime.Now.Day)
                                        {
                                            idEmployees.Add(employeesId);
                                        }
                                }
                                if (EmployeeAttendanceListDateFilter.Any(a => a.StartDate.Day == employeeAttendance) ||
                                    selectedEmployeeLeavesListByIdEmployee.Any(a => a.StartDate.Value.Day == employeeAttendance)
                                    )
                                {
                                    flag = true;
                                }
                                else
                                {
                                    DateTime employeeAttendanceDateTime = new DateTime(today.Year, today.Month, employeeAttendance);
                                    //if (selectedEmployeeLeavesListByIdEmployee.Any(d => employeeAttendance >= d.StartDate.Value.Day && employeeAttendance <= d.EndDate.Value.Day))
                                    if (selectedEmployeeLeavesListByIdEmployee.Any(d => employeeAttendanceDateTime >= d.StartDate && employeeAttendanceDateTime <= d.EndDate))
                                    {
                                        flag = true;
                                    }
                                    else if (CompanyHolidays.Any(d => employeeAttendance >= d.StartDate.Value.Day && employeeAttendance <= d.EndDate.Value.Day && today.Month == d.StartDate.Value.Month))
                                    {
                                        flag = true;
                                    }
                                    else
                                    {
                                        flag = false;
                                        break;
                                    }

                                }
                            }

                            if (flag)
                            {
                                idEmployees.Add(employeesId);
                            }
                        }
                    }
                    idsEmployees.Clear();
                    idsEmployees = idEmployees;


                    EmployeesForFilter.AddRange(Employees.Where(i => !idsEmployees.Contains(i.IdEmployee)).ToList());

                    if (EmployeeAttendanceListForFilter.Count == 0)
                    {
                        EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                        EmployeeAttendanceListForFilter = employeeAttendanceList;
                    }

                    #endregion
                }
                else if (SelectedFilterAttendanceList.ToString() == "Complete day")
                {
                    #region CompleteDay


                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate >= fromDateMonth && w.StartDate <= toDateMonth && w.IdEmployee == 232).ToList();
                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate >= fromDateMonth && w.StartDate <= toDateMonth && w.IdEmployee == 405).ToList();
                    //List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date && w.IdEmployee ==553).ToList();

                    List<EmployeeAttendance> EmployeeAttendanceListGroupBy2 = EmployeeAttendanceList.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date).ToList();
                    var result1 = EmployeeAttendanceListGroupBy2.GroupBy(g => new { g.StartDate.Date, g.IdEmployee })
                   .Select(cl => new EmployeeAttendance
                   {
                       StartDate = cl.First().StartDate,
                       Employee = cl.First().Employee,
                       EndDate = cl.First().EndDate,
                       AccountingDate = cl.First().AccountingDate,
                       DailyHours = cl.First().DailyHours,
                       TotalTime = new TimeSpan(cl.Sum(p => p.TotalTime == null ? 0 : p.TotalTime.Value.Ticks)),
                       WorkedHours = new TimeSpan(cl.Sum(p => p.WorkedHours == null ? 0 : p.WorkedHours.Value.Ticks)),
                       IdEmployeeAttendance = cl.First().IdEmployeeAttendance,
                       IdEmployee = cl.First().IdEmployee,
                       IdCompanyWork = cl.First().IdCompanyWork,
                       CompanyWork = cl.First().CompanyWork,
                       IdCompanyShift = cl.First().IdCompanyShift,
                       CompanyShift = cl.First().CompanyShift,
                       EmployeeCode = cl.First().EmployeeCode,
                       WeekNumber = cl.First().WeekNumber,


                   }).OrderBy(o => o.StartDate).ToList();

                    List<EmployeeAttendance> EmployeeAttendanceListGroupBy = result1.Where(w => w.StartDate.Date >= fromDateMonth.Value.Date && w.StartDate.Date <= toDateMonth.Value.Date && w.DailyHours <= w.WorkedHours).ToList();
                    idsEmployees = EmployeeAttendanceListGroupBy.Select(i => i.IdEmployee).Distinct().ToList();
                    List<Int32> idEmployees = new List<int>();
                    foreach (int employeesId in idsEmployees)
                    {
                        //if (result1.Where(w => w.IdEmployee == employeesId).ToList().Count() == EmployeeAttendanceListGroupBy.Where(w => w.IdEmployee == employeesId).ToList().Count())
                        {
                            bool flag = false;
                            List<EmployeeAttendance> EmployeeAttendanceListDateFilter = EmployeeAttendanceListGroupBy.Where(w => w.IdEmployee == employeesId).ToList();
                            // shubham[skadam] GEOS2-3991 Attendance - wrong incomplete day   10 11 2022
                            List<EmployeeLeave> selectedEmployeeLeavesListByIdEmployee = EmployeeLeaves.Where(w => w.IdEmployee == employeesId
                            && w.StartDate.Value.Date >= fromMonthStartDate && w.StartDate.Value.Date <= toMonthEndDate).ToList();
                            foreach (var employeeAttendance in AllDaysLeaveWeeklyOfDays)
                            {
                                if (today.Year == DateTime.Now.Year)
                                {
                                    if (today.Month == DateTime.Now.Month)
                                        if (employeeAttendance == DateTime.Now.Day)
                                        {
                                            idEmployees.Add(employeesId);
                                        }
                                }
                                if (EmployeeAttendanceListDateFilter.Any(a => a.StartDate.Day == employeeAttendance) ||
                                    selectedEmployeeLeavesListByIdEmployee.Any(a => a.StartDate.Value.Day == employeeAttendance)
                                    )
                                {
                                    flag = true;
                                }
                                else
                                {
                                    DateTime employeeAttendanceDateTime = new DateTime(today.Year, today.Month, employeeAttendance);
                                    //if (selectedEmployeeLeavesListByIdEmployee.Any(d => employeeAttendance >= d.StartDate.Value.Day && employeeAttendance <= d.EndDate.Value.Day))
                                    if (selectedEmployeeLeavesListByIdEmployee.Any(d => employeeAttendanceDateTime >= d.StartDate && employeeAttendanceDateTime <= d.EndDate))
                                    {
                                        flag = true;
                                    }
                                    else if (CompanyHolidays.Any(d => employeeAttendance >= d.StartDate.Value.Day && employeeAttendance <= d.EndDate.Value.Day && today.Month == d.StartDate.Value.Month))
                                    {
                                        flag = true;
                                    }
                                    else
                                    {
                                        flag = false;
                                        break;
                                    }

                                }
                            }
                            if (flag)
                            {
                                idEmployees.Add(employeesId);
                            }
                        }
                    }
                    idsEmployees.Clear();
                    idsEmployees = idEmployees;


                    EmployeesForFilter.AddRange(Employees.Where(i => idsEmployees.Contains(i.IdEmployee)).ToList());

                    if (EmployeeAttendanceListForFilter.Count == 0)
                    {
                        EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                        EmployeeAttendanceListForFilter = employeeAttendanceList;
                    }
                    #endregion
                }
                #region [rdixit][06.01.2022][GEOS2-4055]
                foreach (Department item in Departments)
                {
                    foreach (Employee item1 in item.Employees)
                    {
                        if (EmployeesForFilter.Where(i => i.EmployeeCode == item1.EmployeeCode).FirstOrDefault() != null)
                        {
                            EmployeesForFilter.Where(i => i.EmployeeCode == item1.EmployeeCode).FirstOrDefault().EmployeeJobDescription = new EmployeeJobDescription();
                            EmployeesForFilter.Where(i => i.EmployeeCode == item1.EmployeeCode).FirstOrDefault().EmployeeJobDescription = item1.EmployeeJobDescription;
                            EmployeesForFilter.Where(i => i.EmployeeCode == item1.EmployeeCode).FirstOrDefault().EmployeeMainJDAbbreviation = item1.EmployeeMainJDAbbreviation;
                        }
                    }
                }

                List<uint> DepartmentList = EmployeesForFilter.Where(i => i.EmployeeJobDescription != null).
                    Select(j => j.EmployeeJobDescription.JobDescription.IdDepartment).Distinct().ToList();

                foreach (var item in DepartmentList)
                {
                    Department dept = DepartmentsForFilter.FirstOrDefault(x => x.IdDepartment == item);
                    dept.Employees = new List<Employee>();
                    dept.Employees.AddRange(EmployeesForFilter.Where(i => i.EmployeeJobDescription != null && i.EmployeeJobDescription.JobDescription.IdDepartment == item)
                        .OrderBy(e => e.FullName));
                }
                #endregion

                MyFilterString = string.Empty;

                if (EmployeeAttendanceViewInstance != null)
                {
                    object datacontext = EmployeeAttendanceViewInstance.DataContext;
                    if (datacontext != null)
                    {
                        EmployeeAttendanceViewInstance.DataContext = null;
                        EmployeeAttendanceViewInstance.DataContext = datacontext;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        /// <summary>
        /// [001][SP-65][skale][11-06-2019][GEOS2-1556]Grid data reflection problems
        ///[002][spawar][12-03-2020][GEOS2-36]HRM - Add summatory values of times in attendance.
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerEditValueChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
                IsLoadAttendance = true;
                ShowLoading();
                #region SplitButton [rdixit][04.04.2024][GEOS2-5278]
                List<int> compwise = new List<int>();
                if (GeosAppSettingList1 != null)
                {
                    foreach (var item in GeosAppSettingList1.FirstOrDefault(i => i.IdAppSetting == 119)?.DefaultValue?.Split(';'))
                    {
                        string trimmedItem = item.Trim();
                        string[] parts = trimmedItem.Split(',');
                        compwise.Add(int.Parse(parts[0].Trim('(', ')').Trim()));
                    }
                }
                foreach (var item in HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>())
                {
                    if (compwise.Any(i => i == item.IdCompany))
                        IsSplitButtonEnable = true;
                    else
                        IsSplitButtonEnable = false;
                }
                #endregion
                FillAttendanceListByPlant();
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                if (SelectedItem is Department || SelectedItem == null)
                {
                    HrmCommon.Instance.AttendanceLoadingMessage = Application.Current.Resources["SelectEmployeeError"].ToString();
                }
                GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PlantOwnerEditValueChangedCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

        }
        /// <summary>
        /// [001][SP-65][skale][11-06-2019][GEOS2-1556]Grid data reflection problems
        /// [002][spawar][12-03-2020][GEOS2-36]HRM - Add summatory values of times in attendance.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectedYearChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
                // [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
                ShowLoading();
                IsLoadAttendance = true;
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
               
                HrmCommon.Instance.SelectedPeriod = GeosApplication.Instance.SelectedHRMAttendanceDate.Year;
                var monthStartDate = new DateTime(GeosApplication.Instance.SelectedHRMAttendanceDate.Year, GeosApplication.Instance.SelectedHRMAttendanceDate.Month, 1);
                EmployeeAttendanceViewInstance.scheduler.LimitInterval = new DateTimeRange(new DateTime(GeosApplication.Instance.SelectedHRMAttendanceDate.Year, 1, 1), new DateTime(GeosApplication.Instance.SelectedHRMAttendanceDate.Year, 12, 31));          
                EmployeeAttendanceViewInstance.scheduler.Start = monthStartDate;
                EmployeeAttendanceViewInstance.scheduler.ActiveViewType = DevExpress.Xpf.Scheduling.ViewType.MonthView;
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                EmployeeMonthlyData.ClosePleaseWaitScreen();//[rdixit][GEOS2-8233][01.08.2025]
            }
            catch (Exception ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedYearChangedCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

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
                GeosApplication.Instance.Logger.Log("Method FillAttendanceListByPlant ...", category: Category.Info, priority: Priority.Low);
                HrmCommon.Instance.IsVisibleLabelLoadingFullYearAttendanceInBackground = Visibility.Visible;
                EmployeeMonthlyData.ShowPleaseWaitScreen();

                // From ContactListByPlant
                this.SelectedLeaveRecord = null;

                // Clearing collections
                if (this._employeeListFinalForLeaves != null) this._employeeListFinalForLeaves.ClearWithTemporarySuppressedNotification();
                if (this.Employees != null) this.Employees.ClearWithTemporarySuppressedNotification();
                if (this.EmployeeAttendanceList != null) this.EmployeeAttendanceList.ClearWithTemporarySuppressedNotification();
                if (this.CompanyHolidays != null) this.CompanyHolidays.ClearWithTemporarySuppressedNotification();
                if (this.HolidayList != null) this.HolidayList.ClearWithTemporarySuppressedNotification();
                if (this.EmployeeLeaves != null) this.EmployeeLeaves.ClearWithTemporarySuppressedNotification();
                if (this.LabelItems != null) this.LabelItems.ClearWithTemporarySuppressedNotification();
                if (this.StatusItems != null) this.StatusItems.ClearWithTemporarySuppressedNotification();
                if (this.AppointmentItems != null) this.AppointmentItems.ClearWithTemporarySuppressedNotification();
                if (this.Departments != null) this.Departments.ClearWithTemporarySuppressedNotification();

                StringMonthlyExpectedTotalHoursCount = "00:00";
                StringMonthlyTotalHoursCount = "00:00";

                UpdateschedulerLimitAndSelectedInterval();

                // IsSchedulerViewVisible = Visibility.Visible;
                IsEmployeewiseRegisterAndExpectDays = false;

                // Search clear from ContactListByPlant
                if (EmployeeAttendanceViewInstance != null && EmployeeAttendanceViewInstance.accordionControl != null)
                {
                    EmployeeAttendanceViewInstance.accordionControl.SearchText = null;
                }

                // Search clear from Attendance
                if (EmployeeAttendanceViewInstance != null && IsSchedulerViewVisible == Visibility.Visible)
                {
                    var accordionControl = EmployeeAttendanceViewInstance.accordionControl;
                    var searchControl = LayoutTreeHelper.GetVisualChildren(accordionControl).OfType<SearchControl>().FirstOrDefault();
                    if (searchControl != null)
                        searchControl.SearchText = null;
                }

                MyFilterString = string.Empty;

                // [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
                EmployeeMonthlyData.GetHRMEmployeesDataOnceFromServiceForAttendance_2640(
                    ref HrmService, ref CrmStartUp, ref companyHolidays,
                    ref holidayList, ref employeeLeaves, ref labelItems, ref statusItems,
                    ref appointmentItems, ref fromDate, ref toDate,
                    GridControl1, ref companyShiftsList, ref employees, ref department, ref _employeeListFinalForLeaves);

                #region Rupali Sarode - GEOS2-3751
                EmployeeAttendanceListForFilter = new Hrm.CustomObservableCollection<EmployeeAttendance>();
                EmployeesForFilter = new Hrm.CustomObservableCollection<Employee>();
                EmployeeAttendanceListForFilter = employeeAttendanceList;
                EmployeesForFilter = Employees;
                DepartmentsForFilter = new CustomObservableCollection<Department>();
                DepartmentsForFilter = Departments;//[rdixit][GEOS2-4054][06/01/2023]
                OldEmployeesListForGrid = new CustomObservableCollection<Employee>();
                OldEmployeesListForGrid.AddRange(Employees.Select(x => (Employee)x.Clone()).ToList());
                #endregion
            
                if (EmployeeAttendanceViewInstance != null)
                {
                    object datacontext = EmployeeAttendanceViewInstance.DataContext;
                    if (datacontext != null)
                    {
                        EmployeeAttendanceViewInstance.DataContext = null;
                        EmployeeAttendanceViewInstance.DataContext = datacontext;
                    }
                }
                if (Mouse.OverrideCursor == Cursors.Wait)
                {
                    Mouse.OverrideCursor = null;
                }
                GeosApplication.Instance.Logger.Log("Method FillAttendanceListByPlant executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                if (Mouse.OverrideCursor == Cursors.Wait)//[Sudhir.Jangra][GEOS2-4055][06/01/2023]
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in FillContactListByPlant() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                if (Mouse.OverrideCursor == Cursors.Wait)//[Sudhir.Jangra][GEOS2-4055][06/01/2023]
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in FillContactListByPlant() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                if (Mouse.OverrideCursor == Cursors.Wait)//[Sudhir.Jangra][GEOS2-4055][06/01/2023]
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillContactListByPlant()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UpdateschedulerLimitAndSelectedInterval()
        {
            if (EmployeeAttendanceViewInstance != null && EmployeeAttendanceViewInstance.scheduler != null)
            {
                DateTime? _fromDate;
                DateTime? _toDate;
                FillHrmDataInObjectsByCallingLatestServiceMethods.CalculateFromDateToDateAtSelectedPeriodForFullYear(out _fromDate, out _toDate, null);
                
                EmployeeAttendanceViewInstance.scheduler.LimitInterval = new DevExpress.Mvvm.DateTimeRange(((DateTime)_fromDate).AddMonths(-1),((DateTime)_toDate).AddMonths(1));
                
                FillHrmDataInObjectsByCallingLatestServiceMethods.CalculateFromDateToDateAtSelectedPeriodForOneMonthOnly(out _fromDate, out _toDate, null, null);
                
                EmployeeAttendanceViewInstance.scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange((DateTime)_fromDate, new TimeSpan(1, 0, 0, 0));
                EmployeeAttendanceViewInstance.scheduler.Month = (DateTime)_fromDate;
            }
        }

        private void OpenAttendanceFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenAttendanceFile()...", category: Category.Info, priority: Priority.Low);
                if (SelectedItem == null || SelectedItem is Department)
                {
                    SelectedItem = Departments.Where(i => i.Employees != null).FirstOrDefault().Employees.FirstOrDefault();
                }
                if (SelectedItem is Employee)
                {
                    Employee employee = (Employee)SelectedItem;

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
                        //if (Departments != null && Departments.Count > 0)//[Sudhir.Jangra][GEOS2-4055][03/01/2023]
                        //{
                        //    foreach (Department department in Departments)
                        //    {
                        //        if (department.Employees != null && department.Employees.Any(x => x.IdEmployee == employee.IdEmployee))
                        //        {
                        //            department.Employees.RemoveAll(x => x.IdEmployee == employee.IdEmployee);
                        //        }
                        //    }
                        //}
                    }

                    EmployeeMonthlyData.ClosePleaseWaitScreen();
                }
                GeosApplication.Instance.Logger.Log("Method OpenAttendanceFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenAttendanceFile()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>       
        ///[001]Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri    
        ///[002][skale][07-08-2019][GEOS2-1694]HRM - Attendance green visualization
        ///[003][spawar][12-03-2020][GEOS2-36]HRM - Add summatory values of times in attendance.
        ///[004][smazhar][26-08-2020][GEOS2-2553]If we add a leave with an “All day Event” in the Attendance section, it is represented with 2 days.
        /// </summary>
        /// <param name="obj"></param>
        public void SelectItemForScheduler(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectItemForScheduler()...", category: Category.Info, priority: Priority.Low);

                if (AppointmentItems.Count != 0)
                    AppointmentItems.Clear();

                if (this.EmployeeAttendanceViewInstance != null)
                {
                    SchedulerControlEx schedulerControlEx = this.EmployeeAttendanceViewInstance.scheduler; //  (SchedulerControlEx)values[0];
                    ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();

                    if (SelectedItem == null)
                    {
                        EmployeeMonthlyData.ShowPleaseWaitScreen();
                        if (AppointmentItems != null) AppointmentItems.Clear();
                        schedulerControlEx.SelectedEmployeeHireDate = null;
                        schedulerControlEx.SelectedEmployeeEndDate = null;
                        if (schedulerControlEx.SelectedEmployeeContractSituationList != null)
                        {
                            schedulerControlEx.SelectedEmployeeContractSituationList.Clear();
                        }
                        else
                        {
                            schedulerControlEx.SelectedEmployeeContractSituationList = new List<EmployeeContractSituation>();
                        }
                        schedulerControlEx.SelectedEmployeeWorkingDays = new string[0];
                        EmployeeMonthlyData.ClosePleaseWaitScreen();
                        EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                    }
                    else if (SelectedItem is Employee)
                    {
                        HrmCommon.Instance.AttendanceLoadingMessage = string.Empty;
                        ShowLoading();
                        schedulerControlEx.Tag = "OnSelection";
                        schedulerControlEx.SelectedEmployeeHireDate = ((Employee)SelectedItem).EmployeeContractSituation.ContractSituationEndDate;
                        LoadEmployeeMonthlyData(((Employee)SelectedItem), schedulerControlEx);
                        // schedulerControlEx.SelectedEmployeeEndDate = ((Employee)SelectedItem).EmployeeContractSituation.ContractSituationEndDate;
                        try
                        {
                            if (((Employee)SelectedItem).EmployeeContractSituations.Count() == 0)
                            {
                                //Employee EmployeeContractSituationnew = Employees.Where(w => w.IdEmployee == ((Employee)SelectedItem).IdEmployee).FirstOrDefault();
                                EmployeeAttendance EmployeeContractSituationFromAttendanceList = EmployeeAttendanceList.Where(w => w.IdEmployee == ((Employee)SelectedItem).IdEmployee).FirstOrDefault();
                                Employee EmployeeContractSituation = OldEmployeesListForGrid.Where(w => w.IdEmployee == ((Employee)SelectedItem).IdEmployee).FirstOrDefault();
                                ((Employee)SelectedItem).EmployeeContractSituations = EmployeeContractSituation.EmployeeContractSituations;
                                if (((Employee)SelectedItem).EmployeeContractSituations.Count() == 0)
                                {
                                    ((Employee)SelectedItem).EmployeeContractSituations = EmployeeContractSituationFromAttendanceList.Employee.EmployeeContractSituations;
                                }
                                if (((Employee)SelectedItem).EmployeeContractSituations.Count() == 0)
                                {
                                    EmployeeContractSituationFromAttendanceList = EmployeeAttendanceList.Where(w => w.IdEmployee == ((Employee)SelectedItem).IdEmployee).Where(x => x.Employee.EmployeeContractSituations.Count != 0).FirstOrDefault();
                                    ((Employee)SelectedItem).EmployeeContractSituations = EmployeeContractSituationFromAttendanceList.Employee.EmployeeContractSituations;
                                }
                            }
                            schedulerControlEx.SelectedEmployeeEndDate = (((Employee)SelectedItem)).EmployeeContractSituations.FirstOrDefault(x => x.ContractSituationEndDate == null) != null ?
                                                                GeosApplication.Instance.ServerDateTime.Date :
                                                                ((Employee)SelectedItem).EmployeeContractSituations.OrderByDescending(x => x.ContractSituationEndDate).ToList()
                                                                [0].ContractSituationEndDate;
                        }
                        catch (Exception ex)
                        {
                        }

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

                        ShowAttendanceAppointmentsForSelectedEmployee();
                        ShowLeaveAppointmentsForSelectedEmployee();

                        //[003] Added
                        IsEmployeewiseRegisterAndExpectDays = true;
                        MonthlyAllRegisterHours(schedulerControlEx);

                        _hasShownHolidayAppointments = true;
                        _year = GeosApplication.Instance.SelectedHRMAttendanceDate.Year;
                        ShowCompanyHolidayAppointmentsForSelectedCompany();
                        ConfigureAppointmentLabelsSourceUsingGlobalEmployeeLeaveList();
                        EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                        var tempattendanceList = EmployeeAttendanceList.Where(w => w.IdEmployee == ((Employee)SelectedItem).IdEmployee).ToList();
                        foreach (var temp in tempattendanceList)
                        {
                            EmployeeAttendanceListForFilter.Add(temp);
                        }
                    }
                    else if (SelectedItem is Department)
                    {
                        EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                        HrmCommon.Instance.AttendanceLoadingMessage = Application.Current.Resources["SelectEmployeeError"].ToString();
                    }
                    
                }
                //EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Method SelectItemForScheduler()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectItemForScheduler()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        
        //[rdixit][GEOS2-8233][01.08.2025]
        public void SelectItemForScheduler1(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectItemForScheduler()...", category: Category.Info, priority: Priority.Low);

                if (AppointmentItems.Count != 0)
                    AppointmentItems.Clear();

                if (this.EmployeeAttendanceViewInstance != null)
                {
                    SchedulerControlEx schedulerControlEx = this.EmployeeAttendanceViewInstance.scheduler; //  (SchedulerControlEx)values[0];
                    ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();

                    if (SelectedItem == null)
                    {
                        EmployeeMonthlyData.ShowPleaseWaitScreen();
                        if (AppointmentItems != null) AppointmentItems.Clear();
                        schedulerControlEx.SelectedEmployeeHireDate = null;
                        schedulerControlEx.SelectedEmployeeEndDate = null;
                        if (schedulerControlEx.SelectedEmployeeContractSituationList != null)
                        {
                            schedulerControlEx.SelectedEmployeeContractSituationList.Clear();
                        }
                        else
                        {
                            schedulerControlEx.SelectedEmployeeContractSituationList = new List<EmployeeContractSituation>();
                        }
                        schedulerControlEx.SelectedEmployeeWorkingDays = new string[0];
                        EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                    }
                    else if (SelectedItem is Employee)
                    {
                        HrmCommon.Instance.AttendanceLoadingMessage = string.Empty;
                        ShowLoading();
                        schedulerControlEx.Tag = "OnSelection";
                        schedulerControlEx.SelectedEmployeeHireDate = ((Employee)SelectedItem).EmployeeContractSituation.ContractSituationEndDate;
                        LoadEmployeeMonthlyData(((Employee)SelectedItem), schedulerControlEx);
                        try
                        {
                            if (((Employee)SelectedItem).EmployeeContractSituations.Count() == 0)
                            {
                                EmployeeAttendance EmployeeContractSituationFromAttendanceList = EmployeeAttendanceList.Where(w => w.IdEmployee == ((Employee)SelectedItem).IdEmployee).FirstOrDefault();
                                Employee EmployeeContractSituation = OldEmployeesListForGrid.Where(w => w.IdEmployee == ((Employee)SelectedItem).IdEmployee).FirstOrDefault();
                                ((Employee)SelectedItem).EmployeeContractSituations = EmployeeContractSituation.EmployeeContractSituations;
                                if (((Employee)SelectedItem).EmployeeContractSituations.Count() == 0)
                                {
                                    ((Employee)SelectedItem).EmployeeContractSituations = EmployeeContractSituationFromAttendanceList.Employee.EmployeeContractSituations;
                                }
                                if (((Employee)SelectedItem).EmployeeContractSituations.Count() == 0)
                                {
                                    EmployeeContractSituationFromAttendanceList = EmployeeAttendanceList.Where(w => w.IdEmployee == ((Employee)SelectedItem).IdEmployee).Where(x => x.Employee.EmployeeContractSituations.Count != 0).FirstOrDefault();
                                    ((Employee)SelectedItem).EmployeeContractSituations = EmployeeContractSituationFromAttendanceList.Employee.EmployeeContractSituations;
                                }
                            }
                            schedulerControlEx.SelectedEmployeeEndDate = (((Employee)SelectedItem)).EmployeeContractSituations.FirstOrDefault(x => x.ContractSituationEndDate == null) != null ?
                                                                GeosApplication.Instance.ServerDateTime.Date :
                                                                ((Employee)SelectedItem).EmployeeContractSituations.OrderByDescending(x => x.ContractSituationEndDate).ToList()
                                                                [0].ContractSituationEndDate;
                        }
                        catch (Exception ex)
                        {
                        }

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

                        ShowAttendanceAppointmentsForSelectedEmployee();
                        ShowLeaveAppointmentsForSelectedEmployee();

                        //[003] Added
                        IsEmployeewiseRegisterAndExpectDays = true;
                        MonthlyAllRegisterHours(schedulerControlEx);

                        _hasShownHolidayAppointments = true;
                        _year = GeosApplication.Instance.SelectedHRMAttendanceDate.Year;
                        ShowCompanyHolidayAppointmentsForSelectedCompany();
                        ConfigureAppointmentLabelsSourceUsingGlobalEmployeeLeaveList();
                        EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                        var tempattendanceList = EmployeeAttendanceList.Where(w => w.IdEmployee == ((Employee)SelectedItem).IdEmployee).ToList();
                        foreach (var temp in tempattendanceList)
                        {
                            EmployeeAttendanceListForFilter.Add(temp);
                        }
                    }
                    else if (SelectedItem is Department)
                    {
                        EmployeeAttendanceListForFilter = new CustomObservableCollection<EmployeeAttendance>();
                        HrmCommon.Instance.AttendanceLoadingMessage = Application.Current.Resources["SelectEmployeeError"].ToString();
                    }

                }              
                GeosApplication.Instance.Logger.Log("Method SelectItemForScheduler()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectItemForScheduler()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ConfigureAppointmentLabelsSourceUsingGlobalEmployeeLeaveList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ConfigureAppointmentLabelsSourceUsingGlobalEmployeeLeaveList()...", category: Category.Info, priority: Priority.Low);

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

                GeosApplication.Instance.Logger.Log("Method ConfigureAppointmentLabelsSourceUsingGlobalEmployeeLeaveList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                // IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method ConfigureAppointmentLabelsSourceUsingGlobalEmployeeLeaveList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowCompanyHolidayAppointmentsForSelectedCompany()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowCompanyHolidayAppointmentsForSelectedCompany()...", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log("Method ShowCompanyHolidayAppointmentsForSelectedCompany()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                // IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowCompanyHolidayAppointmentsForSelectedCompany()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowAttendanceAppointmentsForSelectedEmployee()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAttendanceAppointmentsForSelectedEmployee()...", category: Category.Info, priority: Priority.Low);

                List<EmployeeAttendance> AttendanceList = EmployeeAttendanceList.Where(x => x.IdEmployee == ((Employee)SelectedItem).IdEmployee).ToList();
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
                    // [nsatpute][03-10-2024][GEOS2-6451]
                    if (EmpAttendance.IsMobileApiAttendance)
                    {
                        modelAppointment.Description = modelAppointment.Description + " [Mobile]";
                        modelAppointment.IsMobileApiAttendance = true;
                    }
                    else
                    {
                        modelAppointment.IsMobileApiAttendance = false;
                    }

                    modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == EmpAttendance.IdCompanyWork).Value);
                    //else
                    //    modelAppointment.Subject = string.Format("{0} [{1}]", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == EmpAttendanceList.IdCompanyWork).Value ,"Night Shift");
                    modelAppointment.StartDate = EmpAttendance.StartDate;
                    modelAppointment.EndDate = EmpAttendance.EndDate;
                    modelAppointment.Label = EmpAttendance.IdCompanyWork;
                    modelAppointment.IdEmployeeAttendance = EmpAttendance.IdEmployeeAttendance;
                    modelAppointment.AttendanceStatus = EmpAttendance.AttendanceStatus;
                    modelAppointment.DailyHoursCount = 0;


                    #region Rupali Sarode - GEOS2-3751

                    //if (EmpAttendance.CompanyShift != null)
                    //{
                    //    if (EmpAttendance.CompanyShift.CompanyAnnualSchedule != null)
                    //        modelAppointment.DailyHoursCount = EmpAttendance.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                    //}
                    //else if (EmpAttendance.Employee.CompanyShift != null)
                    //{
                    //    if (EmpAttendance.Employee.CompanyShift.CompanyAnnualSchedule != null)
                    //        modelAppointment.DailyHoursCount = EmpAttendance.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                    //}
                    //else
                    //    modelAppointment.DailyHoursCount = 0;

                    if (EmpAttendance.CompanyShift != null)
                    {
                        Tuple<double, TimeSpan, bool> GetDailyHrBreakTimeIsDeduct;
                        GetDailyHrBreakTimeIsDeduct = GetShiftStartTimeForShift((int)EmpAttendance.StartDate.DayOfWeek, EmpAttendance.CompanyShift);
                        modelAppointment.DailyHoursCount = Convert.ToDecimal(GetDailyHrBreakTimeIsDeduct.Item1);
                        modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeduct.Item3;
                        modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeduct.Item2;
                    }
                    else
                    {
                        modelAppointment.DailyHoursCount = 0;
                        modelAppointment.IsDeductBreakTime = false;
                        modelAppointment.EmployeeShiftBreakTime = TimeSpan.Zero;
                    }

                    #endregion

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
                    Tuple<TimeSpan, TimeSpan, bool> GetDailyHrBreakTimeIsDeductShiftWorkingTime;
                    GetDailyHrBreakTimeIsDeductShiftWorkingTime = GetShiftTime(modelAppointment, EmpAttendance.CompanyShift);//[002]
                    modelAppointment.ShiftWorkingTime = GetDailyHrBreakTimeIsDeductShiftWorkingTime.Item1;
                    modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeductShiftWorkingTime.Item3;
                    modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeductShiftWorkingTime.Item2;

                  //  modelAppointment.ShiftWorkingTime = GetShiftTime(modelAppointment, EmpAttendance.CompanyShift);//[002]
                    AppointmentItems.Add(modelAppointment);
                }
                GeosApplication.Instance.Logger.Log("Method ShowAttendanceAppointmentsForSelectedEmployee()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                // IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowAttendanceAppointmentsForSelectedEmployee()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void ShowLeaveAppointmentsForSelectedEmployee()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowLeaveAppointmentsForSelectedEmployee()...", category: Category.Info, priority: Priority.Low);

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
                CreateAppointmentForLeaveAndAddInCollection(tempLeaveList);
                GeosApplication.Instance.Logger.Log("Method ShowLeaveAppointmentsForSelectedEmployee()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                // IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowLeaveAppointmentsForSelectedEmployee()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void CreateAppointmentForLeaveAndAddInCollection(List<EmployeeLeave> tempLeaveList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateAppointmentForLeaveAndAddInCollection()...", category: Category.Info, priority: Priority.Low);

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

                    var IdCompanyShiftofLeave = 0;

                    if (EmployeeLeave.IdCompanyShift != 0)
                    {
                        IdCompanyShiftofLeave = EmployeeLeave.IdCompanyShift;
                    }
                    else if (EmployeeLeave.Employee != null && EmployeeLeave.Employee.IdCompanyShift != 0)
                    {
                        IdCompanyShiftofLeave = EmployeeLeave.Employee.IdCompanyShift;
                    }

                    if (IdCompanyShiftofLeave != 0)
                    {
                        CompanyShift CompanyShift = HrmService.GetCompanyShiftDetailByIdCompanyShift(IdCompanyShiftofLeave);

                        if (CompanyShift != null)
                        {
                         
                            Tuple<TimeSpan, TimeSpan, bool> GetDailyHrBreakTimeIsDeduct;
                            GetDailyHrBreakTimeIsDeduct = GetShiftTime(modelAppointment, EmployeeLeave.CompanyShift);//[002]
                            modelAppointment.ShiftWorkingTime = GetDailyHrBreakTimeIsDeduct.Item1;
                            modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeduct.Item3;
                            modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeduct.Item2;
                            if (CompanyShift.CompanyAnnualSchedule != null)
                            {
                                modelAppointment.DailyHoursCount = CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                            }

                            else
                                modelAppointment.DailyHoursCount = 0;
                        }
                        else
                            modelAppointment.DailyHoursCount = 0;
                    }
                    modelAppointment.ShiftWorkingTime = GetShiftTimeForLeave(modelAppointment, EmployeeLeave);
                    AppointmentItems.Add(modelAppointment);

                }
                GeosApplication.Instance.Logger.Log("Method CreateAppointmentForLeaveAndAddInCollection()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                // IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method CreateAppointmentForLeaveAndAddInCollection()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                // [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
                if (SelectedItem is Employee)
                {
                    EmployeeMonthlyData.ShowPleaseWaitScreen();
                    IsLoadAttendance = true;
                    SelectItemForScheduler1(obj); //[rdixit][GEOS2-8233][01.08.2025]
                    OnPropertyChanged(new PropertyChangedEventArgs("AppointmentItems"));
                    EmployeeMonthlyData.ClosePleaseWaitScreen();
                }
                else if (SelectedItem is Department || SelectedItem == null)
                {
                    EmployeeMonthlyData.ClosePleaseWaitScreen();
                    HrmCommon.Instance.AttendanceLoadingMessage = Application.Current.Resources["SelectEmployeeError"].ToString();
                }
                IsEmployeewiseRegisterAndExpectDays = false;
                Mouse.OverrideCursor = null;
                GeosApplication.Instance.Logger.Log("Method RefreshAttendanceView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshAttendanceView()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                isGridViewVisible = Visibility.Collapsed;
                IsSchedulerViewVisible = Visibility.Visible;
                AttendanceFilterAttendanceIsEnabled = true;
                AttendanceFilterAttendanceVisible = Visibility.Visible;
                AttendanceFilterAttendanceWidth = 50;
                // IsGridViewVisible = Visibility.Hidden;

                GeosApplication.Instance.Logger.Log("Method ShowAttendanceSchedulerView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowAttendanceSchedulerView()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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

                GridControl1 = (GridControl)obj;
                GridControl1.RefreshData();
                AttendanceFilterAttendanceVisible = Visibility.Hidden;
                AttendanceFilterAttendanceVisible = Visibility.Hidden;
                AttendanceFilterAttendanceWidth = 0;
                IsGridViewVisible = Visibility.Visible;
                AttendanceFilterAttendanceIsEnabled = false;

                GeosApplication.Instance.Logger.Log("Method ShowAttendanceGridView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowAttendanceGridView()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                if (EmployeeAttendanceListForFilter == null || EmployeeAttendanceListForFilter?.Count == 0)
                {
                    CustomMessageBox.Show(Application.Current.FindResource("SelectEmployeeError").ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    return;
                }
                EmployeeAttendanceViewInstance.dxgGridControl1.RefreshData();

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
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttendancetList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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

            try
            {
                GeosApplication.Instance.Logger.Log("Method Options_CustomizeCell()...", category: Category.Info, priority: Priority.Low);
                if (e.ColumnFieldName == "AccountingDate")
                {
                    if (e.Value != null && e.Value.ToString() != "Accounting Date" && e.ColumnFieldName == "AccountingDate")
                    {
                        e.Value = string.Format("{0:dd/MM/yyyy}", (DateTime)e.Value);

                    }
                }


                e.Handled = true;
                GeosApplication.Instance.Logger.Log("Method Options_CustomizeCell()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Options_CustomizeCell()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

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
            // shubham[skadam]  GEOS2-3414 HRM - Wrong Visualization of leave in calendar - Attendance Section 11 OCT 2022
            if (NewEmpLeave.IsAllDayEvent == 1)
            {
                if (NewEmpLeave.EndDate.Value.Hour == 0 && NewEmpLeave.EndDate.Value.Minute == 0 && NewEmpLeave.EndDate.Value.Second == 0)
                {
                    NewEmpLeave.EndDate = new DateTime(NewEmpLeave.EndDate.Value.Year, NewEmpLeave.EndDate.Value.Month, NewEmpLeave.EndDate.Value.Day, 12, 00, 00);
                }

            }
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
                    tempLeave.CompanyShift = NewEmpLeave.CompanyShift;
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
                        tempLeave.CompanyShift = NewEmpLeave.CompanyShift;
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
                if(EmployeeAttendanceListForFilter == null || EmployeeAttendanceListForFilter?.Count == 0)
                {
                    CustomMessageBox.Show(Application.Current.FindResource("SelectEmployeeError").ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    return;
                }
                EmployeeAttendanceViewInstance.dxgGridControl1.RefreshData();

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
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintAttendanceList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Sprint 42---HRM	M042-06	Add and Edit Attendance----sdesai
        /// Method to add new attendance
        ///[001]print46 HRM Take values from lookup values instead of the existing tables by Mayuri     
        ///[002][SP-65][skale][11-06-2019][GEOS2-1556]Grid data reflection problems
        ///[003][spawar][12-03-2020][GEOS2-36]HRM - Add summatory values of times in attendance.
        ///[004][cpatil][29-09-2020][GEOS2-2113]HRM - Break time in Attendance (Grid View) (#IES16).
		///[005][nsatpute][25-12-2024][GEOS2-6636] HRM - Need support for partial attendance (2 of 5)
        /// </summary>
        /// <param name="obj"></param>

        public void AddAttendance(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddAttendance()...", category: Category.Info, priority: Priority.Low);
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
                AttendanceViewModel addAttendanceViewModel = new AttendanceViewModel(addAttendanceView);
                //EventHandler handle = delegate { addAttendanceView.Close(); };
                //addAttendanceViewModel.RequestClose += handle;
                //addAttendanceView.DataContext = addAttendanceViewModel;
                //addAttendanceViewModel.IsSplitVisible = false;f
                var workingPlantId = CurrentGeosProvider.IdCompany.ToString();

                //addAttendanceViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                object selectedEmployee = SelectedItem;
                if (IsGridViewVisible == Visibility.Visible)
                    selectedEmployee = null;
                addAttendanceViewModel.Company = CurrentGeosProvider.Company;
                //addAttendanceViewModel.SelectedPlantList = PlantOwners;
                addAttendanceViewModel.Init(null, selectedEmployee,
                    SelectedStartDate, SelectedEndDate, workingPlantId, null, Employees);
                //addAttendanceViewModel.IsNew = true;
                //addAttendanceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewAttendance").ToString();
                //addAttendanceViewModel.EmployeeLeaves = EmployeeLeaves;
                var ownerInfo = (detailView as FrameworkElement);
                addAttendanceView.Owner = Window.GetWindow(ownerInfo);
                addAttendanceView.ShowDialog();
                if (addAttendanceViewModel.IsSave == true)
                {
                    if (addAttendanceViewModel.IsAcceptButton)
                    {
                        IsEmployeewiseRegisterAndExpectDays = true;
                    }

                    foreach (var item in addAttendanceViewModel.NewEmployeeAttendanceList)
                    {
                        TimeSpan timeSpan = new TimeSpan();
                        if (item.EndDate == DateTime.MinValue)
                            item.EndDate = item.StartDate.AddSeconds(1);
                        timeSpan = item.EndDate - item.StartDate;

                        //if (SelectedItem != null)
                        //{
                        //    var SelectedEmployeeLeave = EmployeeLeaves.Where(x => x.IdEmployee == item.IdEmployee).ToList();
                        //    List<EmployeeLeave> NewLeaveOfSelectedEmployeeAsPerDate = new List<EmployeeLeave>();
                        //    foreach (EmployeeLeave employeeLeave in SelectedEmployeeLeave)
                        //    {
                        //        for (var day = employeeLeave.StartDate.Value.Date; day.Date <= employeeLeave.EndDate.Value.Date; day = day.AddDays(1))
                        //        {
                        //            EmployeeLeave e = new EmployeeLeave();
                        //            e = (EmployeeLeave)employeeLeave.Clone();
                        //            e.StartDate = day;
                        //            e.EndDate = day;
                        //            NewLeaveOfSelectedEmployeeAsPerDate.Add(e);
                        //        }
                        //    }

                        //    UI.Helper.Appointment modelAppointment = CreateAppointmentObjUsingInputs(addAttendanceViewModel, item, NewLeaveOfSelectedEmployeeAsPerDate);
                        //    appointmentItems.Add(modelAppointment);
                        //}

                        item.StartTime = addAttendanceViewModel.STime;
                        item.EndTime = addAttendanceViewModel.ETime;
                        item.Employee.TotalWorkedHours = timeSpan.ToString(@"hh\:mm");
                        item.TotalTime = timeSpan;
                        item.CompanyShift = item.Employee.CompanyShift;
                        item.AttendanceStatus = item.AttendanceStatus;
                        
                        //
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

                    //AppointmentItems.AddRange(appointmentItems);
                    CreateAndAddAllAppointmentsInCollection(addAttendanceViewModel);
                    //#region 5336

                    //var values = (object[])obj;
                    //FillAttendanceListByPlant();

                    //Mouse.OverrideCursor = null;
                    //SelectedAttendanceRecord = addAttendanceViewModel.NewEmployeeAttendance;

                    //int indexOfSelectedDepartment = -1;
                    //Employee selected = (Employee)selectedEmployee;
                    //for (int i = 0; i < DepartmentsForFilter.Count; i++)
                    //{
                    //    if (DepartmentsForFilter[i].Employees.Any(employee => employee.IdEmployee.Equals(selected.IdEmployee)))
                    //    {
                    //        indexOfSelectedDepartment = i;

                    //    }
                    //}
                    //if (indexOfSelectedDepartment != -1)
                    //{
                    //    SelectedItem = DepartmentsForFilter[indexOfSelectedDepartment].Employees.FirstOrDefault(x => x.IdEmployee == (int)selected.IdEmployee);
                    //}


                    ////SelectedItem= SelectedAttendanceRecord.Employee;
                    //#endregion

                }


                GeosApplication.Instance.Logger.Log("Method AddAttendance()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddAttendance()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }



        private void CreateAndAddAllAppointmentsInCollection(AttendanceViewModel addAttendanceViewModel)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateAndAddAllAppointmentsInCollection()...", category: Category.Info, priority: Priority.Low);

                if (addAttendanceViewModel.IsSave == true)
                {
                    ObservableCollection<UI.Helper.Appointment> appointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                    foreach (var item in addAttendanceViewModel.NewEmployeeAttendanceList)
                    {
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

                            UI.Helper.Appointment modelAppointment = CreateAppointmentObjUsingInputs(addAttendanceViewModel, item, NewLeaveOfSelectedEmployeeAsPerDate);
                        
                            Tuple<TimeSpan, TimeSpan, bool> GetDailyHrBreakTimeIsDeduct;
                            GetDailyHrBreakTimeIsDeduct = GetShiftTime(modelAppointment, item.CompanyShift);//[002]
                            modelAppointment.ShiftWorkingTime = GetDailyHrBreakTimeIsDeduct.Item1;
                            modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeduct.Item3;
                            modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeduct.Item2;
                            modelAppointment.AttendanceStatus = item.AttendanceStatus;
                            appointmentItems.Add(modelAppointment);
                        }
                    }
                    AppointmentItems.AddRange(appointmentItems);
                }
                GeosApplication.Instance.Logger.Log("Method CreateAndAddAllAppointmentsInCollection()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CreateAndAddAllAppointmentsInCollection()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private UI.Helper.Appointment CreateAppointmentObjUsingInputs(AttendanceViewModel addAttendanceViewModel, EmployeeAttendance item, List<EmployeeLeave> NewLeaveOfSelectedEmployeeAsPerDate)
        {

            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateAppointmentObjUsingInputs()...", category: Category.Info, priority: Priority.Low);

                modelAppointment.Label = item.IdCompanyWork;
                //[001] Changes in Subject as per Lookup Value
                //  modelAppointment.Subject = string.Format("{0}", item.CompanyWork.Name);
                //modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList[GeosApplication.Instance.AttendanceTypeList.FindIndex(x => x.IdLookupValue == item.IdCompanyWork)].Value);
                modelAppointment.Subject = string.Format("{0}", GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == item.IdCompanyWork).Value);
                modelAppointment.StartDate = item.StartDate;
                modelAppointment.EndDate = item.EndDate;
                modelAppointment.IdEmployeeAttendance = item.IdEmployeeAttendance;

                modelAppointment.DailyHoursCount = 0;

                #region Rupali Sarode - GEOS2-3751

                //if (addAttendanceViewModel.CompanyShiftDetails != null)
                //{
                //    if (addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule != null)
                //    {
                //        modelAppointment.DailyHoursCount = addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule.DailyHoursCount;
                //    }
                //}
                //else
                //    modelAppointment.DailyHoursCount = 0;

                if (item.CompanyShift != null)
                {
                    Tuple<double, TimeSpan, bool> GetDailyHrBreakTimeIsDeduct;
                    GetDailyHrBreakTimeIsDeduct = GetShiftStartTimeForShift((int)item.StartDate.DayOfWeek, item.CompanyShift);
                    modelAppointment.DailyHoursCount = Convert.ToDecimal(GetDailyHrBreakTimeIsDeduct.Item1);
                    modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeduct.Item3;
                    modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeduct.Item2;
                }
                else
                {
                    modelAppointment.DailyHoursCount = 0;
                    modelAppointment.IsDeductBreakTime = false;
                    modelAppointment.EmployeeShiftBreakTime = TimeSpan.Zero;
                }
                   
                #endregion

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
                // [nsatpute][03-10-2024][GEOS2-6451]
                if (item.IsMobileApiAttendance)
                {
                    modelAppointment.Description = modelAppointment.Description + " [Mobile]";
                    modelAppointment.IsMobileApiAttendance = true;
                }
                else
                {
                    modelAppointment.IsMobileApiAttendance = false;
                }
                modelAppointment.IdEmployee = item.IdEmployee;
                modelAppointment.AccountingDate = item.AccountingDate;
                modelAppointment.AttendanceStatus = item.AttendanceStatus;
                GeosApplication.Instance.Logger.Log("Method CreateAppointmentObjUsingInputs()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CreateAppointmentObjUsingInputs()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            return modelAppointment;
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
                        modelAppointment.AccountingDate = item.AccountingDate;
                        modelAppointment.AttendanceStatus =GeosApplication.Instance.AttendanceStatusList.Where(i=>i.IdLookupValue== item.IdStatus).FirstOrDefault();
                        modelAppointment.DailyHoursCount = 0;
                      //  modelAppointment.AttendanceStatus = item.Status;
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
                         
                            Tuple<TimeSpan, TimeSpan, bool> GetDailyHrBreakTimeIsDeduct;
                            GetDailyHrBreakTimeIsDeduct = GetShiftTime(modelAppointment, item.CompanyShift);//[002]
                            modelAppointment.ShiftWorkingTime = GetDailyHrBreakTimeIsDeduct.Item1;
                            modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeduct.Item3;
                            modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeduct.Item2;
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
                GeosApplication.Instance.Logger.Log("Get an error in Method EmployeeAttendanceToAppointment()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method SelectedIntervalCommandAction()...", category: Category.Info, priority: Priority.Low);
                SchedulerControl scheduler = e.Source as SchedulerControl;
                SelectedStartDate = scheduler.SelectedInterval.Start;
                SelectedEndDate = scheduler.SelectedInterval.End;

                if (scheduler.ActiveView.Caption == "Month View")
                    SelectedEndDate = SelectedEndDate.AddDays(-1);
                
                //[rdixit][GEOS2-8233][01.08.2025]
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Method SelectedIntervalCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIntervalCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                    AttendanceViewModel addAttendanceViewModel = new AttendanceViewModel(addAttendanceView);
                    //EventHandler handle = delegate { addAttendanceView.Close(); };
                    //addAttendanceViewModel.RequestClose += handle;
                    //addAttendanceView.DataContext = addAttendanceViewModel;

                    //[001] CompanyWork IdCompany  Code comment and changes as per Job description IdCompany
                    // addAttendanceViewModel.WorkingPlantId = employeeAttendance.CompanyWork.IdCompany.ToString();
                    //addAttendanceViewModel.WorkingPlantId = employeeAttendance.Employee.EmployeeJobDescription.IdCompany.ToString();

                    var workingPlantId = employeeAttendance.Employee.EmployeeCompanyIds.Split(',')[0];
                    //addAttendanceViewModel.WorkingPlantId = idEmployeeCompanyIdsSplit[0];

                    //addAttendanceViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                    //addAttendanceViewModel.SelectedPlantList = plantOwners;

                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addAttendanceViewModel.InitReadOnly(employeeAttendance, workingPlantId);
                    else
                        addAttendanceViewModel.EditInit(employeeAttendance,
                            EmployeeAttendanceList, Employees, EmployeeLeaves, workingPlantId);

                    //addAttendanceViewModel.IsNew = false;
                    //addAttendanceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditAttendance").ToString();
                    EmployeeJobDescription empJobDesc = new EmployeeJobDescription();

                    if (employeeAttendance.Employee.EmployeeJobDescription != null)
                    {
                        empJobDesc = employeeAttendance.Employee.EmployeeJobDescription;
                    }

                    //addAttendanceViewModel.EmployeeLeaves = EmployeeLeaves;
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
                            modelAppointment.AttendanceStatus = addAttendanceViewModel.UpdateEmployeeAttendance.AttendanceStatus;
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
                        
                            Tuple<TimeSpan, TimeSpan, bool> GetDailyHrBreakTimeIsDeduct;
                            GetDailyHrBreakTimeIsDeduct = GetShiftTime(modelAppointment, addAttendanceViewModel.UpdateEmployeeAttendance.CompanyShift);//[002]
                            modelAppointment.ShiftWorkingTime = GetDailyHrBreakTimeIsDeduct.Item1;
                            modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeduct.Item3;
                            modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeduct.Item2;

                            AppointmentItems.Add(modelAppointment);

                            addAttendanceViewModel.UpdateEmployeeAttendance.Employee.EmployeeJobDescription = empJobDesc;
                            employeeAttendance.Employee = addAttendanceViewModel.UpdateEmployeeAttendance.Employee;
                            employeeAttendance.IdEmployee = addAttendanceViewModel.UpdateEmployeeAttendance.IdEmployee;
                            employeeAttendance.StartDate = addAttendanceViewModel.UpdateEmployeeAttendance.StartDate;
                            employeeAttendance.StartTime = addAttendanceViewModel.StartTime.Value.TimeOfDay;
                            employeeAttendance.EndDate = addAttendanceViewModel.UpdateEmployeeAttendance.EndDate;
                            employeeAttendance.EndTime = addAttendanceViewModel.EndTime.Value.TimeOfDay;
                           
                            //[Sudhir.Jangra][GEOS2-4019][08/08/2023]
                            employeeAttendance.FileName = addAttendanceViewModel.UpdateEmployeeAttendance.FileName;
                            employeeAttendance.Remark = addAttendanceViewModel.UpdateEmployeeAttendance.Remark;
                           employeeAttendance.Location = employeeAttendance.Location;
                           
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
                                    modelAppointment.AttendanceStatus = item.AttendanceStatus;
                                    #region Rupali Sarode - GEOS2-3751

                                    //if (addAttendanceViewModel.CompanyShiftDetails != null)
                                    //{
                                    //    if (addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule != null)
                                    //    {
                                    //        modelAppointment.DailyHoursCount = addAttendanceViewModel.CompanyShiftDetails.CompanyAnnualSchedule.DailyHoursCount;
                                    //    }
                                    //}
                                    //else
                                    //    modelAppointment.DailyHoursCount = 0;


                                    if (item.CompanyShift != null)
                                    {
                                        Tuple<double, TimeSpan, bool> GetDailyHrBreakTimeIsDeductCompanyShiftDailyHoursCount;
                                        GetDailyHrBreakTimeIsDeductCompanyShiftDailyHoursCount = GetShiftStartTimeForShift((int)item.StartDate.DayOfWeek, item.CompanyShift);
                                        modelAppointment.DailyHoursCount = Convert.ToDecimal(GetDailyHrBreakTimeIsDeductCompanyShiftDailyHoursCount.Item1);
                                        modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeductCompanyShiftDailyHoursCount.Item3;
                                        modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeductCompanyShiftDailyHoursCount.Item2;
                                    }
                                    else
                                    {
                                        modelAppointment.DailyHoursCount = 0;
                                        modelAppointment.IsDeductBreakTime = false;
                                        modelAppointment.EmployeeShiftBreakTime = TimeSpan.Zero;
                                    }
                                    #endregion

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

                                    Tuple<TimeSpan, TimeSpan, bool> GetDailyHrBreakTimeIsDeduct;
                                    GetDailyHrBreakTimeIsDeduct = GetShiftTime(modelAppointment, addAttendanceViewModel.SelectedEmployeeShift.CompanyShift);//[002]
                                    modelAppointment.ShiftWorkingTime = GetDailyHrBreakTimeIsDeduct.Item1;
                                    modelAppointment.IsDeductBreakTime = GetDailyHrBreakTimeIsDeduct.Item3;
                                    modelAppointment.EmployeeShiftBreakTime = GetDailyHrBreakTimeIsDeduct.Item2;

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


                            //#region 5336
                            //var temp = SelectedAttendanceRecord;
                            //var values = (object[])obj;
                            //FillAttendanceListByPlant();


                            //IsEmployeewiseRegisterAndExpectDays = false;

                            //Mouse.OverrideCursor = null;
                            //SelectedAttendanceRecord = temp;

                            //#endregion
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditAttendanceInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditAttendanceInformation()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeProfile()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method FillEmployeeWorkType()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeWorkType()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// Function Created to Fill CompanyWork from Lookup values
        /// </summary>
        public CompanyWork GetCompanyWork(LookupValue obj)
        {
            CompanyWork companywork = new CompanyWork();
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetCompanyWork()...", category: Category.Info, priority: Priority.Low);
                // companywork = new CompanyWork();
                //CompanyWork companyWork = new CompanyWork
                //{
                //    IdCompanyWork = obj.IdLookupValue,
                //    Name = obj.Value,
                //    HtmlColor = obj.HtmlColor
                //};

                //return companyWork;

                companywork.IdCompanyWork = obj.IdLookupValue;
                companywork.Name = obj.Value;
                companywork.HtmlColor = obj.HtmlColor;

                GeosApplication.Instance.Logger.Log("Method GetCompanyWork()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetCompanyWork()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            return companywork;
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

                                            if (EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(schedule.SelectedAppointments[0].CustomFields["IdEmployeeAttendance"])).IdCompanyWork == 305)
                                            {
                                                EmployeeChangelog log = new EmployeeChangelog();
                                                TimeSpan timediff = EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(schedule.SelectedAppointments[0].CustomFields["IdEmployeeAttendance"])).EndDate - EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(schedule.SelectedAppointments[0].CustomFields["IdEmployeeAttendance"])).StartDate;
                                                int currentDays = 0;
                                                double currentHours = 0;
                                                double dayshours = 0;
                                                if (timediff != null)
                                                {
                                                    dayshours = timediff.TotalHours;
                                                    int days = (int)(dayshours / 8);
                                                    double remainingHours = dayshours % 8;
                                                    if (days > 0)
                                                    {
                                                        currentDays = days;
                                                    }
                                                    else
                                                    {
                                                        currentDays = 0;
                                                    }
                                                    if (remainingHours > 0)
                                                    {
                                                        currentHours = remainingHours;
                                                    }
                                                    else
                                                    {
                                                        currentHours = 0;
                                                    }

                                                }
                                                ExistingHours = new EmployeeAnnualAdditionalLeave();
                                                ExistingHours = HrmService.GetEmployeeAnnualCompensationAttendance_V2500(EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(schedule.SelectedAppointments[0].CustomFields["IdEmployeeAttendance"])).IdEmployee,HrmCommon.Instance.SelectedPeriod);
                                               
                                                if (dayshours == (double)ExistingHours.ConvertedHours)
                                                {
                                                  bool IsDeleted=  HrmService.DeleteEmployeeLeaveForAttendance_V2500(ExistingHours.IdEmployeeAnnualLeave);
                                                }
                                                else
                                                {
                                                    double total = -dayshours;
                                                    EmployeeAnnualLeave temp = new EmployeeAnnualLeave();
                                                    temp.IdEmployee = EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(schedule.SelectedAppointments[0].CustomFields["IdEmployeeAttendance"])).IdEmployee;
                                                    temp.Year = (Int32)HrmCommon.Instance.SelectedPeriod;
                                                    temp.IdLeave = 241;
                                                    temp.RegularHoursCount = 0;
                                                    temp.BacklogHoursCount = 0;


                                                    bool isAdd = HrmService.AddEmployeeAttendanceLeave_V2500(temp, total);
                                                }

                                                log.IdEmployee = EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(schedule.SelectedAppointments[0].CustomFields["IdEmployeeAttendance"])).IdEmployee;
                                                log.ChangeLogDatetime = GeosApplication.Instance.ServerDateTime;
                                                log.ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                                log.ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeCompansationAttendanceDeleteChangeLog").ToString(), currentDays, currentHours);
                                                bool ischanged = HrmService.AddEmployeeChangelogs_V2500(log);
                                            }
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
                            if (EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(appointment.CustomFields["IdEmployeeAttendance"])).IdCompanyWork == 305)
                            {
                                EmployeeChangelog log = new EmployeeChangelog();
                                TimeSpan timediff = EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(appointment.CustomFields["IdEmployeeAttendance"])).EndDate - EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(appointment.CustomFields["IdEmployeeAttendance"])).StartDate;
                                int currentDays = 0;
                                double currentHours = 0;
                                double dayshours = 0;
                                if (timediff != null)
                                {
                                    dayshours = timediff.TotalHours;
                                    int days = (int)(dayshours / 8);
                                    double remainingHours = dayshours % 8;
                                    if (days > 0)
                                    {
                                        currentDays = days;
                                    }
                                    else
                                    {
                                        currentDays = 0;
                                    }
                                    if (remainingHours > 0)
                                    {
                                        currentHours = remainingHours;
                                    }
                                    else
                                    {
                                        currentHours = 0;
                                    }

                                }

                                ExistingHours = new EmployeeAnnualAdditionalLeave();
                                ExistingHours = HrmService.GetEmployeeAnnualCompensationAttendance_V2500(EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(appointment.CustomFields["IdEmployeeAttendance"])).IdEmployee, HrmCommon.Instance.SelectedPeriod);
                              
                                if (dayshours == (double)ExistingHours.ConvertedHours)
                                {
                                    bool IsDeleted = HrmService.DeleteEmployeeLeaveForAttendance_V2500(ExistingHours.IdEmployeeAnnualLeave);
                                }
                                else
                                {
                                    double total = -dayshours;
                                    EmployeeAnnualLeave temp = new EmployeeAnnualLeave();
                                    temp.IdEmployee = EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(appointment.CustomFields["IdEmployeeAttendance"])).IdEmployee;
                                    temp.Year = (Int32)HrmCommon.Instance.SelectedPeriod;
                                    temp.IdLeave = 241;
                                    temp.RegularHoursCount = 0;
                                    temp.BacklogHoursCount = 0;


                                    bool isAdd = HrmService.AddEmployeeAttendanceLeave_V2500(temp, total);
                                }
                                log.IdEmployee = EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(appointment.CustomFields["IdEmployeeAttendance"])).IdEmployee;
                                log.ChangeLogDatetime = GeosApplication.Instance.ServerDateTime;
                                log.ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                log.ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeCompansationAttendanceDeleteChangeLog").ToString(), currentDays, currentHours);
                                bool ischanged = HrmService.AddEmployeeChangelogs_V2500(log);
                            }
                            EmployeeAttendanceList.Remove(EmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == Convert.ToInt64(appointment.CustomFields["IdEmployeeAttendance"])));
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                        else
                        {
                            ((AppointmentItemCancelEventArgs)e).Cancel = true;
                        }
                    }
                    //chitra.girigosavi GEOS2-7807 30/04/2025
                    else if (appointment.CustomFields["IdEmployeeLeave"] != null)
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteEmpLeaveMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            var selected_leave = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == (ulong)appointment.CustomFields["IdEmployeeLeave"]);
                            if (selected_leave?.FileName != null)
                            {
                                MessageBoxResult ConformMessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteConformEmpLeaveMessage"].ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo, MessageBoxResult.No);

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
                    else
                    {
                        ((AppointmentItemCancelEventArgs)e).Cancel = true;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteAppointment()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in DeleteAppointment() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in DeleteAppointment() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteAppointment()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in Method DefaultLoadCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeLeaveType()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public static void FillAttendanceStatus()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAttendanceStatus()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.AttendanceStatusList == null)
                {
                    ICrmService myCrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                    GeosApplication.Instance.AttendanceStatusList = new ObservableCollection<LookupValue>(myCrmStartUp.GetLookupValues(97));
                   
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendanceStatus() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendanceStatus() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillAttendanceStatus()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }
        private void CustomRowFilter(RowFilterEventArgs e)
        {
            try
            {
                if (IsGridViewVisible != Visibility.Visible || string.IsNullOrEmpty(e.Source.FilterString))
                {
                    return;
                }

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

                GeosApplication.Instance.Logger.Log("Method CustomRowFilter() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomRowFilter() Method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        private TimeSpan GetBreakTime(int dayOfWeek, CompanyShift selectedCompanyShift)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetBreakTime()...", category: Category.Info, priority: Priority.Low);


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
                GeosApplication.Instance.Logger.Log("Method GetBreakTime()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                return new TimeSpan();
                GeosApplication.Instance.Logger.Log("Get an error in Method GetBreakTime()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method MonthlyAllRegisterHours()...", category: Category.Info, priority: Priority.Low);
                schedulerControlEx.Month = GeosApplication.Instance.SelectedHRMAttendanceDate;
                MonthlyRegisterTotalHoursCount = 0;
                MonthlyExpectedTotalHoursCount = 0;
                StringMonthlyExpectedTotalHoursCount = "00:00";
                StringMonthlyTotalHoursCount = "00:00";
                StringRegisterHoursColour = string.Empty;
                if (SelectedItem != null && SelectedItem is Employee)
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
                            string[] splitValue = MonthlyExpectedTotalHoursCount.ToString(CultureInfo.InvariantCulture).Split('.');

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

                GeosApplication.Instance.Logger.Log("Method MonthlyAllRegisterHours()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in MonthlyAllRegisterHours() Method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        //public static void SetIsManual(ObservableCollection<EmployeeAttendance> EmployeeAttendance)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method SetIsManual()...", category: Category.Info, priority: Priority.Low);

        //        ObservableCollection<UI.Helper.Appointment> appointment = new ObservableCollection<UI.Helper.Appointment>();
        //        foreach (EmployeeAttendance employeeAttendance in EmployeeAttendance)
        //        {
        //            UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
        //            if (employeeAttendance.IsManual == 1)
        //            {
        //                modelAppointment.AttendanceIsManual = true;
        //            }
        //            else
        //            {
        //                modelAppointment.AttendanceIsManual = false;
        //            }
        //            appointment.Add(modelAppointment);
        //        }
        //        GeosApplication.Instance.Logger.Log("Method SetIsManual()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch(Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method SetIsManual()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
        //    }

        //}
        //private TimeSpan GetShiftTime(UI.Helper.Appointment selectedItem, CompanyShift SelectedEmployeeShift)
        //{

        //    try
        //    {
        //        int dayOfWeek = 0;
        //        if (selectedItem.AccountingDate != null)
        //            dayOfWeek = (int)selectedItem.AccountingDate.Value.DayOfWeek;
        //        else
        //            dayOfWeek = (int)selectedItem.StartDate.Value.DayOfWeek;

        //        bool isDeduct = true;
        //        //[cpatil][GEOS2-5640][27-05-2024]
        //        try
        //        {
        //            if (lstCompanies.Any(i => i == SelectedEmployeeShift.CompanySchedule.Company.IdCompany))
        //            {
        //                isDeduct = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            isDeduct = true;
        //        }

        //        GeosApplication.Instance.Logger.Log("Method GetShiftTime()...", category: Category.Info, priority: Priority.Low);
        //        //[cpatil][GEOS2-5640][27-05-2024]
        //        switch (dayOfWeek)
        //        {
        //            case 0:

        //                DateTime? SunStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.SunStartTime.Hours, SelectedEmployeeShift.SunStartTime.Minutes, SelectedEmployeeShift.SunStartTime.Seconds);
        //                DateTime? SunEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.SunEndTime.Hours, SelectedEmployeeShift.SunEndTime.Minutes, SelectedEmployeeShift.SunEndTime.Seconds);

        //                TimeSpan SunBreakTime = SelectedEmployeeShift.SunBreakTime;
        //                if (isDeduct)
        //                    return (((SunEndTime - SunStartTime).Value.Duration()) - SunBreakTime);
        //                else
        //                    return (((SunEndTime - SunStartTime).Value.Duration()));

        //            case 1:
        //                DateTime? MonStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.MonStartTime.Hours, SelectedEmployeeShift.MonStartTime.Minutes, SelectedEmployeeShift.MonStartTime.Seconds);
        //                DateTime? MonEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.MonEndTime.Hours, SelectedEmployeeShift.MonEndTime.Minutes, SelectedEmployeeShift.MonEndTime.Seconds);
        //                TimeSpan MonBreakTime = SelectedEmployeeShift.MonBreakTime;
        //                if (isDeduct)
        //                    return (((MonEndTime - MonStartTime).Value.Duration()) - MonBreakTime);
        //                else
        //                    return (((MonEndTime - MonStartTime).Value.Duration()));

        //            case 2:
        //                DateTime? TueStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.TueStartTime.Hours, SelectedEmployeeShift.TueStartTime.Minutes, SelectedEmployeeShift.TueStartTime.Seconds);
        //                DateTime? TueEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.TueEndTime.Hours, SelectedEmployeeShift.TueEndTime.Minutes, SelectedEmployeeShift.TueEndTime.Seconds);
        //                TimeSpan TueBreakTime = SelectedEmployeeShift.TueBreakTime;
        //                if (isDeduct)
        //                    return (((TueEndTime - TueStartTime).Value.Duration()) - TueBreakTime);
        //                else
        //                    return (((TueEndTime - TueStartTime).Value.Duration()));

        //            case 3:
        //                DateTime? WedStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.WedStartTime.Hours, SelectedEmployeeShift.WedStartTime.Minutes, SelectedEmployeeShift.WedStartTime.Seconds);
        //                DateTime? WedEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.WedEndTime.Hours, SelectedEmployeeShift.WedEndTime.Minutes, SelectedEmployeeShift.WedEndTime.Seconds);
        //                TimeSpan WedBreakTime = SelectedEmployeeShift.WedBreakTime;
        //                if (isDeduct)
        //                    return (((WedEndTime - WedStartTime).Value.Duration()) - WedBreakTime);
        //                else
        //                    return (((WedEndTime - WedStartTime).Value.Duration()));

        //            case 4:
        //                DateTime? ThuStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.ThuStartTime.Hours, SelectedEmployeeShift.ThuStartTime.Minutes, SelectedEmployeeShift.ThuStartTime.Seconds);
        //                DateTime? ThuEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.ThuEndTime.Hours, SelectedEmployeeShift.ThuEndTime.Minutes, SelectedEmployeeShift.ThuEndTime.Seconds);
        //                TimeSpan ThuBreakTime = SelectedEmployeeShift.ThuBreakTime;
        //                if (isDeduct)
        //                    return (((ThuEndTime - ThuStartTime).Value.Duration()) - ThuBreakTime);
        //                else
        //                    return (((ThuEndTime - ThuStartTime).Value.Duration()));

        //            case 5:
        //                DateTime? FriStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.FriStartTime.Hours, SelectedEmployeeShift.FriStartTime.Minutes, SelectedEmployeeShift.FriStartTime.Seconds);
        //                DateTime? FriEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.FriEndTime.Hours, SelectedEmployeeShift.FriEndTime.Minutes, SelectedEmployeeShift.FriEndTime.Seconds);
        //                TimeSpan FriBreakTime = SelectedEmployeeShift.FriBreakTime;
        //                if (isDeduct)
        //                    return (((FriEndTime - FriStartTime).Value.Duration()) - FriBreakTime);
        //                else
        //                    return (((FriEndTime - FriStartTime).Value.Duration()));

        //            case 6:
        //                DateTime? SatStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.SatStartTime.Hours, SelectedEmployeeShift.SatStartTime.Minutes, SelectedEmployeeShift.SatStartTime.Seconds);
        //                DateTime? SatEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.SatEndTime.Hours, SelectedEmployeeShift.SatEndTime.Minutes, SelectedEmployeeShift.SatEndTime.Seconds);
        //                TimeSpan SatBreakTime = SelectedEmployeeShift.SatBreakTime;
        //                if (isDeduct)
        //                    return (((SatEndTime - SatStartTime).Value.Duration()) - SatBreakTime);
        //                else
        //                    return (((SatEndTime - SatStartTime).Value.Duration()));

        //            default:
        //                return (new TimeSpan(08, 00, 00));
        //        }
        //        GeosApplication.Instance.Logger.Log("Method GetShiftTime()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method GetShiftTime()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
        //        return new TimeSpan();

        //    }
        //}
        //[002]   
        private Tuple<TimeSpan, TimeSpan, bool> GetShiftTime(UI.Helper.Appointment selectedItem, CompanyShift SelectedEmployeeShift)
        {

            try
            {
                int dayOfWeek = 0;
                TimeSpan EmployeeBreakTime = TimeSpan.Zero;
                bool isDeduct = false;

                if (selectedItem.AccountingDate != null)
                    dayOfWeek = (int)selectedItem.AccountingDate.Value.DayOfWeek;
                else
                    dayOfWeek = (int)selectedItem.StartDate.Value.DayOfWeek;

                try
                {
                    if (SelectedEmployeeShift != null)
                    {
                        //[cpatil][GEOS2-5640][27-05-2024]
                        if (lstCompanies.Any(i => i == SelectedEmployeeShift.CompanySchedule.Company.IdCompany))
                        {
                            isDeduct = false;
                        }
                        else
                            isDeduct = true;
                    }
                }
                catch (Exception ex)
                {
                    isDeduct = true;
                }

                GeosApplication.Instance.Logger.Log("Method GetShiftTime()...", category: Category.Info, priority: Priority.Low);
                //[cpatil][GEOS2-5640][27-05-2024]
                if (SelectedEmployeeShift != null)
                {
                    switch (dayOfWeek)
                    {
                        case 0:

                            DateTime? SunStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.SunStartTime.Hours, SelectedEmployeeShift.SunStartTime.Minutes, SelectedEmployeeShift.SunStartTime.Seconds);
                            DateTime? SunEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.SunEndTime.Hours, SelectedEmployeeShift.SunEndTime.Minutes, SelectedEmployeeShift.SunEndTime.Seconds);

                            TimeSpan SunBreakTime = SelectedEmployeeShift.SunBreakTime;
                            return new Tuple<TimeSpan, TimeSpan, bool>((((SunEndTime - SunStartTime).Value.Duration()) - SunBreakTime), SelectedEmployeeShift.SunBreakTime, isDeduct);

                        case 1:
                            DateTime? MonStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.MonStartTime.Hours, SelectedEmployeeShift.MonStartTime.Minutes, SelectedEmployeeShift.MonStartTime.Seconds);
                            DateTime? MonEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.MonEndTime.Hours, SelectedEmployeeShift.MonEndTime.Minutes, SelectedEmployeeShift.MonEndTime.Seconds);
                            TimeSpan MonBreakTime = SelectedEmployeeShift.MonBreakTime;
                            return new Tuple<TimeSpan, TimeSpan, bool>((((MonEndTime - MonStartTime).Value.Duration()) - MonBreakTime), SelectedEmployeeShift.MonBreakTime, isDeduct);

                        case 2:
                            DateTime? TueStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.TueStartTime.Hours, SelectedEmployeeShift.TueStartTime.Minutes, SelectedEmployeeShift.TueStartTime.Seconds);
                            DateTime? TueEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.TueEndTime.Hours, SelectedEmployeeShift.TueEndTime.Minutes, SelectedEmployeeShift.TueEndTime.Seconds);
                            TimeSpan TueBreakTime = SelectedEmployeeShift.TueBreakTime;
                            return new Tuple<TimeSpan, TimeSpan, bool>((((TueEndTime - TueStartTime).Value.Duration()) - TueBreakTime), SelectedEmployeeShift.TueBreakTime, isDeduct);

                        case 3:
                            DateTime? WedStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.WedStartTime.Hours, SelectedEmployeeShift.WedStartTime.Minutes, SelectedEmployeeShift.WedStartTime.Seconds);
                            DateTime? WedEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.WedEndTime.Hours, SelectedEmployeeShift.WedEndTime.Minutes, SelectedEmployeeShift.WedEndTime.Seconds);
                            TimeSpan WedBreakTime = SelectedEmployeeShift.WedBreakTime;
                            return new Tuple<TimeSpan, TimeSpan, bool>((((WedEndTime - WedStartTime).Value.Duration()) - WedBreakTime), SelectedEmployeeShift.WedBreakTime, isDeduct);

                        case 4:
                            DateTime? ThuStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.ThuStartTime.Hours, SelectedEmployeeShift.ThuStartTime.Minutes, SelectedEmployeeShift.ThuStartTime.Seconds);
                            DateTime? ThuEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.ThuEndTime.Hours, SelectedEmployeeShift.ThuEndTime.Minutes, SelectedEmployeeShift.ThuEndTime.Seconds);
                            TimeSpan ThuBreakTime = SelectedEmployeeShift.ThuBreakTime;
                            return new Tuple<TimeSpan, TimeSpan, bool>((((ThuEndTime - ThuStartTime).Value.Duration()) - ThuBreakTime), SelectedEmployeeShift.ThuBreakTime, isDeduct);

                        case 5:
                            DateTime? FriStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.FriStartTime.Hours, SelectedEmployeeShift.FriStartTime.Minutes, SelectedEmployeeShift.FriStartTime.Seconds);
                            DateTime? FriEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.FriEndTime.Hours, SelectedEmployeeShift.FriEndTime.Minutes, SelectedEmployeeShift.FriEndTime.Seconds);
                            TimeSpan FriBreakTime = SelectedEmployeeShift.FriBreakTime;
                            return new Tuple<TimeSpan, TimeSpan, bool>((((FriEndTime - FriStartTime).Value.Duration()) - FriBreakTime), SelectedEmployeeShift.FriBreakTime, isDeduct);

                        case 6:
                            DateTime? SatStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployeeShift.SatStartTime.Hours, SelectedEmployeeShift.SatStartTime.Minutes, SelectedEmployeeShift.SatStartTime.Seconds);
                            DateTime? SatEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployeeShift.SatEndTime.Hours, SelectedEmployeeShift.SatEndTime.Minutes, SelectedEmployeeShift.SatEndTime.Seconds);
                            TimeSpan SatBreakTime = SelectedEmployeeShift.SatBreakTime;
                            return new Tuple<TimeSpan, TimeSpan, bool>((((SatEndTime - SatStartTime).Value.Duration()) - SatBreakTime), SelectedEmployeeShift.SatBreakTime, isDeduct);

                        default:
                            return new Tuple<TimeSpan, TimeSpan, bool>((new TimeSpan(08, 00, 00)), (new TimeSpan(00, 30, 00)), isDeduct); ;
                    }
                }
                return new Tuple<TimeSpan, TimeSpan, bool>((new TimeSpan(00, 00, 00)), (new TimeSpan(00, 00, 00)), false);
                GeosApplication.Instance.Logger.Log("Method GetShiftTime()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetShiftTime()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                return new Tuple<TimeSpan, TimeSpan, bool>((new TimeSpan(00, 00, 00)), (new TimeSpan(00, 00, 00)), false);

            }
        }
        //private TimeSpan GetShiftTimeForLeave(UI.Helper.Appointment selectedItem, EmployeeLeave SelectedEmployee)
        //{

        //    try
        //    {
        //        int dayOfWeek = 0;
        //        dayOfWeek = (int)SelectedEmployee.StartDate.Value.DayOfWeek;
        //        bool isDeduct = true;
        //        //[cpatil][GEOS2-5640][27-05-2024]
        //        try
        //        {
        //            if (lstCompanies.Any(i => i == SelectedEmployee.CompanyShift.CompanySchedule.Company.IdCompany))
        //            {
        //                isDeduct = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            isDeduct = true;
        //        }
        //        GeosApplication.Instance.Logger.Log("Method GetShiftTime()...", category: Category.Info, priority: Priority.Low);
        //        //[cpatil][GEOS2-5640][27-05-2024]
        //        switch (dayOfWeek)
        //        {
        //            case 0:
        //                DateTime? SunStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.SunStartTime.Hours, SelectedEmployee.CompanyShift.SunStartTime.Minutes, SelectedEmployee.CompanyShift.SunStartTime.Seconds);
        //                DateTime? SunEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.SunEndTime.Hours, SelectedEmployee.CompanyShift.SunEndTime.Minutes, SelectedEmployee.CompanyShift.SunEndTime.Seconds);
        //                TimeSpan SunBreakTime = SelectedEmployee.CompanyShift.SunBreakTime;
        //                if (isDeduct)
        //                    return (((SunEndTime - SunStartTime).Value.Duration()) - SunBreakTime);
        //                else
        //                    return (((SunEndTime - SunStartTime).Value.Duration()));

        //            case 1:
        //                DateTime? MonStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.MonStartTime.Hours, SelectedEmployee.CompanyShift.MonStartTime.Minutes, SelectedEmployee.CompanyShift.MonStartTime.Seconds);
        //                DateTime? MonEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.MonEndTime.Hours, SelectedEmployee.CompanyShift.MonEndTime.Minutes, SelectedEmployee.CompanyShift.MonEndTime.Seconds);
        //                TimeSpan MonBreakTime = SelectedEmployee.CompanyShift.MonBreakTime;
        //                if (isDeduct)
        //                    return (((MonEndTime - MonStartTime).Value.Duration()) - MonBreakTime);
        //                else
        //                    return (((MonEndTime - MonStartTime).Value.Duration()));

        //            case 2:
        //                DateTime? TueStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.TueStartTime.Hours, SelectedEmployee.CompanyShift.TueStartTime.Minutes, SelectedEmployee.CompanyShift.TueStartTime.Seconds);
        //                DateTime? TueEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.TueEndTime.Hours, SelectedEmployee.CompanyShift.TueEndTime.Minutes, SelectedEmployee.CompanyShift.TueEndTime.Seconds);
        //                TimeSpan TueBreakTime = SelectedEmployee.CompanyShift.TueBreakTime;
        //                if (isDeduct)
        //                    return (((TueEndTime - TueStartTime).Value.Duration()) - TueBreakTime);
        //                else
        //                    return (((TueEndTime - TueStartTime).Value.Duration()));

        //            case 3:
        //                DateTime? WedStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.WedStartTime.Hours, SelectedEmployee.CompanyShift.WedStartTime.Minutes, SelectedEmployee.CompanyShift.WedStartTime.Seconds);
        //                DateTime? WedEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.WedEndTime.Hours, SelectedEmployee.CompanyShift.WedEndTime.Minutes, SelectedEmployee.CompanyShift.WedEndTime.Seconds);
        //                TimeSpan WedBreakTime = SelectedEmployee.CompanyShift.WedBreakTime;
        //                if (isDeduct)
        //                    return (((WedEndTime - WedStartTime).Value.Duration()) - WedBreakTime);
        //                else
        //                    return (((WedEndTime - WedStartTime).Value.Duration()));

        //            case 4:
        //                DateTime? ThuStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.ThuStartTime.Hours, SelectedEmployee.CompanyShift.ThuStartTime.Minutes, SelectedEmployee.CompanyShift.ThuStartTime.Seconds);
        //                DateTime? ThuEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.ThuEndTime.Hours, SelectedEmployee.CompanyShift.ThuEndTime.Minutes, SelectedEmployee.CompanyShift.ThuEndTime.Seconds);
        //                TimeSpan ThuBreakTime = SelectedEmployee.CompanyShift.ThuBreakTime;
        //                if (isDeduct)
        //                    return (((ThuEndTime - ThuStartTime).Value.Duration()) - ThuBreakTime);
        //                else
        //                    return (((ThuEndTime - ThuStartTime).Value.Duration()));

        //            case 5:
        //                DateTime? FriStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.FriStartTime.Hours, SelectedEmployee.CompanyShift.FriStartTime.Minutes, SelectedEmployee.CompanyShift.FriStartTime.Seconds);
        //                DateTime? FriEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.FriEndTime.Hours, SelectedEmployee.CompanyShift.FriEndTime.Minutes, SelectedEmployee.CompanyShift.FriEndTime.Seconds);
        //                TimeSpan FriBreakTime = SelectedEmployee.CompanyShift.FriBreakTime;
        //                if (isDeduct)
        //                    return (((FriEndTime - FriStartTime).Value.Duration()) - FriBreakTime);
        //                else
        //                    return (((FriEndTime - FriStartTime).Value.Duration()));

        //            case 6:
        //                DateTime? SatStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.SatStartTime.Hours, SelectedEmployee.CompanyShift.SatStartTime.Minutes, SelectedEmployee.CompanyShift.SatStartTime.Seconds);
        //                DateTime? SatEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.SatEndTime.Hours, SelectedEmployee.CompanyShift.SatEndTime.Minutes, SelectedEmployee.CompanyShift.SatEndTime.Seconds);
        //                TimeSpan SatBreakTime = SelectedEmployee.CompanyShift.SatBreakTime;
        //                if (isDeduct)
        //                    return (((SatEndTime - SatStartTime).Value.Duration()) - SatBreakTime);
        //                else
        //                    return (((SatEndTime - SatStartTime).Value.Duration()));

        //            default:
        //                return (new TimeSpan(08, 00, 00));
        //        }
        //        GeosApplication.Instance.Logger.Log("Method GetShiftTime()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method GetShiftTime()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
        //        return new TimeSpan();

        //    }
        //}
        private TimeSpan GetShiftTimeForLeave(UI.Helper.Appointment selectedItem, EmployeeLeave SelectedEmployee)
        {

            try
            {
                int dayOfWeek = 0;
                dayOfWeek = (int)SelectedEmployee.StartDate.Value.DayOfWeek;
              
                GeosApplication.Instance.Logger.Log("Method GetShiftTime()...", category: Category.Info, priority: Priority.Low);
                //[cpatil][GEOS2-5640][27-05-2024]
                switch (dayOfWeek)
                {
                    case 0:
                        DateTime? SunStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.SunStartTime.Hours, SelectedEmployee.CompanyShift.SunStartTime.Minutes, SelectedEmployee.CompanyShift.SunStartTime.Seconds);
                        DateTime? SunEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.SunEndTime.Hours, SelectedEmployee.CompanyShift.SunEndTime.Minutes, SelectedEmployee.CompanyShift.SunEndTime.Seconds);
                        TimeSpan SunBreakTime = SelectedEmployee.CompanyShift.SunBreakTime;
                        return (((SunEndTime - SunStartTime).Value.Duration()) - SunBreakTime);

                    case 1:
                        DateTime? MonStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.MonStartTime.Hours, SelectedEmployee.CompanyShift.MonStartTime.Minutes, SelectedEmployee.CompanyShift.MonStartTime.Seconds);
                        DateTime? MonEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.MonEndTime.Hours, SelectedEmployee.CompanyShift.MonEndTime.Minutes, SelectedEmployee.CompanyShift.MonEndTime.Seconds);
                        TimeSpan MonBreakTime = SelectedEmployee.CompanyShift.MonBreakTime;
                        return (((MonEndTime - MonStartTime).Value.Duration()) - MonBreakTime);

                    case 2:
                        DateTime? TueStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.TueStartTime.Hours, SelectedEmployee.CompanyShift.TueStartTime.Minutes, SelectedEmployee.CompanyShift.TueStartTime.Seconds);
                        DateTime? TueEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.TueEndTime.Hours, SelectedEmployee.CompanyShift.TueEndTime.Minutes, SelectedEmployee.CompanyShift.TueEndTime.Seconds);
                        TimeSpan TueBreakTime = SelectedEmployee.CompanyShift.TueBreakTime;
                        return (((TueEndTime - TueStartTime).Value.Duration()) - TueBreakTime);

                    case 3:
                        DateTime? WedStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.WedStartTime.Hours, SelectedEmployee.CompanyShift.WedStartTime.Minutes, SelectedEmployee.CompanyShift.WedStartTime.Seconds);
                        DateTime? WedEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.WedEndTime.Hours, SelectedEmployee.CompanyShift.WedEndTime.Minutes, SelectedEmployee.CompanyShift.WedEndTime.Seconds);
                        TimeSpan WedBreakTime = SelectedEmployee.CompanyShift.WedBreakTime;
                        return (((WedEndTime - WedStartTime).Value.Duration()) - WedBreakTime);

                    case 4:
                        DateTime? ThuStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.ThuStartTime.Hours, SelectedEmployee.CompanyShift.ThuStartTime.Minutes, SelectedEmployee.CompanyShift.ThuStartTime.Seconds);
                        DateTime? ThuEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.ThuEndTime.Hours, SelectedEmployee.CompanyShift.ThuEndTime.Minutes, SelectedEmployee.CompanyShift.ThuEndTime.Seconds);
                        TimeSpan ThuBreakTime = SelectedEmployee.CompanyShift.ThuBreakTime;
                        return (((ThuEndTime - ThuStartTime).Value.Duration()) - ThuBreakTime);

                    case 5:
                        DateTime? FriStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.FriStartTime.Hours, SelectedEmployee.CompanyShift.FriStartTime.Minutes, SelectedEmployee.CompanyShift.FriStartTime.Seconds);
                        DateTime? FriEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.FriEndTime.Hours, SelectedEmployee.CompanyShift.FriEndTime.Minutes, SelectedEmployee.CompanyShift.FriEndTime.Seconds);
                        TimeSpan FriBreakTime = SelectedEmployee.CompanyShift.FriBreakTime;
                        return (((FriEndTime - FriStartTime).Value.Duration()) - FriBreakTime);

                    case 6:
                        DateTime? SatStartTime = new DateTime(selectedItem.StartDate.Value.Year, selectedItem.StartDate.Value.Month, selectedItem.StartDate.Value.Day, SelectedEmployee.CompanyShift.SatStartTime.Hours, SelectedEmployee.CompanyShift.SatStartTime.Minutes, SelectedEmployee.CompanyShift.SatStartTime.Seconds);
                        DateTime? SatEndTime = new DateTime(selectedItem.EndDate.Value.Year, selectedItem.EndDate.Value.Month, selectedItem.EndDate.Value.Day, SelectedEmployee.CompanyShift.SatEndTime.Hours, SelectedEmployee.CompanyShift.SatEndTime.Minutes, SelectedEmployee.CompanyShift.SatEndTime.Seconds);
                        TimeSpan SatBreakTime = SelectedEmployee.CompanyShift.SatBreakTime;
                         return (((SatEndTime - SatStartTime).Value.Duration()) - SatBreakTime);

                    default:
                        return (new TimeSpan(08, 00, 00));
                }
                GeosApplication.Instance.Logger.Log("Method GetShiftTime()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetShiftTime()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                return new TimeSpan();

            }
        }

        private void SplitButtonCommandAction(object obj)//[Sudhir.jangra][GEOS2-5275]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SplitButtonCommandAction....", category: Category.Info, priority: Priority.Low);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                SplitEmployeeAttendanceGridView splitEmployeeAttendanceGridView = new SplitEmployeeAttendanceGridView();
                SplitEmployeeAttendanceGridViewModel splitEmployeeAttendanceGridViewModel = new SplitEmployeeAttendanceGridViewModel();
                EventHandler handle = delegate { splitEmployeeAttendanceGridView.Close(); };
                splitEmployeeAttendanceGridViewModel.RequestClose += handle;
                splitEmployeeAttendanceGridView.DataContext = splitEmployeeAttendanceGridViewModel;
                splitEmployeeAttendanceGridViewModel.WindowHeader = Application.Current.FindResource("SplitEmployeeAttendanceGridTitle").ToString();
                //splitEmployeeAttendanceGridViewModel.Init();
                splitEmployeeAttendanceGridView.ShowDialogWindow();
                if (splitEmployeeAttendanceGridViewModel.IsSave)
                {
                    FillAttendanceListByPlant();
                    IsEmployeewiseRegisterAndExpectDays = false;
                    Mouse.OverrideCursor = null;
                }
                EmployeeMonthlyData.ClosePleaseWaitScreen();
            }
            catch (Exception ex)
            {
                EmployeeMonthlyData.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Get an error in Method SplitButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region chitra.girigosavi GEOS2-7807 28/05/2025
        //chitra.girigosavi GEOS2-7807 28/05/2025
        private UI.Helper.Appointment FillAppointment(EmployeeLeave NewEmpLeave)
        {
            try
            {
                //GeosApplication.Instance.Logger.Log("Method FillAppointment ...", category: Category.Info, priority: Priority.Low);
                //This method executed in loop. Commented the above logging to avoid slowing down application.

                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                //modelAppointment.Subject = string.Format("{0} {1}_{2}", NewEmpLeave.Employee.FirstName, NewEmpLeave.Employee.LastName, NewEmpLeave.CompanyLeave.Name);
                //Added Employee Main JDAbbreviation in Subject [rdixit][21.10.2022][GEOS2-3263]
                modelAppointment.Subject = string.Format("{0} {1}_{2}_{3}", NewEmpLeave.Employee.FirstName, NewEmpLeave.Employee.LastName, NewEmpLeave.Employee.EmployeeMainJDAbbreviation, NewEmpLeave.CompanyLeave.Name);
                modelAppointment.StartDate = NewEmpLeave.StartDate;
                if (Convert.ToBoolean(NewEmpLeave.IsAllDayEvent))
                {
                    TimeSpan NewTime = new TimeSpan(0, 0, 0);
                    if (NewEmpLeave.EndDate.Value.TimeOfDay == NewTime)
                        modelAppointment.EndDate = NewEmpLeave.EndDate.Value.AddDays(1);
                    else
                        modelAppointment.EndDate = NewEmpLeave.EndDate;
                }
                else
                {

                    modelAppointment.EndDate = NewEmpLeave.EndDate;
                }
                Int32 IdCompanyShiftofLeave = NewEmpLeave.Employee.CompanyShift.IdCompanyShift;
                if (IdCompanyShiftofLeave != 0)
                {
                    CompanyShift CompanyShift = HrmService.GetCompanyShiftDetailByIdCompanyShift(IdCompanyShiftofLeave);

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

        //chitra.girigosavi GEOS2-7807 28/05/2025
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

        //chitra.girigosavi GEOS2-7807 30/04/2025
        private void deleteEmployeeLeave(Object e)
        {

            AppointmentItem appointment = ((AppointmentItemCancelEventArgs)e).Appointment;
            string LeaveType = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == (ulong)appointment.CustomFields["IdEmployeeLeave"]).CompanyLeave.Name;
            EmployeeLeave leave = EmployeeLeaves.FirstOrDefault(x => x.IdEmployeeLeave == Convert.ToUInt64(appointment.CustomFields["IdEmployeeLeave"]));
            EmployeeLeave employeeLeave = new EmployeeLeave();
            employeeLeave.EmployeeChangelogs = new List<EmployeeChangelog>();
            employeeLeave.IdEmployee = leave.IdEmployee;
            employeeLeave.IdEmployeeLeave = leave.IdEmployeeLeave;
            employeeLeave.EmployeeChangelogs.Add(new EmployeeChangelog()
            {
                IdEmployee = leave.IdEmployee,
                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveDeleteChangeLog").ToString(), LeaveType, leave.StartDate.Value.ToShortDateString(), leave.EndDate.Value.ToShortDateString())
            });

            bool result = HrmService.DeleteEmployeeLeave(leave.Employee.EmployeeCode, leave.IdEmployeeLeave, employeeLeave);
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
        #endregion
        #endregion

        private void SetUserPermission()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserPermission()...", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method SetUserPermission()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetUserPermission()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            //HrmCommon.Instance.UserPermission = PermissionManagement.PlantViewer;
        }

    }
}
