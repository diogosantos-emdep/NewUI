using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Modules.Hrm.ViewModels;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Prism.Logging;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.Hrm;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using System.ServiceModel;
using Emdep.Geos.Data.Common.Hrm;
using System.Collections.ObjectModel;


namespace Workbench.ViewModels
{
    public class HrmWindowViewModel : ViewModelBase,INotifyPropertyChanged, IDisposable
    {
        #region Services        
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion 

        #region Declaration

        private string showHideMenuButtonToolTip;

        private string moduleName;
        private string visible;

        public string ShowHideMenuButtonToolTip
        {
            get { return showHideMenuButtonToolTip; }
            set
            {
                showHideMenuButtonToolTip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowHideMenuButtonToolTip"));
            }
        }

        public string ModuleName
        {
            get { return moduleName; }
            set
            {
                moduleName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowHideMenuButtonToolTip"));
            }
        }

        #endregion

        #region Properties
        public GeosProvider CurrentGeosProvider { get; private set; }
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }
        public List<GeosProvider> GeosProviderList { get; set; }

        #endregion

        #region Public Commands

        public ICommand Option_ClickCommand { get; set; }

        public ICommand HideTileBarButtonClickCommand { get; set; }

        public ICommand CommandTextInput { get; set; }
       

        #endregion  // Public Commands

        #region Constructor

        public HrmWindowViewModel()
        {
            HideTileBarButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(HideTileBarButtonClickCommandAction);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString();  //Hide menu
            Option_ClickCommand = new DelegateCommand<object>(Option_Click);
            if (GeosApplication.Instance.ObjectPool.ContainsKey("GeosModuleNameList"))
            {
                ModuleName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 7).Select(s => s.Name).FirstOrDefault();
            }
            else
            {
                ModuleName = "Hrm";
            }

            HrmCommon.Instance.GetShortcuts();

            //set hide/show shortcuts on permissions
            Visible = Visibility.Visible.ToString();

            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                Visible = Visibility.Hidden.ToString();
            }
            else
            {
                Visible = Visibility.Visible.ToString();
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

        #endregion // Events

        #region Methods
        private void Option_Click(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method Option_Click ...", category: Category.Info, priority: Priority.Low);
            try
            {
                //if (Visible == Visibility.Hidden.ToString())
                //{
                //    return;
                //}
                string Selected = ((System.Windows.UIElement)((DevExpress.Xpf.Accordion.AccordionControl)obj).SelectedItem).Uid;

                IWorkbenchStartUp objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
                CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();
                // shortcuts
                // Get shortcut for Attendance
                if (Selected == "Attendance")
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
                    ObservableCollection<EmployeeAttendance> EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>(HrmService.GetSelectedIdCompanyEmployeeAttendance_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    ObservableCollection<EmployeeLeave> EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    addAttendanceViewModel.Init(EmployeeAttendanceList, null, DateTime.Today, DateTime.Today,EmployeeLeaves);
                    addAttendanceViewModel.IsNew = true;
                    addAttendanceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewAttendance").ToString();
                    addAttendanceViewModel.EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    addAttendanceView.ShowDialog();                                        
                }

                // Get shortcut for  Employee

                if (Selected == "Employee")
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


                // Get shortcut for  Holiday

                if (Selected == "Holiday")
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

                // Get shortcut for Job Descriptions

                if (Selected == "JobDescriptions")
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

                // Get shortcut for  Leave
                if (Selected == "Leave")
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

                // Get shortcut for Export Attendance List
                if (Selected == "AttendanceList")
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

                // Get shortcut for Organization Chart
                if (Selected == "OrganizationChart")
                {
                    Processing();
                    
                    EmployeeOrganizationViewModel employeeOrganizationViewModel = new EmployeeOrganizationViewModel();
                    employeeOrganizationViewModel.FillDepartmentListByPlant();
                    employeeOrganizationViewModel.OrganizationChartExportToExcel(obj);
                }

                // Get shortcut for Import Attendance
                if (Selected == "ImportAttendance")
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
                // Get shortcut for  SearchEmployee

                if (Selected == "SearchEmployee")
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
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Option_Click....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Option_Click...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        private void ShortcutAction(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                //if (Visible == Visibility.Hidden.ToString())
                //{
                //    return;
                //}
                HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void HideTileBarButtonClickCommandAction(RoutedEventArgs obj)
        {
            if (GeosApplication.Instance.TileBarVisibility == Visibility.Collapsed)
            {
                GeosApplication.Instance.TileBarVisibility = Visibility.Visible;
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString(); //Hide menu
            }
            else
            {
                GeosApplication.Instance.TileBarVisibility = Visibility.Collapsed;
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("ShowMenuButtonToolTip").ToString(); // ShowMenu
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion // Methods
    }
}
