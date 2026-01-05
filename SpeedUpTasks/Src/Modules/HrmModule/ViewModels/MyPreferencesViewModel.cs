using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class MyPreferencesViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmStartUp = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        private int selectedIndexTopOffers;
        private int selectedIndexTopCustomer;
        private int selectedIndexRegion;
        private int? autoRefreshOffOption;
        private long selectedPeriod;
        private bool isAutoRefresh;

        private DateTime offerPeriodDate;
        private DateTime maxDate;
        private DateTime minDate;

        List<Currency> currencies;
        private int selectedIndexCurrency = 0;
        private int selectedIndexAutoRefresh = 0;
        private int selectedIndexWorkbenchUserPermission = 0;
        public List<long> FinancialYearLst { get; set; }

        public List<UserPermission> WorkbenchUserPermissions { get; set; }

        private Visibility loadDataVisisbility;
        private Visibility isSectionVisible;
        private List<object> selectedHrmSections;

        private bool isYearChecked = true;
        private bool isCustomChecked;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        private int? customPeriodOption;

        TimeSpan timeSpanStart = new TimeSpan(0, 0, 0);
        TimeSpan timeSpanEnd = new TimeSpan(23, 59, 59);

        private ObservableCollection<Company> entireCompanyPlantList;
        private ObservableCollection<Customer> companyGroupList;


        private string attendance;
        private string employee;
        private string orgChart;
        private string importAttendance;
        private string holiday;
        private string jobDescription;
        private string leaves;
        private string attendanceList;
        private string searchEmployee;


        #endregion // Declaration

        #region public Properties
        public static bool IsPreferenceChanged { get; set; }
        
        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }
        public ObservableCollection<Customer> CompanyGroupList
        {
            get { return companyGroupList; }
            set
            {
                companyGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyGroupList"));
            }
        }
        public bool IsYearChecked
        {
            get { return isYearChecked; }
            set
            {
                isYearChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsYearChecked"));
            }
        }
        public bool IsCustomChecked
        {
            get { return isCustomChecked; }
            set
            {
                isCustomChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCustomChecked"));
            }
        }
        public DateTime? FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
                //if (FromDate != null && ToDate != null && FromDate > ToDate)
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("MyPreferencesCustomIntervalFailFromDate").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    FromDate = null;

                //}

            }
        }

        public DateTime? ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
                //if (FromDate != null && ToDate != null && FromDate > ToDate)
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("MyPreferencesCustomIntervalFailToDate").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    ToDate = null;
                //}
            }
        }


        public int? CustomPeriodOption
        {
            get
            {
                return customPeriodOption;
            }

            set
            {
                customPeriodOption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomPeriodOption"));
            }
        }

        public List<object> SelectedHrmSections
        {
            get
            {
                return selectedHrmSections;
            }

            set
            {
                selectedHrmSections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedHrmSections"));
            }
        }
        public Visibility IsSectionVisible
        {
            get { return isSectionVisible; }
            set { isSectionVisible = value; OnPropertyChanged(new PropertyChangedEventArgs("IsSectionVisible")); }
        }

        public List<string> lstTopRange { get; set; }
        public List<string> AutoRefreshLst { get; set; }

        public int? AutoRefreshOffOption
        {
            get
            {
                return autoRefreshOffOption;
            }

            set
            {
                autoRefreshOffOption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AutoRefreshOffOption"));
                if (value == 1)
                {
                    IsSectionVisible = Visibility.Visible;
                }
                else
                {
                    IsSectionVisible = Visibility.Collapsed;
                }
            }
        }

        public Visibility LoadDataVisisbility
        {
            get
            {
                return loadDataVisisbility;
            }

            set
            {
                loadDataVisisbility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LoadDataVisisbility"));
            }
        }


        public int SelectedIndexRegion
        {
            get { return selectedIndexRegion; }
            set
            {
                selectedIndexRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexRegion"));
            }
        }
        public long SelectedPeriod
        {
            get { return selectedPeriod; }
            set
            {
                selectedPeriod = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPeriod"));
            }
        }

        public int SelectedIndexTopOffers
        {
            get { return selectedIndexTopOffers; }
            set
            {
                selectedIndexTopOffers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexTopOffers"));
            }
        }

        public int SelectedIndexTopCustomer
        {
            get { return selectedIndexTopCustomer; }
            set
            {
                selectedIndexTopCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexTopCutomer"));
            }
        }

        public DateTime MaxDate
        {
            get { return maxDate; }
            set
            {
                maxDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxDate"));
            }
        }

        public DateTime MinDate
        {
            get { return minDate; }
            set
            {
                minDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinDate"));
            }
        }

        public DateTime OfferPeriodDate
        {
            get { return offerPeriodDate; }
            set
            {
                offerPeriodDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferPeriodDate"));
            }
        }

        public int SelectedIndexCurrency
        {
            get
            {
                return selectedIndexCurrency;
            }
            set
            {
                selectedIndexCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCurrency"));
            }
        }

        public int SelectedIndexAutoRefresh
        {
            get
            {
                return selectedIndexAutoRefresh;
            }

            set
            {
                selectedIndexAutoRefresh = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexAutoRefresh"));
            }
        }

        public bool IsAutoRefresh
        {
            get
            {
                return isAutoRefresh;
            }

            set
            {
                isAutoRefresh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAutoRefresh"));

                if (!isAutoRefresh)
                {
                    LoadDataVisisbility = Visibility.Visible;
                    AutoRefreshOffOption = 0;
                }
                else
                {
                    LoadDataVisisbility = Visibility.Collapsed;
                    AutoRefreshOffOption = null;
                }
            }
        }

        public List<Currency> Currencies
        {
            get
            {
                return currencies;
            }

            set
            {
                currencies = value;
            }
        }

        public int SelectedIndexWorkbenchUserPermission
        {
            get
            {
                return selectedIndexWorkbenchUserPermission;
            }

            set
            {
                selectedIndexWorkbenchUserPermission = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexWorkbenchUserPermission"));
                FillSectionsDeatils(selectedIndexWorkbenchUserPermission);
            }
        }

        public string Attendance
        {
            get
            {
                return attendance;
            }

            set
            {
                attendance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attendance"));
            }
        }

        public string Employee
        {
            get
            {
                return employee;
            }

            set
            {
                employee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Employee"));
            }
        }

        public string Holiday
        {
            get
            {
                return holiday;
            }

            set
            {
                holiday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Holiday"));
            }
        }        
        public string JobDescription
        {
            get
            {
                return jobDescription;
            }

            set
            {
                jobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescription"));
            }
        }

        public string Leaves
        {
            get
            {
                return leaves;
            }

            set
            {
                leaves = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Leaves"));
            }
        }

            public string AttendanceList
            {
            get
            {
                return attendanceList;
            }

            set
            {
                attendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendanceList"));
            }
        }
        public string OrgChart
        {
            get
            {
                return orgChart;
            }

            set
            {
                orgChart = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrgChart"));
            }
        }

        public string ImportAttendance
        {
            get
            {
                return importAttendance;
            }

            set
            {
                importAttendance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("importAttendance"));
            }
        }

        
        public string SearchEmployee
        {
            get
            {
                return searchEmployee;
            }

            set
            {
                searchEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchEmployee"));
            }
        }

        #endregion // Properties

        #region public ICommand

        public ICommand MyPreferencesViewAcceptButtonCommand { get; set; }
        public ICommand MyPreferencesViewCancelButtonCommand { get; set; }

        public ICommand Attendance_KeyDownCommand { get; set; }
        public ICommand Employee_KeyDownCommand { get; set; }
        public ICommand Holiday_KeyDownCommand { get; set; }        
        public ICommand JobDescription_KeyDownCommand { get; set; }
        public ICommand Leaves_KeyDownCommand { get; set; }
        public ICommand AttendanceList_KeyDownCommand { get; set; }
        public ICommand OrgChart_KeyDownCommand { get; set; }
        public ICommand ImportAttendance_KeyDownCommand { get; set; }        
        public ICommand SearchEmployee_KeyDownCommand { get; set; }

        public ICommand Attendance_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Attendance_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Attendance_PreviewKeyDownAllCommand { get; set; }

        public ICommand Employee_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Employee_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Employee_PreviewKeyDownAllCommand { get; set; }

        public ICommand Holiday_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Holiday_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Holiday_PreviewKeyDownAllCommand { get; set; }

        public ICommand JobDescription_PreviewKeyDownCopyCommand { get; set; }
        public ICommand JobDescription_PreviewKeyDownPasteCommand { get; set; }
        public ICommand JobDescription_PreviewKeyDownAllCommand { get; set; }

        public ICommand Leaves_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Leaves_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Leaves_PreviewKeyDownAllCommand { get; set; }

        public ICommand AttendanceList_PreviewKeyDownCopyCommand { get; set; }
        public ICommand AttendanceList_PreviewKeyDownPasteCommand { get; set; }
        public ICommand AttendanceList_PreviewKeyDownAllCommand { get; set; }

        public ICommand OrgChart_PreviewKeyDownCopyCommand { get; set; }
        public ICommand OrgChart_PreviewKeyDownPasteCommand { get; set; }
        public ICommand OrgChart_PreviewKeyDownAllCommand { get; set; }

        public ICommand ImportAttendance_PreviewKeyDownCopyCommand { get; set; }
        public ICommand ImportAttendance_PreviewKeyDownPasteCommand { get; set; }
        public ICommand ImportAttendance_PreviewKeyDownAllCommand { get; set; }

        public ICommand SearchEmployee_PreviewKeyDownCopyCommand { get; set; }
        public ICommand SearchEmployee_PreviewKeyDownPasteCommand { get; set; }
        public ICommand SearchEmployee_PreviewKeyDownAllCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion // ICommand

        #region Events

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                     //me[BindableBase.GetPropertyName(() => AutoRefreshOffOption)] +    // AutoRefresh Off Option
                    //me[BindableBase.GetPropertyName(() => FromDate)] +
                    //me[BindableBase.GetPropertyName(() => ToDate)]+
                    me[BindableBase.GetPropertyName(() => Attendance)]+
                    me[BindableBase.GetPropertyName(() => Employee)]+
                    me[BindableBase.GetPropertyName(() => Holiday)] +
                    me[BindableBase.GetPropertyName(() => JobDescription)] +
                    me[BindableBase.GetPropertyName(() => Leaves)] +
                    me[BindableBase.GetPropertyName(() => AttendanceList)] +
                    me[BindableBase.GetPropertyName(() => OrgChart)]+                    
                    me[BindableBase.GetPropertyName(() => ImportAttendance)]+                    
                    me[BindableBase.GetPropertyName(() => SearchEmployee)];


                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        

        string IDataErrorInfo.this[string columnName]
        {
            get
          {
                if (!allowValidation) return null;
                //string AutoRefreshOffOptionProp = BindableBase.GetPropertyName(() => AutoRefreshOffOption);   // selectedIndexSalesUnit
                string FromDateProp = BindableBase.GetPropertyName(() => FromDate);
                string ToDateProp = BindableBase.GetPropertyName(() => ToDate);
                string AttendanceProp = BindableBase.GetPropertyName(() => Attendance);
                string EmployeeProp = BindableBase.GetPropertyName(() => Employee);
                string HolidayProp = BindableBase.GetPropertyName(() => Holiday);
                string JobDescriptionProp = BindableBase.GetPropertyName(() => JobDescription);
                string LeavesProp = BindableBase.GetPropertyName(() => Leaves);
                string AttendanceListProp = BindableBase.GetPropertyName(() => AttendanceList);
                string OrgChartProp = BindableBase.GetPropertyName(() => OrgChart);
                string ImportAttendanceProp = BindableBase.GetPropertyName(() => ImportAttendance);                
                string SearchEmployeeProp = BindableBase.GetPropertyName(() => SearchEmployee);

                //if (columnName == AutoRefreshOffOptionProp)
                //    return MyPreferencesValidations.GetErrorMessage(AutoRefreshOffOptionProp, AutoRefreshOffOption);
                //else 


                if (columnName == FromDateProp && IsCustomChecked == true)               //From Date
                {
                    if (FromDate != null && ToDate != null)
                    {
                        int result = DateTime.Compare(FromDate.Value, ToDate.Value);
                        if (result > 0)
                            return "The date you entered occurs after the end date.";
                        else if (result == 0)
                            return "From Date is the same as To Date";
                    }
                    return MyPreferencesValidations.GetErrorMessage(FromDateProp, FromDate);
                }
                else if (columnName == ToDateProp && IsCustomChecked == true)               //To Date
                {
                    if (FromDate != null && ToDate != null)
                    {
                        int result = DateTime.Compare(ToDate.Value, FromDate.Value);
                        if (result < 0)
                            return "The date you entered occurs before the start date.";
                        else if (result == 0)
                            return "To Date is the same as From Date";
                    }
                    return MyPreferencesValidations.GetErrorMessage(ToDateProp, ToDate);
                }
                else if (columnName == AttendanceProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(AttendanceProp, Attendance);
                }
                else if (columnName == EmployeeProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(EmployeeProp, Employee);
                }
                else if (columnName == HolidayProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(HolidayProp, Holiday);
                }
                else if (columnName == JobDescriptionProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(JobDescriptionProp, JobDescription);
                }
                else if (columnName == LeavesProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(LeavesProp, Leaves);
                }
                else if (columnName == AttendanceListProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(AttendanceListProp, AttendanceList);
                }                
                else if (columnName == OrgChartProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(OrgChartProp, OrgChart);
                }                
                else if (columnName == ImportAttendanceProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(ImportAttendanceProp, ImportAttendance);
                }                
                else if (columnName == SearchEmployeeProp)
                {
                    return MyPreferencesValidations.GetErrorMessage(SearchEmployeeProp, SearchEmployee);
                }
                return null;
            }
        }

        #endregion

        #region Constructor

        public MyPreferencesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel ...", category: Category.Info, priority: Priority.Low);

                MyPreferencesViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveMyPreference));
                MyPreferencesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                Attendance_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Attendance_KeyDown);
                Employee_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Employee_KeyDown);
                Holiday_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Holiday_KeyDown);
                OrgChart_KeyDownCommand = new DelegateCommand<KeyEventArgs>(OrgChart_KeyDown);
                ImportAttendance_KeyDownCommand = new DelegateCommand<KeyEventArgs>(importAttendance_KeyDown);
                JobDescription_KeyDownCommand = new DelegateCommand<KeyEventArgs>(JobDescription_KeyDown);
                Leaves_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Leaves_KeyDown);
                AttendanceList_KeyDownCommand = new DelegateCommand<KeyEventArgs>(AttendanceList_KeyDown);                
                SearchEmployee_KeyDownCommand = new DelegateCommand<KeyEventArgs>(SearchEmployee_KeyDown);

                Attendance_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Attendance_PreviewKeyDown_Copy);
                Attendance_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Attendance_PreviewKeyDown_Paste);
                Attendance_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Attendance_PreviewKeyDown_All);

                Employee_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Employee_PreviewKeyDown_Copy);
                Employee_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Employee_PreviewKeyDown_Paste);
                Employee_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Employee_PreviewKeyDown_All);

                Holiday_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Holiday_PreviewKeyDown_Copy);
                Holiday_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Holiday_PreviewKeyDown_Paste);
                Holiday_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Holiday_PreviewKeyDown_All);

                OrgChart_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(OrgChart_PreviewKeyDown_Copy);
                OrgChart_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(OrgChart_PreviewKeyDown_Paste);
                OrgChart_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(OrgChart_PreviewKeyDown_All);

                ImportAttendance_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(importAttendance_PreviewKeyDown_Copy);
                ImportAttendance_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(importAttendance_PreviewKeyDown_Paste);
                ImportAttendance_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(importAttendance_PreviewKeyDown_All);

                JobDescription_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(JobDescription_PreviewKeyDown_Copy);
                JobDescription_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(JobDescription_PreviewKeyDown_Paste);
                JobDescription_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(JobDescription_PreviewKeyDown_All);

                Leaves_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Leaves_PreviewKeyDown_Copy);
                Leaves_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Leaves_PreviewKeyDown_Paste);
                Leaves_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Leaves_PreviewKeyDown_All);

                AttendanceList_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(AttendanceList_PreviewKeyDown_Copy);
                AttendanceList_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(AttendanceList_PreviewKeyDown_Paste);
                AttendanceList_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(AttendanceList_PreviewKeyDown_All);

                
                SearchEmployee_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(SearchEmployee_PreviewKeyDown_Copy);
                SearchEmployee_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(SearchEmployee_PreviewKeyDown_Paste);
                SearchEmployee_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(SearchEmployee_PreviewKeyDown_All);

                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                FillTopRangeList();
                FillCurrencyDetails();
                FillUserPermissionsList();
                MaxDate = GeosApplication.Instance.ServerDateTime.Date;
                FillFinancialYear();
                IsSectionVisible = Visibility.Collapsed;

                if (GeosApplication.Instance.UserSettings != null)
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption"))
                    {
                        if (GeosApplication.Instance.UserSettings["CustomPeriodOption"].Equals("0"))
                        {
                            IsYearChecked = true;
                            IsCustomChecked = false;
                            if (GeosApplication.Instance.UserSettings.ContainsKey("HrmOfferPeriod"))
                            {
                                //SelectedPeriod = GeosApplication.Instance.HrmOfferYear;
                                //GeosApplication.Instance.SelectedyearStarDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.HrmOfferYear), 1, 1).Add(timeSpanStart);
                                //GeosApplication.Instance.SelectedyearEndDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.HrmOfferYear), 12, 31).Add(timeSpanEnd);
                            }
                            else
                            {
                                SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;
                                GeosApplication.Instance.SelectedyearStarDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.ServerDateTime.Date.Year), 1, 1).Add(timeSpanStart);
                                GeosApplication.Instance.SelectedyearEndDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.ServerDateTime.Date.Year), 12, 31).Add(timeSpanEnd);
                            }
                        }
                        else if (GeosApplication.Instance.UserSettings["CustomPeriodOption"].Equals("1"))
                        {
                            IsYearChecked = false;
                            IsCustomChecked = true;
                            FromDate = GeosApplication.Instance.SelectedyearStarDate;
                            ToDate = GeosApplication.Instance.SelectedyearEndDate;
                            //SelectedPeriod = GeosApplication.Instance.HrmOfferYear;

                        }
                        else
                        {
                            IsYearChecked = true;
                            IsCustomChecked = false;
                            //SelectedPeriod = GeosApplication.Instance.HrmOfferYear;
                            //FromDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.HrmOfferYear), 1, 1);
                            //ToDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.HrmOfferYear), 12, 31);
                        }
                    }


                    if (GeosApplication.Instance.UserSettings.ContainsKey("HrmTopOffers"))
                    {
                        SelectedIndexTopOffers = lstTopRange.FindIndex(i => i.Contains(GeosApplication.Instance.UserSettings["HrmTopOffers"].ToString()));
                    }

                    if (GeosApplication.Instance.UserSettings.ContainsKey("HrmTopCustomers"))
                    {
                        SelectedIndexTopCustomer = lstTopRange.FindIndex(i => i.Contains(GeosApplication.Instance.UserSettings["HrmTopCustomers"].ToString()));
                    }

                    if (GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"].ToString() == "Yes")
                            IsAutoRefresh = true;
                        else
                        {
                            IsAutoRefresh = false;

                            if (GeosApplication.Instance.UserSettings.ContainsKey("LoadDataOn"))
                            {
                                if (!string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["LoadDataOn"]))
                                {
                                    AutoRefreshOffOption = Convert.ToInt32(GeosApplication.Instance.UserSettings["LoadDataOn"]);
                                }
                            }

                        }
                    }

                    if (GeosApplication.Instance.UserSettings.ContainsKey("CurrentProfile"))
                    {
                        if (GeosApplication.Instance.IdUserPermission > 0)
                        {
                            GeosApplication.Instance.UserSettings["CurrentProfile"] = GeosApplication.Instance.IdUserPermission.ToString();
                            SelectedIndexWorkbenchUserPermission = WorkbenchUserPermissions.FindIndex(i => i.IdPermission == Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString()));
                        }
                        else
                        {
                            int value;
                            if (int.TryParse(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString(), out value))
                                SelectedIndexWorkbenchUserPermission = WorkbenchUserPermissions.FindIndex(i => i.IdPermission == Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString()));
                            else
                                SelectedIndexWorkbenchUserPermission = -1;
                        }
                    }

                    // shortcuts
                    // Get shortcut for Attendance
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Attendance"))
                    {
                        Attendance = GeosApplication.Instance.UserSettings["Attendance"].ToString();
                    }

                    // Get shortcut for  Employee
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Employee"))
                    {
                        Employee = GeosApplication.Instance.UserSettings["Employee"].ToString();
                    }

                    // Get shortcut for  Holiday
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Holiday"))
                    {
                        Holiday = GeosApplication.Instance.UserSettings["Holiday"].ToString();
                    }

                    // Get shortcut for  JobDescription
                    if (GeosApplication.Instance.UserSettings.ContainsKey("JobDescriptions"))
                    {
                        JobDescription = GeosApplication.Instance.UserSettings["JobDescriptions"].ToString();
                    }

                    // Get shortcut for  Leaves
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Leave"))
                    {
                        Leaves = GeosApplication.Instance.UserSettings["Leave"].ToString();
                    }

                    // Get shortcut for AttendanceList
                    if (GeosApplication.Instance.UserSettings.ContainsKey("AttendanceList"))
                    {
                        AttendanceList = GeosApplication.Instance.UserSettings["AttendanceList"].ToString();
                    }

                    // Get shortcut for  OrgChart
                    if (GeosApplication.Instance.UserSettings.ContainsKey("OrganizationChart"))
                    {
                        OrgChart = GeosApplication.Instance.UserSettings["OrganizationChart"].ToString();
                    }

                    // Get shortcut for ImportAttendance
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendance"))
                    {
                        ImportAttendance = GeosApplication.Instance.UserSettings["ImportAttendance"].ToString();
                    }

                    // Get shortcut for SearchEmployee
                    if (GeosApplication.Instance.UserSettings.ContainsKey("SearchEmployee"))
                    {
                        SearchEmployee = GeosApplication.Instance.UserSettings["SearchEmployee"].ToString();
                    }
                }

                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

                //if (GeosApplication.Instance.SelectedHrmsectionsList.Count == 0 || GeosApplication.Instance.SelectedHrmsectionsList[0] == null)
                //{
                //    SelectedHrmSections = new List<object>(GeosApplication.Instance.HrmsectionsList);
                //}
                //else
                //{
                //    SelectedHrmSections = new List<object>(GeosApplication.Instance.SelectedHrmsectionsList);
                //}
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in MyPreferencesViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Attendance_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Attendance_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Attendance = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Attendance_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Attendance_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Attendance_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Attendance_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Attendance = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Attendance_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Attendance_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Attendance_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Attendance_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Attendance = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Attendance_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Attendance_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Employee_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Employee_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Employee = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Employee_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Employee_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Employee_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Employee_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Employee = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Employee_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Employee_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Employee_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Employee_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Employee = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Employee_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Employee_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Holiday_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Holiday_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Holiday = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Holiday_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Holiday_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Holiday_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Holiday_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Holiday = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Holiday_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Holiday_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Holiday_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Holiday_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Holiday = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Holiday_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Holiday_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OrgChart_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method OrgChart_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                OrgChart = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method OrgChart_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OrgChart_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OrgChart_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method OrgChart_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                OrgChart = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method OrgChart_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OrgChart_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OrgChart_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method OrgChart_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                OrgChart = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method OrgChart_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OrgChart_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void importAttendance_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method importAttendance_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ImportAttendance = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method importAttendance_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method importAttendance_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void importAttendance_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method importAttendance_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ImportAttendance = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method importAttendance_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method importAttendance_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void importAttendance_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method importAttendance_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ImportAttendance = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method importAttendance_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method importAttendance_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void JobDescription_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method JobDescription_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                JobDescription = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method JobDescription_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method JobDescription_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void JobDescription_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method JobDescription_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                JobDescription = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method JobDescription_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method JobDescription_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void JobDescription_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method JobDescription_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                JobDescription = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method JobDescription_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method JobDescription_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Leaves_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Leaves_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Leaves = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Leaves_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Leaves_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Leaves_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Leaves_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Leaves = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Leaves_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Leaves_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Leaves_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Leaves_PreviewKeyDown_AllLeaves_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Leaves = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Email_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Leaves_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AttendanceList_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method AttendanceList_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                AttendanceList = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method AttendanceList_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AttendanceList_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AttendanceList_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method AttendanceList_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                AttendanceList = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method AttendanceList_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AttendanceList_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AttendanceList_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method AttendanceList_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                AttendanceList = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method AttendanceList_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AttendanceList_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

       

        private void SearchEmployee_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchEmployee_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SearchEmployee = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method SearchEmployee_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchEmployee_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SearchEmployee_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchEmployee_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SearchEmployee = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method SearchEmployee_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchEmployee_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SearchEmployee_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchEmployee_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SearchEmployee = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method SearchEmployee_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchEmployee_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
               
                HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor

        #region Methods

        private void FillTopRangeList()
        {
            GeosApplication.Instance.Logger.Log("Constructor FillTopRangeList ...", category: Category.Info, priority: Priority.Low);
            lstTopRange = new List<string>();
            lstTopRange.Add("10");
            lstTopRange.Add("20");
            lstTopRange.Add("30");
            lstTopRange.Add("40");
            lstTopRange.Add("50");
            lstTopRange.Add("60");
            lstTopRange.Add("70");
            lstTopRange.Add("80");
            lstTopRange.Add("90");
            lstTopRange.Add("100");
            GeosApplication.Instance.Logger.Log("Constructor FillTopRangeList() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// method for fill financial year.
        /// </summary>
        private void FillFinancialYear()
        {
            FinancialYearLst = new List<long>();
            for (long i = maxDate.Year + 1; i >= 2000; i--)
            {
                FinancialYearLst.Add(i);
            }
        }

        /// <summary>
        /// Method for fill user permission list.
        /// </summary>
        private void FillUserPermissionsList()
        {
            GeosApplication.Instance.Logger.Log("Method FillUserPermissionsList ...", category: Category.Info, priority: Priority.Low);

            List<int> userPermissionlst = new List<int>() { 20, 21, 22 };
            // this code remove duplicate User Permissions in list.
            WorkbenchUserPermissions = new List<UserPermission>(GeosApplication.Instance.ActiveUser.UserPermissions.Where(up => userPermissionlst.Contains(up.IdPermission)).GroupBy(x => x.IdPermission).Select(c => c.First()).ToList());
            WorkbenchUserPermissions = WorkbenchUserPermissions.OrderBy(c => c.IdPermission).ToList();

            GeosApplication.Instance.Logger.Log("Method FillUserPermissionsList() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillAutoRefreshLst()
        {
            GeosApplication.Instance.Logger.Log("Method FillAutoRefreshLst ...", category: Category.Info, priority: Priority.Low);

            AutoRefreshLst = new List<string>();
            AutoRefreshLst.Add("Yes");
            AutoRefreshLst.Add("No");

            GeosApplication.Instance.Logger.Log("Method FillAutoRefreshLst() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        private void SaveMyPreference(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveMyPreference ...", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();
                //if (!IsAutoRefresh)
                //{
                //    //string error = EnableValidationAndGetError();
                //    //PropertyChanged(this, new PropertyChangedEventArgs("AutoRefreshOffOption"));
                //    if (IsCustomChecked)
                //    {
                //        PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                //        PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));
                //    }
                //    //if (error != null)
                //    //{
                //    //    return;
                //    //}
                //}
                //else
                //{
                //    if (IsCustomChecked)
                //    {
                //        PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                //        PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));
                //    }
                //}
                PropertyChanged(this, new PropertyChangedEventArgs("Attendance"));
                PropertyChanged(this, new PropertyChangedEventArgs("Employee"));
                PropertyChanged(this, new PropertyChangedEventArgs("Holiday"));
                PropertyChanged(this, new PropertyChangedEventArgs("JobDescription"));
                PropertyChanged(this, new PropertyChangedEventArgs("Leaves"));
                PropertyChanged(this, new PropertyChangedEventArgs("AttendanceList"));
                PropertyChanged(this, new PropertyChangedEventArgs("OrgChart"));
                PropertyChanged(this, new PropertyChangedEventArgs("ImportAttendance"));                
                PropertyChanged(this, new PropertyChangedEventArgs("SearchEmployee"));

                if (error != null)
                {
                    return;
                }

                string selectedHrmSectionStr = string.Empty;

                List<string> Records = new List<string>();
                Records.Add(Attendance);
                Records.Add(Employee);
                Records.Add(Holiday);
                Records.Add(JobDescription);
                Records.Add(Leaves);
                Records.Add(AttendanceList);
                Records.Add(OrgChart);
                Records.Add(ImportAttendance);
                Records.Add(SearchEmployee);
                

                var query = Records.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList();

                if(query!=null && query.Count>0)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DuplicateShortcutKey").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                if (SelectedHrmSections != null && SelectedHrmSections.Count > 0)
                {
                    //foreach (HrmSections HrmSection in SelectedHrmSections)
                    //{
                    //    if (HrmSection != null)
                    //    {
                    //        if (string.IsNullOrEmpty(selectedHrmSectionStr))
                    //            selectedHrmSectionStr = HrmSection.IdSection.ToString();
                    //        else
                    //            selectedHrmSectionStr += "," + HrmSection.IdSection.ToString();
                    //    }
                    //}

                    //GeosApplication.Instance.SelectedHrmsectionsList = SelectedHrmSections.Cast<HrmSections>().ToList();
                }

                else
                {
                    //GeosApplication.Instance.SelectedHrmsectionsList = null;
                }

                List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();

                //if (IsYearChecked == true)
                //{
                //    CustomPeriodOption = 0;
                //    GeosApplication.Instance.UserSettings["CustomPeriodOption"] = Convert.ToString(CustomPeriodOption);

                //    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption"))
                //    {

                //        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("HrmOfferPeriod"))
                //        {
                //            GeosApplication.Instance.UserSettings["HrmOfferPeriod"] = SelectedPeriod.ToString();
                //        }
                //        else
                //        {
                //            GeosApplication.Instance.UserSettings["HrmOfferPeriod"] = SelectedPeriod.ToString();
                //        }
                //        //GeosApplication.Instance.HrmOfferYear = Convert.ToInt64(GeosApplication.Instance.UserSettings["HrmOfferPeriod"].ToString());
                //        //GeosApplication.Instance.SelectedyearStarDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.HrmOfferYear), 1, 1).Add(timeSpanStart);
                //        //GeosApplication.Instance.SelectedyearEndDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.HrmOfferYear), 12, 31).Add(timeSpanEnd);

                //    }
                //}
                //else if (IsCustomChecked == true)
                //{
                //    CustomPeriodOption = 1;
                //    GeosApplication.Instance.UserSettings["CustomPeriodOption"] = Convert.ToString(CustomPeriodOption);
                //    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption"))
                //    {
                //        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("HrmOfferPeriod"))
                //        {
                //            GeosApplication.Instance.UserSettings["HrmOfferFromInterval"] = FromDate.Value.ToString("yyyy/MM/dd");
                //        }
                //        else
                //        {
                //            GeosApplication.Instance.UserSettings["HrmOfferFromInterval"] = DateTime.Now.ToString("yyyy/MM/dd");
                //        }

                //        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("HrmOfferPeriod"))
                //        {
                //            GeosApplication.Instance.UserSettings["HrmOfferToInterval"] = ToDate.Value.ToString("yyyy/MM/dd");
                //        }
                //        else
                //        {
                //            GeosApplication.Instance.UserSettings["HrmOfferToInterval"] = DateTime.Now.ToString("yyyy/MM/dd");
                //        }

                //        GeosApplication.Instance.SelectedyearStarDate = FromDate != null ? (DateTime)FromDate : GeosApplication.Instance.ServerDateTime.Date;

                //        GeosApplication.Instance.SelectedyearEndDate = ToDate != null ? (DateTime)ToDate : GeosApplication.Instance.ServerDateTime.Date;


                //        GeosApplication.Instance.SelectedyearStarDate.Add(timeSpanStart);
                //        GeosApplication.Instance.SelectedyearEndDate.Add(timeSpanEnd);
                //    }

                //}

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("HrmTopOffers"))
                {
                    GeosApplication.Instance.UserSettings["HrmTopOffers"] = lstTopRange[SelectedIndexTopOffers].ToString();
                }
                else
                {
                    GeosApplication.Instance.UserSettings["HrmTopOffers"] = lstTopRange[SelectedIndexTopOffers].ToString();
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("HrmTopCustomers"))
                {
                    GeosApplication.Instance.UserSettings["HrmTopCustomers"] = lstTopRange[SelectedIndexTopCustomer].ToString();
                }
                else
                {
                    GeosApplication.Instance.UserSettings["HrmTopCustomers"] = lstTopRange[SelectedIndexTopCustomer].ToString();
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCurrency"))
                {
                    GeosApplication.Instance.UserSettings["SelectedCurrency"] = Currencies[selectedIndexCurrency].Name;
                }
                else
                {
                    GeosApplication.Instance.UserSettings["SelectedCurrency"] = Currencies[selectedIndexCurrency].Name;
                }

                if (GeosApplication.Instance.UserSettings != null
                    && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh")
                    && GeosApplication.Instance.UserSettings.ContainsKey("LoadDataOn"))
                {
                    if (IsAutoRefresh)
                    {
                        GeosApplication.Instance.UserSettings["AutoRefresh"] = "Yes";
                        GeosApplication.Instance.UserSettings["LoadDataOn"] = Convert.ToString(AutoRefreshOffOption);
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings["AutoRefresh"] = "No";
                        GeosApplication.Instance.UserSettings["LoadDataOn"] = Convert.ToString(AutoRefreshOffOption);
                    }
                }
                else
                {
                    if (IsAutoRefresh)
                    {
                        GeosApplication.Instance.UserSettings["AutoRefresh"] = "Yes";
                        GeosApplication.Instance.UserSettings["LoadDataOn"] = Convert.ToString(AutoRefreshOffOption);
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings["AutoRefresh"] = "No";
                        GeosApplication.Instance.UserSettings["LoadDataOn"] = Convert.ToString(AutoRefreshOffOption);
                    }
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedHrmSectionLoadData"))
                {
                    GeosApplication.Instance.UserSettings["SelectedHrmSectionLoadData"] = selectedHrmSectionStr;
                }
                else
                {
                    GeosApplication.Instance.UserSettings["SelectedHrmSectionLoadData"] = selectedHrmSectionStr;
                }

                if (SelectedIndexWorkbenchUserPermission > -1)
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CurrentProfile"))
                    {

                        GeosApplication.Instance.UserSettings["CurrentProfile"] = WorkbenchUserPermissions[SelectedIndexWorkbenchUserPermission].Permission.IdPermission.ToString();

                        int value1;
                        if (int.TryParse(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString(), out value1))
                            GeosApplication.Instance.IdUserPermission = Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString());
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings["CurrentProfile"] = WorkbenchUserPermissions[SelectedIndexWorkbenchUserPermission].Permission.IdPermission.ToString(); ;
                    }
                }

                // shortcuts
                if (GeosApplication.Instance.UserSettings != null)
                {
                    
                    //Set shortcut for Attendance
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Attendance"))
                    {
                        GeosApplication.Instance.UserSettings["Attendance"] = Attendance.TrimStart().TrimEnd();
                    }
                                      
                    //Set shortcut for Employee
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Employee"))
                    {
                        GeosApplication.Instance.UserSettings["Employee"] = Employee.TrimStart().TrimEnd();
                    }

                    // Set shortcut for  Holiday
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Holiday"))
                    {
                        GeosApplication.Instance.UserSettings["Holiday"] = Holiday.TrimStart().TrimEnd();
                    }

                    // Set shortcut for  JobDescription
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("JobDescriptions"))
                    {
                        GeosApplication.Instance.UserSettings["JobDescriptions"] = JobDescription.TrimStart().TrimEnd();
                    }

                    // Set shortcut for  Leaves
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Leave"))
                    {
                        GeosApplication.Instance.UserSettings["Leave"] = Leaves.TrimStart().TrimEnd();
                    }

                    // Set shortcut for AttendanceList
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AttendanceList"))
                    {
                        GeosApplication.Instance.UserSettings["AttendanceList"] = AttendanceList.TrimStart().TrimEnd();
                    }                 
                    
                    //Set shortcut for OrgChart
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("OrganizationChart"))
                    {
                        GeosApplication.Instance.UserSettings["OrganizationChart"] = OrgChart.TrimStart().TrimEnd();
                    }

                    //Set shortcut for ImportAttendance
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendance"))
                    {
                        GeosApplication.Instance.UserSettings["ImportAttendance"] = ImportAttendance.TrimStart().TrimEnd();
                    }
                                       
                    //Set shortcut for SearchEmployee
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SearchEmployee"))
                    {
                        GeosApplication.Instance.UserSettings["SearchEmployee"] = SearchEmployee.TrimStart().TrimEnd();
                    }                   

                    HrmCommon.Instance.GetShortcuts();
                }

                

                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                }

                ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");

                //GeosApplication.Instance.HrmOfferYear = Convert.ToInt64(GeosApplication.Instance.UserSettings["HrmOfferPeriod"].ToString());
                //GeosApplication.Instance.HrmTopCustomers = Convert.ToUInt16(GeosApplication.Instance.UserSettings["HrmTopCustomers"].ToString());
                //GeosApplication.Instance.HrmTopOffers = Convert.ToUInt16(GeosApplication.Instance.UserSettings["HrmTopOffers"].ToString());
                GeosApplication.Instance.IdCurrencyByRegion = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.IdCurrency).SingleOrDefault();
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();

                int value;
                if (int.TryParse(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString(), out value))
                    GeosApplication.Instance.IdUserPermission = Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString());

                // FillGroupList();
                // FillCompanyPlantList();

                FillPlantOrSalesOwnerList();

                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                //GeosApplication.Instance.SelectedyearStarDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.HrmOfferYear), 1, 1);
                //GeosApplication.Instance.SelectedyearEndDate = new DateTime(Convert.ToInt32(GeosApplication.Instance.HrmOfferYear), 12, 31);

                //if (GeosApplication.Instance.SelectedyearEndDate.Year < GeosApplication.Instance.ServerDateTime.Year)
                //{
                //    GeosApplication.Instance.RemainingDays = "0";
                //}
                //else
                //{
                //    GeosApplication.Instance.RemainingDays = (Math.Round((GeosApplication.Instance.SelectedyearEndDate.Date - GeosApplication.Instance.ServerDateTime.Date).TotalDays, 0)).ToString();
                //}

                try
                {
                    IList<Customer> TempCompanyGroupList = null;
                    if (GeosApplication.Instance.IdUserPermission == 21)
                    {
                        if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                        {
                            List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                            var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));


                            //CompanyGroupList = new ObservableCollection<Customer>(HrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                            GeosApplication.Instance.ObjectPool.Remove("Hrm_COMPANYGROUP21");
                            GeosApplication.Instance.ObjectPool.Add("Hrm_COMPANYGROUP21", CompanyGroupList);
                            //EntireCompanyPlantList = new ObservableCollection<Company>(HrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                            GeosApplication.Instance.ObjectPool.Remove("Hrm_COMPANYPLANT21");
                            GeosApplication.Instance.ObjectPool.Add("Hrm_COMPANYPLANT21", EntireCompanyPlantList);
                        }
                    }
                    else
                    {

                        //CompanyGroupList = new ObservableCollection<Customer>(HrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Remove("Hrm_COMPANYGROUP");
                        GeosApplication.Instance.ObjectPool.Add("Hrm_COMPANYGROUP", CompanyGroupList);
                        //EntireCompanyPlantList = new ObservableCollection<Company>(HrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Remove("Hrm_COMPANYPLANT");
                        GeosApplication.Instance.ObjectPool.Add("Hrm_COMPANYPLANT", EntireCompanyPlantList);
                    }
                }
                catch (Exception ex)
                {
                }

                //GeosApplication.Instance.RemainingDays = (Math.Round((GeosApplication.Instance.SelectedyearEndDate.Date.AddDays(+1) - GeosApplication.Instance.SelectedyearStarDate.Date).TotalDays, 0)).ToString();

                int RemainDays = (int)(GeosApplication.Instance.SelectedyearEndDate.Date - DateTime.Now.Date).TotalDays;
                if (RemainDays <= 0)
                {
                    RemainDays = 0;
                }
                GeosApplication.Instance.RemainingDays = RemainDays.ToString();
                IsPreferenceChanged = true;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method SaveMyPreference() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SaveMyPreference() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        static IEnumerable<CultureInfo> GetCultureInfosByCurrencySymbol(string currencySymbol)
        {
            if (currencySymbol == null)
            {
            }

            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Where(x => new RegionInfo(x.LCID).ISOCurrencySymbol == currencySymbol);
        }


        /// <summary>
        /// Method for get fill currency list and get IdCurrency By current System Region Culture.
        /// </summary>
        public void FillCurrencyDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails ...", category: Category.Info, priority: Priority.Low);

                Currencies = GeosApplication.Instance.Currencies.ToList();
                SelectedIndexCurrency = Currencies.FindIndex(i => i.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion);

                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// [001][GEOS2-2074][cpatil][18-02-2020]Hrm - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        private void FillPlantOrSalesOwnerList()
        {
            GeosApplication.Instance.Logger.Log("Method FillPlantOrSalesOwnerList ...", category: Category.Info, priority: Priority.Low);
            GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Hidden;
            GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Hidden;

            // Start (SalesOwner) - Selected Sales Owners User list for Hrm. 
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                if (GeosApplication.Instance.SalesOwnerUsersList == null)
                {
                    GeosApplication.Instance.SalesOwnerUsersList = WorkbenchStartUp.GetManagerUsers(GeosApplication.Instance.ActiveUser.IdUser);
                  
                }

                //GeosApplication.Instance.SelectedSalesOwnerUsersList = new List<object>();
                UserManagerDtl usrDefault = GeosApplication.Instance.SalesOwnerUsersList.FirstOrDefault(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser);

                GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Hidden;
                GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Visible;

                if (GeosApplication.Instance.SelectedSalesOwnerUsersList == null)
                {
                    GeosApplication.Instance.SelectedSalesOwnerUsersList = new List<object>();

                    if (usrDefault != null)
                    {
                        GeosApplication.Instance.SelectedSalesOwnerUsersList.Add(usrDefault);
                    }
                    else
                    {
                        GeosApplication.Instance.SelectedSalesOwnerUsersList.AddRange(GeosApplication.Instance.SalesOwnerUsersList);
                    }

                }
                else if (GeosApplication.Instance.SelectedSalesOwnerUsersList.Count == 0)
                {
                    if (usrDefault != null)
                    {
                        GeosApplication.Instance.SelectedSalesOwnerUsersList.Add(usrDefault);
                    }
                    else
                    {
                        GeosApplication.Instance.SelectedSalesOwnerUsersList.AddRange(GeosApplication.Instance.SalesOwnerUsersList);
                    }
                }

                GeosApplication.Instance.SelectedSalesOwnerUsersList = new List<object>(GeosApplication.Instance.SelectedSalesOwnerUsersList);

            }
            // End (SalesOwner)

            // Start (PlantOwner) - Selected Plant Owners User list for Hrm. 
            if (GeosApplication.Instance.IdUserPermission == 22)
            {
                //[002] Changed service method GetAllCompaniesDetails to GetAllCompaniesDetails_V2040
                if (GeosApplication.Instance.PlantOwnerUsersList == null)
                {
                    //GeosApplication.Instance.PlantOwnerUsersList = HrmStartUp.GetAllCompaniesDetails_V2040(GeosApplication.Instance.ActiveUser.IdUser);
                }

                GeosApplication.Instance.PlantOwnerUsersList = GeosApplication.Instance.PlantOwnerUsersList.OrderBy(o => o.Country.IdCountry).ToList();
                // GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();

                GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Visible;
                GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Hidden;

                // EmdepSite emdepSite = HrmStartUp.GetEmdepSiteById(Convert.ToInt32(GeosApplication.Instance.ActiveUser...Site.ConnectPlantId));
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == serviceurl);

                if (GeosApplication.Instance.SelectedPlantOwnerUsersList == null)
                {
                    GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();

                    if (usrDefault != null)
                    {
                        GeosApplication.Instance.SelectedPlantOwnerUsersList.Add(usrDefault);
                    }
                    else
                    {
                        GeosApplication.Instance.SelectedPlantOwnerUsersList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                    }
                }
                else if (GeosApplication.Instance.SelectedPlantOwnerUsersList.Count == 0)
                {
                    if (usrDefault != null)
                    {
                        GeosApplication.Instance.SelectedPlantOwnerUsersList.Add(usrDefault);
                    }
                    else
                    {
                        GeosApplication.Instance.SelectedPlantOwnerUsersList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                    }
                }

                GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>(GeosApplication.Instance.SelectedPlantOwnerUsersList);
            }
            GeosApplication.Instance.Logger.Log("Method FillPlantOrSalesOwnerList() executed successfully", category: Category.Info, priority: Priority.Low);
            // End (PlantOwner)
        }


        /// <summary>
        /// Method for fill list of all sections of Hrm module.
        /// </summary>
        private void FillSectionsDeatils(int indexSelectedProfile)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSectionsDeatils ...", category: Category.Info, priority: Priority.Low);

                int idCurrentSelectedProfile = WorkbenchUserPermissions[indexSelectedProfile].IdPermission;

                //List<HrmSections> HrmList = new List<HrmSections>();

                //HrmList.Add(new HrmSections() { IdSection = 1, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelDashboard1").ToString() });
                //HrmList.Add(new HrmSections() { IdSection = 2, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelDashboard2").ToString() });
                //HrmList.Add(new HrmSections() { IdSection = 3, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelDashboard3").ToString() });

                if (idCurrentSelectedProfile == 22)
                {
                    //HrmList.Add(new HrmSections() { IdSection = 4, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelDashboardOperations").ToString() });
                    //HrmList.Add(new HrmSections() { IdSection = 5, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelDashboardEngineeringAnalysis").ToString() });
                }

                //HrmList.Add(new HrmSections() { IdSection = 6, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelActivities").ToString() });
                //HrmList.Add(new HrmSections() { IdSection = 7, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelOrgCharts").ToString() });
                //HrmList.Add(new HrmSections() { IdSection = 8, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelEmployees").ToString() });
                //HrmList.Add(new HrmSections() { IdSection = 9, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelTimeline").ToString() });

                //HrmList.Add(new HrmSections() { IdSection = 10, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelPipeline").ToString() });
                //HrmList.Add(new HrmSections() { IdSection = 11, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelOrders").ToString() });
                //HrmList.Add(new HrmSections() { IdSection = 12, SectionName = System.Windows.Application.Current.FindResource("ItemsForecastHeader").ToString() });
                ////HrmList.Add(new HrmSections() { IdSection = 13, SectionName = System.Windows.Application.Current.FindResource("ReportDashboardHeader").ToString() });

                //HrmList.Add(new HrmSections() { IdSection = 15, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelTargetForecast").ToString() });
                //HrmList.Add(new HrmSections() { IdSection = 16, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelPlantQuota").ToString() });
                //HrmList.Add(new HrmSections() { IdSection = 17, SectionName = System.Windows.Application.Current.FindResource("HrmMainViewModelUsers").ToString() });

                //GeosApplication.Instance.HrmsectionsList = HrmList;

                string[] HrmSelectedStr = GeosApplication.Instance.UserSettings["SelectedHrmSectionLoadData"].Split(',');
                //GeosApplication.Instance.SelectedHrmsectionsList = new List<HrmSections>();

                //if (HrmSelectedStr != null && HrmSelectedStr[0] != "")
                //{
                //    foreach (var item in HrmSelectedStr)
                //    {
                //        GeosApplication.Instance.SelectedHrmsectionsList.Add(GeosApplication.Instance.HrmsectionsList.FirstOrDefault(Hrm => Hrm.IdSection == Convert.ToInt16(item.ToString())));
                //    }
                //}

                //if (GeosApplication.Instance.SelectedHrmsectionsList.Count == 0 || GeosApplication.Instance.SelectedHrmsectionsList[0] == null)
                //{
                //    SelectedHrmSections = new List<object>(GeosApplication.Instance.HrmsectionsList);
                //}
                //else
                //{
                //    SelectedHrmSections = new List<object>(GeosApplication.Instance.SelectedHrmsectionsList);
                //}
                GeosApplication.Instance.Logger.Log("Method FillAllObjectsOneTime() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSectionsDeatils() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsPreferenceChanged = false;
            RequestClose(null, null);            
        }

       
        private void Attendance_KeyDown(KeyEventArgs obj)
        { 
            GeosApplication.Instance.Logger.Log("Method Attendance_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Attendance = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Attendance_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Attendance_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Holiday_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Holiday_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Holiday = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Holiday_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Holiday_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Employee_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Employee_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Employee = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Employee_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Employee_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OrgChart_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method OrgChart_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                OrgChart = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method OrgChart_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OrgChart_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void importAttendance_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method importAttendance_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                ImportAttendance = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method importAttendance_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method importAttendance_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void JobDescription_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method JobDescription_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                JobDescription = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method JobDescription_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method JobDescription_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Leaves_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Leaves_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Leaves = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Leaves_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Leaves_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AttendanceList_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method AttendanceList_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                AttendanceList = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method AttendanceList_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AttendanceList_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

       

        private void SearchEmployee_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchEmployee_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if(validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(),ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                SearchEmployee = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method SearchEmployee_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchEmployee_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        private string GetShortcutKey(KeyEventArgs obj)
        {
            string ShortcutKey = "";
            if (obj.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                ShortcutKey = obj.Key.ToString();
            }
            else
            {
                if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    ShortcutKey = "ctrl";
                }
                if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    if (ShortcutKey != "")
                    {
                        ShortcutKey = ShortcutKey + " + shift";
                    }
                    else
                    {
                        ShortcutKey = "shift";
                    }
                }
                if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                {
                    if (ShortcutKey != "")
                    {
                        ShortcutKey = ShortcutKey + " + alt";
                    }
                    else
                    {
                        ShortcutKey = "alt";
                    }
                }

                if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows)
                {
                    if (ShortcutKey != "")
                    {
                        ShortcutKey = ShortcutKey + " + windows";
                    }
                    else
                    {
                        ShortcutKey = "windows";
                    }
                }
                if (obj.Key == Key.System)
                {
                    if (obj.SystemKey.ToString().Contains("Left") || obj.SystemKey.ToString().Contains("Right"))
                    {
                        //checking
                    }
                    else
                    {
                        ShortcutKey = ShortcutKey + " + " + obj.SystemKey.ToString();
                    }
                }
                else
                {
                    if (obj.Key.ToString().Contains("Left") || obj.Key.ToString().Contains("Right"))
                    {
                        //checking
                    }
                    else
                    {
                        ShortcutKey = ShortcutKey + " + " + obj.Key.ToString();
                    }
                }
            }
            return ShortcutKey;
        }

        private string GetFirstCharCapital(string str)
        {
            if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            else
                return char.ToUpper(str[0]) + str.Substring(1);
        }


        public bool ShortKeyValidate(string shortcutKey)
        {
            bool status = true;

            if(shortcutKey.ToUpper().Contains(Key.Escape.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.LWin.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains("VOLUME") ||
                shortcutKey.ToUpper().Contains("MEDIA") ||
                shortcutKey.ToUpper().Contains("SYSTEM") ||
                shortcutKey.ToUpper().Contains("OEM") ||
                shortcutKey.ToUpper().Contains("NUM") ||
                shortcutKey.ToUpper().Contains(Key.Subtract.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Multiply.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Divide.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Tab.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Add.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Return.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Decimal.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D1.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D2.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D3.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D4.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D5.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D6.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D7.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D8.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D9.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D0.ToString().ToUpper()))
            {
                status = false;
            }
            return status;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion // Methods
    }

}
