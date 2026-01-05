using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.ViewModels;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.Hrm
{
    public sealed class HrmCommon : Prism.Mvvm.BindableBase
    {
        /// <summary>
        /// [001][skale][2019-21-5][GEOS2-273][SP63]Add new fields Company, Organization,Location in employees grid
        /// </summary>
    
        #region Services        
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion
        #region  Declaration

        private decimal? percentageForHighRemainingLeaves;
        private string attendance;
        private string employee;
        private string holiday;
        private string jobDescriptions;
        private string leave;
        private string attendanceList;
        private string organizationChart;
        private string importAttendance;
        private string searchEmployee;

        private static readonly HrmCommon instance = new HrmCommon();
        private ObservableCollection<Employee> employeeListFinal;
        private int colsSpan;
        private bool isPermissionReadOnly;
        private bool isPermissionEnabled;
        private long selectedPeriod;
        private ObservableCollection<Company> userAuthorizedPlantsList;
        private List<object> selectedAuthorizedPlantsList;
        private ObservableCollection<Company> isCompanyList;
        private ObservableCollection<Company> isOrganizationList;
        private ObservableCollection<Company> isLocationList;

        private ObservableCollection<Company> combineIslocationIsorganizationIscompanyList;
        private List<Traveller> travellerList;
        //private List<object> selectedCompanyList;
        //private List<object> selectedOrganizationList;
        //private List<object> selectedLocationList;
        //private List<object> selectedCombineIslocationIsorganizationIscompanyList;  //[001] added

        private PermissionManagement userPermission;
  
        Employee activeEmployee;
        private int idUserPermission;
        private Visibility isVisibleLabelLoadingFullYearAttendanceInBackground;
        private string emailExitEventJDList;
       
        private INavigationService mainWindowINavigationService;

        private ObservableCollection<Employee_trips_transfers> transfersList;

        #endregion

        #region Public Properties

        public ObservableCollection<Employee_trips_transfers> TransfersList
        {
            get
            {
                return transfersList;
            }

            set
            {
                transfersList = value;
                OnPropertyChanged("TransfersList");
            }
        }

        public INavigationService MainWindowINavigationService
        {
            get
            {                
               return mainWindowINavigationService;
            }
            set
            {
                this.mainWindowINavigationService = value;
                OnPropertyChanged(nameof(MainWindowINavigationService));
            }
        }

        public decimal? PercentageForHighRemainingLeaves
        {
            get
            {
                return percentageForHighRemainingLeaves;
            }
            set
            {
                this.percentageForHighRemainingLeaves = value;
                OnPropertyChanged(nameof(PercentageForHighRemainingLeaves));
            }
        }

        public Visibility IsVisibleLabelLoadingFullYearAttendanceInBackground
        {
            get
            {
                return isVisibleLabelLoadingFullYearAttendanceInBackground;
            }
            set
            {
                isVisibleLabelLoadingFullYearAttendanceInBackground = value;
                OnPropertyChanged(nameof(IsVisibleLabelLoadingFullYearAttendanceInBackground));
            }
        }

        #region GEOS2-3887
        // shubham[skadam]GEOS2-3887 Attendance loading message not working properly 23 11 2022
        string attendanceLoadingMessage = string.Empty;
        public string AttendanceLoadingMessage
        {
            get
            {
                return attendanceLoadingMessage;
            }
            set
            {
                attendanceLoadingMessage = value;
                OnPropertyChanged(nameof(AttendanceLoadingMessage));
            }
        }
        #endregion
        public List<GeosProvider> GeosProviderList { get; set; }
        public GeosProvider CurrentGeosProvider { get; private set; }

        public List<long> FinancialYearList { get; set; }
        public string Attendance
        {
            get
            {
                return attendance;
            }

            set
            {
                attendance = value;
                this.OnPropertyChanged("Attendance");
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
                this.OnPropertyChanged("Employee");
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
                this.OnPropertyChanged("Holiday");
            }
        }

        public string JobDescriptions
        {
            get
            {
                return jobDescriptions;
            }

            set
            {
                jobDescriptions = value;
                this.OnPropertyChanged("JobDescriptions");
            }
        }

        public string Leave
        {
            get
            {
                return leave;
            }

            set
            {
                leave = value;
                this.OnPropertyChanged("Leave");
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
                this.OnPropertyChanged("AttendanceList");
            }
        }

        public string OrganizationChart
        {
            get
            {
                return organizationChart;
            }

            set
            {
                organizationChart = value;
                this.OnPropertyChanged("OrganizationChart");
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
                this.OnPropertyChanged("ImportAttendance");
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
                this.OnPropertyChanged("SearchEmployee");
            }
        }
        
        public static HrmCommon Instance
        {
            get { return instance; }
        }
        //public ObservableCollection<Employee> EmployeeListFinal
        //{
        //    get
        //    {
        //        return employeeListFinal;

        //    }

        //    set
        //    {
        //        employeeListFinal = value;
        //        this.OnPropertyChanged("EmployeeListFinal");
        //    }
        //}

        public int ColsSpan
        {
            get
            {
                return colsSpan;
            }

            set
            {
                colsSpan = value;
                this.OnPropertyChanged("ColsSpan");
            }
        }

        public bool IsPermissionReadOnly
        {
            get { return isPermissionReadOnly; }
            set
            {
                isPermissionReadOnly = value;
                OnPropertyChanged("IsPermissionReadOnly");
            }
        }

        public bool IsPermissionEnabled
        {
            get { return isPermissionEnabled; }
            set
            {
                isPermissionEnabled = value;
                OnPropertyChanged("IsPermissionEnabled");
            }

        }

        public long SelectedPeriod
        {
            get { return selectedPeriod; }
            set
            {
                if (selectedPeriod != value)
                {
                    selectedPeriod = value;
                    OnPropertyChanged("SelectedPeriod");
                }
            }

        }

        public ObservableCollection<Company> UserAuthorizedPlantsList
        {
            get
            {
                return userAuthorizedPlantsList;
            }

            set
            {
                userAuthorizedPlantsList = value;
                OnPropertyChanged("UserAuthorizedPlantsList");
            }
        }

        public List<object> SelectedAuthorizedPlantsList
        {
            get
            {
                return selectedAuthorizedPlantsList;
            }

            set
            {
                var bothlistsItemsAreEqual = true;

                if(value==null)
                {
                    selectedAuthorizedPlantsList = null;
                }
                else if (selectedAuthorizedPlantsList == null ||
                    selectedAuthorizedPlantsList.Count != value.Count
                    )
                {
                    bothlistsItemsAreEqual = false;
                }
                else
                {
                    foreach (var item1 in selectedAuthorizedPlantsList)
                    {
                        var item1IdCompany = ((Company)item1).IdCompany;
                        var founditem1IdCompanyInValueList = false;
                        foreach (var item2 in value)
                        {
                            var item2IdCompany = ((Company)item2).IdCompany;
                            if (item1IdCompany == item2IdCompany)
                            {
                                founditem1IdCompanyInValueList = true;
                            }

                        }
                        if (!founditem1IdCompanyInValueList)
                        {
                            bothlistsItemsAreEqual = false;
                            break;
                        }

                    }

                    if (!bothlistsItemsAreEqual)
                    {
                        foreach (var item1 in value)
                        {
                            var item1IdCompany = ((Company)item1).IdCompany;
                            var founditem1IdCompanyInValueList = false;
                            foreach (var item2 in selectedAuthorizedPlantsList)
                            {
                                var item2IdCompany = ((Company)item2).IdCompany;
                                if (item1IdCompany == item2IdCompany)
                                {
                                    founditem1IdCompanyInValueList = true;
                                }

                            }
                            if (!founditem1IdCompanyInValueList)
                            {
                                bothlistsItemsAreEqual = false;
                                break;
                            }

                        }
                    }
                }

                if (!bothlistsItemsAreEqual)
                {
                    selectedAuthorizedPlantsList = value;
                    OnPropertyChanged("SelectedAuthorizedPlantsList");
                }
                
            }
        }
        public ObservableCollection<Company> IsCompanyList
        {
            get
            {
                return isCompanyList;
            }

            set
            {
                isCompanyList = value;
                OnPropertyChanged("IsCompanyList");
            }
        }

        public ObservableCollection<Company> IsOrganizationList
        {
            get
            {
                return isOrganizationList;
            }

            set
            {
                isOrganizationList = value;
                OnPropertyChanged("IsOrganizationList");
            }
        }

        public ObservableCollection<Company> IsLocationList
        {
            get
            {
                return isLocationList;
            }

            set
            {
                isLocationList = value;
                OnPropertyChanged("IsLocationList");
            }
        }
        public List<Traveller> TravellerList
        {
            get { return travellerList; }
            set { travellerList = value; OnPropertyChanged("TravellerList"); }
        }
        //public List<object> SelectedCompanyList
        //{
        //    get
        //    {
        //        return selectedCompanyList;
        //    }

        //    set
        //    {
        //        selectedCompanyList = value;
        //        OnPropertyChanged("SelectedCompanyList");
        //    }
        //}

        //public List<object> SelectedOrganizationList
        //{
        //    get
        //    {
        //        return selectedOrganizationList;
        //    }

        //    set
        //    {
        //        selectedOrganizationList = value;
        //        OnPropertyChanged("SelectedOrganizationList");
        //    }
        //}

        //public List<object> SelectedLocationList
        //{
        //    get
        //    {
        //        return selectedLocationList;
        //    }

        //    set
        //    {
        //        selectedLocationList = value;
        //        OnPropertyChanged("SelectedLocationList");
        //    }
        //}

        //[001] added
        public ObservableCollection<Company> CombineIslocationIsorganizationIscompanyList
        {
            get
            {
                return combineIslocationIsorganizationIscompanyList;
            }

            set
            {
                combineIslocationIsorganizationIscompanyList = value;
                OnPropertyChanged("CombineIslocationIsorganizationIscompanyList");
            }
        }

        public PermissionManagement UserPermission
        {
            get { return userPermission; }
            set
            {
                userPermission = value;
                OnPropertyChanged("UserPermission");
            }
        }
       
        public Employee ActiveEmployee
        {
            get { return activeEmployee; }
            set
            {
                activeEmployee = value;
                OnPropertyChanged("ActiveEmployee");
            }
        }
        public int IdUserPermission
        {
            get { return idUserPermission; }
            set
            {
                idUserPermission = value;
                OnPropertyChanged("IdUserPermission");
            }
        }

        public string EmailExitEventJDList
        {
            get
            {
                return emailExitEventJDList;
            }

            set
            {
                emailExitEventJDList = value;
                OnPropertyChanged("EmailExitEventJDList");
            }
        }
  

        //public List<object> SelectedCombineIslocationIsorganizationIscompanyList
        //{
        //    get
        //    {
        //        return selectedCombineIslocationIsorganizationIscompanyList;
        //    }

        //    set
        //    {
        //        selectedCombineIslocationIsorganizationIscompanyList = value;
        //        OnPropertyChanged("SelectedCombineIslocationIsorganizationIscompanyList");
        //    }
        //}
        //END

        #endregion

        #region Constructor
        public HrmCommon()
        {

        }

        #endregion

        #region Methods
        public void GetShortcuts()
        {
            if (GeosApplication.Instance.UserSettings != null)
            {
                // shortcuts
                // Get shortcut for Attendance
                if (GeosApplication.Instance.UserSettings.ContainsKey("Attendance"))
                {
                    Attendance = GeosApplication.Instance.UserSettings["Attendance"].ToString();
                }

                // Get shortcut for Employee
                if (GeosApplication.Instance.UserSettings.ContainsKey("Employee"))
                {
                    Employee = GeosApplication.Instance.UserSettings["Employee"].ToString();
                }

                // Get shortcut for Holiday
                if (GeosApplication.Instance.UserSettings.ContainsKey("Holiday"))
                {
                    Holiday = GeosApplication.Instance.UserSettings["Holiday"].ToString();
                }

                // Get shortcut for Job Descriptions
                if (GeosApplication.Instance.UserSettings.ContainsKey("JobDescriptions"))
                {
                    JobDescriptions = GeosApplication.Instance.UserSettings["JobDescriptions"].ToString();
                }

                // Get shortcut for Leave
                if (GeosApplication.Instance.UserSettings.ContainsKey("Leave"))
                {
                    Leave = GeosApplication.Instance.UserSettings["Leave"].ToString();
                }

                // Get shortcut for AttendanceList
                if (GeosApplication.Instance.UserSettings.ContainsKey("AttendanceList"))
                {
                    AttendanceList = GeosApplication.Instance.UserSettings["AttendanceList"].ToString();
                }

                // Get shortcut for OrganizationChart
                if (GeosApplication.Instance.UserSettings.ContainsKey("OrganizationChart"))
                {
                    OrganizationChart = GeosApplication.Instance.UserSettings["OrganizationChart"].ToString();
                }

                // Get shortcut for ImportAttendance
                if (GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendance"))
                {
                    ImportAttendance = GeosApplication.Instance.UserSettings["ImportAttendance"].ToString();
                }

                // Get shortcut Search Employee
                if (GeosApplication.Instance.UserSettings.ContainsKey("SearchEmployee"))
                {
                    SearchEmployee = GeosApplication.Instance.UserSettings["SearchEmployee"].ToString();
                }
            }
        }

        public void OpenWindowClickOnShortcutKey(System.Windows.Input.KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method OpenWindowClickOnShortcutKey ...", category: Category.Info, priority: Priority.Low);
            try
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

                string[] Keys = ShortcutKey.Split('+');
                IWorkbenchStartUp objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
                CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();
                if (GeosApplication.Instance.UserSettings != null)
                {
                    // shortcuts
                    // Get shortcut for Attendance
                    if (GeosApplication.Instance.IsHRMPermissionEnabled)//[rdixit][05.11.2024][GEOS2-6499]
                    {
                        if (GeosApplication.Instance.UserSettings.ContainsKey("Attendance"))
                        {
                            string[] StoredKeys = GeosApplication.Instance.UserSettings["Attendance"].ToString().Split('+');
                            int count = getComparedShortcutKeyCount(Keys, StoredKeys);
                            if (count == StoredKeys.Count())
                            {
                                ShowAddNewAttendanceView();
                            }
                        }

                        // Get shortcut for  Employee
                        if (GeosApplication.Instance.UserSettings.ContainsKey("Employee"))
                        {
                            string[] StoredKeys = GeosApplication.Instance.UserSettings["Employee"].ToString().Split('+');

                            int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                            if (count == StoredKeys.Count())
                            {
                                Processing();

                                AddNewEmployeeView addEmployee = new AddNewEmployeeView();
                                AddNewEmployeeViewModel addNewEmployeeViewModel = new AddNewEmployeeViewModel();
                                EventHandler handle = delegate { addEmployee.Close(); };
                                addNewEmployeeViewModel.RequestClose += handle;
                                addNewEmployeeViewModel.Init();
                                addEmployee.DataContext = addNewEmployeeViewModel;
                                addEmployee.ShowDialogWindow();

                            }
                        }

                        // Get shortcut for Holiday
                        if (GeosApplication.Instance.UserSettings.ContainsKey("Holiday"))
                        {
                            string[] StoredKeys = GeosApplication.Instance.UserSettings["Holiday"].ToString().Split('+');

                            int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                            if (count == StoredKeys.Count())
                            {
                                Processing();
                                AddHolidaysView addHolidaysView = new AddHolidaysView();
                                AddHolidaysViewModel addHolidaysViewModel = new AddHolidaysViewModel();
                                EventHandler handle = delegate { addHolidaysView.Close(); };
                                addHolidaysViewModel.RequestClose += handle;
                                addHolidaysView.DataContext = addHolidaysViewModel;
                                List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                                ObservableCollection<CompanyHoliday> CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                                addHolidaysViewModel.ExistHolidays(CompanyHolidays);
                                addHolidaysViewModel.Init();
                                addHolidaysViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddHolidaysInformation").ToString();
                                addHolidaysViewModel.IsNew = true;
                                addHolidaysView.ShowDialog();
                            }
                            //Account = GeosApplication.Instance.UserSettings["Account"].ToString();
                        }

                        // Get shortcut for Job Descriptions
                        if (GeosApplication.Instance.UserSettings.ContainsKey("JobDescriptions"))
                        {
                            string[] StoredKeys = GeosApplication.Instance.UserSettings["JobDescriptions"].ToString().Split('+');

                            int count = getComparedShortcutKeyCount(Keys, StoredKeys);
                            if (count == StoredKeys.Count())
                            {
                                Processing();
                                AddNewJobDescriptionsView addNewJobDescriptionsView = new AddNewJobDescriptionsView();
                                AddNewJobDescriptionsViewModel addNewJobDescriptionsViewModel = new AddNewJobDescriptionsViewModel();
                                EventHandler handle = delegate { addNewJobDescriptionsView.Close(); };
                                addNewJobDescriptionsViewModel.RequestClose += handle;
                                addNewJobDescriptionsView.DataContext = addNewJobDescriptionsViewModel;
                                ObservableCollection<JobDescription> JobDescriptionList = new ObservableCollection<JobDescription>(HrmService.GetAllJobDescriptions_V2046());
                                addNewJobDescriptionsViewModel.Init(JobDescriptionList);
                                addNewJobDescriptionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddJobDescriptionInformation").ToString();
                                addNewJobDescriptionsViewModel.IsNew = true;
                                addNewJobDescriptionsView.ShowDialog();
                            }
                            //Appointment = GeosApplication.Instance.UserSettings["Appointment"].ToString();
                        }

                        // Get shortcut for Leave
                        if (GeosApplication.Instance.UserSettings.ContainsKey("Leave"))
                        {
                            string[] StoredKeys = GeosApplication.Instance.UserSettings["Leave"].ToString().Split('+');
                            int count = getComparedShortcutKeyCount(Keys, StoredKeys);
                            if (count == StoredKeys.Count())
                            {
                                ShowAddNewLeaveView();
                            }
                            //Call = GeosApplication.Instance.UserSettings["Call"].ToString();
                        }
                    }
                    // Get shortcut for AttendanceList
                    if (GeosApplication.Instance.UserSettings.ContainsKey("AttendanceList"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["AttendanceList"].ToString().Split('+');
                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);
                        if (count == StoredKeys.Count())
                        {
                            Processing();
                            ExportAttendanceView exportAttendanceView = new ExportAttendanceView();
                            ExportAttendanceViewModel exportAttendanceViewModel = new ExportAttendanceViewModel();
                            EventHandler handle = delegate { exportAttendanceView.Close(); };
                            exportAttendanceViewModel.RequestClose += handle;
                            exportAttendanceView.DataContext = exportAttendanceViewModel;
                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                            exportAttendanceView.ShowDialog();
                        }
                        //Task = GeosApplication.Instance.UserSettings["Task"].ToString();
                    }

                    // Get shortcut for Organization Chart
                    if (GeosApplication.Instance.UserSettings.ContainsKey("OrganizationChart"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["OrganizationChart"].ToString().Split('+');
                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);
                        if (count == StoredKeys.Count())
                        {
                            Processing();
                            EmployeeOrganizationViewModel employeeOrganizationViewModel = new EmployeeOrganizationViewModel();
                            employeeOrganizationViewModel.FillDepartmentListByPlant();
                            employeeOrganizationViewModel.OrganizationChartExportToExcel(obj);
                        }
                        //Email = GeosApplication.Instance.UserSettings["Email"].ToString();
                    }

                    if (GeosApplication.Instance.IsHRMPermissionEnabled)//[rdixit][05.11.2024][GEOS2-6499]
                    {
                        // Get shortcut for Import Attendance
                        if (GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendance"))
                        {
                            string[] StoredKeys = GeosApplication.Instance.UserSettings["ImportAttendance"].ToString().Split('+');
                            int count = getComparedShortcutKeyCount(Keys, StoredKeys);
                            if (count == StoredKeys.Count())
                            {
                                Processing();
                                ImportAttendanceFileView importAttendanceFileView = new ImportAttendanceFileView();
                                ImportAttendanceFileViewModel importAttendanceFileViewModel = new ImportAttendanceFileViewModel();
                                List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                                ObservableCollection<EmployeeAttendance> EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>(HrmService.GetSelectedIdCompanyEmployeeAttendance_V2045(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                                importAttendanceFileViewModel.Init(EmployeeAttendanceList);
                                EventHandler handle = delegate { importAttendanceFileView.Close(); };
                                importAttendanceFileViewModel.RequestClose += handle;
                                importAttendanceFileView.DataContext = importAttendanceFileViewModel;
                                importAttendanceFileView.ShowDialog();
                            }
                            //Email = GeosApplication.Instance.UserSettings["Email"].ToString();
                        }
                    }
                    // Get shortcut Search SearchEmployee
                    if (GeosApplication.Instance.UserSettings.ContainsKey("SearchEmployee"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["SearchEmployee"].ToString().Split('+');
                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);
                        if (count == StoredKeys.Count())
                        {
                            Processing();
                            SearchEmployeeViewModel searchEmployeeViewModel = new SearchEmployeeViewModel();
                            SearchEmployeeView searchEmployeeView = new SearchEmployeeView();
                            EventHandler handle = delegate { searchEmployeeView.Close(); };
                            searchEmployeeViewModel.RequestClose += handle;
                            searchEmployeeView.DataContext = searchEmployeeViewModel;
                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                            searchEmployeeView.ShowDialog();
                        }
                        //SearchOpportunityOrOrder = GeosApplication.Instance.UserSettings["SearchOpportunityOrOrder"].ToString();
                    }
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method OpenWindowClickOnShortcutKey....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenWindowClickOnShortcutKey...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void ShowAddNewAttendanceView()
        {
            Processing();
            AttendanceView addAttendanceView = new AttendanceView();
            AttendanceViewModel addAttendanceViewModel = new AttendanceViewModel(addAttendanceView);
            //EventHandler handle = delegate { addAttendanceView.Close(); };
            //addAttendanceViewModel.RequestClose += handle;
            //addAttendanceView.DataContext = addAttendanceViewModel;
            //addAttendanceViewModel.IsSplitVisible = false;
            //addAttendanceViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
            //List<Company> PlantOwners = new List<Company>();

            //if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            //{
            //    PlantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
            //}
            ////addAttendanceViewModel.SelectedPlantList = PlantOwners;

            //var plantOwnersIds = string.Join(",", PlantOwners.Select(i => i.IdCompany));
            //ObservableCollection<EmployeeAttendance> EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>(HrmService.GetSelectedIdCompanyEmployeeAttendance_V2045(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));

            //ObservableCollection<EmployeeLeave> EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
            //ObservableCollection<Employee> EmployeeListFinal = new ObservableCollection<Employee>(); ;

            //foreach (var item in addAttendanceViewModel.SelectedPlantList)
            //{
            //    ObservableCollection<Employee> tempEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2110(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));

            //    if (EmployeeListFinal == null)
            //    {
            //        EmployeeListFinal = new ObservableCollection<Employee>();
            //    }

            //    EmployeeListFinal.AddRange(tempEmployeeList);
            //}
            var objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
            var GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
            var CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();
            var workingPlantId = CurrentGeosProvider.IdCompany.ToString();

            addAttendanceViewModel.Init(null, null, DateTime.Today, DateTime.Today,
                workingPlantId, null, null);
            //addAttendanceViewModel.IsNew = true;
            //addAttendanceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewAttendance").ToString();
            //addAttendanceViewModel.EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
            addAttendanceView.ShowDialog();
        }

        public void ShowAddNewLeaveView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAddNewLeaveView ...", category: Category.Info, priority: Priority.Low);

                FillHrmDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();
                AddNewLeaveViewWithSplitting addNewLeaveView = new AddNewLeaveViewWithSplitting();
                AddNewLeaveViewModelWithSplitting addNewLeaveViewModel = new AddNewLeaveViewModelWithSplitting(addNewLeaveView);
                //addNewLeaveView.DataContext = addNewLeaveViewModel;
                var objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
                CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();
                var workingPlantId = CurrentGeosProvider.IdCompany.ToString();
                //List<Company> plantOwners = new List<Company>();
                //List<Company> plantOwnersIds = new List<Company>();
                //if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                //{
                //    plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                //    plantOwnersIds = plantOwners.Where(i => i.IdCompany == CurrentGeosProvider.IdCompany).ToList();
                //}
                //var plantOwnersIdsJoined = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().AsEnumerable().Select(i => i.IdCompany));
                //addNewLeaveViewModel.SelectedPlantList = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                //addNewLeaveViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                //object selectedEmployee = null;
                //ObservableCollection<EmployeeLeave> EmployeeLeaves = new ObservableCollection<EmployeeLeave>();// HrmService.GetEmployeeLeavesBySelectedIdCompany_V2110(plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                //DateTime? fromDate;
                //DateTime? toDate;
                //FillHrmDataInObjectsByCallingLatestServiceMethods.CalculateFromDateToDateAtSelectedPeriodForOneMonthOnly(out fromDate, out toDate, null, null);
                //ObservableCollection<EmployeeAttendance> EmployeeAttendanceListForNewLeave = new ObservableCollection<EmployeeAttendance>();// HrmService.GetSelectedIdCompanyEmployeeAttendance_V2110(plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission,(DateTime)fromDate, (DateTime)toDate));

                // ObservableCollection<Employee> EmployeeListFinalForLeaves = new ObservableCollection<Employee>();

                //foreach (var item in addNewLeaveViewModel.SelectedPlantList)
                //{
                //ObservableCollection<Employee> EmployeeListFinalForLeaves =
                //    new ObservableCollection<Employee>(
                //    HrmService.GetAllEmployeesForLeaveByIdCompany_V2120(
                //    plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod,
                //    HrmCommon.Instance.ActiveEmployee.Organization,
                //    HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                //    HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));
                //    if (EmployeeListFinalForLeaves == null)
                //    {
                //        EmployeeListFinalForLeaves = new ObservableCollection<Employee>();
                //    }
                //    EmployeeListFinalForLeaves.AddRange(tempEmployeeList);
                //}

                EmployeeAttendanceViewModel employeeAttendanceViewModelObj = null;
                Employee selectedEmployee = null;
                var selectedStartDate = DateTime.Today;
                var selectedEndDate = DateTime.Today;
                LookupValue defaultLeaveType = null;
                bool? isAllDayEventParameter = null;
                List<EmployeeLeave> listOfleaveStartTimeAndEndTime = new List<EmployeeLeave>();
                int defaultIdCompanyShift = 0;

                if (MainWindowINavigationService != null && MainWindowINavigationService.Current is EmployeeAttendanceViewModel)
                {
                    employeeAttendanceViewModelObj = (EmployeeAttendanceViewModel)MainWindowINavigationService.Current;

                    if (employeeAttendanceViewModelObj != null &&
                        employeeAttendanceViewModelObj.SelectedItem is Employee &&
                        employeeAttendanceViewModelObj.IsSchedulerViewVisible == Visibility.Visible)
                    {
                        selectedEmployee = (Employee)employeeAttendanceViewModelObj.SelectedItem;

                        if (selectedEmployee != null)
                        {
                            try
                            {
                                GeosApplication.Instance.Logger.Log("Start setting default data.", category: Category.Info, priority: Priority.Low);

                                defaultLeaveType = GeosApplication.Instance.EmployeeLeaveList.FirstOrDefault(x => x.IdLookupValue == 241); // 241 is Compensatory

                                var employeeAttendanceList = employeeAttendanceViewModelObj.EmployeeAttendanceList.Where(x => x.IdEmployee == selectedEmployee.IdEmployee).ToList();
                                var employeeLeaveList = employeeAttendanceViewModelObj.EmployeeLeaves.Where(x => x.IdEmployee == selectedEmployee.IdEmployee).ToList();
                                var SelectedStartDateStartOfDay = employeeAttendanceViewModelObj.SelectedStartDate.Date;
                                var SelectedEndDateEndOfDay = employeeAttendanceViewModelObj.SelectedEndDate.Date.Add(new TimeSpan(23, 59, 59));
                                
                                // Suppose, Existing leave start date= 11-Jan-2021 8:00:00, End Date= 20-Jan-2021 17:00:00
                                //Scenario 1 - Selected Date range is exactly between the existing leave date range
                                // Selected date range on calendar is 12-Jan-2021 8:00:00 to 13-Jan-2021 17:00:00
                                //Scenario 2 - Selected Date range is NOT exactly between the existing leave date range. It starts before existing leave.
                                // Selected date range on calendar is 1-Jan-2021 8:00:00 to 11-Jan-2021 17:00:00
                                //Scenario 3 - Selected Date range is NOT exactly between the existing leave date range. It ends after existing leave.
                                // Selected date range on calendar is 20-Jan-2021 8:00:00 to 31-Jan-2021 17:00:00
                                //Scenario 4 - Opposite to Scenario 1. The existing leave date range is exactly between Selected Date range.
                                // Selected date range on calendar is 1-Jan-2021 8:00:00 to 31-Jan-2021 17:00:00
                                var overlappingAttendanceList = employeeAttendanceList.Where(
                                    x => (SelectedStartDateStartOfDay >= x.StartDate && SelectedEndDateEndOfDay <= x.EndDate) ||
                                         (SelectedStartDateStartOfDay < x.StartDate && SelectedEndDateEndOfDay <= x.EndDate && SelectedEndDateEndOfDay >= x.StartDate) ||
                                         (SelectedStartDateStartOfDay >= x.StartDate && SelectedEndDateEndOfDay > x.EndDate && SelectedStartDateStartOfDay <= x.EndDate) ||
                                         (SelectedStartDateStartOfDay <= x.StartDate && SelectedEndDateEndOfDay >= x.EndDate)
                                    ).ToList().OrderBy(x=>x.StartDate).ToList();

                                var overlappingLeavesList = employeeLeaveList.Where(
                                    x => (SelectedStartDateStartOfDay >= x.StartDate && SelectedEndDateEndOfDay <= x.EndDate) ||
                                         (SelectedStartDateStartOfDay < x.StartDate && SelectedEndDateEndOfDay <= x.EndDate && SelectedEndDateEndOfDay >= x.StartDate) ||
                                         (SelectedStartDateStartOfDay >= x.StartDate && SelectedEndDateEndOfDay > x.EndDate && SelectedStartDateStartOfDay <= x.EndDate) ||
                                         (SelectedStartDateStartOfDay <= x.StartDate && SelectedEndDateEndOfDay >= x.EndDate)
                                    ).ToList().OrderBy(x => x.StartDate).ToList();

                                var overlappingAttendanceNotFound = (overlappingAttendanceList == null || overlappingAttendanceList.Count == 0);
                                var overlappingLeavesNotFound = (overlappingLeavesList == null || overlappingLeavesList.Count == 0);
                                if (overlappingAttendanceNotFound && overlappingLeavesNotFound)
                                {
                                    selectedStartDate = employeeAttendanceViewModelObj.SelectedStartDate;
                                    selectedEndDate = employeeAttendanceViewModelObj.SelectedEndDate;
                                    isAllDayEventParameter = true;
                                }
                                else if (!overlappingAttendanceNotFound)
                                {
                                    // GEOS2-3081 
                                    // If the Selected employee attendance day have records (attendance total time ≠ 0) and the if the attendance total time in the selected day < (total working time of the shift) 
                                    //a.Use the selected day, “All day event” not checked (false).
                                    //b.	Propose Add leave to each gap in maximum 3 gaps (similar to Split Attendance) :

                                    // Scenarios for single day to find Non-Working Time Duration in the shift timing. It is Possible Leave duration.
                                    // 1) 1 Possible Leave duration found before the work start.
                                    // 2) 1 Possible Leave duration found after the work end.

                                    DateTime firstItemDate = overlappingAttendanceList.FirstOrDefault().StartDate.Date;

                                    var selectedEmployeeShifts = HrmService.GetEmployeeShiftsByIdEmployee(selectedEmployee.IdEmployee);

                                    var currentCompanyShift = selectedEmployeeShifts.FirstOrDefault(x =>
                                    x.CompanyShift.IdCompanyShift ==
                                    overlappingAttendanceList.FirstOrDefault().CompanyShift.IdCompanyShift); // overlappingAttendanceList.FirstOrDefault().CompanyShift;

                                    var currentDayOfWeek = employeeAttendanceViewModelObj.SelectedStartDate.DayOfWeek;

                                    var StartTime = GetShiftStartTime((int)currentDayOfWeek, currentCompanyShift.CompanyShift, true, firstItemDate);
                                    var EndTime = GetShiftStartTime((int)currentDayOfWeek, currentCompanyShift.CompanyShift, false, firstItemDate);

                                    if (currentCompanyShift.CompanyShift.IsNightShift == 1)
                                    {
                                        EndTime = EndTime.AddDays(1);
                                    }

                                    bool selectedCalendarDurationIsOnSingleDayOnlyAndDayShift = !overlappingAttendanceList.Exists(x => 
                                    (x.StartDate.Date != firstItemDate && x.EndDate.Date != firstItemDate)) && (currentCompanyShift.CompanyShift.IsNightShift == 0);
                                    bool selectedCalendarDurationIsOnTwoDaysOnlyAndNightShift = !overlappingAttendanceList.Exists(x => 
                                    (x.StartDate.Date != firstItemDate && x.EndDate.Date != firstItemDate.AddDays(1))) && (currentCompanyShift.CompanyShift.IsNightShift == 1);

                                    if (!(selectedCalendarDurationIsOnSingleDayOnlyAndDayShift || selectedCalendarDurationIsOnTwoDaysOnlyAndNightShift))
                                    {
                                        GeosApplication.Instance.Logger.Log("For showing new default values, selected calender days duration should be one day for day shift and two days for night shift.", 
                                            category: Category.Info, priority: Priority.Low);
                                    } 
                                    else if (selectedCalendarDurationIsOnSingleDayOnlyAndDayShift || selectedCalendarDurationIsOnTwoDaysOnlyAndNightShift)
                                    {

                                        var theAttendanceIsFullDay = (overlappingAttendanceList.FirstOrDefault().StartDate <= StartTime &&
                                        overlappingAttendanceList.FirstOrDefault().EndDate >= EndTime);

                                        var theWorkStartedAtShiftStartCorrectly =
                                            (overlappingAttendanceList.FirstOrDefault().StartDate <= StartTime);

                                        var theWorkEndAtShiftEndCorrectly =
                                            (overlappingAttendanceList.LastOrDefault().EndDate >= EndTime);

                                        if (!theAttendanceIsFullDay)
                                        {
                                            isAllDayEventParameter = false;
                                            
                                            for (int i = 0; i < overlappingAttendanceList.Count; i++)
                                            {
                                                if(i==0 && !theWorkStartedAtShiftStartCorrectly)  //first attendance in the working day
                                                {
                                                    //if (!theWorkStartedAtShiftStartCorrectly)
                                                    //{
                                                        EmployeeLeave employeeLeave = new EmployeeLeave();
                                                        employeeLeave.StartDate = StartTime;
                                                        employeeLeave.EndDate = overlappingAttendanceList[0].StartDate.AddMinutes(-1);
                                                        listOfleaveStartTimeAndEndTime.Add(employeeLeave);
                                                    //}
                                                }

                                                if (i < (overlappingAttendanceList.Count - 1)) // check this is not last item
                                                {
                                                    //Find Gap between current item and next item
                                                    EmployeeLeave employeeLeave = new EmployeeLeave();
                                                    var currentItem = overlappingAttendanceList[i];
                                                    var nextItem = overlappingAttendanceList[i+1];

                                                    if (currentItem.EndDate!= nextItem.StartDate && currentItem.EndDate.AddMinutes(1) != nextItem.StartDate)
                                                    {
                                                        employeeLeave.StartDate = currentItem.EndDate.AddMinutes(1);
                                                        employeeLeave.EndDate = nextItem.StartDate.AddMinutes(-1);
                                                        listOfleaveStartTimeAndEndTime.Add(employeeLeave);
                                                    }
                                                }

                                                if (i == (overlappingAttendanceList.Count - 1) && !theWorkEndAtShiftEndCorrectly) //Last attendance in the working day
                                                {
                                                    //if (!theWorkEndAtShiftEndCorrectly)
                                                    //{
                                                    EmployeeLeave employeeLeave = new EmployeeLeave();
                                                    employeeLeave.StartDate = overlappingAttendanceList[i].EndDate.AddMinutes(1);
                                                    employeeLeave.EndDate = EndTime;
                                                    listOfleaveStartTimeAndEndTime.Add(employeeLeave);
                                                    //}
                                                }
                                            }

                                            //if (!theWorkStartedAtShiftStartCorrectly)
                                            //{
                                            //    EmployeeLeave employeeLeave = new EmployeeLeave();
                                            //    employeeLeave.StartDate = StartTime;
                                            //    employeeLeave.EndDate = overlappingAttendanceList.FirstOrDefault().StartDate.AddMinutes(-1);
                                            //    listOfleaveStartTimeAndEndTime.Add(employeeLeave);
                                            //}

                                            //if (!theWorkEndAtShiftEndCorrectly)
                                            //{
                                            //    EmployeeLeave employeeLeave = new EmployeeLeave();
                                            //    employeeLeave.StartDate = overlappingAttendanceList.FirstOrDefault().EndDate.AddMinutes(1);

                                            //    if (overlappingAttendanceList.Count == 1)
                                            //    {
                                            //        employeeLeave.EndDate = EndTime;
                                            //    }
                                            //    else if (overlappingAttendanceList.Count > 1)
                                            //    {
                                            //        employeeLeave.EndDate = overlappingAttendanceList[1].StartDate.AddMinutes(-1);
                                            //    }

                                            //    listOfleaveStartTimeAndEndTime.Add(employeeLeave);
                                            //}

                                            //if (!theWorkStartedAtShiftStartCorrectly && !theWorkEndAtShiftEndCorrectly &&
                                            //    overlappingAttendanceList.Count > 1)
                                            //{
                                            //    EmployeeLeave employeeLeave = new EmployeeLeave();
                                            //    employeeLeave.StartDate = overlappingAttendanceList[1].EndDate.AddMinutes(1);
                                            //    employeeLeave.EndDate = EndTime;
                                            //    listOfleaveStartTimeAndEndTime.Add(employeeLeave);
                                            //}

                                            //When there are more than 3 leave gaps found, show default 1 leave VIEW
                                            if (listOfleaveStartTimeAndEndTime.Count > 3)
                                            {
                                                //listOfleaveStartTimeAndEndTime.RemoveRange(1, listOfleaveStartTimeAndEndTime.Count - 1);
                                                listOfleaveStartTimeAndEndTime.Clear();
                                            }
                                            
                                            // This is used to show one leave only VIEW
                                            //if (listOfleaveStartTimeAndEndTime.Count == 1)
                                            //{
                                            //    selectedStartDate = employeeAttendanceViewModelObj.SelectedStartDate;
                                            //    selectedEndDate = employeeAttendanceViewModelObj.SelectedEndDate;
                                            //    isAllDayEventParameter = true;
                                            //}

                                            if (listOfleaveStartTimeAndEndTime.Count >= 1)
                                            {
                                                selectedStartDate = listOfleaveStartTimeAndEndTime[0].StartDate.Value;
                                                selectedEndDate = listOfleaveStartTimeAndEndTime[0].EndDate.Value;
                                                defaultIdCompanyShift = overlappingAttendanceList.FirstOrDefault().CompanyShift.IdCompanyShift;
                                            }
                                        }
                                    }
                                }
                                GeosApplication.Instance.Logger.Log("End setting default data.", category: Category.Info, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                                GeosApplication.Instance.Logger.Log("Get an error in setting default data." + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                }

                addNewLeaveViewModel.Init(null,
                        selectedEmployee, selectedStartDate, selectedEndDate,
                        1, workingPlantId, null, defaultLeaveType,
                        isAllDayEventParameter, listOfleaveStartTimeAndEndTime, defaultIdCompanyShift);

                //addNewLeaveViewModel.IsNew = true;
                //addNewLeaveViewModel.LeaveTitle = System.Windows.Application.Current.FindResource("AddNewLeave").ToString();
                FillHrmDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
                addNewLeaveView.ShowDialog();

                if (addNewLeaveViewModel.IsSave)
                {
                    foreach (var item in addNewLeaveViewModel.NewEmployeeLeaveList)
                    {
                        if (item.StartDate.HasValue)
                        {
                            item.StartTime = item.StartDate.Value.TimeOfDay;
                        }
                        if (item.EndDate.HasValue)
                        {
                            item.EndTime = item.EndDate.Value.TimeOfDay;
                        }
                    }
                    //[rdixit][18.12.2024][GEOS2-6571]
                    if(employeeAttendanceViewModelObj==null)
                    {
                        employeeAttendanceViewModelObj = new EmployeeAttendanceViewModel();
                    }
                    if (employeeAttendanceViewModelObj?.EmployeeLeaves == null)
                        employeeAttendanceViewModelObj.EmployeeLeaves = new CustomObservableCollection<EmployeeLeave>();
                    employeeAttendanceViewModelObj.EmployeeLeaves.AddRange(addNewLeaveViewModel.NewEmployeeLeaveList);
                    employeeAttendanceViewModelObj.SelectItemForScheduler(null);
                }

                GeosApplication.Instance.Logger.Log("Method ShowAddNewLeaveView....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowAddNewLeaveView...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private DateTime GetShiftStartTime(int dayOfWeek, CompanyShift companyShift, bool isStartTime, DateTime? selectedDate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetShiftStartTime()...", category: Category.Info, priority: Priority.Low);
                switch (dayOfWeek)
                {
                    case 0:
                        TimeSpan SunStartTime = companyShift.SunStartTime;
                        TimeSpan SunEndTime = companyShift.SunEndTime;
                        if (isStartTime)
                            return selectedDate.Value.Date.AddHours(SunStartTime.Hours).AddMinutes(SunStartTime.Minutes);
                        else
                            return selectedDate.Value.Date.AddHours(SunEndTime.Hours).AddMinutes(SunEndTime.Minutes);

                    case 1:
                        TimeSpan MonStartTime = companyShift.MonStartTime;
                        TimeSpan MonEndTime = companyShift.MonEndTime;
                        if (isStartTime)
                            return selectedDate.Value.Date.AddHours(MonStartTime.Hours).AddMinutes(MonStartTime.Minutes);
                        else
                            return  selectedDate.Value.Date.AddHours(MonEndTime.Hours).AddMinutes(MonEndTime.Minutes);

                    case 2:
                        TimeSpan TueStartTime = companyShift.TueStartTime;
                        TimeSpan TueEndTime = companyShift.TueEndTime;
                        if (isStartTime)
                            return selectedDate.Value.Date.AddHours(TueStartTime.Hours).AddMinutes(TueStartTime.Minutes);
                        else
                            return  selectedDate.Value.Date.AddHours(TueEndTime.Hours).AddMinutes(TueEndTime.Minutes);

                    case 3:
                        TimeSpan WedStartTime = companyShift.WedStartTime;
                        TimeSpan WedEndTime = companyShift.WedEndTime;
                        if (isStartTime)
                            return selectedDate.Value.Date.AddHours(WedStartTime.Hours).AddMinutes(WedStartTime.Minutes);
                        else
                            return  selectedDate.Value.Date.AddHours(WedEndTime.Hours).AddMinutes(WedEndTime.Minutes);

                    case 4:
                        TimeSpan ThuStartTime = companyShift.ThuStartTime;
                        TimeSpan ThuEndTime = companyShift.ThuEndTime;
                        if (isStartTime)
                            return selectedDate.Value.Date.AddHours(ThuStartTime.Hours).AddMinutes(ThuStartTime.Minutes);
                        else
                            return  selectedDate.Value.Date.AddHours(ThuEndTime.Hours).AddMinutes(ThuEndTime.Minutes);

                    case 5:
                        TimeSpan FriStartTime = companyShift.FriStartTime;
                        TimeSpan FriEndTime = companyShift.FriEndTime;
                        if (isStartTime)
                            return selectedDate.Value.Date.AddHours(FriStartTime.Hours).AddMinutes(FriStartTime.Minutes);
                        else
                            return  selectedDate.Value.Date.AddHours(FriEndTime.Hours).AddMinutes(FriEndTime.Minutes);

                    case 6:
                        TimeSpan SatStartTime = companyShift.SatStartTime;
                        TimeSpan SatEndTime = companyShift.SatEndTime;
                        if (isStartTime)
                            return selectedDate.Value.Date.AddHours(SatStartTime.Hours).AddMinutes(SatStartTime.Minutes);
                        else
                            return  selectedDate.Value.Date.AddHours(SatEndTime.Hours).AddMinutes(SatEndTime.Minutes);

                    default:
                        return (DateTime)selectedDate;
                }
                GeosApplication.Instance.Logger.Log("Method GetShiftStartTime()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetShiftStartTime()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                return new DateTime();

            }
        }

        private void Processing()
        {
            if (!DXSplashScreen.IsActive)
            {
                //DXSplashScreen.Show<SplashScreenView>(); 
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

        private int getComparedShortcutKeyCount(string[] Keys, string[] StoredKeys)
        {
            int count = 0;
            if (Keys.Count() == StoredKeys.Count())
            {
                foreach (string key in Keys)
                {
                    foreach (string storedKey in StoredKeys)
                    {
                        if (key.ToUpper().TrimStart().TrimEnd() == storedKey.ToUpper().TrimStart().TrimEnd())
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }
        #endregion
    }
}