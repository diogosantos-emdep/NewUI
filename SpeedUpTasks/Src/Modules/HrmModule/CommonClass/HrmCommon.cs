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
        private List<long> financialYearList;
        private ObservableCollection<Company> isOrganizationList;
        private ObservableCollection<Company> isLocationList;

        private ObservableCollection<Company> combineIslocationIsorganizationIscompanyList;

        //private List<object> selectedCompanyList;
        //private List<object> selectedOrganizationList;
        //private List<object> selectedLocationList;
        //private List<object> selectedCombineIslocationIsorganizationIscompanyList;  //[001] added

        private PermissionManagement userPermission;
  
        Employee activeEmployee;
        private int idUserPermission;

        #endregion

        #region Public Properties
        public List<GeosProvider> GeosProviderList { get; set; }
        public GeosProvider CurrentGeosProvider { get; private set; }
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
        public ObservableCollection<Employee> EmployeeListFinal
        {
            get
            {
                return employeeListFinal;

            }

            set
            {
                employeeListFinal = value;
                this.OnPropertyChanged("EmployeeListFinal");
            }
        }

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
                selectedPeriod = value;
                OnPropertyChanged("SelectedPeriod");
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
                if (selectedAuthorizedPlantsList != null)
                {
                    if (selectedAuthorizedPlantsList.Count == value.Count &&
                    selectedAuthorizedPlantsList.Count == 1 &&
                    ((Company)selectedAuthorizedPlantsList[0]).IdCompany == ((Company)value[0]).IdCompany)
                        return;
                }

                selectedAuthorizedPlantsList = value;
                OnPropertyChanged("SelectedAuthorizedPlantsList");
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
        
        public List<long> FinancialYearList
        {
            get
            {
                return financialYearList;
            }

            set
            {
                financialYearList = value;
                OnPropertyChanged("FinancialYearList");
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
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Attendance"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["Attendance"].ToString().Split('+');
                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);
                        if (count == StoredKeys.Count())
                        {
                            Processing();
                            AttendanceView addAttendanceView = new AttendanceView();
                            AttendanceViewModel addAttendanceViewModel = new AttendanceViewModel();
                            EventHandler handle = delegate { addAttendanceView.Close(); };
                            addAttendanceViewModel.RequestClose += handle;
                            addAttendanceView.DataContext = addAttendanceViewModel;
                            addAttendanceViewModel.IsSplitVisible = false;
                            addAttendanceViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                            List<Company> PlantOwners = new List<Company>();

                            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                            {
                                PlantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                            }
                            addAttendanceViewModel.SelectedPlantList = PlantOwners;

                            var plantOwnersIds = string.Join(",", PlantOwners.Select(i => i.IdCompany));
                            ObservableCollection<EmployeeAttendance> EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>(HrmService.GetSelectedIdCompanyEmployeeAttendance_V2045(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                            addAttendanceViewModel.Init(EmployeeAttendanceList, null, DateTime.Today, DateTime.Today);
                            addAttendanceViewModel.IsNew = true;
                            addAttendanceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewAttendance").ToString();
                            addAttendanceViewModel.EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                            addAttendanceView.ShowDialog();
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
                            Processing();
                            AddNewLeaveView addNewLeaveView = new AddNewLeaveView();
                            AddNewLeaveViewModel addNewLeaveViewModel = new AddNewLeaveViewModel();
                            EventHandler handle = delegate { addNewLeaveView.Close(); };
                            addNewLeaveViewModel.RequestClose += handle;
                            addNewLeaveView.DataContext = addNewLeaveViewModel;
                            addNewLeaveViewModel.WorkingPlantId = CurrentGeosProvider.IdCompany.ToString();
                            List<Company> plantOwners = new List<Company>();
                            List<Company> plantOwnersIds = new List<Company>();
                            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                            {
                                plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                                plantOwnersIds = plantOwners.Where(i => i.IdCompany == CurrentGeosProvider.IdCompany).ToList();
                            }
                            addNewLeaveViewModel.SelectedPlantList = plantOwners;
                            addNewLeaveViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                            object selectedEmployee = null;
                            ObservableCollection<EmployeeLeave> EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2045(plantOwnersIds.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                            addNewLeaveViewModel.Init(EmployeeLeaves, selectedEmployee, DateTime.Today, DateTime.Today, 2);
                            addNewLeaveViewModel.IsNew = true;
                            addNewLeaveViewModel.LeaveTitle = System.Windows.Application.Current.FindResource("AddNewLeave").ToString();
                            addNewLeaveView.ShowDialog();
                        }
                        //Call = GeosApplication.Instance.UserSettings["Call"].ToString();
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