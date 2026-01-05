using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Emdep.Geos.Utility;
using Emdep.Geos.UI.CustomControls;
using System.ServiceModel;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.XtraScheduler;
using System.Windows.Threading;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class HrmMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        /// <summary>
        /// [M049-07][20180810][Add option to add and edit shifts][adadibathina]
        /// </summary>

        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private int idUser;
        private long selectedPeriod;
        #endregion // Services

        #region Properties       
        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }
        public int IdHrmUserViewPermission{ get; private set; }

        #endregion

        #region Command
        public ICommand LoadedEventCommand { get; set; }
        #endregion

        TileBarItemsHelper tileBarItemsHelperDashboard;
        TileBarItemsHelper tileBarItemsHelperEmployees;
        TileBarItemsHelper tileBarItemsHelperOrganization;
        TileBarItemsHelper tileBarItemsHelperLeaves;
        TileBarItemsHelper tileBarItemLeavesSchedule;
        TileBarItemsHelper tileBarItemLeavesSummary;
        TileBarItemsHelper tileBarItemsHelperAttendance;
        TileBarItemsHelper tileBarItemsHelperConfiguration;
        TileBarItemsHelper tileBarItemConfigutionCompanies;
        TileBarItemsHelper tileBarItemConfigutionDepartments;
        TileBarItemsHelper tileBarItemConfigutionHolidays;
        TileBarItemsHelper tileBarItemConfigutionJobDescription;
        TileBarItemsHelper tileBarConfigutionShifts;
        TileBarItemsHelper tileBarConfigutionLeaves;
        TileBarItemsHelper tileBarConfigutionWorkTypes;
        TileBarItemsHelper tileBarConfigutionMyPreferences;

        #region Constructor
        /// <summary>
        /// [001][skale][2019-21-5][GEOS2-273][SP63]Add new fields Company, Organization,Location in employees grid
        /// </summary>
        public HrmMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor HrmMainViewModel()...", category: Category.Info, priority: Priority.Low);

                LoadedEventCommand = new RelayCommand(new Action<object>(UserControl_Loaded));

                GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails(GeosApplication.Instance.ActiveUser.IdUser);
                GeosApplication.Instance.PlantOwnerUsersList = GeosApplication.Instance.PlantOwnerUsersList.OrderBy(o => o.Country.IdCountry).ToList();
                GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();
                HrmCommon.Instance.SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;
                HrmCommon.Instance.UserAuthorizedPlantsList = new ObservableCollection<Company>(HrmService.GetAuthorizedPlantsByIdUser_V2031(GeosApplication.Instance.ActiveUser.IdUser));

                HrmCommon.Instance.IsCompanyList = new ObservableCollection<Company>(HrmCommon.Instance.UserAuthorizedPlantsList.Where(x => x.IsCompany == 1).OrderBy(x=>x.Country.Name).ThenBy(x=>x.Alias));
                HrmCommon.Instance.IsOrganizationList = new ObservableCollection<Company>(HrmCommon.Instance.UserAuthorizedPlantsList.Where(x => x.IsOrganization == 1).OrderBy(x => x.Country.Name).ThenBy(x => x.Alias));
                HrmCommon.Instance.IsLocationList = new ObservableCollection<Company>(HrmCommon.Instance.UserAuthorizedPlantsList.Where(x => x.IsLocation == 1).OrderBy(x => x.Country.Name).ThenBy(x => x.Alias));

                //[001] added
                ObservableCollection<Company> tempCommonList = new ObservableCollection<Company>();

                if (HrmCommon.Instance.IsCompanyList != null)
                    tempCommonList.AddRange(HrmCommon.Instance.IsCompanyList);

                if (HrmCommon.Instance.IsOrganizationList != null)
                    tempCommonList.AddRange(HrmCommon.Instance.IsOrganizationList);

                if (HrmCommon.Instance.IsLocationList != null)
                    tempCommonList.AddRange(HrmCommon.Instance.IsLocationList);

                tempCommonList = new ObservableCollection<Company>(tempCommonList.OrderBy(Company => Company.IdCountry).ToList());

                ObservableCollection<Company> tempCollection = new ObservableCollection<Company>(tempCommonList.GroupBy(x => x.IdCompany).Select(group => group.First()));
                HrmCommon.Instance.CombineIslocationIsorganizationIscompanyList = new ObservableCollection<Company>(tempCollection.OrderBy(x => x.Country.Name).ThenBy(x => x.Alias));
                //end

                HrmCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();

                //HrmCommon.Instance.IsPermissionReadOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 38);

                //if (HrmCommon.Instance.IsPermissionReadOnly)
                //    HrmCommon.Instance.IsPermissionEnabled = false;
                //else
                HrmCommon.Instance.IsPermissionEnabled = true;
                HrmCommon.Instance.ActiveEmployee = HrmService.GetEmployeeCurrentDetail(GeosApplication.Instance.ActiveUser.IdUser, HrmCommon.Instance.SelectedPeriod);

                HrmCommon.Instance.IdUserPermission = SelectIdUserPermission();

                HrmCommon.Instance.UserPermission = SelectUserPermission();

                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == serviceurl);
                Company selectedPlant = HrmCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.ShortName == serviceurl);

                if (usrDefault != null)
                {
                    GeosApplication.Instance.SelectedPlantOwnerUsersList.Add(usrDefault);
                }
                else
                {
                    GeosApplication.Instance.SelectedPlantOwnerUsersList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                }

                if (selectedPlant != null)
                {
                    HrmCommon.Instance.SelectedAuthorizedPlantsList.Add(selectedPlant);
                }
                else
                {
                    HrmCommon.Instance.SelectedAuthorizedPlantsList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                }


                TileCollection = new ObservableCollection<TileBarItemsHelper>();

                tileBarItemsHelperDashboard = new TileBarItemsHelper();
                tileBarItemsHelperDashboard.Caption = System.Windows.Application.Current.FindResource("HRMDashboard").ToString();
                tileBarItemsHelperDashboard.BackColor = "#00879C";
                tileBarItemsHelperDashboard.GlyphUri = "Dashboard.png";
                tileBarItemsHelperDashboard.Visibility = Visibility.Hidden;
                tileBarItemsHelperDashboard.NavigateCommand = new DelegateCommand(NavigateDashboardView);
                TileCollection.Add(tileBarItemsHelperDashboard);
                
                tileBarItemsHelperEmployees = new TileBarItemsHelper();
                tileBarItemsHelperEmployees.Caption = System.Windows.Application.Current.FindResource("HRMEmployees").ToString();
                tileBarItemsHelperEmployees.BackColor = "#CC6D00";
                tileBarItemsHelperEmployees.GlyphUri = "hrm_employee.png";
                tileBarItemsHelperEmployees.Visibility = Visibility.Hidden;
                tileBarItemsHelperEmployees.NavigateCommand = new DelegateCommand(NavigateEmployeesView);

                //  tileBarItemsHelperEmployees.Children = new ObservableCollection<TileBarItemsHelper>();

                //Employees Profile.
                //TileBarItemsHelper tileBarItemEmployeeProfile = new TileBarItemsHelper()
                //{
                //    Caption = System.Windows.Application.Current.FindResource("EmployeesProfile").ToString(),
                //    BackColor = "#00879C",
                //    GlyphUri = "bProfile.png",
                //    Visibility = Visibility.Visible,
                //    //NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenEmployeeProfileView(), null, this); })
                //    NavigateCommand = new DelegateCommand(NavigateEmployeesView)
                //};
                //tileBarItemsHelperEmployees.Children.Add(tileBarItemEmployeeProfile);

                TileCollection.Add(tileBarItemsHelperEmployees);

                tileBarItemsHelperOrganization = new TileBarItemsHelper();
                tileBarItemsHelperOrganization.Caption = System.Windows.Application.Current.FindResource("HRMOrganization").ToString();
                tileBarItemsHelperOrganization.BackColor = "#5C5C5C";
                tileBarItemsHelperOrganization.GlyphUri = "hrm_organization.png";
                tileBarItemsHelperOrganization.Visibility = Visibility.Hidden;
                tileBarItemsHelperOrganization.NavigateCommand = new DelegateCommand(NavigateEmployeesOrganizationView);
                TileCollection.Add(tileBarItemsHelperOrganization);

                //Leaves
                tileBarItemsHelperLeaves = new TileBarItemsHelper();
                tileBarItemsHelperLeaves.Caption = System.Windows.Application.Current.FindResource("HRMLeaves").ToString();
                tileBarItemsHelperLeaves.BackColor = "#3E7038";
                tileBarItemsHelperLeaves.GlyphUri = "hrm_Leaves.png";
                tileBarItemsHelperLeaves.Visibility = Visibility.Hidden;
                tileBarItemsHelperLeaves.Children = new ObservableCollection<TileBarItemsHelper>();

                tileBarItemLeavesSchedule = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("LeavesSchedule").ToString(),
                    BackColor = "#3E7038",
                    GlyphUri = "bLeavesSchedule.png",
                    Visibility = Visibility.Hidden,
                    NavigateCommand = new DelegateCommand(NavigateEmployeesLeavesView)
                };
                tileBarItemsHelperLeaves.Children.Add(tileBarItemLeavesSchedule);

                //[Sprint_61] [17-04-2019] (#70119) New section Leave Summary under Leaves section---[sdesai]
                tileBarItemLeavesSummary = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("LeavesSummary").ToString(),
                    BackColor = "#3E7038",
                    GlyphUri = "bLeavesSummary.png",
                    Visibility = Visibility.Hidden,
                    NavigateCommand = new DelegateCommand(NavigateEmployeesLeavesSummaryView)

                };
                tileBarItemsHelperLeaves.Children.Add(tileBarItemLeavesSummary);

                TileCollection.Add(tileBarItemsHelperLeaves);

                //Attendance
                tileBarItemsHelperAttendance = new TileBarItemsHelper();
                tileBarItemsHelperAttendance.Caption = System.Windows.Application.Current.FindResource("HRMAttendance").ToString();
                tileBarItemsHelperAttendance.BackColor = "#840a6a";
                tileBarItemsHelperAttendance.GlyphUri = "hrm_attendance.png";
                tileBarItemsHelperAttendance.Visibility = Visibility.Hidden;
                tileBarItemsHelperAttendance.NavigateCommand = new DelegateCommand(NavigateEmployeesAttendanceView);
                TileCollection.Add(tileBarItemsHelperAttendance);

                //Configuration
                tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("HRMConfiguration").ToString();
                tileBarItemsHelperConfiguration.BackColor = "#8b99e8";
                tileBarItemsHelperConfiguration.GlyphUri = "hrm_configuration.png";
                tileBarItemsHelperConfiguration.Visibility = Visibility.Hidden;
                tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                tileBarItemConfigutionCompanies = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("Companies").ToString(),
                    BackColor = "#8b99e8",
                    GlyphUri = "bCompanies.png",
                    Visibility = Visibility.Hidden,
                    NavigateCommand = new DelegateCommand(NavigateEmployeesConfigurationView)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigutionCompanies);

                tileBarItemConfigutionDepartments = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("Departments").ToString(),
                    BackColor = "#8b99e8",
                    GlyphUri = "bDepartment.png",
                    Visibility = Visibility.Hidden,
                    NavigateCommand = new DelegateCommand(NavigateDepartmentView)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigutionDepartments);

                //Sprint 41-Add Holidays Section ---sdesai
                tileBarItemConfigutionHolidays = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("Holidays").ToString(),
                    BackColor = "#8b99e8",
                    GlyphUri = "bHolidays.png",
                    Visibility = Visibility.Hidden,
                    NavigateCommand = new DelegateCommand(NavigateHolidaysView)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigutionHolidays);

                //Sprint 41-Add Job Descriptions Section ---sdesai
                tileBarItemConfigutionJobDescription = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("JobDescriptions").ToString(),
                    BackColor = "#8b99e8",
                    GlyphUri = "bJobDescriptions.png",
                    Visibility = Visibility.Hidden,
                    NavigateCommand = new DelegateCommand(NavigateJobDescriptionsView)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigutionJobDescription);
                //Sprint 49 
                //TileCollection.Add(tileBarItemsHelperConfiguration);

                //Sprint 49 - Add option to add and edit shifts -- adadibathina

                tileBarConfigutionShifts = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("Shifts").ToString(),
                    BackColor = "#8b99e8",
                    GlyphUri = "bShifts.png",
                    Visibility = Visibility.Hidden,
                    NavigateCommand = new DelegateCommand(NavigateShiftsView)

                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionShifts);

                tileBarConfigutionLeaves = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("Leaves").ToString(),
                    BackColor = "#8b99e8",
                    GlyphUri = "bLeaves.png",
                    Visibility = Visibility.Hidden,
                    NavigateCommand = new DelegateCommand(NavigateLeavesView)

                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionLeaves);

                tileBarConfigutionWorkTypes = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WorkTypes").ToString(),
                    BackColor = "#8b99e8",
                    GlyphUri = "bWorkTypes.png",
                    Visibility = Visibility.Hidden,
                    NavigateCommand = new DelegateCommand(NavigateWorkTypesView)

                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionWorkTypes);

                //sjadhav
                tileBarConfigutionMyPreferences = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("MyPreferences").ToString(),
                    BackColor = "#8b99e8",
                    GlyphUri = "MyPreference_Black.png",
                    Visibility = Visibility.Hidden,
                    NavigateCommand = new DelegateCommand(NavigateMyPreferenceView)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionMyPreferences);

                TileCollection.Add(tileBarItemsHelperConfiguration);

                //GetHRMDataOnceFromService();

                GeosApplication.Instance.Logger.Log("Constructor Constructor HrmMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor HrmMainViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Constructor HrmMainViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor HrmMainViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UserControl_Loaded(object obj)
        {
            //Employees = new ObservableCollection<Employee>();
            //EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
            //CompanyHolidays = new ObservableCollection<CompanyHoliday>();
            //HolidayList = new ObservableCollection<LookupValue>();
            //EmployeeLeaves = new ObservableCollection<EmployeeLeave>();
            //LabelItems = new ObservableCollection<LabelHelper>();
            //StatusItems = new ObservableCollection<StatusHelper>();
            //appointment = new ObservableCollection<UI.Helper.Appointment>();
            //EmployeeListFinal = new ObservableCollection<Employee>();

            GetHRMDataOnceFromService();

            Hrm.HrmCommon.Instance.FinancialYearList = GeosApplication.Instance.FillFinancialYear();
            _HrmMainView1 = (HrmMainView)((System.Windows.RoutedEventArgs)obj).Source;

            _HrmMainView1.DXTabControl1.SelectTabItem(5);

            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.DataBind,
              new Action(() =>
              SetDataEmployeeAttendanceViewModel()));

            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.ApplicationIdle,
              new Action(() =>
              SetDataEmployeeProfileViewModel()));
            
            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.ApplicationIdle,
              new Action(() =>
              SetDataDashboardViewModel()));

            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.ApplicationIdle,
              new Action(() =>
              SetDataEmployeeOrganizationViewModel()));

            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.ApplicationIdle,
              new Action(() =>
              SetDataEmployeeLeavesScheduleViewModel()));

            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.ApplicationIdle,
              new Action(() =>
              SetDataEmployeeLeavesSummaryViewModel()));

            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.ApplicationIdle,
              new Action(() =>
              SetDataEmployeeConfigurationViewModel()));
        }

        private void SetDataDashboardViewModel()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
          _HrmMainView1.DashboardView1.DataContext = dashboardViewModel;
            tileBarItemsHelperDashboard.Visibility = Visibility.Visible;
        }
        private void SetDataEmployeeProfileViewModel()
        {
            EmployeeProfileViewModel employeeProfileViewModel = new EmployeeProfileViewModel();
            _HrmMainView1.EmployeeProfileView1.DataContext = employeeProfileViewModel;
            employeeProfileViewModel.Init();
            tileBarItemsHelperEmployees.Visibility = Visibility.Visible;
        }
        private void SetDataEmployeeOrganizationViewModel()
        {
            EmployeeOrganizationViewModel employeeOrganizationViewModel = new EmployeeOrganizationViewModel();
            _HrmMainView1.EmployeeOrganizationView1.DataContext = employeeOrganizationViewModel;
            employeeOrganizationViewModel.Init();
            tileBarItemsHelperOrganization.Visibility = Visibility.Visible;
        }
        private void SetDataEmployeeLeavesScheduleViewModel()
        {
            EmployeeLeavesViewModel employeeLeavesScheduleViewModel = new EmployeeLeavesViewModel();
            _HrmMainView1.EmployeeLeavesScheduleView1.DataContext = employeeLeavesScheduleViewModel;
            employeeLeavesScheduleViewModel.Init(EmployeeListFinalForLeaves, EmployeeAttendanceListForNewLeave, Departments, EmployeeLeaves);
            tileBarItemsHelperLeaves.Visibility = Visibility.Visible;
            tileBarItemLeavesSchedule.Visibility = Visibility.Visible;
        }

        private void SetDataEmployeeLeavesSummaryViewModel()
        {
            EmployeeLeavesSummaryViewModel employeeLeavesSummaryViewModel = new EmployeeLeavesSummaryViewModel();
            _HrmMainView1.EmployeeLeavesSummaryView1.DataContext = employeeLeavesSummaryViewModel;
            employeeLeavesSummaryViewModel.Init();
            tileBarItemsHelperLeaves.Visibility = Visibility.Visible;
            tileBarItemLeavesSummary.Visibility = Visibility.Visible;
        }
        private void SetDataEmployeeAttendanceViewModel()
        {
            EmployeeAttendanceViewModel employeeAttendanceViewModel = new EmployeeAttendanceViewModel();
            _HrmMainView1.EmployeeAttendanceView1.DataContext = employeeAttendanceViewModel;
            employeeAttendanceViewModel.Init(Employees, EmployeeAttendanceList, CompanyHolidays,
                HolidayList, EmployeeLeaves, LabelItems, StatusItems, appointment, EmployeeListFinal);
            tileBarItemsHelperAttendance.Visibility = Visibility.Visible;
        }
        private void SetDataEmployeeConfigurationViewModel()
        {
            EmployeeConfigurationViewModel employeeConfigurationViewModel = new EmployeeConfigurationViewModel();
            _HrmMainView1.EmployeeConfigurationView1.DataContext = employeeConfigurationViewModel;
            employeeConfigurationViewModel.Init();
            tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
        }

        ObservableCollection<Employee> Employees;
        ObservableCollection<EmployeeAttendance> EmployeeAttendanceList;
        ObservableCollection<EmployeeAttendance> EmployeeAttendanceListForNewLeave;
        ObservableCollection<CompanyHoliday> CompanyHolidays;
        ObservableCollection<LookupValue> HolidayList;
        ObservableCollection<EmployeeLeave> EmployeeLeaves;
        ObservableCollection<LabelHelper> LabelItems;
        ObservableCollection<StatusHelper> StatusItems;
        ObservableCollection<UI.Helper.Appointment> appointment;
        private HrmMainView _HrmMainView1;
        ObservableCollection<Employee> EmployeeListFinal;
        ObservableCollection<Employee> EmployeeListFinalForLeaves;
        ObservableCollection<Department> Departments;
        public List<Company> SelectedPlantList { get; set; } // Added

        public void GetHRMDataOnceFromService()
        {
            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            {
                appointment = new ObservableCollection<UI.Helper.Appointment>();
                List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                //List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission);
                // [003] Changed service method GetAllEmployeesForAttendanceByIdCompany_V2037 to GetAllEmployeesForAttendanceByIdCompany_V2039
                Employees = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                // [003] Changed service method GetSelectedIdCompanyEmployeeAttendance_V2037 to GetSelectedIdCompanyEmployeeAttendance_V2039

                // [004] Changed service method GetSelectedIdCompanyEmployeeAttendance_V2044 to GetSelectedIdCompanyEmployeeAttendance_V2045
                // EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>(HrmService.GetSelectedIdCompanyEmployeeAttendance_V2045(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>(HrmService.GetSelectedIdCompanyEmployeeAttendance_V2060(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                EmployeeAttendanceListForNewLeave = new ObservableCollection<EmployeeAttendance>(HrmService.GetEmployeeAttendanceForNewLeave(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                Departments = new ObservableCollection<Department>(HrmService.GetAllEmployeesByDepartmentByIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                EmployeeAttendanceViewModel.SetIsManual(EmployeeAttendanceList);
                CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds));
                HolidayList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(28).AsEnumerable());
                // [003] Changed service method GetEmployeeLeavesBySelectedIdCompany_V2032 to GetEmployeeLeavesBySelectedIdCompany_V2039
                EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2045(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                SelectedPlantList = plantOwners;
                //Akshay Start
                foreach (var item in SelectedPlantList)
                {
                     EmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2044(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));
                    EmployeeListFinalForLeaves = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForLeaveByIdCompany_V2041(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));
                }
                    //Akshay End

                LabelItems = new ObservableCollection<LabelHelper>();
                StatusItems = new ObservableCollection<StatusHelper>();

                //SelectedItem = null;
                //IsFromRefresh = true;
                //[001] Code Comment and Fill employee work
                // CompanyWorksList = new ObservableCollection<CompanyWork>(HrmService.GetAllCompanyWorks());
                EmployeeAttendanceViewModel.FillEmployeeWorkType();
                EmployeeAttendanceViewModel.FillEmployeeLeaveType();
                //if (EmployeeAttendanceList.Count > 0)
                //{
                //    SelectedAttendanceRecord = EmployeeAttendanceList[0];
                //}

                //TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

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
                    if (i == 0)
                        statusItem.Brush = new SolidColorBrush(Colors.Transparent); //BrushConverter().ConvertFromString("#7833FF ") as SolidColorBrush;
                    else
                        statusItem.Brush = new SolidColorBrush(Colors.SlateBlue);
                    statusItem.Caption = "Night Shift";
                    StatusItems.Add(statusItem);
                }

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

                // AppointmentItems = appointment;
            }

            //StringMonthlyExpectedTotalHoursCount = "00:00";
            //StringMonthlyTotalHoursCount = "00:00";

        }

        #endregion

        #region Methods

        private void AddSettings()
        {
            GeosApplication.Instance.Logger.Log("HrmMainViewModel Method AddSettings()...", Category.Exception, priority: Priority.Low);
            bool addsettings = false;
            List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
            if (!GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceFilePath"))
            {
                // lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceFilePath", ""));
                addsettings = true;
            }
            if (!GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceDataSourceSelectedIndex"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceDataSourceSelectedIndex", ""));
                addsettings = true;
            }
            if (addsettings)
            {
                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));

                }
                ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
            }


            GeosApplication.Instance.Logger.Log("HrmMainViewModel Method AddSettings()...executed successfully", Category.Exception, priority: Priority.Low);

        }

        /// <summary>
        /// Method for Navigate Dashboard
        /// </summary>
        private void NavigateDashboardView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDashboardView()...", category: Category.Info, priority: Priority.Low);
                _HrmMainView1.DXTabControl1.SelectTabItem(0);
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.DashboardView", new DashboardViewModel(), null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDashboardView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateDashboardView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Employees View
        /// </summary>
        private void NavigateEmployeesView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()...", category: Category.Info, priority: Priority.Low);
                _HrmMainView1.DXTabControl1.SelectTabItem(1);

                //EmployeeProfileViewModel employeeProfileViewModel = new EmployeeProfileViewModel();
                //employeeProfileViewModel.Init();
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeProfileView", employeeProfileViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Employees Organization View
        /// </summary>
        private void NavigateEmployeesOrganizationView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesOrganizationView()...", category: Category.Info, priority: Priority.Low);
                _HrmMainView1.DXTabControl1.SelectTabItem(2);
                //EmployeeOrganizationViewModel employeeOrganizationViewModel = new EmployeeOrganizationViewModel();
                //employeeOrganizationViewModel.Init();
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeOrganizationView", employeeOrganizationViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesOrganizationView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Employees Leaves View
        /// </summary>
        private void NavigateEmployeesLeavesView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesLeavesView()...", category: Category.Info, priority: Priority.Low);
                _HrmMainView1.DXTabControl1.SelectTabItem(3);
                // EmployeeLeavesViewModel employeeLeavesViewModel = new EmployeeLeavesViewModel();  
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeLeaves", new EmployeeLeavesViewModel(), null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesLeavesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary> 
        /// Method for Navigate Employees Configuration View
        /// </summary>

        private void NavigateEmployeesConfigurationView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesConfigurationViewModel()...", category: Category.Info, priority: Priority.Low);
                _HrmMainView1.DXTabControl1.SelectTabItem(6);
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

                //EmployeeConfigurationViewModel employeeConfigurationViewModel = new EmployeeConfigurationViewModel();
                //employeeConfigurationViewModel.Init();

                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeConfigurationView", employeeConfigurationViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EmployeeConfigurationView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Employees Attendance View
        /// </summary>
        private void NavigateEmployeesAttendanceView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesAttendanceView()...", category: Category.Info, priority: Priority.Low);
                _HrmMainView1.DXTabControl1.SelectTabItem(5);
                //GetHRMDataOnceFromService();
                //EmployeeAttendanceViewModel employeeAttendanceViewModel = new EmployeeAttendanceViewModel();
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeAttendanceView", employeeAttendanceViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesAttendanceView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesAttendanceView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Department View
        /// </summary>
        private void NavigateDepartmentView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDepartmentView()...", category: Category.Info, priority: Priority.Low);

                EmployeeDepartmentsViewModel employeeDepartmentsViewModel = new EmployeeDepartmentsViewModel();
                employeeDepartmentsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeDepartmentsView", employeeDepartmentsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDepartmentView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateDepartmentView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Sprint 41-[HRM-M041-07] New configuration section Holidays---sdesai
        /// Method to Navigate Employees Holidays View
        /// </summary>
        private void NavigateHolidaysView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesHolidaysView()...", category: Category.Info, priority: Priority.Low);

                EmployeesHolidaysViewModel employeesHolidaysViewModel = new EmployeesHolidaysViewModel();
                employeesHolidaysViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeesHolidaysView", employeesHolidaysViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesHolidaysView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesHolidaysView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void NavigateJobDescriptionsView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateJobDescriptionsView()...", category: Category.Info, priority: Priority.Low);

                EmployeeJobDescriptionsViewModel employeeJobDescriptionsViewModel = new EmployeeJobDescriptionsViewModel();
                employeeJobDescriptionsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeJobDescriptionsView", employeeJobDescriptionsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateJobDescriptionsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateJobDescriptionsView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Sprint 49-[M049-07][20180810][Add option to add and edit shifts][adadibathina]
        /// Method to Navigate Configuration Shifts
        /// </summary>
        public void NavigateShiftsView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateShiftsView()...", category: Category.Info, priority: Priority.Low);

                EmployeeShiftsViewModel employeeShiftsViewModel = new EmployeeShiftsViewModel();
                employeeShiftsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeShiftsView", employeeShiftsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateShiftsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateShiftsView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Navigate Configuration Leaves
        /// </summary>
        private void NavigateLeavesView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateLeavesView()...", category: Category.Info, priority: Priority.Low);
                _HrmMainView1.DXTabControl1.SelectTabItem(4);

                //LeavesViewModel leavesViewModel = new LeavesViewModel();
                //leavesViewModel.Init();
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.LeavesView", leavesViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateLeavesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateLeavesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Navigate Configuration My Preference
        /// </summary>
        private void NavigateMyPreferenceView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateMyPreferenceView()...", category: Category.Info, priority: Priority.Low);
                //MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                ////myPreferencesViewModel.Init();
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.MyPreferencesView", myPreferencesViewModel, null, this, true);

                //IsBusy = true;
                MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                MyPreferencesView myPreferencesView = new MyPreferencesView();
                EventHandler handle = delegate { myPreferencesView.Close(); };
                myPreferencesViewModel.RequestClose += handle;
                myPreferencesView.DataContext = myPreferencesViewModel;
                //IsBusy = false;
                myPreferencesView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method NavigateMyPreferenceView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateMyPreferenceView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Navigate Configuration Work Types
        /// </summary>
        private void NavigateWorkTypesView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateWorkTypesView()...", category: Category.Info, priority: Priority.Low);
                WorkTypesViewModel workTypesViewModel = new WorkTypesViewModel();
                workTypesViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.WorkTypesView", workTypesViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateWorkTypesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateWorkTypesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [Sprint 61]-[17-04-2019][(#70119) New section Leave Summary under Leaves section][sdesai]
        /// Method to navigate Leaves summary
        /// </summary>
        private void NavigateEmployeesLeavesSummaryView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesLeavesSummaryView()...", category: Category.Info, priority: Priority.Low);
                _HrmMainView1.DXTabControl1.SelectTabItem(4);
                //EmployeeLeavesSummaryViewModel employeeLeavesSummaryViewModel = new EmployeeLeavesSummaryViewModel();
                //employeeLeavesSummaryViewModel.Init();
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeLeavesSummaryView", employeeLeavesSummaryViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesLeavesSummaryView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesLeavesSummaryView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 26 View
        /// 30 Change
        /// 38 Admin
        /// 39 Watch All
        /// 40 Watch Organization
        /// 41 Watch Department
        /// </summary>
        /// <returns></returns>
        public PermissionManagement SelectUserPermission()
        {
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 26)
             && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30)
             && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 38)
             && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 39))
            {
                return PermissionManagement.SuperAdmin;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 26)
                 && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30)
                 && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 39))
            {
                return PermissionManagement.Admin;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 26)
                  && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30)
                  && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 40))
            {
                return PermissionManagement.PlantViewer;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 26)
               && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30)
               && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 41))
            {
                return PermissionManagement.GlobalViewer;
            }

            return PermissionManagement.None;
        }

        public int SelectIdUserPermission()
        {
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 39))
            {
                return 39;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 40))
            {
                return 40;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 41))
            {
                return 41;
            }

            return 0;
        }

        #endregion
    }
}
